// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Represents a line series point rendered at a specific x-axis category in a vertical stacked bar chart.
/// </summary>
public sealed record VerticalStackedBarChartLineDataPoint
{
    /// <summary>
    /// Gets the y-axis numeric value rendered for the line point.
    /// </summary>
    [JsonPropertyName("y")]
    public double Y { get; init; }

    /// <summary>
    /// Gets the optional text that overrides the numeric value displayed in the tooltip.
    /// </summary>
    [JsonPropertyName("yAxisCalloutData")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? YAxisCalloutData { get; init; }

    /// <summary>
    /// Gets the legend text shown for the line point.
    /// </summary>
    [JsonPropertyName("legend")]
    public string Legend { get; init; } = string.Empty;

    /// <summary>
    /// Gets the optional color used to render the line point.
    /// Accepts an HTML hex color string (e.g. <c>#0099BC</c>) or a CSS variable.
    /// </summary>
    [JsonPropertyName("color")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Color { get; init; }

    /// <summary>
    /// Gets a value indicating whether the line point is rendered against the secondary y-axis.
    /// </summary>
    [JsonPropertyName("useSecondaryYScale")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? UseSecondaryYScale { get; init; }
}
