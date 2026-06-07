// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Abstract base class for FluentUI chart components.
/// Provides parameters that are common across all chart types.
/// </summary>
public abstract partial class FluentChartBase : FluentComponentBase, IAsyncDisposable
{
    private const string TOOLTIP_BRIDGE_JS = "./_content/Microsoft.FluentUI.AspNetCore.Components.Charts/js/chart-tooltip-bridge.js";

    private IJSObjectReference? _jsModule;
    private DotNetObjectReference<FluentChartBase>? _dotNetRef;

    /// <summary>The stable DOM id of the hidden tooltip portal div rendered by each chart.</summary>
    internal string _tooltipPortalId = string.Empty;

    /// <summary>The current tooltip context passed to <see cref="TooltipTemplate"/>.</summary>
    internal TooltipContext _tooltipContext = new();

    /// <summary />
    [Inject]
    private new IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary />
    protected FluentChartBase(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
        _tooltipPortalId = $"{Id}-tooltip-portal";
    }

    /// <summary>
    /// Gets or sets the title text displayed on the chart.
    /// </summary>
    [Parameter]
    public string? ChartTitle { get; set; }

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
    /// Gets or sets a value indicating whether bar or arc labels are hidden.
    /// </summary>
    [Parameter]
    public bool HideLabels { get; set; }

    /// <summary>
    /// Gets or sets the label displayed for the legend list.
    /// </summary>
    [Parameter]
    public string? LegendListLabel { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether bars or arcs are rendered with rounded corners.
    /// </summary>
    [Parameter]
    public bool RoundedCorners { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether a gradient fill is applied to the bars or arcs.
    /// </summary>
    [Parameter]
    public bool EnableGradient { get; set; }

    /// <summary>
    /// Gets or sets the width of the chart. Accepts any valid CSS size value (e.g. <c>"400px"</c>, <c>"100%"</c>).
    /// When <see langword="null"/>, the chart sizes itself automatically.
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// Gets or sets the height of the chart. Accepts any valid CSS size value (e.g. <c>"300px"</c>, <c>"50vh"</c>).
    /// When <see langword="null"/>, the chart sizes itself automatically.
    /// </summary>
    [Parameter]
    public string? Height { get; set; }

    /// <summary />
    internal virtual string? StyleValue => DefaultStyleBuilder
        .AddStyle("width", Width, when: Width is not null)
        .AddStyle("height", Height, when: Height is not null)
        .Build();

    /// <summary>
    /// Gets or sets a value indicating whether multiple legend items can be selected simultaneously.
    /// When <see langword="true"/>, clicking a legend item adds it to the active selection rather than replacing the current selection.
    /// When <see langword="false"/> (default), only a single legend item can be selected at a time.
    /// </summary>
    [Parameter]
    public bool AllowMultipleLegendSelection { get; set; }

    /// <summary>
    /// Gets or sets the culture used for locale-aware number formatting of values in the chart.
    /// Defaults to <see cref="CultureInfo.CurrentCulture"/> to display using the OS culture.
    /// </summary>
    [Parameter]
    public CultureInfo? Culture { get; set; }// = CultureInfo.CurrentCulture;

    /// <summary>
    /// Gets or sets a custom tooltip template rendered when the user hovers over a chart element.
    /// When set, the Blazor-rendered content of this template is injected into the chart's tooltip
    /// via a JavaScript bridge, replacing the default tooltip body.
    /// The <see cref="TooltipContext"/> argument contains the data for the hovered element.
    /// </summary>
    [Parameter]
    public RenderFragment<TooltipContext>? TooltipTemplate { get; set; }

    /// <summary>
    /// Returns <see langword="true"/> when a tooltip template is set on this component.
    /// Override in subclasses that expose additional template parameters.
    /// </summary>
    protected virtual bool HasTooltipTemplate => TooltipTemplate is not null;

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && HasTooltipTemplate)
        {
            _jsModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", TOOLTIP_BRIDGE_JS);
            _dotNetRef ??= DotNetObjectReference.Create(this);
            await _jsModule.InvokeVoidAsync("initTooltipBridge", Id, _tooltipPortalId, _dotNetRef);
        }
    }

    /// <summary>
    /// Called from JavaScript when the tooltip data changes so that Blazor can update
    /// <see cref="_tooltipContext"/> and re-render the portal div with the new content.
    /// </summary>
    /// <param name="legend">Legend label of the hovered element.</param>
    /// <param name="yValue">Y-axis / value string of the hovered element.</param>
    /// <param name="xValue">X-axis / range string (axis charts only).</param>
    /// <param name="color">CSS color string of the hovered element.</param>
    /// <param name="rawJson">Full data-point JSON for chart-specific access.</param>
    /// <param name="xStart">Start of the x-axis range as an ISO 8601 string (GanttChart only).</param>
    /// <param name="xEnd">End of the x-axis range as an ISO 8601 string (GanttChart only).</param>
    [JSInvokable]
    public async Task UpdateTooltipContextAsync(string? legend, string? yValue, string? xValue, string? color, string? rawJson, string? xStart, string? xEnd)
    {
        _tooltipContext = BuildTooltipContext(legend, yValue, xValue, color, rawJson, xStart, xEnd);
        await InvokeAsync(StateHasChanged);
        // The JS MutationObserver on the portal div detects the DOM update and pushes
        // the new content into the chart's shadow DOM automatically.
    }

    /// <summary>
    /// Builds the <see cref="TooltipContext"/> (or a subclass) from the JavaScript callback arguments.
    /// Override in subclasses to return a richer context type.
    /// </summary>
    protected virtual TooltipContext BuildTooltipContext(
        string? legend, string? yValue, string? xValue, string? color, string? rawJson,
        string? xStart, string? xEnd)
    {
        var palette = DataVizPaletteExtensions.TryGetDataVizPaletteFromToken(color);
        return new TooltipContext
        {
            Legend = legend,
            YValue = yValue,
            XValue = xValue,
            Color = palette,
            CustomColor = palette is null ? color : null,
            RawJson = rawJson,
        };
    }

    /// <inheritdoc />
    public override async ValueTask DisposeAsync()
    {
        if (_jsModule is not null)
        {
            try
            {
                await _jsModule.InvokeVoidAsync("destroyTooltipBridge", Id);
                await _jsModule.DisposeAsync();
            }
            catch (Exception ex) when (ex is JSDisconnectedException || ex is OperationCanceledException)
            {
                // Client disconnected — safe to ignore.
            }
        }

        _dotNetRef?.Dispose();
        GC.SuppressFinalize(this);
    }
}
