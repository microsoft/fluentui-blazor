// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// FluentScatterChart displays data as one or more scatter series on Cartesian axes.
/// </summary>
public partial class FluentScatterChart
{
    /// <summary />
    public FluentScatterChart(LibraryConfiguration configuration) : base(configuration)
    {
    }

    /// <summary />
    internal string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-scatter-chart")
        .Build();

    /// <summary>
    /// Gets or sets the data for the scatter chart.
    /// </summary>
    [Parameter, EditorRequired]
    public IReadOnlyList<ScatterChartSeries> ChartData { get; set; } = [];
}
