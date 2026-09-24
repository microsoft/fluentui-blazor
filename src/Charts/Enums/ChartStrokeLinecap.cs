// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Specifies the SVG line cap style applied to an overlaid line stroke.
/// </summary>
public enum ChartStrokeLinecap
{
    /// <summary>
    /// The stroke ends abruptly at the edge of the last point.
    /// </summary>
    [Description("butt")]
    Butt,

    /// <summary>
    /// The stroke ends with a rounded cap centered on the last point.
    /// </summary>
    [Description("round")]
    Round,

    /// <summary>
    /// The stroke ends with a square cap that extends beyond the last point.
    /// </summary>
    [Description("square")]
    Square,

    /// <summary>
    /// The stroke cap is inherited from the parent element.
    /// </summary>
    [Description("inherit")]
    Inherit,
}
