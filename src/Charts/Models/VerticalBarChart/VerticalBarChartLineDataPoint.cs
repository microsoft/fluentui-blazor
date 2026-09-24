// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Represents the overlaid line series point rendered on top of a <see cref="VerticalBarChartDataPoint"/>.
/// </summary>
public sealed record VerticalBarChartLineDataPoint
{
    /// <summary>
    /// Gets the y-axis numeric value rendered for the line point.
    /// </summary>
    [JsonPropertyName("y")]
    public double Y { get; init; }

    /// <summary>
    /// Gets the optional text that overrides the numeric value displayed in the tooltip.
    /// </summary>
    [JsonPropertyName("yAxisCalloutData")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? YAxisCalloutData { get; init; }

    /// <summary>
    /// Gets a value indicating whether the line point is rendered against the secondary y-axis.
    /// </summary>
    [JsonPropertyName("useSecondaryYScale")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? UseSecondaryYScale { get; init; }
}
