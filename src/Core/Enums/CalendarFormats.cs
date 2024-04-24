using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Defines the format of days shown in a <see cref="FluentCalendar"/> component.
/// </summary>
public enum DayFormat
{
    /// <summary>
    /// The day format uses 2 digits.
    /// </summary>
    [Description("2-digit")]
    TwoDigit,

    /// <summary>
    /// The day format is numeric.
    /// </summary>
    Numeric
}

/// <summary>
/// Defines the view display in a <see cref="FluentCalendar"/> component.
/// </summary>
public enum CalendarViews
{
    /// <summary>
    /// Display the Days View only
    /// </summary>
    Days,

    /// <summary>
    /// Display the Months View only
    /// </summary>
    Months,

    /// <summary>
    /// Display the Years View only
    /// </summary>
    Years
}
