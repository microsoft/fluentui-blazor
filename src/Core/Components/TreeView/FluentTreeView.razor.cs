// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
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

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentTreeView"/> class.
    /// </summary>
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(TreeItemChangedEventArgs))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(TreeItemToggleEventArgs))]
    public FluentTreeView()
    {
        Id = Identifier.NewId();
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
    /// Gets or sets whether the tree view element is not highlighted to indicate that it is selected.
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
    /// Gets or sets the selected item id.
    /// </summary>
    [Parameter]
    public string? SelectedId { get; set; }

    /// <summary>
    /// Called whenever the selected item changes.
    /// </summary>
    [Parameter]
    public EventCallback<string?> SelectedIdChanged { get; set; }

    /// <summary>
    /// Gets or sets the selected <see cref="FluentTreeItem" /> item.
    /// </summary>
    [Parameter]
    public FluentTreeItem? CurrentSelected { get; set; }

    /// <summary>
    /// Called whenever the selected <see cref="FluentTreeItem" /> changes.
    /// </summary>
    [Parameter]
    public EventCallback<FluentTreeItem?> CurrentSelectedChanged { get; set; }

    /// <summary>
    /// Gets or sets the selected <see cref="ITreeViewItem" /> item.
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
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && SelectionMode != TreeSelectionMode.Single)
        {
            // Import the JavaScript module
            var jsModule = await JSModule.ImportJavaScriptModuleAsync(JAVASCRIPT_FILE);

            // Call a function from the JavaScript module
            await jsModule.InvokeVoidAsync("Microsoft.FluentUI.Blazor.TreeView.Initialize", Id, true);
        }
    }
}
