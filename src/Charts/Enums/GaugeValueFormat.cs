// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Specifies how the gauge center value is formatted.
/// </summary>
public enum GaugeValueFormat
{
    /// <summary>
    /// Displays the current value as a percentage of the total range.
    /// </summary>
    [Description("percentage")]
    Percentage,

    /// <summary>
    /// Displays the current value as a fraction of the total range.
    /// </summary>
    [Description("fraction")]
    Fraction,
}
