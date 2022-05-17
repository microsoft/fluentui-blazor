using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentSkeleton : FluentComponentBase
{
    /// <summary>
    /// Gets or sets the shape of the skeleton. See <see cref="FluentUI.Shape"/>
    /// </summary>
    [Parameter]
    public Shape? Shape { get; set; }

    /// <summary>
    /// Gets or sets if the skeleton is shimmered
    /// </summary>
    [Parameter]
    public bool? Shimmer { get; set; }

    /// <summary>
    /// Gets or sets the skeleton pattern
    /// </summary>
    [Parameter]
    public string? Pattern { get; set; }
}