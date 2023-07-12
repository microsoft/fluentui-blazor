using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace Microsoft.Fast.Components.FluentUI;
public partial class FluentAnchoredRegion : FluentComponentBase
{
    protected string? ClassValue => new CssBuilder(Class)
        .Build();

    protected string? StyleValue => new StyleBuilder()
        .AddStyle(Style)
        .AddStyle("z-index: 9999")
        .AddStyle("background-color: var(--neutral-layer-floating)")
        .AddStyle("box-shadow", "var(--elevation-shadow-tooltip)", () => Shadow == ElevationShadow.Tooltip)
        .AddStyle("box-shadow", "var(--elevation-shadow-flyout)", () => Shadow == ElevationShadow.Flyout)
        .AddStyle("box-shadow", "var(--elevation-shadow-dialog)", () => Shadow == ElevationShadow.Dialog)
        .Build();

    /// <summary>
    /// The HTML ID of the anchor element this region is positioned relative to
    /// This must be set for the component positioning logic to be active.
    /// </summary>
    [Parameter]
    public string? Anchor { get; set; }

    /// <summary>
    /// The HTML ID of the viewport element this region is positioned relative to
    /// If unset the parent element of the anchored region is used.
    /// </summary>
    [Parameter]
    public string? Viewport { get; set; }

    /// <summary>
    /// Sets what logic the component uses to determine horizontal placement.
    /// Locktodefault forces the default position
    /// Dynamic decides placement based on available space
    /// Uncontrolled (default) does not control placement on the horizontal axis
    /// See <seealso cref="AxisPositioningMode"/>
    /// </summary>
    [Parameter]
    public AxisPositioningMode? HorizontalPositioningMode { get; set; } = AxisPositioningMode.Dynamic;

    /// <summary>
    /// The default horizontal position of the region relative to the anchor element
    /// Default is unset. See <seealso cref="HorizontalPosition"/>
    /// </summary>
    [Parameter]
    public HorizontalPosition? HorizontalDefaultPosition { get; set; } = HorizontalPosition.Unset;

    /// <summary>
    /// Whether the region remains in the viewport (ie. detaches from the anchor) on the horizontal axis
    /// </summary>
    [Parameter]
    public bool HorizontalViewportLock { get; set; }

    /// <summary>
    /// Whether the region overlaps the anchor on the horizontal axis. Default is false which places the region adjacent to the anchor element.
    /// </summary>
    [Parameter]
    public bool HorizontalInset { get; set; }

    /// <summary>
    /// How narrow the space allocated to the default position has to be before the widest area
    /// is selected for layout
    /// </summary>
    [Parameter]
    public int HorizontalThreshold { get; set; }

    /// <summary>
    /// Defines how the width of the region is calculated
    /// Default is "Content". See <seealso cref="AxisScalingMode"/>
    /// </summary>
    [Parameter]
    public AxisScalingMode? HorizontalScaling { get; set; } = AxisScalingMode.Content;

    /// <summary>
    /// Sets what logic the component uses to determine vertical placement.
    /// Locktodefault forces the default position
    /// Dynamic decides placement based on available space
    /// Uncontrolled (default) does not control placement on the vertical axis
    /// See <seealso cref="AxisPositioningMode"/>
    /// </summary>
    [Parameter]
    public AxisPositioningMode? VerticalPositioningMode { get; set; } = AxisPositioningMode.Dynamic;

    /// <summary>
    /// The default vertical position of the region relative to the anchor element
    /// Default is "Unset".See <seealso cref="VerticalPosition"/>
    /// </summary>
    [Parameter]
    public VerticalPosition? VerticalDefaultPosition { get; set; } = VerticalPosition.Unset;

    /// <summary>
    /// Whether the region remains in the viewport (ie. detaches from the anchor) on the vertical axis
    /// </summary>
    [Parameter]
    public bool? VerticalViewportLock { get; set; }

    /// <summary>
    ///Whether the region overlaps the anchor on the vertical axis
    /// </summary>
    [Parameter]
    public bool VerticalInset { get; set; }

    /// <summary>
    /// How short the space allocated to the default position has to be before the tallest area
    /// is selected for layout
    /// </summary>
    [Parameter]
    public int VerticalThreshold { get; set; }

    /// <summary>
    /// Defines how the height of the region is calculated
    /// Default is "Content". See <seealso cref="AxisScalingMode"/>
    /// </summary>
    [Parameter]
    public AxisScalingMode? VerticalScaling { get; set; } = AxisScalingMode.Content;

    /// <summary>
    /// Whether the region is positioned using css "position: fixed".
    /// Otherwise the region uses "position: absolute".
    /// Fixed placement allows the region to break out of parent containers,
    /// </summary>
    [Parameter]
    public bool? FixedPlacement { get; set; }

    /// <summary>
    /// Defines what triggers the anchored region to revaluate positioning
    /// Default is "Anchor". In 'anchor' mode only anchor resizes and attribute changes will provoke an update. In 'auto' mode the component also updates because of - any scroll event on the document, window resizes and viewport resizes. See <seealso cref="AutoUpdateMode"/>
    /// </summary>
    [Parameter]
    public AutoUpdateMode? AutoUpdateMode { get; set; } = FluentUI.AutoUpdateMode.Auto;

    [Parameter]
    public ElevationShadow Shadow { get; set; } = ElevationShadow.None;

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}