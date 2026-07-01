// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Provides source-generated JSON serialization metadata for donut chart payloads.
/// </summary>
[JsonSerializable(typeof(DonutDataPoint))]
[JsonSerializable(typeof(IReadOnlyList<DonutDataPoint>))]
[ExcludeFromCodeCoverage(Justification = "This class is used for source-generated JSON serialization and does not contain any logic to be tested.")]
internal sealed partial class DonutChartDataJsonSerializerContext : JsonSerializerContext
{
}
