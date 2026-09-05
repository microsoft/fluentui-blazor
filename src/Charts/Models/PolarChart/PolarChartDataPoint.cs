// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Represents a single data point in a polar chart series.
/// </summary>
public sealed record PolarChartDataPoint
{
    /// <summary>
    /// Gets the angular value for the point.
    /// </summary>
    [JsonPropertyName("theta")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Theta { get; init; }

    /// <summary>
    /// Gets the radial value for the point.
    /// </summary>
    [JsonPropertyName("r")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ChartAxisValue? R { get; init; }

    /// <summary>
    /// Gets the legacy angular category value.
    /// </summary>
    [JsonPropertyName("x")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? X { get; init; }

    /// <summary>
    /// Gets the legacy radial numeric value.
    /// </summary>
    [JsonPropertyName("y")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Y { get; init; }

    /// <summary>
    /// Gets the radial-axis callout text.
    /// </summary>
    [JsonPropertyName("radialAxisCalloutData")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? RadialAxisCalloutData { get; init; }

    /// <summary>
    /// Gets the angular-axis callout text.
    /// </summary>
    [JsonPropertyName("angularAxisCalloutData")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AngularAxisCalloutData { get; init; }

    /// <summary>
    /// Gets the marker size for the point.
    /// </summary>
    [JsonPropertyName("markerSize")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? MarkerSize { get; init; }

    /// <summary>
    /// Gets the palette color used to render the point.
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
    /// Gets the optional point label text.
    /// </summary>
    [JsonPropertyName("text")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Text { get; init; }
}
