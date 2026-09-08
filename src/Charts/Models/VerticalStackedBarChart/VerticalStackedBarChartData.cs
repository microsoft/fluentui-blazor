// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Represents the stacked bar rendered for a single x-axis category in a vertical stacked bar chart.
/// </summary>
public sealed record VerticalStackedBarChartData
{
    /// <summary>
    /// Gets the x-axis value rendered for this stacked bar.
    /// Accepts a numeric value, a date/time value, or a category label.
    /// </summary>
    [JsonPropertyName("xAxisPoint")]
    public required ChartAxisValue XAxisPoint { get; init; }

    /// <summary>
    /// Gets the collection of segments stacked within this bar.
    /// </summary>
    [JsonPropertyName("chartData")]
    public IReadOnlyList<VerticalStackedBarChartDataPoint> ChartData { get; init; } = [];

    /// <summary>
    /// Gets the optional line series points rendered at this x-axis category.
    /// </summary>
    [JsonPropertyName("lineData")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<VerticalStackedBarChartLineDataPoint>? LineData { get; init; }
}
