using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentSkeleton : FluentComponentBase
{
    protected string? ClassValue => new CssBuilder(Class)
        .Build();

    protected string? StyleValue => new StyleBuilder(Style)
        .AddStyle("width", Width, () => !string.IsNullOrWhiteSpace(Width))
        .AddStyle("height", Height, () => !string.IsNullOrWhiteSpace(Height))
        .Build();

    /// <summary>
    /// Indicates the Skeleton should have a filled style.
    /// </summary>
    [Parameter]
    public string? Fill { get; set; }

    /// <summary>
    /// Gets or sets the shape of the skeleton. See <see cref="AspNetCore.Components.SkeletonShape"/>
    /// </summary>
    [Parameter]
    public SkeletonShape? Shape { get; set; } = SkeletonShape.Rect;

    /// <summary>
    /// Gets or sets the skeleton pattern.
    /// </summary>
    [Parameter]
    public string? Pattern { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the skeleton is shimmered.
    /// </summary>
    [Parameter]
    public bool? Shimmer { get; set; }

    /// <summary>
    /// Gets or sets the width of the skeleton.
    /// </summary>
    [Parameter]
    public string Width { get; set; } = "50px";

    /// <summary>
    /// Gets or sets the height of the skeleton.
    /// </summary>
    [Parameter]
    public string Height { get; set; } = "50px";

    /// <summary>
    /// Gets or sets a value indicating whether the skeleton is visible.
    /// </summary>
    [Parameter]
    public bool Visible { get; set; } = true;

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}
