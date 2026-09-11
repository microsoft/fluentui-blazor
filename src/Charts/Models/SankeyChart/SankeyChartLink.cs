// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Represents a link between two nodes in a sankey chart payload.
/// </summary>
public sealed record SankeyChartLink
{
    /// <summary>
    /// Gets the source node index.
    /// </summary>
    [JsonPropertyName("source")]
    public int Source { get; init; }

    /// <summary>
    /// Gets the target node index.
    /// </summary>
    [JsonPropertyName("target")]
    public int Target { get; init; }

    /// <summary>
    /// Gets the link value.
    /// </summary>
    [JsonPropertyName("value")]
    public double Value { get; init; }
}
