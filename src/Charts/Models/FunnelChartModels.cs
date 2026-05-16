// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;

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
    /// Fill color for the sub value. The value is serialized as its token string
    /// (e.g. <c>"color5"</c>) and resolved to an actual hex color by the web component.
    /// </summary>
    [JsonPropertyName("color")]
    public DataVizPalette? Color { get; init; }
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
    /// The value is serialized as its token string (e.g. <c>"color5"</c>) and resolved
    /// to an actual hex color by the web component. If not provided, the web component
    /// falls back to its default palette.
    /// </summary>
    [JsonPropertyName("color")]
    public DataVizPalette? Color { get; init; }

    /// <summary>
    /// Gets optional callout data for the x-axis portion of the tooltip.
    /// </summary>
    [JsonPropertyName("subValues")]
    public IReadOnlyList<FunnelSubValue> SubValues { get; init; } = [];
}
