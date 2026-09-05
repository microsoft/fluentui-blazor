// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json;
using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// FluentLineChart displays data as one or more connected line series on Cartesian axes.
/// </summary>
public partial class FluentLineChart
{
    /// <summary />
    public FluentLineChart(LibraryConfiguration configuration) : base(configuration)
    {
    }

    /// <summary />
    internal string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-line-chart")
        .Build();

    /// <summary>
    /// Gets or sets the data for the line chart.
    /// </summary>
    [Parameter, EditorRequired]
    public IReadOnlyList<LineChartSeries> ChartData { get; set; } = [];

    /// <summary>
    /// Gets or sets a value indicating whether markers are rendered for individual points.
    /// </summary>
    [Parameter]
    public bool ShowMarkers { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether multiple point shapes may be used across series.
    /// </summary>
    [Parameter]
    public bool AllowMultipleShapesForPoints { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the callout is rendered using stacked content.
    /// </summary>
    [Parameter]
    public bool IsCalloutForStack { get; set; }

    /// <summary>
    /// Gets or sets the optional colored bars rendered behind x-axis ranges.
    /// </summary>
    [Parameter]
    public IReadOnlyList<LineChartColorFillBar>? ColorFillBars { get; set; }

    /// <summary>
    /// Gets or sets the maximum width for the primary y-axis tick labels.
    /// </summary>
    [Parameter]
    public string? YAxisTickLabelMaxWidth { get; set; }

    /// <summary>
    /// Gets or sets the label rendered beside the secondary y-axis when one or more series use
    /// <see cref="LineChartSeries.UseSecondaryYScale"/>.
    /// </summary>
    [Parameter]
    public string? SecondaryYAxisTitle { get; set; }

    /// <summary>
    /// Gets or sets the pre-serialized JSON payload passed to the web component's
    /// <c>event-annotation-props</c> attribute.
    /// The value must match the TypeScript <c>LineChartEventAnnotationProps</c> shape.
    /// </summary>
    [Parameter]
    public string? EventAnnotationProps { get; set; }

    /// <summary>
    /// Serializes <see cref="ColorFillBars"/> to a JSON array string for the web component attribute.
    /// </summary>
    internal string? ColorFillBarsJson =>
        ColorFillBars is not null
            ? JsonSerializer.Serialize(ColorFillBars, LineChartDataJsonSerializerContext.Default.IReadOnlyListLineChartColorFillBar)
            : null;
}
