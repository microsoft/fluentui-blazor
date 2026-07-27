// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json;
using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// A FluentGanttChart is a component that displays data in a Gantt chart format, which is a type of horizontal bar chart
/// that illustrates a project schedule.
/// Each bar represents a task or activity, with the length of the bar corresponding to the duration of the task.
/// The x-axis typically represents time, while the y-axis lists the tasks or activities.
/// This component is useful for visualizing project timelines, task dependencies, and overall progress.
/// </summary>
public partial class FluentGanttChart : FluentCartesianChartBase
{
    /// <summary />
    public FluentGanttChart(LibraryConfiguration configuration) : base(configuration)
    {
    }

    /// <summary />
    internal string? ClassValue => DefaultClassBuilder
       .AddClass("fluent-gantt-chart")
       .Build();

    /// <summary>
    /// Gets or sets the data for the Gantt chart.
    /// </summary>
    [Parameter, EditorRequired]
    public IReadOnlyList<GanttChartDataPoint> ChartData { get; set; } = [];

    /// <summary>
    /// Gets or sets a value indicating whether a gradient fill is applied to the bars, arcs or areas.
    /// </summary>
    [Parameter]
    public bool EnableGradient { get; set; }

    /// <summary>
    /// Gets or sets whether to use a single color for all bars in the chart.
    /// </summary>
    [Parameter]
    public bool UseSingleColor { get; set; }

    /// <summary>
    /// Gets or sets the fixed height of each individual bar in pixels.
    /// When not set, the bar height is calculated automatically.
    /// </summary>
    [Parameter]
    public int? BarHeight { get; set; }

    /// <summary>
    /// Gets or sets the explicit set of x-axis tick values to render when the chart uses a
    /// <b>date</b> x-axis. When set, only the specified dates appear as tick marks instead of
    /// the auto-generated ones.
    /// For numeric axes, use the base-class <c>TickValues</c> property instead.
    /// </summary>
    [Parameter]
    public IEnumerable<DateTime>? DateTickValues { get; set; }

    /// <inheritdoc />
    /// <remarks>
    /// When <see cref="DateTickValues"/> is set, dates are converted to Unix millisecond timestamps
    /// before serialisation so the web component receives the expected numeric format.
    /// </remarks>
    internal override string? TickValuesJson =>
        DateTickValues is not null
            ? JsonSerializer.Serialize(DateTickValues.Select(d => (double)new DateTimeOffset(d).ToUnixTimeMilliseconds()), ChartJsonSerializerContext.Default.IEnumerableDouble)
            : base.TickValuesJson;
}
