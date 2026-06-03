// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Represents the data available to a custom tooltip template.
/// The properties mirror the data point object passed by the JavaScript
/// <c>tooltipRenderer</c> callback and are chart-type agnostic so that a single
/// <see cref="TooltipContext"/> can be used across all five chart types.
/// </summary>
public class TooltipContext
{
    /// <summary>
    /// Gets the legend label associated with the hovered data element.
    /// </summary>
    public string? Legend { get; init; }

    /// <summary>
    /// Gets the primary Y-axis (or value) formatted string for the hovered data element.
    /// </summary>
    public string? YValue { get; init; }

    /// <summary>
    /// Gets the secondary X-axis (or range) formatted string for the hovered data element.
    /// Only populated for axis-based charts (Gantt, HorizontalBarChartWithAxis).
    /// </summary>
    public string? XValue { get; init; }

    /// <summary>
    /// Gets the <see cref="DataVizPalette"/> color associated with the hovered data element,
    /// or <see langword="null"/> when the data point uses a custom color.
    /// Use <see cref="DataVizPaletteExtensions.ToDataVizPaletteHex(DataVizPalette?, string?, bool)"/> with
    /// <see cref="CustomColor"/> as the fallback to resolve the displayed color in both cases.
    /// </summary>
    public DataVizPalette? Color { get; init; }

    /// <summary>
    /// Gets the raw CSS color string (e.g. <c>#ff0000</c>) when the data point uses
    /// <see cref="DataVizPalette.Custom"/>. <see langword="null"/> when <see cref="Color"/> is set.
    /// </summary>
    public string? CustomColor { get; init; }

    /// <summary>
    /// Gets the raw JSON object serialized from the JavaScript data point.
    /// Use this when you need to access chart-specific properties not covered
    /// by the typed members above.
    /// </summary>
    public string? RawJson { get; init; }
}
