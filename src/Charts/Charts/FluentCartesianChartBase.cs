// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

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
}
