// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

#pragma warning disable MA0048 // File name must match type name

/// <summary>
/// Represents a single data point in a horizontal bar chart series.
/// </summary>
public sealed record HorizontalBarChartDataPoint
{
    /// <summary>
    /// Gets the legend text shown for the bar segment.
    /// </summary>
    [JsonPropertyName("legend")]
    public string Legend { get; init; } = string.Empty;

    /// <summary>
    /// Gets the numeric value represented by the bar segment.
    /// </summary>
    [JsonPropertyName("data")]
    public double Data { get; init; }

    /// <summary>
    /// Gets the optional total bar length used for ratio-style rendering.
    /// </summary>
    [JsonPropertyName("total")]
    public double? Total { get; init; }

    /// <summary>
    /// Gets the solid color used to render the bar segment.
    /// If not provided, the component may fall back to its default palette behavior.
    /// </summary>
    [JsonPropertyName("color")]
    public string? Color { get; init; }

    /// <summary>
    /// Gets the optional two-color gradient used to render the bar segment.
    /// The array should contain exactly two color values: start and end.
    /// </summary>
    [JsonPropertyName("gradient")]
    public string[]? Gradient { get; init; }
}

/// <summary>
/// Represents one horizontal bar chart series in the data payload.
/// </summary>
public sealed record HorizontalBarChartSeries
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
    public IReadOnlyList<HorizontalBarChartDataPoint> ChartData { get; init; } = [];

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
#pragma warning restore MA0048 // File name must match type name
