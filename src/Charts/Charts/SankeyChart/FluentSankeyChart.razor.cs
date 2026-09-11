// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// FluentSankeyChart displays flow values between linked nodes.
/// </summary>
public partial class FluentSankeyChart : FluentChartBase
{
    /// <summary />
    public FluentSankeyChart(LibraryConfiguration configuration) : base(configuration)
    {
    }

    /// <summary />
    internal string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-sankey-chart")
        .Build();

    /// <summary>
    /// Gets or sets the data for the sankey chart.
    /// </summary>
    [Parameter, EditorRequired]
    public SankeyChartData ChartData { get; set; } = new();

    /// <summary>
    /// Gets or sets the optional stroke color applied to chart paths.
    /// </summary>
    [Parameter]
    public string? PathColor { get; set; }
}
