// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

#pragma warning disable MA0048 // File name must match type name

/// <summary>
/// Represents a single data point in a donut chart.
/// </summary>
public sealed record DonutDataPoint
{
    /// <summary>
    /// Gets the legend text shown for the donut segment.
    /// </summary>
    [JsonPropertyName("legend")]
    public string Legend { get; init; } = string.Empty;

    /// <summary>
    /// Gets the numeric value of the donut segment.
    /// </summary>
    [JsonPropertyName("data")]
    public double Data { get; init; }

    /// <summary>
    /// Gets the color used to render the donut segment and legend.
    /// If not provided, the web component falls back to its default palette.
    /// </summary>
    [JsonPropertyName("color")]
    public string? Color { get; init; }

    /// <summary>
    /// Gets optional callout data for the x-axis portion of the tooltip.
    /// </summary>
    [JsonPropertyName("xAxisCalloutData")]
    public string? XAxisCalloutData { get; init; }

    /// <summary>
    /// Gets optional callout data for the y-axis portion of the tooltip.
    /// If not provided, the component may fall back to the numeric data value.
    /// </summary>
    [JsonPropertyName("yAxisCalloutData")]
    public string? YAxisCalloutData { get; init; }
}
#pragma warning restore MA0048 // File name must match type name
