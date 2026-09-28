// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Represents the data payload for a sparkline chart.
/// </summary>
public sealed record SparklineChartData
{
    /// <summary>
    /// Gets the optional title included in the chart data payload.
    /// </summary>
    [JsonPropertyName("chartTitle")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ChartTitle { get; init; }

    /// <summary>
    /// Gets the sparkline series rendered by the chart.
    /// </summary>
    [JsonPropertyName("lineChartData")]
    public IReadOnlyList<SparklineChartSeries> LineChartData { get; init; } = [];
}
