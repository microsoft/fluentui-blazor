// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json;
using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// FluentHeatMapChart displays data as a colored Cartesian grid.
/// </summary>
public partial class FluentHeatMapChart
{
    /// <summary />
    public FluentHeatMapChart(LibraryConfiguration configuration) : base(configuration)
    {
    }

    /// <summary />
    internal string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-heat-map-chart")
        .Build();

    /// <summary>
    /// Gets or sets the data for the heat map chart.
    /// </summary>
    [Parameter, EditorRequired]
    public IReadOnlyList<HeatMapChartData> ChartData { get; set; } = [];

    /// <summary>
    /// Gets or sets the control points used for the color scale domain.
    /// </summary>
    [Parameter]
    public IEnumerable<double>? DomainValuesForColorScale { get; set; }

    /// <summary>
    /// Gets or sets the colors mapped to <see cref="DomainValuesForColorScale"/>.
    /// </summary>
    [Parameter]
    public IEnumerable<string>? RangeValuesForColorScale { get; set; }

    /// <summary>
    /// Gets or sets the d3 time-format string used for x-axis date labels.
    /// </summary>
    [Parameter]
    public string? XAxisDateFormatString { get; set; }

    /// <summary>
    /// Gets or sets the d3 time-format string used for y-axis date labels.
    /// </summary>
    [Parameter]
    public string? YAxisDateFormatString { get; set; }

    /// <summary>
    /// Gets or sets the d3 number-format string used for x-axis numeric labels.
    /// </summary>
    [Parameter]
    public string? XAxisNumberFormatString { get; set; }

    /// <summary>
    /// Gets or sets the d3 number-format string used for y-axis numeric labels.
    /// </summary>
    [Parameter]
    public string? YAxisNumberFormatString { get; set; }

    /// <summary>
    /// Gets or sets the maximum width for y-axis tick labels before truncation.
    /// </summary>
    [Parameter]
    public string? YAxisTickLabelMaxWidth { get; set; }

    /// <summary>
    /// Gets or sets the default sort order applied to string axis labels.
    /// </summary>
    [Parameter]
    public HeatMapSortOrder SortOrder { get; set; } = HeatMapSortOrder.Alphabetical;

    /// <summary>
    /// Gets or sets the dictionary of x-axis string keys to display labels.
    /// </summary>
    [Parameter]
    public IDictionary<string, string>? XAxisStringLabels { get; set; }

    /// <summary>
    /// Gets or sets the dictionary of y-axis string keys to display labels.
    /// </summary>
    [Parameter]
    public IDictionary<string, string>? YAxisStringLabels { get; set; }

    /// <summary>
    /// Serializes <see cref="DomainValuesForColorScale"/> to a JSON array string for the web component attribute.
    /// </summary>
    internal string? DomainValuesForColorScaleJson =>
        DomainValuesForColorScale is not null
            ? JsonSerializer.Serialize(DomainValuesForColorScale, ChartJsonSerializerContext.Default.IEnumerableDouble)
            : null;

    /// <summary>
    /// Serializes <see cref="RangeValuesForColorScale"/> to a JSON array string for the web component attribute.
    /// </summary>
    internal string? RangeValuesForColorScaleJson =>
        RangeValuesForColorScale is not null
            ? JsonSerializer.Serialize(RangeValuesForColorScale, ChartJsonSerializerContext.Default.IEnumerableString)
            : null;

    /// <summary>
    /// Serializes <see cref="XAxisStringLabels"/> to a JSON object string for the web component attribute.
    /// </summary>
    internal string? XAxisStringLabelsJson =>
        XAxisStringLabels is not null
            ? JsonSerializer.Serialize(XAxisStringLabels, ChartJsonSerializerContext.Default.IDictionaryStringString)
            : null;

    /// <summary>
    /// Serializes <see cref="YAxisStringLabels"/> to a JSON object string for the web component attribute.
    /// </summary>
    internal string? YAxisStringLabelsJson =>
        YAxisStringLabels is not null
            ? JsonSerializer.Serialize(YAxisStringLabels, ChartJsonSerializerContext.Default.IDictionaryStringString)
            : null;
}
