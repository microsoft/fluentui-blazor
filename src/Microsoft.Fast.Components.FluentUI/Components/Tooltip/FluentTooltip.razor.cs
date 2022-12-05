using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentTooltip : FluentComponentBase
{
    /// <summary>
    /// Gets or sets if the tooltip is visible
    /// </summary>
    [Parameter]
    public bool Visible { get; set; }

    /// <summary>
    /// Gets or sets the anchor
    /// </summary>
    [Parameter]
    public string? Anchor { get; set; }

    /// <summary>
    /// Gets or sets the delay (in miliseconds)
    /// </summary>
    [Parameter]
    public int? Delay { get; set; } = 300;

    /// <summary>
    /// Gets or sets the tooltip's position. See <see cref="FluentUI.TooltipPosition"/>
    /// </summary>
    [Parameter]
    public TooltipPosition? Position { get; set; }

    /// <summary>
    /// Controls when the tooltip updates its position, default is anchor which only updates when
    /// the anchor is resized.  auto will update on scroll/resize events.
    /// Corresponds to anchored-region auto-update-mode.
    /// </summary>
    [Parameter]
    public AutoUpdateMode? AutoUpdateMode { get; set; }

    /// <summary>
    /// Gets or sets wether the horizontal viewport is locked
    /// </summary>
    [Parameter]
    public bool HorizontalViewportLock { get; set; }

    /// <summary>
    /// Gets or sets wether the vertical viewport is locked
    /// </summary>
    [Parameter]
    public bool VerticalViewportLock { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}