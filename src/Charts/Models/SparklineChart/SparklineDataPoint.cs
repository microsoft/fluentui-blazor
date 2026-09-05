// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Represents a single point in a sparkline series.
/// </summary>
public sealed record SparklineDataPoint
{
    /// <summary>
    /// Gets the x-axis value rendered for this point.
    /// Accepts either a numeric value or a date/time value.
    /// </summary>
    [JsonPropertyName("x")]
    public ChartAxisValue X { get; init; }

    /// <summary>
    /// Gets the y-axis numeric value rendered for this point.
    /// </summary>
    [JsonPropertyName("y")]
    public double Y { get; init; }
}
