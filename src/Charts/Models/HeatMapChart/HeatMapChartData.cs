// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Represents one heat map chart series in the data payload.
/// </summary>
public sealed record HeatMapChartData
{
    /// <summary>
    /// Gets the legend text shown for the series.
    /// </summary>
    [JsonPropertyName("legend")]
    public string Legend { get; init; } = string.Empty;

    /// <summary>
    /// Gets the representative numeric value used to derive the legend color.
    /// </summary>
    [JsonPropertyName("value")]
    public double Value { get; init; }

    /// <summary>
    /// Gets the collection of heat map cells rendered within the series.
    /// </summary>
    [JsonPropertyName("data")]
    public IReadOnlyList<HeatMapChartDataPoint> Data { get; init; } = [];
}
