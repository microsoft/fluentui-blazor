using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentTreeView : FluentComponentBase
{
    [Parameter]
    public bool? RenderCollapsedNodes { get; set; }
}