// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Represents one colored x-axis range in a line chart fill bar.
/// </summary>
public sealed record LineChartColorFillBarData
{
    /// <summary>
    /// Gets the start x-axis value of the colored range.
    /// </summary>
    [JsonPropertyName("startX")]
    public ChartAxisValue StartX { get; init; }

    /// <summary>
    /// Gets the end x-axis value of the colored range.
    /// </summary>
    [JsonPropertyName("endX")]
    public ChartAxisValue EndX { get; init; }
}
