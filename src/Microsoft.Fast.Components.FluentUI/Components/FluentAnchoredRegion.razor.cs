using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;
public partial class FluentAnchoredRegion
{
    /// <summary>
    /// The html id of the HTMLElement used as the anchor around which the positioning region is placed. This must be set for the component's positioning logic to be active.
    /// </summary>
    [Parameter]
    public string? Anchor { get; set; }

    /// <summary>
    /// The ID of the HTMLElement to be used as the viewport used to determine available layout space around the anchor element. If unset the parent element of the anchored region is used.
    /// </summary>
    [Parameter]
    public string? Viewport { get; set; }

    /// <summary>
    /// Default is 'uncontrolled'.
    /// </summary>
    [Parameter]
    public Positioning? HorizontalPositioningMode { get; set; } = Positioning.Uncontrolled;

    /// <summary>
    /// Default is 'unset'
    /// </summary>
    [Parameter]
    public HorizontalPosition? HorizontalDefaultPosition { get; set; } = HorizontalPosition.Unset;

    /// <summary>
    /// Indicates whether the region should overlap the anchor on the horizontal axis. Default is false which places the region adjacent to the anchor element.
    /// </summary>
    [Parameter]
    public bool HorizontalInset { get; set; }

    /// <summary>
    /// Numeric value that defines how small in pixels the region must be to the edge of the viewport to switch to the opposite side of the anchor. The component favors the default position until this value is crossed. When there is not enough space on either side or the value is unset the side with the most space is chosen.
    /// </summary>
    [Parameter]
    public int HorizontalThreshold { get; set; }

    /// <summary>
    /// Default is "Content"
    /// </summary>
    [Parameter]
    public Scaling? HorizontalScaling { get; set; } = Scaling.Content;

    /// <summary>
    /// Default is "Uncontrolled"
    /// </summary>
    [Parameter]
    public Positioning? VerticalPositioningMode { get; set; } = Positioning.Uncontrolled;

    /// <summary>
    /// Default is "Unset"
    /// </summary>
    [Parameter]
    public VerticalPosition? VerticalDefaultPosition { get; set; } = VerticalPosition.Unset;

    /// <summary>
    /// Indicates whether the region should overlap the anchor on the vertical axis. Default is false which places the region adjacent to the anchor element.
    /// </summary>
    [Parameter]
    public bool VerticalInset { get; set; }

    /// <summary>
    /// Numeric value that defines how small the region must be to the edge of the viewport to switch to the opposite side of the anchor. The component favors the default position until this value is crossed. When there is not enough space on either side or the value is unset the side with the most space is chosen.
    /// </summary>
    [Parameter]
    public int VerticalThreshold { get; set; }

    /// <summary>
    /// Default is "Content"
    /// </summary>
    [Parameter]
    public Scaling? VerticalScaling { get; set; } = Scaling.Content;

    /// <summary>
    /// Default is "Anchor". In 'anchor' mode only anchor resizes and attribute changes will provoke an update. In 'auto' mode the component also updates because of - any scroll event on the document, window resizes and viewport resizes
    /// </summary>
    [Parameter]
    public UpdateMode? AutoUpdateMode { get; set; } = UpdateMode.Anchor;
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object>? AdditionalAttributes { get; set; }
}