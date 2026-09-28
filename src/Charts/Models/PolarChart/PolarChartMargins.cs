// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Represents explicit margins for a polar chart.
/// </summary>
public sealed record PolarChartMargins
{
    /// <summary>
    /// Gets the top margin in pixels.
    /// </summary>
    [JsonPropertyName("top")]
    public double Top { get; init; }

    /// <summary>
    /// Gets the right margin in pixels.
    /// </summary>
    [JsonPropertyName("right")]
    public double Right { get; init; }

    /// <summary>
    /// Gets the bottom margin in pixels.
    /// </summary>
    [JsonPropertyName("bottom")]
    public double Bottom { get; init; }

    /// <summary>
    /// Gets the left margin in pixels.
    /// </summary>
    [JsonPropertyName("left")]
    public double Left { get; init; }
}
