// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

#pragma warning disable MA0048 // File name must match type name

/// <summary>
/// Represents a single sub value within a stacked funnel stage
/// </summary>
public sealed record FunnelSubValue
{
  /// <summary>
  /// Category name for the sub value
  /// </summary>
  [JsonPropertyName("category")]
  public string Category { get; init; } = string.Empty;

  /// <summary>
  /// Numeric value for the sub value
  /// </summary>
  [JsonPropertyName("value")]
  public double Value { get; init; }

  /// <summary>
  /// Fill color palette token for the sub value. Use <see cref="DataVizPalette.Custom"/>
  /// and set <see cref="CustomColor"/> to supply an exact hex or CSS color string.
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
  /// otherwise the palette token string, or <c>null</c> when no color is set.
  /// </summary>
  [JsonPropertyName("color")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public string? SerializedColor => Color == DataVizPalette.Custom ? CustomColor : Color?.ToAttributeValue();
}

/// <summary>
/// Represents a single data point in a funnel chart.
/// </summary>
public sealed record FunnelDataPoint
{
  /// <summary>
  /// Gets the legend text shown for the funnel segment.
  /// </summary>
  [JsonPropertyName("stage")]
  public string Stage { get; init; } = string.Empty;

  /// <summary>
  /// Gets the numeric value of the funnel segment.
  /// </summary>
  [JsonPropertyName("value")]
  public double Value { get; init; }

  /// <summary>
  /// Gets the color used to render the funnel segment and legend.
  /// Use <see cref="DataVizPalette.Custom"/> and set <see cref="CustomColor"/> to supply
  /// an exact hex or CSS color string. If not provided, the web component falls back to
  /// its default palette.
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
  /// otherwise the palette token string, or <c>null</c> when no color is set.
  /// </summary>
  [JsonPropertyName("color")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public string? SerializedColor => Color == DataVizPalette.Custom ? CustomColor : Color?.ToAttributeValue();

  /// <summary>
  /// Gets optional callout data for the x-axis portion of the tooltip.
  /// </summary>
  [JsonPropertyName("subValues")]
  public IReadOnlyList<FunnelSubValue> SubValues { get; init; } = [];
}
