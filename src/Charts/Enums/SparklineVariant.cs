// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Specifies the sparkline rendering variant.
/// </summary>
public enum SparklineVariant
{
    /// <summary>
    /// Renders the sparkline as a line.
    /// </summary>
    [Description("line")]
    Line,

    /// <summary>
    /// Renders the sparkline as an area.
    /// </summary>
    [Description("area")]
    Area,
}
