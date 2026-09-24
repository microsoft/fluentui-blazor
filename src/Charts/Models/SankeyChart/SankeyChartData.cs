// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Represents the top-level sankey chart payload.
/// </summary>
public sealed record SankeyChartData
{
    /// <summary>
    /// Gets the chart nodes.
    /// </summary>
    [JsonPropertyName("nodes")]
    public IReadOnlyList<SankeyChartNode> Nodes { get; init; } = [];

    /// <summary>
    /// Gets the chart links.
    /// </summary>
    [JsonPropertyName("links")]
    public IReadOnlyList<SankeyChartLink> Links { get; init; } = [];
}
