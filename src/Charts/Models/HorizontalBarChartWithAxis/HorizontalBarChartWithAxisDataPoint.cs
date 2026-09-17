// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Represents a single data point in a horizontal bar chart series.
/// </summary>
public sealed record HorizontalBarChartWithAxisDataPoint
{
    /// <summary>
    /// Gets the numeric value of the bar segment, which determines its length along the x-axis.
    /// </summary>
    [JsonPropertyName("x")]
    public double X { get; init; }

    /// <summary>
    /// Gets the category or label of the bar segment, which determines its position along the y-axis.
    /// </summary>
    [JsonPropertyName("y")]
    public string Y { get; init; } = string.Empty;

    /// <summary>
    /// Gets the legend text shown for the bar segment.
    /// </summary>
    [JsonPropertyName("legend")]
    public string Legend { get; init; } = string.Empty;

    /// <summary>
    /// Gets the solid color used to render the bar segment.
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
    /// Gets the optional two-color gradient used to render the bar segment.
    /// The array should contain exactly two color values: start and end.
    /// </summary>
    [JsonPropertyName("gradient")]
    public string[]? Gradient { get; init; }

    /// <summary>
    /// Gets the optional accessibility data for the tooltip callout.
    /// When set, <see cref="CalloutAccessibilityData.AriaLabel"/> is used as the accessible label
    /// for the bar's callout element.
    /// </summary>
    [JsonPropertyName("callOutAccessibilityData")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public CalloutAccessibilityData? CallOutAccessibilityData { get; init; }

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

