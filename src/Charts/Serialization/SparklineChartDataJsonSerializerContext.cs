// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Provides source-generated JSON serialization metadata for sparkline chart payloads.
/// </summary>
[JsonSerializable(typeof(SparklineChartData))]
[JsonSerializable(typeof(SparklineChartSeries))]
[JsonSerializable(typeof(SparklineDataPoint))]
[JsonSerializable(typeof(IReadOnlyList<SparklineChartSeries>))]
[JsonSerializable(typeof(IReadOnlyList<SparklineDataPoint>))]
[ExcludeFromCodeCoverage(Justification = "This class is used for source-generated JSON serialization and does not contain any logic to be tested.")]
internal sealed partial class SparklineChartDataJsonSerializerContext : JsonSerializerContext
{
}
