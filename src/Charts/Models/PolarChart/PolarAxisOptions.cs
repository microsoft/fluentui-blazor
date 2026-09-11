// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Represents axis options for a polar chart.
/// </summary>
public sealed record PolarAxisOptions
{
    /// <summary>
    /// Gets the number of ticks to render.
    /// </summary>
    [JsonPropertyName("tickCount")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? TickCount { get; init; }

    /// <summary>
    /// Gets the explicit tick values to render.
    /// </summary>
    [JsonPropertyName("tickValues")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<ChartAxisValue>? TickValues { get; init; }

    /// <summary>
    /// Gets the explicit tick labels to render.
    /// </summary>
    [JsonPropertyName("tickText")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<string>? TickText { get; init; }

    /// <summary>
    /// Gets the tick format string.
    /// </summary>
    [JsonPropertyName("tickFormat")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? TickFormat { get; init; }

    /// <summary>
    /// Gets the tick step value.
    /// </summary>
    [JsonPropertyName("tickStep")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? TickStep { get; init; }

    /// <summary>
    /// Gets the axis origin tick value.
    /// </summary>
    [JsonPropertyName("tick0")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ChartAxisValue? Tick0 { get; init; }

    /// <summary>
    /// Gets the categorical ordering strategy.
    /// </summary>
    [JsonIgnore]
    public ChartCategoryOrder? CategoryOrder { get; init; }

    /// <summary>
    /// Gets the serialized category order string sent to the web component.
    /// </summary>
    [JsonPropertyName("categoryOrder")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SerializedCategoryOrder => CategoryOrder?.ToAttributeValue();

    /// <summary>
    /// Gets the numeric scale type.
    /// </summary>
    [JsonIgnore]
    public ChartAxisScaleType? ScaleType { get; init; }

    /// <summary>
    /// Gets the serialized scale type string sent to the web component.
    /// </summary>
    [JsonPropertyName("scaleType")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SerializedScaleType => ScaleType?.ToAttributeValue();

    /// <summary>
    /// Gets the minimum axis range value.
    /// </summary>
    [JsonPropertyName("rangeStart")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ChartAxisValue? RangeStart { get; init; }

    /// <summary>
    /// Gets the maximum axis range value.
    /// </summary>
    [JsonPropertyName("rangeEnd")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ChartAxisValue? RangeEnd { get; init; }

    /// <summary>
    /// Gets the angular unit.
    /// </summary>
    [JsonIgnore]
    public PolarAxisUnit? Unit { get; init; }

    /// <summary>
    /// Gets the serialized angular unit string sent to the web component.
    /// </summary>
    [JsonPropertyName("unit")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SerializedUnit => Unit?.ToAttributeValue();
}
