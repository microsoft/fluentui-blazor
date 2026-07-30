// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Controls how the area fill is computed.
/// </summary>
public enum AreaChartMode
{
    /// <summary>
    /// Stacked — each area fills from the top of the previous series' line up to its own line.
    /// </summary>
    [Description("tonexty")]
    Tonexty,

    /// <summary>
    /// Non-stacked — each area fills independently from y = 0 to its own line.
    /// </summary>
    [Description("tozeroy")]
    Tozeroy,
}
