// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Specifies the grid outline shape used by a polar chart.
/// </summary>
public enum PolarChartShape
{
    /// <summary>
    /// Renders the grid using concentric circles.
    /// </summary>
    [Description("circle")]
    Circle,

    /// <summary>
    /// Renders the grid using concentric polygons.
    /// </summary>
    [Description("polygon")]
    Polygon,
}
