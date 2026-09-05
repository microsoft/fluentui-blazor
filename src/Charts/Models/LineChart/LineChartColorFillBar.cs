// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Represents a color fill bar rendered behind matching line chart ranges.
/// </summary>
public sealed record LineChartColorFillBar
{
    /// <summary>
    /// Gets the legend text associated with the fill bar.
    /// </summary>
    [JsonPropertyName("legend")]
    public string Legend { get; init; } = string.Empty;

    /// <summary>
    /// Gets the CSS color value used to render the fill bar.
    /// </summary>
    [JsonPropertyName("color")]
    public string Color { get; init; } = string.Empty;

    /// <summary>
    /// Gets the colored x-axis ranges rendered for this fill bar.
    /// </summary>
    [JsonPropertyName("data")]
    public IReadOnlyList<LineChartColorFillBarData> Data { get; init; } = [];

    /// <summary>
    /// Gets whether the fill bar should use a patterned fill.
    /// </summary>
    [JsonPropertyName("applyPattern")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ApplyPattern { get; init; }
}
