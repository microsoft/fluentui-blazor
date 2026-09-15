// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// FluentSparklineChart displays compact trend data as a sparkline.
/// </summary>
public partial class FluentSparklineChart : FluentChartBase
{
    /// <summary />
    public FluentSparklineChart(LibraryConfiguration configuration) : base(configuration)
    {
    }

    /// <summary />
    internal string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-sparkline-chart")
        .Build();

    /// <summary>
    /// Gets or sets the data for the sparkline chart.
    /// </summary>
    [Parameter, EditorRequired]
    public SparklineChartData ChartData { get; set; } = new();

    /// <summary>
    /// Gets or sets the rendering variant used by the sparkline chart.
    /// </summary>
    [Parameter]
    public SparklineVariant? Variant { get; set; }

    /// <summary>
    /// Gets or sets the optional chart color override.
    /// </summary>
    [Parameter]
    public string? Color { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the legend is shown.
    /// </summary>
    [Parameter]
    public bool ShowLegend { get; set; }

    /// <summary>
    /// Gets or sets the optional width reserved for the value text.
    /// </summary>
    [Parameter]
    public double? ValueTextWidth { get; set; }
}
