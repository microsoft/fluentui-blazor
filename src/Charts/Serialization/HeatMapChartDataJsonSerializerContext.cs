// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Provides source-generated JSON serialization metadata for heat map chart payloads.
/// </summary>
[JsonSerializable(typeof(ChartAxisValue))]
[JsonSerializable(typeof(HeatMapAccessibilityData))]
[JsonSerializable(typeof(HeatMapChartData))]
[JsonSerializable(typeof(IReadOnlyList<HeatMapChartData>))]
[JsonSerializable(typeof(HeatMapChartDataPoint))]
[JsonSerializable(typeof(IReadOnlyList<HeatMapChartDataPoint>))]
[ExcludeFromCodeCoverage(Justification = "This class is used for source-generated JSON serialization and does not contain any logic to be tested.")]
internal sealed partial class HeatMapChartDataJsonSerializerContext : JsonSerializerContext
{
}
