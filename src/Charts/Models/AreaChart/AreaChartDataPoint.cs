// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Represents a single data point in an area chart series.
/// </summary>
public sealed record AreaChartDataPoint
{
    /// <summary>
    /// Gets the x-axis value rendered for this point.
    /// Accepts either a numeric value or a date/time value.
    /// </summary>
    [JsonPropertyName("x")]
    public ChartAxisValue X { get; init; }

    /// <summary>
    /// Gets the y-axis numeric value rendered for this point.
    /// </summary>
    [JsonPropertyName("y")]
    public double Y { get; init; }

    /// <summary>
    /// Gets optional accessibility data for the x-axis callout item.
    /// </summary>
    [JsonPropertyName("xAxisCalloutAccessibilityData")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public CalloutAccessibilityData? XAxisCalloutAccessibilityData { get; init; }

    /// <summary>
    /// Gets optional accessibility data for the series callout item.
    /// </summary>
    [JsonPropertyName("callOutAccessibilityData")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public CalloutAccessibilityData? CallOutAccessibilityData { get; init; }
}
