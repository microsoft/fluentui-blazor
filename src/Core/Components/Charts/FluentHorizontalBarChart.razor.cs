// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Enums;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// A FluentHorizontalBarChart is a component that displays data in a horizontal bar chart format.
/// </summary>
public partial class FluentHorizontalBarChart : FluentComponentBase
{
    /// <summary />
    public FluentHorizontalBarChart(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    // <summary />
    internal string? ClassValue => DefaultClassBuilder
       .AddClass("fluent-horizontal-bar-chart")
       .Build();

    // <summary />
    internal string? StyleValue => DefaultStyleBuilder
       .Build();

    internal string SerializedData => JsonSerializer.Serialize(ChartData, typeof(HorizontalBarChartSeries), HorizontalBarChartDataJsonSerializerContext.Default);

    /// <summary>
    /// Gets or sets the data for the horizontal bar chart.
    /// </summary>
    [Parameter, EditorRequired]
    public IReadOnlyList<HorizontalBarChartSeries>? ChartData { get; set; }

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
    /// Gets or sets the label displayed for the legend list.
    /// </summary>
    [Parameter]
    public string? LegendListLabel { get; set; }

    /// <summary>
    /// Gets or sets the title text displayed on the chart.
    /// </summary>
    [Parameter]
    public string? ChartTitle { get; set; }

    /// <summary>
    /// Gets a value indicating whether the component has data that can be rendered safely.
    /// </summary>
    protected bool HasRenderableData =>
        ChartData is { Count: > 0 };
}
