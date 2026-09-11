// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Specifies the angular rendering direction used by a polar chart.
/// </summary>
public enum PolarChartDirection
{
    /// <summary>
    /// Angles increase in the clockwise direction.
    /// </summary>
    [Description("clockwise")]
    Clockwise,

    /// <summary>
    /// Angles increase in the counterclockwise direction.
    /// </summary>
    [Description("counterclockwise")]
    Counterclockwise,
}
