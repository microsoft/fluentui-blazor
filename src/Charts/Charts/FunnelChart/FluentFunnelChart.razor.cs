// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Globalization;
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

    /// <summary />
    internal string? StyleValue => DefaultStyleBuilder
        .AddStyle("width", Width.HasValue ? string.Create(CultureInfo.InvariantCulture, $"{Width.Value}px") : null, when: Width.HasValue)
        .AddStyle("height", Height.HasValue ? string.Create(CultureInfo.InvariantCulture, $"{Height.Value}px") : null, when: Height.HasValue)
        .Build();

    /// <summary>
    /// Gets or sets the data for the funnel chart.
    /// </summary>
    [Parameter, EditorRequired]
    public IReadOnlyList<FunnelDataPoint> ChartData { get; set; } = [];

    /// <summary>
    /// Gets or sets the height of the funnel chart.
    /// </summary>
    [Parameter]
    public int? Height { get; set; }

    /// <summary>
    /// Gets or sets the width of the funnel chart.
    /// </summary>
    [Parameter]
    public int? Width { get; set; }

    /// <summary>
    /// Gets or sets the orientation of the funnel chart.
    /// </summary>
    [Parameter]
    public Orientation Orientation { get; set; } = Orientation.Horizontal;
}
