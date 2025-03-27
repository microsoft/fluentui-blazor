// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentMenu
{
    /// <summary>
    /// Use IMenuService to create the menu, if this service was injected.
    /// This value must be defined before the component is rendered (you can't change it during the component lifecycle).
    /// Default, true.
    /// </summary>
    [Parameter]
    [Obsolete("This property is not supported anymore and will be removed in a future release.")]
    public bool UseMenuService { get; set; } = true;

    /// <summary>
    /// Gets or sets the identifier of the source component clickable by the end user.
    /// </summary>
    [Parameter]
    [Obsolete("This property is not supported anymore and will be removed in a future release.")]
    public string Anchor { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the Menu status.
    /// </summary>
    [Parameter]
    [Obsolete("This property is not supported anymore and will be removed in a future release.")]
    public bool Open { get; set; }

    /// <summary>
    /// Gets or sets the horizontal menu position.
    /// </summary>
    [Parameter]
    [Obsolete("This property is not supported anymore and will be removed in a future release.")]
    public HorizontalPosition HorizontalPosition { get; set; } = HorizontalPosition.Unset;

    /// <summary>
    /// Gets or sets a value indicating whether the region overlaps the anchor on the horizontal axis.
    /// Default is false which places the region adjacent to the anchor element.
    /// </summary>
    [Parameter]
    [Obsolete("This property is not supported anymore and will be removed in a future release.")]
    public bool HorizontalInset { get; set; } = true;

    /// <summary>
    /// Gets or sets the vertical menu position.
    /// </summary>
    [Parameter]
    [Obsolete("This property is not supported anymore and will be removed in a future release.")]
    public VerticalPosition VerticalPosition { get; set; } = VerticalPosition.Bottom;

    /// <summary>
    /// Gets or sets a value indicating whether the region overlaps the anchor on the vertical axis.
    /// </summary>
    [Parameter]
    [Obsolete("This property is not supported anymore and will be removed in a future release.")]
    public bool VerticalInset { get; set; } = false;

    /// <summary>
    /// Gets or sets the width of this menu.
    /// </summary>
    [Parameter]
    [Obsolete("This property is not supported anymore and will be removed in a future release.")]
    public string? Width { get; set; }

    /// <summary>
    /// Raised when the <see cref="Open"/> property changed.
    /// </summary>
    [Parameter]
    [Obsolete("This property is not supported anymore and will be removed in a future release.")]
    public EventCallback<bool> OpenChanged { get; set; }

    /// <summary>
    /// Draw the menu below the component clicked (true) or
    /// using the mouse coordinates (false).
    /// </summary>
    [Parameter]
    [Obsolete("This property is not supported anymore and will be removed in a future release.")]
    public bool Anchored { get; set; } = true;

    /// <summary>
    /// Gets or sets how short the space allocated to the default position has to be before the tallest area is selected for layout.
    /// </summary>
    [Parameter]
    [Obsolete("This property is not supported anymore and will be removed in a future release.")]
    public int VerticalThreshold { get; set; } = 0;

    /// <summary>
    /// Gets or sets how narrow the space allocated to the default position has to be before the widest area is selected for layout.
    /// </summary>
    [Parameter]
    [Obsolete("This property is not supported anymore and will be removed in a future release.")]
    public int HorizontalThreshold { get; set; } = 200;

    /// <summary>
    /// Gets or sets the Horizontal viewport lock.
    /// </summary>
    [Parameter]
    [Obsolete("This property is not supported anymore and will be removed in a future release.")]
    public bool HorizontalViewportLock { get; set; }

    /// <summary>
    /// Gets or sets the horizontal scaling mode.
    /// </summary>
    [Parameter]
    [Obsolete("This property is not supported anymore and will be removed in a future release.")]
    public AxisScalingMode? HorizontalScaling { get; set; }
}
