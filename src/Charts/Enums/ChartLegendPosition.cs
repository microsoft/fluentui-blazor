// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>Specifies the position of a chart legend.</summary>
public enum ChartLegendPosition
{
    /// <summary>Render the legend above the chart.</summary>
    [Description("top")]
    Top,

    /// <summary>Render the legend below the chart.</summary>
    [Description("bottom")]
    Bottom,

    /// <summary>Render the legend at the start of the chart.</summary>
    [Description("start")]
    Start,

    /// <summary>Render the legend at the end of the chart.</summary>
    [Description("end")]
    End,
}
