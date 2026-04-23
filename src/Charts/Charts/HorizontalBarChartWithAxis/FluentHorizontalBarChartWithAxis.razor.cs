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
}
