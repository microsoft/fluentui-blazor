// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents a tree view component.
/// </summary>
public partial class FluentTreeView : FluentComponentBase
{
    private const string JAVASCRIPT_FILE = FluentJSModule.JAVASCRIPT_ROOT + "TreeView/FluentTreeView.razor.js";
    private readonly Dictionary<ITreeViewItem, IReadOnlyList<ITreeViewItem>> _childrenByItem = [];
    private readonly Dictionary<ITreeViewItem, bool?> _selectionStates = [];
    private readonly HashSet<ITreeViewItem> _selectedItemSet = [];

    internal ConcurrentDictionary<string, FluentTreeItem> InternalItems { get; } = new(StringComparer.Ordinal);

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentTreeView"/> class.
    /// </summary>
    public FluentTreeView(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary/>
    protected string? ClassValue => DefaultClassBuilder
        .Build();

    /// <summary/>
    protected string? StyleValue => DefaultStyleBuilder
        .Build();

    /// <summary />
    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        if (LazyLoadItems && SelectionMode == TreeSelectionMode.MultipleRecursive)
        {
            throw new InvalidOperationException(
                $"{nameof(LazyLoadItems)} cannot be used together with {nameof(TreeSelectionMode.MultipleRecursive)} selection.");
        }

        var normalizedItems = BuildSelectionStateCache();
        if (SelectedItems is not null && !AreSameItems(SelectedItems, normalizedItems))
        {
            SelectedItems = normalizedItems;

            if (SelectedItemsChanged.HasDelegate)
            {
                await SelectedItemsChanged.InvokeAsync(SelectedItems);
            }
        }
    }

    internal bool? GetSelectionState(ITreeViewItem item)
        => _selectionStates.TryGetValue(item, out var state) ? state : false;

    private List<ITreeViewItem> BuildSelectionStateCache()
    {
        _childrenByItem.Clear();
        _selectionStates.Clear();
        _selectedItemSet.Clear();

        if (SelectionMode != TreeSelectionMode.MultipleRecursive)
        {
            return SelectedItems?.ToList() ?? [];
        }

        var normalizedItems = SelectedItems?.ToList() ?? [];
        foreach (var selectedItem in SelectedItems ?? [])
        {
            _selectedItemSet.Add(selectedItem);
        }

        foreach (var item in Items ?? [])
        {
            BuildSelectionState(item, normalizedItems);
        }

        return normalizedItems;
    }

    private bool? BuildSelectionState(ITreeViewItem item, List<ITreeViewItem> normalizedItems)
    {
        var children = item.Items?.ToArray() ?? [];
        _childrenByItem[item] = children;

        if (children.Length == 0)
        {
            var leafState = _selectedItemSet.Contains(item);
            _selectionStates[item] = leafState;
            return leafState;
        }

        var childStates = children.Select(child => BuildSelectionState(child, normalizedItems)).ToList();
        bool? itemState = childStates.All(childState => childState == true)
            ? true
            : _selectedItemSet.Contains(item) || childStates.Any(childState => childState != false)
                ? null
                : false;

        _selectionStates[item] = itemState;
        if (itemState == true && _selectedItemSet.Add(item))
        {
            normalizedItems.Add(item);
        }

        return itemState;
    }

    private static bool AreSameItems(IEnumerable<ITreeViewItem>? first, IEnumerable<ITreeViewItem> second)
        => first is not null && first.SequenceEqual(second);

    /// <summary>
    /// Gets or sets the size of the tree. Default is <see cref="TreeSize.Medium"/>.
    /// </summary>
    [Parameter]
    public TreeSize? Size { get; set; } = TreeSize.Medium;

    /// <summary>
    /// Gets or sets the appearance of the tree. Default is <see cref="TreeAppearance.Subtle"/>.
    /// </summary>
    [Parameter]
    public TreeAppearance? Appearance { get; set; } = TreeAppearance.Subtle;

    /// <summary>
    /// Gets or sets whether the selection highlight is hidden.
    /// When <c>true</c>, the selected tree item is not visually highlighted.
    /// </summary>
    [Parameter]
    public bool HideSelection { get; set; }

    /// <summary>
    /// Gets or sets the list of items to bind to the tree.
    /// </summary>
    [Parameter]
    public IEnumerable<ITreeViewItem>? Items { get; set; }

    /// <summary>
    /// Gets or sets the template for rendering tree items.
    /// </summary>
    [Parameter]
    public RenderFragment<ITreeViewItem>? ItemTemplate { get; set; }

    /// <summary>
    /// Can only be used when the <see cref="Items"/> is defined.
    /// Gets or sets whether the tree should use lazy loading when expanding nodes.
    /// If True, the tree will only render the children of a node when it is expanded and will remove them when it is collapsed.
    /// This cannot be combined with <see cref="TreeSelectionMode.MultipleRecursive"/>.
    /// </summary>
    [Parameter]
    public bool LazyLoadItems { get; set; } = false;

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the id of the currently selected tree item.
    /// See also <see cref="SelectedItem"/> (returns the <see cref="ITreeViewItem"/> data model),
    /// and <see cref="CurrentSelected"/> (returns the <see cref="FluentTreeItem"/> component instance).
    /// </summary>
    [Parameter]
    public string? SelectedId { get; set; }

    /// <summary>
    /// Called whenever the selected item changes.
    /// </summary>
    [Parameter]
    public EventCallback<string?> SelectedIdChanged { get; set; }

    /// <summary>
    /// Gets or sets the selected <see cref="FluentTreeItem"/> component instance.
    /// See also <see cref="SelectedId"/> (returns the item id string),
    /// and <see cref="SelectedItem"/> (returns the <see cref="ITreeViewItem"/> data model).
    /// </summary>
    [Parameter]
    public FluentTreeItem? CurrentSelected { get; set; }

    /// <summary>
    /// Called whenever the selected <see cref="FluentTreeItem" /> changes.
    /// </summary>
    [Parameter]
    public EventCallback<FluentTreeItem?> CurrentSelectedChanged { get; set; }

    /// <summary>
    /// Gets or sets the selected <see cref="ITreeViewItem"/> data model item.
    /// See also <see cref="SelectedId"/> (returns the item id string),
    /// and <see cref="CurrentSelected"/> (returns the <see cref="FluentTreeItem"/> component instance).
    /// </summary>
    [Parameter]
    public ITreeViewItem? SelectedItem { get; set; }

    /// <summary>
    /// Called whenever the selected <see cref="ITreeViewItem" /> changes.
    /// </summary>
    [Parameter]
    public EventCallback<ITreeViewItem?> SelectedItemChanged { get; set; }

    /// <summary>
    /// Gets or sets whether the tree allows multiple selections.
    /// This Multiple Selection feature is only available when the <see cref="Items"/> parameter is used to generate the tree.
    /// By default, the tree allows only single selection.
    /// </summary>
    [Parameter]
    public TreeSelectionMode SelectionMode { get; set; } = TreeSelectionMode.Single;

    /// <summary>
    /// Gets or sets the visibility of the multi-selection checkbox.
    /// By default all items are visible.
    /// </summary>
    [Parameter]
    public Func<ITreeViewItem, TreeSelectionVisibility>? MultipleSelectionVisibility { get; set; }

    /// <summary>
    /// Gets or sets the multi-selected <see cref="ITreeViewItem" /> items.
    /// In <see cref="TreeSelectionMode.MultipleRecursive"/> mode, fully selected
    /// branches include their ancestors in this collection.
    /// </summary>
    [Parameter]
    public IEnumerable<ITreeViewItem>? SelectedItems { get; set; }

    /// <summary>
    /// Called whenever the multi-selected <see cref="ITreeViewItem" /> changes.
    /// </summary>
    [Parameter]
    public EventCallback<IEnumerable<ITreeViewItem>?> SelectedItemsChanged { get; set; }

    /// <summary>
    /// Called whenever <see cref="FluentTreeItem.Expanded"/> changes on an item within the tree.
    /// You cannot update FluentTreeItem properties.
    /// </summary>
    [Parameter]
    public EventCallback<FluentTreeItem> OnExpandedChanged { get; set; }

    /// <summary>
    /// Called whenever the selected item changes.
    /// You cannot update FluentTreeItem properties.
    /// </summary>
    [Parameter]
    public EventCallback<FluentTreeItem> OnSelectedChanged { get; set; }

    /// <summary />
    [ExcludeFromCodeCoverage(Justification = "JavaScript is not covered by unit tests")]
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && SelectionMode != TreeSelectionMode.Single)
        {
            // Import the JavaScript module
            if (!await JSModule.TryImportJavaScriptModuleAsync(JAVASCRIPT_FILE))
            {
                return;
            }

            // Call a function from the JavaScript module
            await JSModule.ObjectReference.InvokeVoidAsync("Microsoft.FluentUI.Blazor.TreeView.Initialize", Id, true);
        }
    }
}
