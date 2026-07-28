// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Provides source-generated JSON serialization metadata for area chart payloads.
/// </summary>
[JsonSerializable(typeof(AreaChartSeries))]
[JsonSerializable(typeof(IReadOnlyList<AreaChartSeries>))]
[JsonSerializable(typeof(AreaChartDataPoint))]
[JsonSerializable(typeof(IReadOnlyList<AreaChartDataPoint>))]
[ExcludeFromCodeCoverage(Justification = "This class is used for source-generated JSON serialization and does not contain any logic to be tested.")]
internal sealed partial class AreaChartDataJsonSerializerContext : JsonSerializerContext
{
}
