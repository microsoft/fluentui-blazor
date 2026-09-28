// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>Specifies the horizontal alignment of a chart title.</summary>
public enum ChartTitleAlign
{
    /// <summary>Align the title at the start of the chart.</summary>
    [Description("start")]
    Start,

    /// <summary>Center the title.</summary>
    [Description("center")]
    Center,

    /// <summary>Align the title at the end of the chart.</summary>
    [Description("end")]
    End,
}
