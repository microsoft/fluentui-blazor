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
    /// Serializes gauge chart segments using the gauge chart serializer context.
    /// </summary>
    /// <param name="value">The gauge chart segments.</param>
    /// <returns>A JSON string suitable for the <c>fluent-gauge-chart</c> component.</returns>
    public static string Serialize(IReadOnlyList<GaugeChartSegment> value) =>
        JsonSerializer.Serialize(
            value,
            GaugeChartDataJsonSerializerContext.Default.IReadOnlyListGaugeChartSegment);

    /// <summary>
    /// Serializes sparkline chart data using the sparkline chart serializer context.
    /// </summary>
    /// <param name="value">The sparkline chart data payload.</param>
    /// <returns>A JSON string suitable for the <c>fluent-sparkline-chart</c> component.</returns>
    public static string Serialize(SparklineChartData value) =>
        JsonSerializer.Serialize(
            value,
            SparklineChartDataJsonSerializerContext.Default.SparklineChartData);

    /// <summary>
    /// Serializes vertical bar chart data using the vertical bar chart serializer context.
    /// </summary>
    /// <param name="value">The vertical bar chart series collection.</param>
    /// <returns>A JSON string suitable for the <c>fluent-vertical-bar-chart</c> component.</returns>
    public static string Serialize(IReadOnlyList<VerticalBarChartSeries> value) =>
        JsonSerializer.Serialize(
            value,
            VerticalBarChartDataJsonSerializerContext.Default.IReadOnlyListVerticalBarChartSeries);

    /// <summary>
    /// Serializes grouped vertical bar chart data using the grouped vertical bar chart serializer context.
    /// </summary>
    /// <param name="value">The grouped vertical bar chart series collection.</param>
    /// <returns>A JSON string suitable for the <c>fluent-grouped-vertical-bar-chart</c> component.</returns>
    public static string Serialize(IReadOnlyList<GroupedVerticalBarChartSeries> value) =>
        JsonSerializer.Serialize(
            value,
            GroupedVerticalBarChartDataJsonSerializerContext.Default.IReadOnlyListGroupedVerticalBarChartSeries);

    /// <summary>
    /// Serializes vertical stacked bar chart data using the vertical stacked bar chart serializer context.
    /// </summary>
    /// <param name="value">The vertical stacked bar chart series collection.</param>
    /// <returns>A JSON string suitable for the <c>fluent-vertical-stacked-bar-chart</c> component.</returns>
    public static string Serialize(IReadOnlyList<VerticalStackedBarChartSeries> value) =>
        JsonSerializer.Serialize(
            value,
            VerticalStackedBarChartDataJsonSerializerContext.Default.IReadOnlyListVerticalStackedBarChartSeries);

    /// <summary>
    /// Serializes polar chart data using the polar chart serializer context.
    /// </summary>
    /// <param name="value">The polar chart series collection.</param>
    /// <returns>A JSON string suitable for the <c>fluent-polar-chart</c> component.</returns>
    public static string Serialize(IReadOnlyList<PolarChartSeries> value) =>
        JsonSerializer.Serialize(
            value,
            PolarChartDataJsonSerializerContext.Default.IReadOnlyListPolarChartSeries);

    /// <summary>
    /// Serializes sankey chart data using the sankey chart serializer context.
    /// </summary>
    /// <param name="value">The sankey chart payload.</param>
    /// <returns>A JSON string suitable for the <c>fluent-sankey-chart</c> component.</returns>
    public static string Serialize(SankeyChartData value) =>
        JsonSerializer.Serialize(
            value,
            SankeyChartDataJsonSerializerContext.Default.SankeyChartData);

    /// <summary>
    /// Serializes line chart data using the line chart serializer context.
    /// </summary>
    /// <param name="value">The line chart series collection.</param>
    /// <returns>A JSON string suitable for the <c>fluent-line-chart</c> component.</returns>
    public static string Serialize(IReadOnlyList<LineChartSeries> value) =>
        JsonSerializer.Serialize(
            value,
            LineChartDataJsonSerializerContext.Default.IReadOnlyListLineChartSeries);

    /// <summary>
    /// Serializes scatter chart data using the scatter chart serializer context.
    /// </summary>
    /// <param name="value">The scatter chart series collection.</param>
    /// <returns>A JSON string suitable for the <c>fluent-scatter-chart</c> component.</returns>
    public static string Serialize(IReadOnlyList<ScatterChartSeries> value) =>
        JsonSerializer.Serialize(
            value,
            ScatterChartDataJsonSerializerContext.Default.IReadOnlyListScatterChartSeries);

    /// <summary>
    /// Serializes heat map chart data using the heat map chart serializer context.
    /// </summary>
    /// <param name="value">The heat map chart series collection.</param>
    /// <returns>A JSON string suitable for the <c>fluent-heat-map-chart</c> component.</returns>
    public static string Serialize(IReadOnlyList<HeatMapChartData> value) =>
        JsonSerializer.Serialize(
            value,
            HeatMapChartDataJsonSerializerContext.Default.IReadOnlyListHeatMapChartData);
}
