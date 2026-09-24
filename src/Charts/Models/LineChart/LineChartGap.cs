// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Represents a gap range between line chart data points.
/// </summary>
public sealed record LineChartGap
{
    /// <summary>
    /// Gets the zero-based index of the first point in the gap.
    /// </summary>
    [JsonPropertyName("startIndex")]
    public int StartIndex { get; init; }

    /// <summary>
    /// Gets the zero-based index of the last point in the gap.
    /// </summary>
    [JsonPropertyName("endIndex")]
    public int EndIndex { get; init; }
}
