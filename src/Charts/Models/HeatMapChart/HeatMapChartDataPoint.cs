// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Represents a single cell in a heat map chart series.
/// </summary>
public sealed record HeatMapChartDataPoint
{
    /// <summary>
    /// Gets the x-axis value rendered for this cell.
    /// Accepts a numeric value, date/time value, or string category label.
    /// </summary>
    [JsonPropertyName("x")]
    public ChartAxisValue X { get; init; }

    /// <summary>
    /// Gets the y-axis value rendered for this cell.
    /// Accepts a numeric value, date/time value, or string category label.
    /// </summary>
    [JsonPropertyName("y")]
    public ChartAxisValue Y { get; init; }

    /// <summary>
    /// Gets the numeric value used to determine the cell color.
    /// </summary>
    [JsonPropertyName("value")]
    public double Value { get; init; }

    /// <summary>
    /// Gets the optional text rendered inside the cell.
    /// </summary>
    [JsonPropertyName("rectText")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? RectText { get; init; }

    /// <summary>
    /// Gets the optional ratio shown in the tooltip.
    /// </summary>
    [JsonPropertyName("ratio")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double[]? Ratio { get; init; }

    /// <summary>
    /// Gets the optional descriptive message shown in the tooltip.
    /// </summary>
    [JsonPropertyName("descriptionMessage")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? DescriptionMessage { get; init; }

    /// <summary>
    /// Gets the optional accessibility data for the tooltip callout.
    /// </summary>
    [JsonPropertyName("callOutAccessibilityData")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public HeatMapAccessibilityData? CallOutAccessibilityData { get; init; }
}
