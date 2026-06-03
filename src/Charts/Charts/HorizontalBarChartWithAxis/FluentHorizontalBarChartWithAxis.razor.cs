// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// A FluentHorizontalBarChartWithAxis is a component that displays data in a horizontal bar chart format with an axis.
/// </summary>
public partial class FluentHorizontalBarChartWithAxis : FluentCartesianChartBase
{
    /// <summary />
    public FluentHorizontalBarChartWithAxis(LibraryConfiguration configuration) : base(configuration)
    {
    }

    /// <summary />
    internal string? ClassValue => DefaultClassBuilder
       .AddClass("fluent-horizontal-bar-chart-with-axis")
       .Build();

    /// <summary>
    /// Gets or sets the data for the horizontal bar chart.
    /// </summary>
    [Parameter, EditorRequired]
    public IReadOnlyList<HorizontalBarChartWithAxisDataPoint> ChartData { get; set; } = [];

    /// <summary>
    /// Gets or sets whether to use a single color for all bars in the chart.
    /// </summary>
    [Parameter]
    public bool UseSingleColor { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the full Y-axis labels are shown.
    /// When <see langword="false"/> (default), long labels are truncated.
    /// </summary>
    [Parameter]
    public bool ShowYAxisLabels { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether a tooltip is shown on Y-axis labels when they are truncated.
    /// </summary>
    [Parameter]
    public bool ShowYAxisLabelsTooltip { get; set; }

    /// <summary>
    /// Gets or sets the sort order applied to categorical Y-axis groups.
    /// Defaults to <see cref="ChartCategoryOrder.Default"/>.
    /// </summary>
    [Parameter]
    public ChartCategoryOrder YAxisCategoryOrder { get; set; } = ChartCategoryOrder.Default;

    /// <summary>
    /// Gets or sets the fixed height of each individual bar in pixels.
    /// When not set, the bar height is calculated automatically.
    /// </summary>
    [Parameter]
    public int? BarHeight { get; set; }

    /// <summary>
    /// Gets or sets the number of tick marks on the X axis.
    /// </summary>
    [Parameter]
    public int? XAxisTickCount { get; set; }

    /// <summary>
    /// Gets or sets the number of tick marks on the Y axis (numeric axis only).
    /// </summary>
    [Parameter]
    public int? YAxisTickCount { get; set; }

    /// <summary>
    /// Gets or sets the fractional padding (0–1) between bars on the categorical Y axis.
    /// Defaults to 0.5.
    /// </summary>
    [Parameter]
    public double? YAxisPadding { get; set; }
}
