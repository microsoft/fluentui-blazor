// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Represents one vertical bar chart series in the data payload.
/// </summary>
public sealed record VerticalBarChartSeries
{
    /// <summary>
    /// Gets the optional title shown for the data series.
    /// </summary>
    [JsonPropertyName("chartSeriesTitle")]
    public string? ChartSeriesTitle { get; init; }

    /// <summary>
    /// Gets the collection of data points rendered within the series.
    /// </summary>
    [JsonPropertyName("chartData")]
    public IReadOnlyList<VerticalBarChartDataPoint> ChartData { get; init; } = [];

    /// <summary>
    /// Gets the optional benchmark value used to render the benchmark indicator.
    /// </summary>
    [JsonPropertyName("benchmarkData")]
    public double? BenchmarkData { get; init; }

    /// <summary>
    /// Gets optional text displayed alongside the chart data for the series.
    /// </summary>
    [JsonPropertyName("chartDataText")]
    public string? ChartDataText { get; init; }
}
