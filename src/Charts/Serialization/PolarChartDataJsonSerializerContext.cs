// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Provides source-generated JSON serialization metadata for polar chart payloads.
/// </summary>
[JsonSerializable(typeof(ChartAxisValue))]
[JsonSerializable(typeof(IReadOnlyList<ChartAxisValue>))]
[JsonSerializable(typeof(PolarChartSeries))]
[JsonSerializable(typeof(IReadOnlyList<PolarChartSeries>))]
[JsonSerializable(typeof(PolarChartDataPoint))]
[JsonSerializable(typeof(IReadOnlyList<PolarChartDataPoint>))]
[JsonSerializable(typeof(PolarLineOptions))]
[JsonSerializable(typeof(PolarAxisOptions))]
[JsonSerializable(typeof(PolarChartMargins))]
[JsonSerializable(typeof(IDictionary<string, string>))]
[JsonSerializable(typeof(Dictionary<string, string>))]
[ExcludeFromCodeCoverage(Justification = "This class is used for source-generated JSON serialization and does not contain any logic to be tested.")]
internal sealed partial class PolarChartDataJsonSerializerContext : JsonSerializerContext
{
}
