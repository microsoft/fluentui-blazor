// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Represents the bars rendered for a single x-axis category in a grouped vertical bar chart.
/// </summary>
public sealed record GroupedVerticalBarChartData
{
    /// <summary>
    /// Gets the category label rendered on the x-axis for this group.
    /// </summary>
    [JsonPropertyName("xAxisPoint")]
    public required string XAxisPoint { get; init; }

    /// <summary>
    /// Gets the collection of bars rendered side by side within this category.
    /// </summary>
    [JsonPropertyName("series")]
    public IReadOnlyList<GroupedVerticalBarChartDataPoint> Series { get; init; } = [];

    /// <summary>
    /// Gets the optional line series points rendered at this x-axis category.
    /// </summary>
    [JsonPropertyName("lineData")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<GroupedVerticalBarChartLineDataPoint>? LineData { get; init; }
}
