// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json;
using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// FluentGroupedVerticalBarChart displays data as clusters of vertical bars grouped by category,
/// allowing comparison of multiple series side by side within each category.
/// </summary>
public partial class FluentGroupedVerticalBarChart : FluentCartesianChartBase
{
    /// <summary />
    public FluentGroupedVerticalBarChart(LibraryConfiguration configuration) : base(configuration)
    {
    }

    /// <summary />
    internal string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-grouped-vertical-bar-chart")
        .Build();

    /// <summary>
    /// Gets or sets the data for the grouped vertical bar chart.
    /// </summary>
    [Parameter, EditorRequired]
    public IReadOnlyList<GroupedVerticalBarChartSeries> ChartData { get; set; } = [];

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
    /// Gets or sets a value indicating whether the tooltip aggregates all values in the group (stack)
    /// rather than showing only the hovered bar.
    /// </summary>
    [Parameter]
    public bool IsCalloutForStack { get; set; }

    /// <summary>
    /// Serializes <see cref="Colors"/> to a JSON array string for the web component attribute.
    /// </summary>
    internal string? ColorsJson =>
        Colors is not null ? JsonSerializer.Serialize(Colors, ChartJsonSerializerContext.Default.IEnumerableString) : null;
}
