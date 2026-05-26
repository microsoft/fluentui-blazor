// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Provides source-generated JSON serialization metadata for Gantt chart payloads.
/// </summary>
[JsonSerializable(typeof(ChartAxisValue))]
[JsonSerializable(typeof(GanttChartXRange))]
[JsonSerializable(typeof(GanttChartDataPoint))]
[JsonSerializable(typeof(CalloutAccessibilityData))]
[JsonSerializable(typeof(IReadOnlyList<GanttChartDataPoint>))]
internal sealed partial class GanttChartDataJsonSerializerContext : JsonSerializerContext
{
}
