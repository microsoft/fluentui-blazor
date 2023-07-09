using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentTreeView : FluentComponentBase
{
    /// <summary>
    /// Gets or sets whether the tree should render nodes under collapsed items
    /// Defaults to false
    /// </summary>
    [Parameter]
    public bool RenderCollapsedNodes { get; set; }

    /// <summary>
    /// Gets or sets the currently selected tree item
    /// </summary>
    [Parameter]
    public FluentTreeItem? CurrentSelected { get; set; } = default!;

    /// <summary>
    /// Called when <see cref="CurrentSelected"/> changes.
    /// </summary>
    [Parameter]
    public EventCallback<FluentTreeItem?> CurrentSelectedChanged { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Called whenever <see cref="FluentTreeItem.Selected"/> changes on an
    /// item within the tree.
    /// </summary>
    [Parameter]
    public EventCallback<FluentTreeItem> OnSelectedChange { get; set; }

    /// <summary>
    /// Called whenever <see cref="FluentTreeItem.Expanded"/> changes on an
    /// item within the tree.
    /// </summary>
    [Parameter]
    public EventCallback<FluentTreeItem> OnExpandedChange { get; set; }

    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(TreeChangeEventArgs))]
    public FluentTreeView()
    {
    }

    internal async Task ItemExpandedChangeAsync(FluentTreeItem item)
    {
        if (OnExpandedChange.HasDelegate)
        {
            await OnExpandedChange.InvokeAsync(item);
        }
    }

    internal async Task ItemSelectedChangeAsync(FluentTreeItem item)
    {
        if (OnSelectedChange.HasDelegate)
        {
            await OnSelectedChange.InvokeAsync(item);
        }
    }
}