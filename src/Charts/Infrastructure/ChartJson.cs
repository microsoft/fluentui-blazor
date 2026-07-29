// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Provides shared source-generated JSON serialization helpers for chart payloads.
/// </summary>
[ExcludeFromCodeCoverage(Justification = "Thin convenience wrapper around source-generated JSON contexts; behavior is covered by serializer context tests.")]
public static class ChartJson
{
    /// <summary>
    /// Serializes area chart data using the area chart serializer context.
    /// </summary>
    /// <param name="value">The area chart data payload.</param>
    /// <returns>A JSON string suitable for the <c>fluent-area-chart</c> component.</returns>
    public static string Serialize(IReadOnlyList<AreaChartSeries> value) =>
        JsonSerializer.Serialize(
            value,
            AreaChartDataJsonSerializerContext.Default.IReadOnlyListAreaChartSeries);

    /// <summary>
    /// Serializes donut chart data using the donut chart serializer context.
    /// </summary>
    /// <param name="value">The donut chart data payload.</param>
    /// <returns>A JSON string suitable for the <c>fluent-donut-chart</c> component.</returns>
    public static string Serialize(IReadOnlyList<DonutDataPoint> value) =>
        JsonSerializer.Serialize(
            value,
            DonutChartDataJsonSerializerContext.Default.IReadOnlyListDonutDataPoint);

    /// <summary>
    /// Serializes horizontal bar chart data using the horizontal bar chart serializer context.
    /// </summary>
    /// <param name="value">The horizontal bar chart series collection.</param>
    /// <returns>A JSON string suitable for the <c>fluent-horizontal-bar-chart</c> component.</returns>
    public static string Serialize(IReadOnlyList<HorizontalBarChartSeries> value) =>
        JsonSerializer.Serialize(
            value,
            HorizontalBarChartDataJsonSerializerContext.Default.IReadOnlyListHorizontalBarChartSeries);

    /// <summary>
    /// Serializes horizontal bar chart data with using the horizontal bar chart
    /// serializer context.
    /// </summary>
    /// <param name="value">The horizontal bar chart series collection.</param>
    /// <returns>
    /// A JSON string suitable for the <c> fluent-horizontal-bar-chart</c>
    /// component.
    /// </returns>
    public static string Serialize(IReadOnlyList<HorizontalBarChartWithAxisDataPoint> value) =>
        JsonSerializer.Serialize(
            value,
            HorizontalBarChartWithAxisDataJsonSerializerContext.Default.IReadOnlyListHorizontalBarChartWithAxisDataPoint);

    /// <summary>
    /// Serializes funnel chart data using the funnel chart serializer context.
    /// </summary>
    /// <param name="value">The funnel chart data payload.</param>
    /// <returns>A JSON string suitable for the <c>fluent-funnel-chart</c> component.</returns>
    public static string Serialize(IReadOnlyList<FunnelDataPoint> value) =>
        JsonSerializer.Serialize(
            value,
            FunnelChartDataJsonSerializerContext.Default.IReadOnlyListFunnelDataPoint);

    /// <summary>
    /// Serializes Gantt chart data using the Gantt chart serializer context.
    /// </summary>
    /// <param name="value">The Gantt chart data payload.</param>
    /// <returns>A JSON string suitable for the <c>fluent-gantt-chart</c> component.</returns>
    public static string Serialize(IReadOnlyList<GanttChartDataPoint> value) =>
        JsonSerializer.Serialize(
            value,
            GanttChartDataJsonSerializerContext.Default.IReadOnlyListGanttChartDataPoint);

    /// <summary>
    /// Serializes vertical bar chart data using the vertical bar chart serializer context.
    /// </summary>
    /// <param name="value">The vertical bar chart series collection.</param>
    /// <returns>A JSON string suitable for the <c>fluent-vertical-bar-chart</c> component.</returns>
    public static string Serialize(IReadOnlyList<VerticalBarChartSeries> value) =>
        JsonSerializer.Serialize(
            value,
            VerticalBarChartDataJsonSerializerContext.Default.IReadOnlyListVerticalBarChartSeries);
}
