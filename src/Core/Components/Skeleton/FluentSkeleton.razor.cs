using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentSkeleton : FluentComponentBase
{
    protected string? ClassValue => new CssBuilder(Class)
        .Build();

    protected string? StyleValue => new StyleBuilder().AddStyle(Style)
        .AddStyle("width", Width, () => !string.IsNullOrWhiteSpace(Width))
        .AddStyle("height", Height, () => !string.IsNullOrWhiteSpace(Height))
        .Build();


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
    /// Gets or sets the width of the skeleton
    /// </summary>
    [Parameter]
    public string Width { get; set; } = "50px";

    /// <summary>
    /// Gets or sets the height of the skeleton
    /// </summary>
    [Parameter]
    public string Height { get; set; } = "50px";

    /// <summary>
    /// Gets or sets whether the skeleton is visible
    /// </summary>
    [Parameter]
    public bool Visible { get; set; } = true;

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}