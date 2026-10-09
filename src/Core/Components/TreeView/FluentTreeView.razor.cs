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

    internal ConcurrentDictionary<string, FluentTreeItem> InternalItems { get; } = new(StringComparer.Ordinal);

    internal TreeViewThreeStateSelection RecursiveSelection { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentTreeView"/> class.
    /// </summary>
    public FluentTreeView(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
        RecursiveSelection = new TreeViewThreeStateSelection(this);
    }

    /// <summary/>
    protected string? ClassValue => DefaultClassBuilder
        .Build();

    /// <summary/>
    protected string? StyleValue => DefaultStyleBuilder
        .Build();

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
    /// MultipleRecursive uses `TreeViewThreeStateSelection` class
    /// to select or deselect descendants and update ancestor states. All descendant data must be available.
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
    /// Gets or sets a function that determines each checkbox's state:
    /// <see langword="true"/> (checked), <see langword="false"/> (unchecked),
    /// or <see langword="null"/> (indeterminate).
    /// Only applies when <see cref="Items"/> is used with <see cref="TreeSelectionMode.Multiple"/>.
    /// When omitted, checkbox states are determined by <see cref="SelectedItems"/>.
    /// Ignored in <see cref="TreeSelectionMode.MultipleRecursive"/>, which calculates checkbox states automatically.
    /// </summary>
    /// <remarks>
    /// The function is evaluated during rendering and must be a projection of <see cref="SelectedItems"/>.
    /// It must return <see langword="true"/> only for items contained in <see cref="SelectedItems"/>;
    /// <see langword="null"/> may be used for unselected items whose descendants are partially selected.
    /// User interactions are calculated from membership in <see cref="SelectedItems"/>, not from this displayed state.
    /// The function must not modify the selection.
    /// It does not select descendants automatically or load missing items.
    /// Handle <see cref="SelectedItemsChanged"/> to update the selection used by the function.
    /// </remarks>
    [Parameter]
    public Func<ITreeViewItem, bool?>? CheckState { get; set; }

    /// <summary>
    /// Gets or sets the multi-selected <see cref="ITreeViewItem" /> items.
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

    internal async Task OnSelectedItemsChangedAsync(IEnumerable<ITreeViewItem> selectedItems)
    {
        // If SelectionMode is MultipleRecursive
        if (RecursiveSelection.IsRecursive)
        {
            var selection = new TreeViewThreeStateSelection
            {
                Items = Items,
                SelectedItems = SelectedItems,
            };

            // Applies a checkbox selection change recursively and recalculates the selection of ancestors.
            selection.OnSelectedItemsChanged(selectedItems);

            if (SelectedItemsChanged.HasDelegate)
            {
                await SelectedItemsChanged.InvokeAsync(selection.SelectedItems);
            }
        }

        // Raise the SelectedItemsChanged event
        else
        {
            if (SelectedItemsChanged.HasDelegate)
            {
                await SelectedItemsChanged.InvokeAsync(selectedItems);
            }
        }
    }

    /// <summary />
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if (RecursiveSelection.IsRecursive)
        {
            RecursiveSelection.Items = Items;
            RecursiveSelection.SelectedItems = SelectedItems;
        }
    }

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

        if (RecursiveSelection.HasCheckState)
        {
            if (!await JSModule.TryImportJavaScriptModuleAsync(JAVASCRIPT_FILE))
            {
                return;
            }

            await JSModule.ObjectReference.InvokeVoidAsync("Microsoft.FluentUI.Blazor.TreeView.UpdateCheckStates", Id);
        }
    }
}
