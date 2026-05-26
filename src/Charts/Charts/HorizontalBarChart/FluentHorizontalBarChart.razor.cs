// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// A FluentHorizontalBarChart is a component that displays data in a horizontal bar chart format.
/// </summary>
public partial class FluentHorizontalBarChart : FluentChartBase
{
    /// <summary />
    public FluentHorizontalBarChart(LibraryConfiguration configuration) : base(configuration)
    {
    }

    /// <summary />
    internal string? ClassValue => DefaultClassBuilder
       .AddClass("fluent-horizontal-bar-chart")
       .Build();

    /// <summary>
    /// Gets or sets the data for the horizontal bar chart.
    /// </summary>
    [Parameter, EditorRequired]
    public IReadOnlyList<HorizontalBarChartSeries> ChartData { get; set; } = [];

    /// <summary>
    /// Gets or sets the visual <see cref="HorizontalBarChartVariant"/> variant to use for rendering
    /// the horizontal bar chart.
    /// </summary>
    /// <remarks>
    /// Specify this property to control the appearance or style of the chart.
    /// If not set, the default variant is used.
    /// </remarks>
    [Parameter]
    public HorizontalBarChartVariant? Variant { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the ratio is hidden in the component output.
    /// </summary>
    [Parameter]
    public bool HideRatio { get; set; }

    /// <summary>
    /// Gets or sets the chart data mode. Accepted values are <c>"default"</c>, <c>"fraction"</c>, and <c>"percentage"</c>.
    /// </summary>
    [Parameter]
    public string? ChartDataMode { get; set; }

    /// <summary>
    /// Gets a value indicating whether the component has data that can be rendered safely.
    /// </summary>
    protected bool HasRenderableData =>
        ChartData is { Count: > 0 };
}
