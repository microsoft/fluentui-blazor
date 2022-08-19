using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentTreeView : FluentComponentBase
{
    private readonly Dictionary<string, FluentTreeItem> items = new();

    /// <summary>
    /// Gets or sets whether the tree should render nodes under collapsed items
    /// Defaults to 'false'
    /// </summary>
    [Parameter]
    public bool RenderCollapsedNodes { get; set; }

    /// <summary>
    /// Gets or sets the currently selected tree item
    /// </summary>
    [Parameter]
    public FluentTreeItem CurrentSelected { get; set; } = default!;

    [Parameter]
    public EventCallback<FluentTreeItem> CurrentSelectedChanged { get; set; }

    [Parameter]
    public EventCallback<FluentTreeItem> OnSelectedChange { get; set; }

    [Parameter]
    public EventCallback<FluentTreeItem> OnExpandedChange { get; set; }

    public async Task HandleOnSelectedChanged(TreeChangeEventArgs args)
    {
        string? treeItemId = args.AffectedId;
        if (items.TryGetValue(treeItemId!, out FluentTreeItem? treeItem))
        {
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
        items.Add(item.TreeItemId, item);
    }

    internal void Unregister(FluentTreeItem item)
    {
        items.Remove(item.TreeItemId);
    }
}