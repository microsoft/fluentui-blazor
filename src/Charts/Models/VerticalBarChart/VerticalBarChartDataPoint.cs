// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Represents a single data point in a horizontal bar chart series.
/// </summary>
public sealed record VerticalBarChartDataPoint
{
    /// <summary>
    /// Gets the legend text shown for the bar segment.
    /// </summary>
    [JsonPropertyName("legend")]
    public string Legend { get; init; } = string.Empty;

    /// <summary>
    /// Gets the numeric value represented by the bar segment.
    /// </summary>
    [JsonPropertyName("data")]
    public double Data { get; init; }

    /// <summary>
    /// Gets the optional total bar length used for ratio-style rendering.
    /// </summary>
    [JsonPropertyName("total")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Total { get; init; }

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
}
