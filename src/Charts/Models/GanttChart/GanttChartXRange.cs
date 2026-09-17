// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;
#pragma warning disable MA0048 // File name must match type name

/// <summary>
/// Represents the x-axis range of a <see cref="GanttChartDataPoint"/>.
/// Both <see cref="Start"/> and <see cref="End"/> accept numeric values
/// (e.g. Unix timestamps in milliseconds) as well as <see cref="DateTime"/> or
/// <see cref="DateTimeOffset"/> values that are serialized as ISO 8601 strings.
/// </summary>
public sealed record GanttChartXRange
{
    /// <summary>
    /// Gets the start value of the range along the x-axis.
    /// Assign a <see cref="double"/>, <see cref="DateTime"/>, or <see cref="DateTimeOffset"/>.
    /// </summary>
    [JsonPropertyName("start")]
    public ChartAxisValue Start { get; init; }

    /// <summary>
    /// Gets the end value of the range along the x-axis.
    /// Assign a <see cref="double"/>, <see cref="DateTime"/>, or <see cref="DateTimeOffset"/>.
    /// </summary>
    [JsonPropertyName("end")]
    public ChartAxisValue End { get; init; }
}

#pragma warning restore MA0048 // File name must match type name
