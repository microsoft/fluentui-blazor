// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// FluentFunnelChart shows how quantities change through sequential steps, such as conversion or engagement stages.
/// </summary>
public partial class FluentFunnelChart : FluentChartBase
{
    /// <summary />
    public FluentFunnelChart(LibraryConfiguration configuration) : base(configuration)
    {
    }

    /// <summary />
    internal string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-funnel-chart")
        .Build();

    /// <summary>
    /// Gets or sets the data for the funnel chart.
    /// </summary>
    [Parameter, EditorRequired]
    public IReadOnlyList<FunnelDataPoint> ChartData { get; set; } = [];

    /// <summary>
    /// Gets or sets the orientation of the funnel chart.
    /// </summary>
    [Parameter]
    public Orientation Orientation { get; set; } = Orientation.Horizontal;
}
