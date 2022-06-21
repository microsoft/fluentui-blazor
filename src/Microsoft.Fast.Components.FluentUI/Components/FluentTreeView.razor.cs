using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentTreeView : FluentComponentBase
{
    /// <summary>
    /// Gets or sets whether the tree should render nodes under collapsed items
    /// Defaults to 'false'
    /// </summary>
    [Parameter]
    public bool RenderCollapsedNodes { get; set; } = false;

    /// <summary>
    /// Gets or sets the currently selected tree item
    /// </summary>
    [Parameter]
    public ElementReference? CurrentSelected { get; set; }

    [Parameter]
    public EventCallback<ElementReference?> CurrentSelectedChanged { get; set; }

    private async Task OnSelectedChange(TreeChangeEventArgs args)
    {
        await CurrentSelectedChanged.InvokeAsync(args.AffectedItem);
    }

    /// <summary>
    /// Gets or sets the currently selected tree item
    /// </summary>
    [Parameter]
    public ElementReference? CurrentExpanded { get; set; }

    [Parameter]
    public EventCallback<ElementReference?> CurrentExpandedChanged { get; set; }

    private async Task OnExpandedChange(TreeChangeEventArgs args)
    {
        await CurrentExpandedChanged.InvokeAsync(args.AffectedItem);
    }
}