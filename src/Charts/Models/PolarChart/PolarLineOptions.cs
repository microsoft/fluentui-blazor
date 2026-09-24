// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Represents line rendering options for a polar chart series.
/// </summary>
public sealed record PolarLineOptions
{
    /// <summary>
    /// Gets the line stroke width.
    /// </summary>
    [JsonPropertyName("strokeWidth")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? StrokeWidth { get; init; }

    /// <summary>
    /// Gets the dash pattern applied to the line stroke.
    /// </summary>
    [JsonPropertyName("strokeDasharray")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? StrokeDasharray { get; init; }

    /// <summary>
    /// Gets the dash offset applied to the line stroke.
    /// </summary>
    [JsonPropertyName("strokeDashoffset")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? StrokeDashoffset { get; init; }

    /// <summary>
    /// Gets the SVG stroke line cap style.
    /// </summary>
    [JsonIgnore]
    public ChartStrokeLinecap? StrokeLinecap { get; init; }

    /// <summary>
    /// Gets the serialized stroke line cap string sent to the web component.
    /// </summary>
    [JsonPropertyName("strokeLinecap")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SerializedStrokeLinecap => StrokeLinecap?.ToAttributeValue();

    /// <summary>
    /// Gets the curve interpolation mode.
    /// </summary>
    [JsonIgnore]
    public PolarLineCurve? Curve { get; init; }

    /// <summary>
    /// Gets the serialized curve string sent to the web component.
    /// </summary>
    [JsonPropertyName("curve")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SerializedCurve => Curve?.ToAttributeValue();
}
