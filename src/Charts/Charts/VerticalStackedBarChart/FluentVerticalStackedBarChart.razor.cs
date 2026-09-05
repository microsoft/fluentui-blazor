// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json;
using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// FluentVerticalStackedBarChart displays data as a series of vertically stacked bars,
/// where each bar is divided into segments representing a part-to-whole relationship.
/// </summary>
public partial class FluentVerticalStackedBarChart : FluentCartesianChartBase
{
    /// <summary />
    public FluentVerticalStackedBarChart(LibraryConfiguration configuration) : base(configuration)
    {
    }

    /// <summary />
    internal string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-vertical-stacked-bar-chart")
        .Build();

    /// <summary>
    /// Gets or sets the data for the vertical stacked bar chart.
    /// </summary>
    [Parameter, EditorRequired]
    public IReadOnlyList<VerticalStackedBarChartSeries> ChartData { get; set; } = [];

    /// <summary>
    /// Gets or sets the width of each bar. Use <see langword="null"/> to fill the available band automatically.
    /// </summary>
    [Parameter]
    public string? BarWidth { get; set; }

    /// <summary>
    /// Gets or sets the maximum width of each bar when its width is calculated automatically.
    /// </summary>
    [Parameter]
    public string? MaxBarWidth { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the first resolved series color is used for every bar.
    /// </summary>
    [Parameter]
    public bool UseSingleColor { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether a gradient fill is applied to the bars.
    /// </summary>
    [Parameter]
    public bool EnableGradient { get; set; }

    /// <summary>
    /// Gets or sets the ordered color palette used when a data point does not provide a color.
    /// </summary>
    [Parameter]
    public IReadOnlyList<string>? Colors { get; set; }

    /// <summary>
    /// Gets or sets the maximum gap in pixels between bars when computed automatically.
    /// </summary>
    [Parameter]
    public string? BarGapMax { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the tooltip aggregates all segment values in the stack
    /// rather than showing only the hovered segment.
    /// </summary>
    [Parameter]
    public bool IsCalloutForStack { get; set; }

    /// <summary>
    /// Serializes <see cref="Colors"/> to a JSON array string for the web component attribute.
    /// </summary>
    internal string? ColorsJson =>
        Colors is not null ? JsonSerializer.Serialize(Colors, ChartJsonSerializerContext.Default.IEnumerableString) : null;
}
