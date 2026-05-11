// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Enums;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// A FluentHorizontalBarChartWithAxis is a component that displays data in a horizontal bar chart format with an axis.
/// </summary>
public partial class FluentHorizontalBarChartWithAxis : FluentComponentBase
{
    /// <summary />
    public FluentHorizontalBarChartWithAxis(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    internal string? ClassValue => DefaultClassBuilder
       .AddClass("fluent-horizontal-bar-chart-with-axis")
       .Build();

    /// <summary />
    internal string? StyleValue => DefaultStyleBuilder
       .Build();

    /// <summary>
    /// Gets or sets the data for the horizontal bar chart.
    /// </summary>
    [Parameter, EditorRequired]
    public IReadOnlyList<HorizontalBarChartWithAxisDataPoint> ChartData { get; set; } = [];

    /// <summary>
    /// Gets or sets the title text displayed on the chart.
    /// </summary>
    [Parameter]
    public string? ChartTitle { get; set; }

    /// <summary>
    /// Gets or sets the height of the horizontal bar chart.
    /// </summary>
    [Parameter]
    public int? Height { get; set; }

    /// <summary>
    /// Gets or sets the width of the horizontal bar chart.
    /// </summary>
    [Parameter]
    public int? Width { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether legends are hidden in the component output.
    /// </summary>
    [Parameter]
    public bool HideLegends { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the tooltip is hidden.
    /// </summary>
    [Parameter]
    public bool HideTooltip { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the labels are hidden.
    /// </summary>
    [Parameter]
    public bool HideLabels { get; set; }

    /// <summary>
    /// Gets or sets the label displayed for the legend list.
    /// </summary>
    [Parameter]
    public string? LegendListLabel { get; set; }

    /// <summary>
    /// Gets or sets whether to use a single color for all bars in the chart.
    /// </summary>
    [Parameter]
    public bool UseSingleColor { get; set; }

    /// <summary>
    /// Gets or sets whether gradient rendering is enabled.
    /// </summary>
    [Parameter]
    public bool EnableGradient { get; set; }

    /// <summary>
    /// Gets or sets whether the bars in the chart should have rounded corners.
    /// </summary>
    [Parameter]
    public bool RoundedCorners { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether multiple legend items can be selected simultaneously.
    /// When <see langword="true"/>, clicking a legend item adds it to the active selection rather than replacing the current selection.
    /// When <see langword="false"/> (default), only a single legend item can be selected at a time.
    /// </summary>
    [Parameter]
    public bool AllowMultipleLegendSelection { get; set; }

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
    /// Defaults to <see cref="HorizontalBarChartWithAxisCategoryOrder.Default"/>.
    /// </summary>
    [Parameter]
    public HorizontalBarChartWithAxisCategoryOrder YAxisCategoryOrder { get; set; } = HorizontalBarChartWithAxisCategoryOrder.Default;

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

    /// <summary>
    /// Gets or sets the minimum value of the X axis domain.
    /// When not set, the domain minimum is derived from the data.
    /// </summary>
    [Parameter]
    public double? XMinValue { get; set; }

    /// <summary>
    /// Gets or sets the maximum value of the X axis domain.
    /// When not set, the domain maximum is derived from the data.
    /// </summary>
    [Parameter]
    public double? XMaxValue { get; set; }

    /// <summary>
    /// Gets or sets the minimum value of the Y axis domain (numeric axis only).
    /// </summary>
    [Parameter]
    public double? YMinValue { get; set; }

    /// <summary>
    /// Gets or sets the maximum value of the Y axis domain (numeric axis only).
    /// </summary>
    [Parameter]
    public double? YMaxValue { get; set; }

    /// <summary>
    /// Gets or sets the culture used for locale-aware number formatting of values in the chart.
    /// Defaults to <see cref="CultureInfo.CurrentCulture"/> to display using the OS culture.
    /// </summary>
    [Parameter]
    public CultureInfo Culture { get; set; } = CultureInfo.CurrentCulture;
}
