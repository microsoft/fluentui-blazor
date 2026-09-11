// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Represents a single bar in a vertical bar chart.
/// </summary>
public sealed record VerticalBarChartDataPoint
{
    /// <summary>
    /// Gets the x-axis value rendered for this bar.
    /// Accepts a numeric value, a date/time value, or a category label.
    /// </summary>
    [JsonPropertyName("x")]
    public required ChartAxisValue X { get; init; }

    /// <summary>
    /// Gets the y-axis numeric value rendered for this bar.
    /// </summary>
    [JsonPropertyName("y")]
    public double Y { get; init; }

    /// <summary>
    /// Gets the optional legend text shown for the bar.
    /// </summary>
    [JsonPropertyName("legend")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Legend { get; init; }

    /// <summary>
    /// Gets the optional text or date that overrides the x-axis value displayed in the tooltip.
    /// </summary>
    [JsonPropertyName("xAxisCalloutData")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? XAxisCalloutData { get; init; }

    /// <summary>
    /// Gets the optional text that overrides the numeric value displayed in the tooltip.
    /// </summary>
    [JsonPropertyName("yAxisCalloutData")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? YAxisCalloutData { get; init; }

    /// <summary>
    /// Gets the solid color used to render the bar.
    /// Use <see cref="DataVizPalette.Custom"/> and set <see cref="CustomColor"/> to supply
    /// an exact hex or CSS color string. If not provided, the component falls back to its
    /// default palette.
    /// </summary>
    [JsonIgnore]
    public DataVizPalette? Color { get; init; }

    /// <summary>
    /// Custom color value used when <see cref="Color"/> is <see cref="DataVizPalette.Custom"/>.
    /// Accepts an HTML hex color string (e.g. <c>#0099BC</c>) or a CSS variable.
    /// </summary>
    [JsonIgnore]
    public string? CustomColor { get; init; }

    /// <summary>
    /// Gets the serialized color value sent to the web component.
    /// Returns <see cref="CustomColor"/> when <see cref="Color"/> is <see cref="DataVizPalette.Custom"/>,
    /// otherwise the palette token string, or <see langword="null"/> when no color is set.
    /// </summary>
    [JsonPropertyName("color")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SerializedColor => Color == DataVizPalette.Custom ? CustomColor : Color?.ToAttributeValue();

    /// <summary>
    /// Gets the optional two-color gradient used to render the bar.
    /// The array should contain exactly two color values: start and end.
    /// </summary>
    [JsonPropertyName("gradient")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? Gradient { get; init; }

    /// <summary>
    /// Gets the optional text rendered as the visible bar label instead of the formatted numeric value.
    /// </summary>
    [JsonPropertyName("barLabel")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? BarLabel { get; init; }

    /// <summary>
    /// Gets the optional line series point overlaid on this bar's x-axis category.
    /// </summary>
    [JsonPropertyName("lineData")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public VerticalBarChartLineDataPoint? LineData { get; init; }
}
