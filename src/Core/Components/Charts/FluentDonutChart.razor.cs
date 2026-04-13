// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// A FluentDonutChart is a component that displays data in a donut chart format.
/// </summary>
public partial class FluentDonutChart : FluentComponentBase
{

    /// <summary />
    public FluentDonutChart(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    internal string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-donut-chart")
        .Build();

    /// <summary />
    internal string? StyleValue => DefaultStyleBuilder
        .AddStyle("width", Width.HasValue ? $"{Width.Value}px" : null, when: Width.HasValue)
        .AddStyle("height", Height.HasValue ? $"{Height.Value}px" : null, when: Height.HasValue)
        .Build();

    internal string SerializedData => JsonSerializer.Serialize(ChartData, typeof(DonutChartData), DonutChartDataJsonSerializerContext.Default);

    /// <summary>
    /// Gets or sets the data for the donut chart.
    /// </summary>
    [Parameter, EditorRequired]
    public DonutChartData ChartData { get; set; } = new();

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
    /// Gets or sets a value indicating whether legends are hidden in the component output.
    /// </summary>
    [Parameter]
    public bool HideLegends { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the tooltip is hidden.
    /// </summary>
    [Parameter]
    public bool HideTooltip { get; set; }

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

    /// <summary>
    /// Gets or sets the label text displayed for the legend list.
    /// </summary>
    [Parameter]
    public string? LegendListLabel { get; set; }
}
