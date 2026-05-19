// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

#pragma warning disable MA0048 // File name must match type name

/// <summary>
/// Represents the x-axis range of a <see cref="GanttChartDataPoint"/>.
/// Both <see cref="Start"/> and <see cref="End"/> accept numeric values
/// (e.g. Unix timestamps in milliseconds) as well as <see cref="DateTime"/> or
/// <see cref="DateTimeOffset"/> values that are serialized as ISO 8601 strings.
/// </summary>
public sealed record GanttChartXRange
{
    /// <summary>
    /// Gets the start value of the range along the x-axis.
    /// Assign a <see cref="double"/>, <see cref="DateTime"/>, or <see cref="DateTimeOffset"/>.
    /// </summary>
    [JsonPropertyName("start")]
    public ChartAxisValue Start { get; init; }

    /// <summary>
    /// Gets the end value of the range along the x-axis.
    /// Assign a <see cref="double"/>, <see cref="DateTime"/>, or <see cref="DateTimeOffset"/>.
    /// </summary>
    [JsonPropertyName("end")]
    public ChartAxisValue End { get; init; }
}

/// <summary>
/// Represents a single data point in a Gantt chart.
/// </summary>
public sealed record GanttChartDataPoint
{
    /// <summary>
    /// Gets the numeric range rendered along the x-axis, representing the start and end
    /// of the bar segment.
    /// </summary>
    [JsonPropertyName("x")]
    public required GanttChartXRange X { get; init; }

    /// <summary>
    /// Gets the category or label of the bar segment, which determines its position
    /// along the y-axis. The chart distributes string values evenly along the axis.
    /// </summary>
    [JsonPropertyName("y")]
    public string Y { get; init; } = string.Empty;

    /// <summary>
    /// Gets the legend text shown for the bar segment.
    /// </summary>
    [JsonPropertyName("legend")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Legend { get; init; }

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
    /// The array must contain exactly two color values: start color and end color.
    /// Overrides <see cref="Color"/> when the chart's <c>EnableGradient</c> parameter is <see langword="true"/>.
    /// </summary>
    [JsonPropertyName("gradient")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? Gradient { get; init; }

    /// <summary>
    /// Gets optional callout data for the x-axis portion of the tooltip.
    /// </summary>
    [JsonPropertyName("xAxisCalloutData")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? XAxisCalloutData { get; init; }

    /// <summary>
    /// Gets optional callout data for the y-axis portion of the tooltip.
    /// </summary>
    [JsonPropertyName("yAxisCalloutData")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? YAxisCalloutData { get; init; }

    /// <summary>
    /// Gets the optional accessibility data for the tooltip callout.
    /// When set, <see cref="CalloutAccessibilityData.AriaLabel"/> is used as the accessible
    /// label for the bar's callout element.
    /// </summary>
    [JsonPropertyName("callOutAccessibilityData")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public CalloutAccessibilityData? CallOutAccessibilityData { get; init; }
}

#pragma warning restore MA0048 // File name must match type name
