using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentTreeView : FluentComponentBase
{
    /// <summary>
    /// Gets or sets wether to render collapsed nodes
    /// </summary>
    [Parameter]
    public bool? RenderCollapsedNodes { get; set; }
}