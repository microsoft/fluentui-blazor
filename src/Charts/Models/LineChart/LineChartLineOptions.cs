// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Represents per-series stroke customization for a line chart.
/// </summary>
public sealed record LineChartLineOptions
{
    /// <summary>
    /// Gets the stroke width in pixels for the line.
    /// </summary>
    [JsonPropertyName("strokeWidth")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? StrokeWidth { get; init; }

    /// <summary>
    /// Gets the SVG dash pattern applied to the line.
    /// </summary>
    [JsonPropertyName("strokeDasharray")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? StrokeDasharray { get; init; }

    /// <summary>
    /// Gets the SVG dash offset applied to the line.
    /// </summary>
    [JsonPropertyName("strokeDashoffset")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? StrokeDashoffset { get; init; }

    /// <summary>
    /// Gets the serialized SVG line cap style applied to the line.
    /// </summary>
    [JsonPropertyName("strokeLinecap")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SerializedStrokeLinecap => StrokeLinecap?.ToAttributeValue();

    /// <summary>
    /// Gets the typed SVG line cap style applied to the line.
    /// </summary>
    [JsonIgnore]
    public ChartStrokeLinecap? StrokeLinecap { get; init; }

    /// <summary>
    /// Gets the border width in pixels applied around the line.
    /// </summary>
    [JsonPropertyName("lineBorderWidth")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? LineBorderWidth { get; init; }

    /// <summary>
    /// Gets the border color applied around the line.
    /// </summary>
    [JsonPropertyName("lineBorderColor")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? LineBorderColor { get; init; }
}
