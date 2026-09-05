// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Represents one series in a polar chart payload.
/// </summary>
public sealed record PolarChartSeries
{
    /// <summary>
    /// Gets the legend text shown for the series.
    /// </summary>
    [JsonPropertyName("legend")]
    public string Legend { get; init; } = string.Empty;

    /// <summary>
    /// Gets the collection of data points rendered for the series.
    /// </summary>
    [JsonPropertyName("data")]
    public IReadOnlyList<PolarChartDataPoint> Data { get; init; } = [];

    /// <summary>
    /// Gets the palette color used to render the series.
    /// </summary>
    [JsonIgnore]
    public DataVizPalette? Color { get; init; }

    /// <summary>
    /// Gets the custom CSS color used when <see cref="Color"/> is <see cref="DataVizPalette.Custom"/>.
    /// </summary>
    [JsonIgnore]
    public string? CustomColor { get; init; }

    /// <summary>
    /// Gets the serialized color value sent to the web component.
    /// </summary>
    [JsonPropertyName("color")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SerializedColor => Color == DataVizPalette.Custom ? CustomColor : Color?.ToAttributeValue();

    /// <summary>
    /// Gets the polar series rendering type.
    /// </summary>
    [JsonIgnore]
    public PolarSeriesType? Type { get; init; }

    /// <summary>
    /// Gets the serialized polar series type string sent to the web component.
    /// </summary>
    [JsonPropertyName("type")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SerializedType => Type?.ToAttributeValue();

    /// <summary>
    /// Gets the optional line rendering options.
    /// </summary>
    [JsonPropertyName("lineOptions")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PolarLineOptions? LineOptions { get; init; }
}
