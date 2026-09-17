// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Describes the position at which a <see cref="FluentTooltip"/> is shown.
/// </summary>
public enum TooltipPosition
{
    /// <summary>
    /// The tooltip is positioned above the anchor element.
    /// </summary>
    [Obsolete("This value is obsolete. Use the Positioning.Above value instead.")]
    Top,

    /// <summary>
    /// The tooltip is positioned below the anchor element.
    /// </summary>
    [Obsolete("This value is obsolete. Use the Positioning.Below value instead.")]
    Bottom,

    /// <summary>
    /// The tooltip is positioned to the left of the anchor element.
    /// </summary>
    [Obsolete("This value is obsolete. Use the Positioning.Before value instead.")]
    Left,

    /// <summary>
    /// The tooltip is positioned to the right of the anchor element.
    /// </summary>
    [Obsolete("This value is obsolete. Use the Positioning.After value instead.")]
    Right,

    /// <summary>
    /// The tooltip is positioned at the start of the anchor element.
    /// </summary>
    [Obsolete("This value is obsolete. Use the Positioning.AboveStart value instead.")]
    Start,

    /// <summary>
    /// The tooltip is positioned at the end of the anchor element.
    /// </summary>
    [Obsolete("This value is obsolete. Use the Positioning.AboveEnd value instead.")]
    End,
}
