namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Describes the position at which a <see cref="FluentTooltip"/> is shown.
/// </summary>
public enum TooltipPosition
{
    /// <summary>
    /// The tooltip is positioned above the anchor element.
    /// </summary>
    Top,

    /// <summary>
    /// The tooltip is positioned below the anchor element.
    /// </summary>
    Bottom,

    /// <summary>
    /// The tooltip is positioned to the left of the anchor element.
    /// </summary>
    Left,

    /// <summary>
    /// The tooltip is positioned to the right of the anchor element.
    /// </summary>
    Right,

    /// <summary>
    /// The tooltip is positioned at the start of the anchor element.
    /// </summary>
    Start,

    /// <summary>
    /// The tooltip is positioned at the end of the anchor element.
    /// </summary>
    End
}
