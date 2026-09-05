// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json;
using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// FluentPolarChart displays data using radial and angular axes.
/// </summary>
public partial class FluentPolarChart : FluentChartBase
{
    /// <summary />
    public FluentPolarChart(LibraryConfiguration configuration) : base(configuration)
    {
    }

    /// <summary />
    internal string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-polar-chart")
        .Build();

    /// <summary>
    /// Gets or sets the data for the polar chart.
    /// </summary>
    [Parameter, EditorRequired]
    public IReadOnlyList<PolarChartSeries> ChartData { get; set; } = [];

    /// <summary>
    /// Gets or sets a value indicating whether markers are shown for each point.
    /// </summary>
    [Parameter]
    public bool ShowMarkers { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether grouped multi-value tooltips are enabled.
    /// </summary>
    [Parameter]
    public bool EnableMultiValueCallout { get; set; }

    /// <summary>
    /// Gets or sets the grid outline shape.
    /// </summary>
    [Parameter]
    public PolarChartShape? Shape { get; set; }

    /// <summary>
    /// Gets or sets the angular rendering direction.
    /// </summary>
    [Parameter]
    public PolarChartDirection? Direction { get; set; }

    /// <summary>
    /// Gets or sets the size of the inner hole as a ratio from 0 to 1.
    /// </summary>
    [Parameter]
    public double? Hole { get; set; }

    /// <summary>
    /// Gets or sets the radial axis options.
    /// </summary>
    [Parameter]
    public PolarAxisOptions? RadialAxis { get; set; }

    /// <summary>
    /// Gets or sets the angular axis options.
    /// </summary>
    [Parameter]
    public PolarAxisOptions? AngularAxis { get; set; }

    /// <summary>
    /// Gets or sets explicit chart margins in pixels.
    /// </summary>
    [Parameter]
    public PolarChartMargins? Margins { get; set; }

    /// <summary>
    /// Gets or sets the locale-aware date formatting options for date axis values.
    /// </summary>
    [Parameter]
    public IDictionary<string, string>? DateLocalizeOptions { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether date axis values are formatted in UTC.
    /// </summary>
    [Parameter]
    public bool UseUTC { get; set; }

    /// <summary>
    /// Serializes <see cref="RadialAxis"/> to a JSON object string for the web component attribute.
    /// </summary>
    internal string? RadialAxisJson =>
        RadialAxis is not null
            ? JsonSerializer.Serialize(RadialAxis, PolarChartDataJsonSerializerContext.Default.PolarAxisOptions)
            : null;

    /// <summary>
    /// Serializes <see cref="AngularAxis"/> to a JSON object string for the web component attribute.
    /// </summary>
    internal string? AngularAxisJson =>
        AngularAxis is not null
            ? JsonSerializer.Serialize(AngularAxis, PolarChartDataJsonSerializerContext.Default.PolarAxisOptions)
            : null;

    /// <summary>
    /// Serializes <see cref="Margins"/> to a JSON object string for the web component attribute.
    /// </summary>
    internal string? MarginsJson =>
        Margins is not null
            ? JsonSerializer.Serialize(Margins, PolarChartDataJsonSerializerContext.Default.PolarChartMargins)
            : null;

    /// <summary>
    /// Serializes <see cref="DateLocalizeOptions"/> to a JSON object string for the web component attribute.
    /// </summary>
    internal string? DateLocalizeOptionsJson =>
        DateLocalizeOptions is not null
            ? JsonSerializer.Serialize(DateLocalizeOptions, PolarChartDataJsonSerializerContext.Default.IDictionaryStringString)
            : null;
}
