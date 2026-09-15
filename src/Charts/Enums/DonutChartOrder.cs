// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Specifies the order used to render donut chart segments and legends.
/// </summary>
public enum DonutChartOrder
{
    /// <summary>
    /// Render segments and legends in the order provided by the data source.
    /// </summary>
    [Description("default")]
    Default,

    /// <summary>
    /// Render segments and legends sorted by value from largest to smallest.
    /// </summary>
    [Description("sorted")]
    Sorted,
}
