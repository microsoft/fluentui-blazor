using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentSkeleton : FluentComponentBase
{
    /// <summary>
    /// Indicates the Skeleton should have a filled style.
    /// </summary>
    [Parameter]
    public string? Fill { get; set; }

    /// <summary>
    /// Gets or sets the shape of the skeleton. See <see cref="FluentUI.SkeletonShape"/>
    /// </summary>
    [Parameter]
    public SkeletonShape? Shape { get; set; }

    /// <summary>
    /// Gets or sets the skeleton pattern
    /// </summary>
    [Parameter]
    public string? Pattern { get; set; }

    /// <summary>
    /// Gets or sets if the skeleton is shimmered
    /// </summary>
    [Parameter]
    public bool? Shimmer { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}