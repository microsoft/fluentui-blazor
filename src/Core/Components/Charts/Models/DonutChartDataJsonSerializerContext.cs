// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Provides source-generated JSON serialization metadata for donut chart payloads.
/// </summary>
[JsonSerializable(typeof(DonutChartData))]
[JsonSerializable(typeof(DonutChartDataPoint))]
[JsonSerializable(typeof(IReadOnlyList<DonutChartDataPoint>))]
internal sealed partial class DonutChartDataJsonSerializerContext : JsonSerializerContext
{
}
