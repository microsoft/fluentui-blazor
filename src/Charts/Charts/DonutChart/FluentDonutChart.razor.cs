// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Globalization;
using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// FluentDonutChart displays data in a donut chart format.
/// </summary>
public partial class FluentDonutChart : FluentChartBase
{

    /// <summary />
    public FluentDonutChart(LibraryConfiguration configuration) : base(configuration)
    {
    }

    /// <summary />
    internal string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-donut-chart")
        .Build();

    /// <summary />
    internal string? StyleValue => DefaultStyleBuilder
        .AddStyle("width", Width.HasValue ? string.Create(CultureInfo.InvariantCulture, $"{Width.Value}px") : null, when: Width.HasValue)
        .AddStyle("height", Height.HasValue ? string.Create(CultureInfo.InvariantCulture, $"{Height.Value}px") : null, when: Height.HasValue)
        .Build();

    /// <summary>
    /// Gets or sets the data for the donut chart.
    /// </summary>
    [Parameter, EditorRequired]
    public IReadOnlyList<DonutDataPoint> ChartData { get; set; } = [];

    /// <summary>
    /// Gets or sets the height of the donut chart.
    /// </summary>
    [Parameter]
    public int? Height { get; set; }

    /// <summary>
    /// Gets or sets the width of the donut chart.
    /// </summary>
    [Parameter]
    public int? Width { get; set; }

    /// <summary>
    /// Gets or sets whether label values should be displayed as percentages of the total rather than raw values.
    /// </summary>
    [Parameter]
    public bool ShowLabelsInPercent { get; set; }

    /// <summary>
    /// Gets or sets the inner radius of the component, in pixels.
    /// </summary>
    /// <remarks>If <see langword="null"/>, a default inner radius is used. The value must be non-negative.
    /// This property is typically used to control the thickness of ring-shaped visual elements.</remarks>
    [Parameter]
    public int? InnerRadius { get; set; }

    /// <summary>
    /// Gets or sets the value displayed inside the donut hole. This is typically used to show a summary
    /// or total value related to the data represented by the chart.
    /// </summary>
    [Parameter]
    public string? ValueInsideDonut { get; set; }
}
