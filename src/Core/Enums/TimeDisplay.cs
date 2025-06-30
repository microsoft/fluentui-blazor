// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Specifies the format for displaying time in input elements with time component.
/// This is a hint to the browser, so results may vary.
/// </summary>
public enum TimeDisplay
{
    /// <summary>
    /// Show hours and minutes
    /// </summary>
    HourMinute,

    /// <summary>
    /// Show hours, minutes and seconds
    /// </summary>
    HourMinuteSeconds,
}
