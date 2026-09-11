// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Specifies how string axis labels are ordered in the heat map chart.
/// </summary>
public enum HeatMapSortOrder
{
    /// <summary>
    /// Preserve the insertion order of string labels.
    /// </summary>
    [Description("none")]
    None,

    /// <summary>
    /// Sort string labels alphabetically in ascending order.
    /// </summary>
    [Description("alphabetical")]
    Alphabetical,
}
