// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Defines the format of days shown in a <see cref="FluentCalendar{TValue}"/> component.
/// </summary>
public enum CalendarDayFormat
{
    /// <summary>
    /// The day format uses 2 digits.
    /// </summary>
    [Description("2-digit")]
    TwoDigit,

    /// <summary>
    /// The day format is numeric.
    /// </summary>
    Numeric,
}
