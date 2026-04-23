// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Provides source-generated JSON serialization metadata for horizontal bar chart payloads.
/// </summary>
[JsonSerializable(typeof(HorizontalBarChartWithAxisDataPoint))]
[JsonSerializable(typeof(IReadOnlyList<HorizontalBarChartWithAxisDataPoint>))]
internal sealed partial class HorizontalBarChartWithAxisDataJsonSerializerContext : JsonSerializerContext
{
}
