// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Specifies the rendered series type for a polar chart series.
/// </summary>
public enum PolarSeriesType
{
    /// <summary>
    /// Renders the series as a filled polar area.
    /// </summary>
    [Description("areapolar")]
    AreaPolar,

    /// <summary>
    /// Renders the series as a polar line.
    /// </summary>
    [Description("linepolar")]
    LinePolar,

    /// <summary>
    /// Renders the series as polar scatter markers.
    /// </summary>
    [Description("scatterpolar")]
    ScatterPolar,
}
