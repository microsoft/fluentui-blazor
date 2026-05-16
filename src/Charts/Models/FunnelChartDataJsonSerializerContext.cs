// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Provides source-generated JSON serialization metadata for funnel chart payloads.
/// </summary>
[JsonSerializable(typeof(FunnelDataPoint))]
[JsonSerializable(typeof(IReadOnlyList<FunnelDataPoint>))]
internal sealed partial class FunnelChartDataJsonSerializerContext : JsonSerializerContext
{
}
