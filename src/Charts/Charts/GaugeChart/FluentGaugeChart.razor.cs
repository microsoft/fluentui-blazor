// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// FluentGaugeChart displays progress or ranges on a semicircular gauge.
/// </summary>
public partial class FluentGaugeChart : FluentChartBase
{
    /// <summary />
    public FluentGaugeChart(LibraryConfiguration configuration) : base(configuration)
    {
    }

    /// <summary />
    internal string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-gauge-chart")
        .Build();

    /// <summary>
    /// Gets or sets the segments rendered by the gauge chart.
    /// </summary>
    [Parameter, EditorRequired]
    public IReadOnlyList<GaugeChartSegment> Segments { get; set; } = [];

    /// <summary>
    /// Gets or sets the current gauge value.
    /// </summary>
    [Parameter]
    public double ChartValue { get; set; }

    /// <summary>
    /// Gets or sets the minimum value shown on the gauge scale.
    /// </summary>
    [Parameter]
    public double MinValue { get; set; }

    /// <summary>
    /// Gets or sets the optional maximum value shown on the gauge scale.
    /// </summary>
    [Parameter]
    public double? MaxValue { get; set; }

    /// <summary>
    /// Gets or sets the optional label rendered below the center value.
    /// </summary>
    [Parameter]
    public string? Sublabel { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the min and max labels are hidden.
    /// </summary>
    [Parameter]
    public bool HideMinMax { get; set; }

    /// <summary>
    /// Gets or sets the format used for the center value label.
    /// </summary>
    [Parameter]
    public GaugeValueFormat? ChartValueFormat { get; set; }

    /// <summary>
    /// Gets or sets the template used to format the center value label.
    /// </summary>
    [Parameter]
    public string? ChartValueFormatTemplate { get; set; }

    /// <summary>
    /// Gets or sets the visual variant of the gauge chart.
    /// </summary>
    [Parameter]
    public GaugeChartVariant? Variant { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether gradient fills are enabled for the segments.
    /// </summary>
    [Parameter]
    public bool EnableGradient { get; set; }
}
