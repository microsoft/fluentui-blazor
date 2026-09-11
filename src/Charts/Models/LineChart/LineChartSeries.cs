// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Represents one line chart series in the data payload.
/// </summary>
public sealed record LineChartSeries
{
    /// <summary>
    /// Gets the legend text shown for the series.
    /// </summary>
    [JsonPropertyName("legend")]
    public string Legend { get; init; } = string.Empty;

    /// <summary>
    /// Gets the collection of data points rendered within the series.
    /// </summary>
    [JsonPropertyName("data")]
    public IReadOnlyList<LineChartDataPoint> Data { get; init; } = [];

    /// <summary>
    /// Gets the solid color used to render the series.
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
    /// Gets the optional explicit gap ranges rendered in the line.
    /// </summary>
    [JsonPropertyName("gaps")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<LineChartGap>? Gaps { get; init; }

    /// <summary>
    /// Gets whether this series uses the secondary (right) y-axis scale.
    /// </summary>
    [JsonPropertyName("useSecondaryYScale")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? UseSecondaryYScale { get; init; }

    /// <summary>
    /// Gets the optional per-series line styling.
    /// </summary>
    [JsonPropertyName("lineOptions")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public LineChartLineOptions? LineOptions { get; init; }
}
