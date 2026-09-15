// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Provides source-generated JSON serialization metadata for line chart payloads.
/// </summary>
[JsonSerializable(typeof(ChartAxisValue))]
[JsonSerializable(typeof(LineChartSeries))]
[JsonSerializable(typeof(IReadOnlyList<LineChartSeries>))]
[JsonSerializable(typeof(LineChartDataPoint))]
[JsonSerializable(typeof(IReadOnlyList<LineChartDataPoint>))]
[JsonSerializable(typeof(LineChartGap))]
[JsonSerializable(typeof(IReadOnlyList<LineChartGap>))]
[JsonSerializable(typeof(LineChartLineOptions))]
[JsonSerializable(typeof(LineChartColorFillBar))]
[JsonSerializable(typeof(IReadOnlyList<LineChartColorFillBar>))]
[JsonSerializable(typeof(LineChartColorFillBarData))]
[JsonSerializable(typeof(IReadOnlyList<LineChartColorFillBarData>))]
[ExcludeFromCodeCoverage(Justification = "This class is used for source-generated JSON serialization and does not contain any logic to be tested.")]
internal sealed partial class LineChartDataJsonSerializerContext : JsonSerializerContext
{
}
