using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentTreeView : FluentComponentBase
{
    private readonly Dictionary<string, FluentTreeItem> items = new();

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
    public FluentTreeItem CurrentSelected { get; set; } = default!;

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public EventCallback<FluentTreeItem> CurrentSelectedChanged { get; set; }

    [Parameter]
    public EventCallback<FluentTreeItem> OnSelectedChange { get; set; }

    [Parameter]
    public EventCallback<FluentTreeItem> OnExpandedChange { get; set; }

    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(TreeChangeEventArgs))]

    public FluentTreeView()
    {
        
    }

    public async Task HandleOnSelectedChanged(TreeChangeEventArgs args)
    {
        string? treeItemId = args.AffectedId;
        if (items.TryGetValue(treeItemId!, out FluentTreeItem? treeItem))
        {
            treeItem.SetSelected(args.Selected ?? false);
            treeItem.SetExpanded(args.Expanded ?? false);

            await CurrentSelectedChanged.InvokeAsync(treeItem);
            await OnSelectedChange.InvokeAsync(treeItem);
        }
    }

    public async Task HandleOnExpandedChanged(TreeChangeEventArgs args)
    {
        string? treeItemId = args.AffectedId;
        if (items.TryGetValue(treeItemId!, out FluentTreeItem? treeItem))
        {
            await OnExpandedChange.InvokeAsync(treeItem);
        }
    }

    internal void Register(FluentTreeItem item)
    {
        items.Add(item.Id, item);
    }

    internal void Unregister(FluentTreeItem item)
    {
        items.Remove(item.Id);
    }
}