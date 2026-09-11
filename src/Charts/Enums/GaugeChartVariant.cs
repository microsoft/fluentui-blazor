// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Specifies the visual variant used by the gauge chart.
/// </summary>
public enum GaugeChartVariant
{
    /// <summary>
    /// Renders the gauge as a single tracked segment.
    /// </summary>
    [Description("single-segment")]
    SingleSegment,

    /// <summary>
    /// Renders the gauge as multiple segments.
    /// </summary>
    [Description("multiple-segments")]
    MultipleSegments,
}
