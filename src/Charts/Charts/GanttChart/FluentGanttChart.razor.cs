// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

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
}
