// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>Specifies the position of a chart title.</summary>
public enum ChartTitlePosition
{
    /// <summary>Render the title above the chart.</summary>
    [Description("top")]
    Top,

    /// <summary>Render the title below the chart.</summary>
    [Description("bottom")]
    Bottom,
}
