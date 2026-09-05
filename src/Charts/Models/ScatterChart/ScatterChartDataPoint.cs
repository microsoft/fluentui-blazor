// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Represents a single data point in a scatter chart series.
/// </summary>
public sealed record ScatterChartDataPoint
{
    /// <summary>
    /// Gets the x-axis value rendered for this point.
    /// Accepts a numeric value, date/time value, or string category label.
    /// </summary>
    [JsonPropertyName("x")]
    public ChartAxisValue X { get; init; }

    /// <summary>
    /// Gets the y-axis numeric value rendered for this point.
    /// </summary>
    [JsonPropertyName("y")]
    public double Y { get; init; }

    /// <summary>
    /// Gets the optional marker size in pixels.
    /// </summary>
    [JsonPropertyName("markerSize")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? MarkerSize { get; init; }
}
