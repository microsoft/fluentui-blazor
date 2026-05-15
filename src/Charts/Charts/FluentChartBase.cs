// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Abstract base class for FluentUI chart components.
/// Provides parameters that are common across all chart types.
/// </summary>
public abstract partial class FluentChartBase : FluentComponentBase
{
    /// <summary />
    protected FluentChartBase(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
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
}
