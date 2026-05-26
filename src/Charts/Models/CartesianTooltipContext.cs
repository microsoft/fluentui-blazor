// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Extends <see cref="TooltipContext"/> with axis-range properties for Cartesian charts
/// (<see cref="FluentGanttChart"/> and <see cref="FluentHorizontalBarChartWithAxis"/>).
/// </summary>
/// <remarks>
/// <para>
/// For <see cref="FluentGanttChart"/>, <see cref="XStart"/> and <see cref="XEnd"/> contain
/// the ISO 8601 representation of the bar's start and end dates (or numeric strings for
/// numeric axes). These can be parsed with <see cref="System.DateTime.Parse(string)"/> or
/// <see cref="double.Parse(string)"/> as appropriate.
/// </para>
/// <para>
/// For <see cref="FluentHorizontalBarChartWithAxis"/>, <see cref="XStart"/> and
/// <see cref="XEnd"/> are empty; the x-axis value is carried in <see cref="TooltipContext.XValue"/>.
/// </para>
/// </remarks>
public sealed class CartesianTooltipContext : TooltipContext
{
    /// <summary>
    /// Gets the start of the x-axis range for the hovered data element.
    /// For <see cref="FluentGanttChart"/> this is an ISO 8601 date string (e.g.
    /// <c>"2009-01-01T00:00:00.000Z"</c>).  Empty for other chart types.
    /// </summary>
    public string? XStart { get; init; }

    /// <summary>
    /// Gets the end of the x-axis range for the hovered data element.
    /// For <see cref="FluentGanttChart"/> this is an ISO 8601 date string (e.g.
    /// <c>"2009-02-28T00:00:00.000Z"</c>).  Empty for other chart types.
    /// </summary>
    public string? XEnd { get; init; }
}
