// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// FluentAreaChart displays data in an area chart format, allowing users to visualize trends over time or categories.
/// </summary>
public partial class FluentAreaChart
{
    /// <summary />
    public FluentAreaChart(LibraryConfiguration configuration) : base(configuration)
    {
    }

    /// <summary />
    internal string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-area-chart")
        .Build();

    /// <summary>
    /// Gets or sets the data for the area chart.
    /// </summary>
    [Parameter, EditorRequired]
    public IReadOnlyList<AreaChartSeries> ChartData { get; set; } = [];

    /// <summary>
    /// Gets or sets a value indicating whether a gradient fill is applied to the bars, arcs or areas.
    /// </summary>
    [Parameter]
    public bool EnableGradient { get; set; }

    /// <summary>
    /// Gets or sets the mode of the area chart, which determines how the areas are stacked or displayed.
    /// </summary>
    [Parameter]
    public AreaChartMode Mode { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to display the title for the secondary Y-axis.
    /// </summary>
    [Parameter]
    public bool SecondaryYAxisTitle { get; set; }

    /// <summary>
    /// Gets or sets the maximum width for the tick labels on the secondary Y-axis.
    /// </summary>
    [Parameter]
    public string? SecondaryYAxisTickLabelMaxWidth { get; set; }
}
