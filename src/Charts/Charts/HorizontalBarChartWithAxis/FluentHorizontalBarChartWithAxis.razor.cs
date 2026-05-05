// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
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
}
