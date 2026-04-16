// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Provides shared source-generated JSON serialization helpers for chart payloads.
/// </summary>
public static class ChartJson
{
    /// <summary>
    /// Serializes donut chart data using the donut chart serializer context.
    /// </summary>
    /// <param name="value">The donut chart data payload.</param>
    /// <returns>A JSON string suitable for the <c>fluent-donut-chart</c> component.</returns>
    public static string Serialize(DonutChartData value) =>
        JsonSerializer.Serialize(
            value,
            DonutChartDataJsonSerializerContext.Default.DonutChartData);

    /// <summary>
    /// Serializes horizontal bar chart data using the horizontal bar chart serializer context.
    /// </summary>
    /// <param name="value">The horizontal bar chart series collection.</param>
    /// <returns>A JSON string suitable for the <c>fluent-horizontal-bar-chart</c> component.</returns>
    public static string Serialize(IReadOnlyList<HorizontalBarChartSeries> value) =>
        JsonSerializer.Serialize(
            value,
            HorizontalBarChartDataJsonSerializerContext.Default.IReadOnlyListHorizontalBarChartSeries);
}
