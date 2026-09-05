// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Provides source-generated JSON serialization metadata for scatter chart payloads.
/// </summary>
[JsonSerializable(typeof(ChartAxisValue))]
[JsonSerializable(typeof(ScatterChartSeries))]
[JsonSerializable(typeof(IReadOnlyList<ScatterChartSeries>))]
[JsonSerializable(typeof(ScatterChartDataPoint))]
[JsonSerializable(typeof(IReadOnlyList<ScatterChartDataPoint>))]
[ExcludeFromCodeCoverage(Justification = "This class is used for source-generated JSON serialization and does not contain any logic to be tested.")]
internal sealed partial class ScatterChartDataJsonSerializerContext : JsonSerializerContext
{
}
