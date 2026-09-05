// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Represents a single data point in a line chart series.
/// </summary>
public sealed record LineChartDataPoint
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
    /// Gets the optional x-axis callout text displayed in the tooltip.
    /// </summary>
    [JsonPropertyName("xAxisCalloutData")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? XAxisCalloutData { get; init; }

    /// <summary>
    /// Gets the optional y-axis callout text displayed in the tooltip.
    /// </summary>
    [JsonPropertyName("yAxisCalloutData")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? YAxisCalloutData { get; init; }

    /// <summary>
    /// Gets whether the tooltip callout should be hidden for this point.
    /// </summary>
    [JsonPropertyName("hideCallout")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? HideCallout { get; init; }
}
