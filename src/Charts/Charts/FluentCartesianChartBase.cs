// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Abstract base class for FluentUI chart components that use Cartesian axes (x/y).
/// Extends <see cref="FluentChartBase"/> with axis-specific parameters for titles,
/// tick formatting, domain clamping, and label layout.
/// Only Cartesian charts (e.g. <see cref="FluentHorizontalBarChartWithAxis"/>) should
/// inherit from this class; non-axis charts (DonutChart, FunnelChart, HorizontalBarChart)
/// extend <see cref="FluentChartBase"/> directly.
/// </summary>
public abstract partial class FluentCartesianChartBase : FluentChartBase
{
    /// <summary />
    protected FluentCartesianChartBase(LibraryConfiguration configuration) : base(configuration)
    {
        // Pre-seed with a CartesianTooltipContext so the portal div cast is always safe.
        _tooltipContext = new CartesianTooltipContext();
    }

    /// <summary>
    /// Gets or sets a custom tooltip template rendered when the user hovers over a chart element.
    /// Use this in preference to the base <c>TooltipTemplate</c> for Cartesian charts to receive
    /// a <see cref="CartesianTooltipContext"/> that exposes <see cref="CartesianTooltipContext.XStart"/>
    /// and <see cref="CartesianTooltipContext.XEnd"/> in addition to the base properties.
    /// </summary>
    [Parameter]
    public RenderFragment<CartesianTooltipContext>? CartesianTooltipTemplate { get; set; }

    /// <inheritdoc />
    protected override bool HasTooltipTemplate => base.HasTooltipTemplate || CartesianTooltipTemplate is not null;

    /// <inheritdoc />
    protected override TooltipContext BuildTooltipContext(
        string? legend, string? yValue, string? xValue, string? color, string? rawJson,
        string? xStart, string? xEnd)
    {
        var palette = DataVizPaletteExtensions.TryGetDataVizPaletteFromToken(color);
        return new CartesianTooltipContext
        {
            Legend = legend,
            YValue = yValue,
            XValue = xValue,
            Color = palette,
            CustomColor = palette is null ? color : null,
            RawJson = rawJson,
            XStart = xStart,
            XEnd = xEnd,
        };
    }

    /// <summary>
    /// Gets or sets the label rendered beneath the x-axis.
    /// </summary>
    [Parameter]
    public string? XAxisTitle { get; set; }

    /// <summary>
    /// Gets or sets the label rendered beside the y-axis.
    /// </summary>
    [Parameter]
    public string? YAxisTitle { get; set; }

    /// <summary>
    /// Gets or sets a d3 format string (e.g. <c>'.2f'</c>, <c>'+,.0f'</c>) used to format
    /// x-axis number tick labels. Has no effect on date-type axes.
    /// </summary>
    [Parameter]
    public string? XAxisTickFormat { get; set; }

    /// <summary>
    /// Gets or sets a d3 format string (e.g. <c>'.2f'</c>, <c>'+,.0f'</c>) used to format
    /// y-axis number tick labels.
    /// </summary>
    [Parameter]
    public string? YAxisTickFormat { get; set; }

    /// <summary>
    /// Gets or sets the gap in pixels between axis tick lines and their text labels.
    /// Defaults to 6.
    /// </summary>
    [Parameter]
    public int? TickPadding { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether long x-axis text labels are wrapped onto
    /// multiple lines instead of being truncated.
    /// </summary>
    [Parameter]
    public bool WrapXAxisLabels { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether x-axis text labels are rotated 45° to reduce overlap.
    /// </summary>
    [Parameter]
    public bool RotateXAxisLabels { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the value axis is allowed to extend below zero
    /// when data contains negative values.
    /// When <see langword="false"/> (default), the domain is clamped to a minimum of 0.
    /// </summary>
    [Parameter]
    public bool SupportNegativeData { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the auto-generated axis domain is rounded to
    /// "nice" values (equivalent to calling d3's <c>.nice()</c> on the tick scale).
    /// </summary>
    [Parameter]
    public bool RoundedTicks { get; set; }

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
    /// When not set, the domain minimum is derived from the data.
    /// </summary>
    [Parameter]
    public double? YMinValue { get; set; }

    /// <summary>
    /// Gets or sets the maximum value of the Y axis domain (numeric axis only).
    /// When not set, the domain maximum is derived from the data.
    /// </summary>
    [Parameter]
    public double? YMaxValue { get; set; }

    /// <summary>
    /// Gets or sets the explicit set of x-axis tick values to render.
    /// When set, only the specified values appear as tick marks instead of the auto-generated ones.
    /// Use <c>double[]</c> for numeric axes. For date axes on <see cref="FluentGanttChart"/>,
    /// use <c>DateTickValues</c> instead.
    /// </summary>
    [Parameter]
    public IEnumerable<double>? TickValues { get; set; }

    /// <summary>
    /// Gets or sets a d3-time-format specifier string (e.g. <c>"%m/%d"</c>, <c>"%Y-%m"</c>) for date x-axis tick labels.
    /// Only applicable when the x-axis uses a date/time scale (e.g. in <see cref="FluentGanttChart"/>).
    /// When set, this overrides the locale-aware <c>DateLocalizeOptions</c> / <c>Culture</c> fallback.
    /// </summary>
    [Parameter]
    public string? TickFormat { get; set; }

    /// <summary>
    /// Gets or sets the pixel width of the stroke (outline) drawn on each bar.
    /// When not set, no stroke is applied.
    /// </summary>
    [Parameter]
    public double? StrokeWidth { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether a tooltip is shown on x-axis labels when they are
    /// truncated. Truncation occurs when a label exceeds the maximum display length.
    /// </summary>
    [Parameter]
    public bool ShowXAxisLabelsTooltip { get; set; }

    /// <summary>
    /// Gets or sets the <c>Intl.DateTimeFormatOptions</c>-equivalent formatting options applied
    /// to date x-axis tick labels. Keys and values correspond to the JavaScript
    /// <a href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Intl/DateTimeFormat/DateTimeFormat">
    /// <c>Intl.DateTimeFormat</c></a> options object (e.g. <c>{ "year", "numeric" }, { "month", "short" }</c>).
    /// When not set, the component auto-selects an appropriate format based on the visible date range.
    /// Only applicable when the x-axis uses a date/time scale.
    /// </summary>
    [Parameter]
    public IDictionary<string, string>? DateLocalizeOptions { get; set; }

    // ── Computed JSON helpers ─────────────────────────────────────────────────

    /// <summary>
    /// Serializes <see cref="TickValues"/> to a JSON array string for the web component attribute.
    /// Overridden by <see cref="FluentGanttChart"/> to also handle date tick values.
    /// </summary>
    internal virtual string? TickValuesJson =>
        TickValues is not null ? JsonSerializer.Serialize(TickValues, ChartJsonSerializerContext.Default.IEnumerableDouble) : null;

    /// <summary>
    /// Serializes <see cref="DateLocalizeOptions"/> to a JSON object string for the web component attribute.
    /// </summary>
    internal string? DateLocalizeOptionsJson =>
        DateLocalizeOptions is not null ? JsonSerializer.Serialize(DateLocalizeOptions, ChartJsonSerializerContext.Default.IDictionaryStringString) : null;
}

[JsonSerializable(typeof(IEnumerable<double>))]
[JsonSerializable(typeof(double[]))]
[JsonSerializable(typeof(List<double>))]
[JsonSerializable(typeof(IDictionary<string, string>))]
[JsonSerializable(typeof(Dictionary<string, string>))]
[JsonSourceGenerationOptions(WriteIndented = false)]
[ExcludeFromCodeCoverage(Justification = "This class is used for source-generated JSON serialization and does not contain any logic to be tested.")]
#pragma warning disable MA0048 // File name must match type name
internal partial class ChartJsonSerializerContext : JsonSerializerContext
#pragma warning restore MA0048 // File name must match type name
{
}
