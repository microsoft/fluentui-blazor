// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Provides source-generated JSON serialization metadata for vertical stacked bar chart payloads.
/// </summary>
[JsonSerializable(typeof(VerticalStackedBarChartData))]
[JsonSerializable(typeof(VerticalStackedBarChartDataPoint))]
[JsonSerializable(typeof(VerticalStackedBarChartLineDataPoint))]
[JsonSerializable(typeof(IReadOnlyList<VerticalStackedBarChartData>))]
[ExcludeFromCodeCoverage(Justification = "This class is used for source-generated JSON serialization and does not contain any logic to be tested.")]

internal sealed partial class VerticalStackedBarChartDataJsonSerializerContext : JsonSerializerContext
{
}
