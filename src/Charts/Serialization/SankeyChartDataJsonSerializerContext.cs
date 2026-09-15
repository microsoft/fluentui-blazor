// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Provides source-generated JSON serialization metadata for sankey chart payloads.
/// </summary>
[JsonSerializable(typeof(SankeyChartData))]
[JsonSerializable(typeof(SankeyChartNode))]
[JsonSerializable(typeof(SankeyChartLink))]
[JsonSerializable(typeof(IReadOnlyList<SankeyChartNode>))]
[JsonSerializable(typeof(IReadOnlyList<SankeyChartLink>))]
[ExcludeFromCodeCoverage(Justification = "This class is used for source-generated JSON serialization and does not contain any logic to be tested.")]
internal sealed partial class SankeyChartDataJsonSerializerContext : JsonSerializerContext
{
}
