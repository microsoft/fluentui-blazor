// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents a tree view component.
/// </summary>
public partial class FluentTreeView : FluentComponentBase
{
    internal ConcurrentDictionary<string, FluentTreeItem> InternalItems { get; } = new(StringComparer.Ordinal);

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentTreeView"/> class.
    /// </summary>
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(TreeItemChangedEventArgs))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(TreeItemToggleEventArgs))]
    public FluentTreeView()
    {
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
}
