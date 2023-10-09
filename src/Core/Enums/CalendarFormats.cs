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
/// Defines the format of the day names shown in a <see cref="FluentCalendar"/> component.
/// </summary>
public enum WeekdayFormat
{
    /// <summary>
    /// The day name uses a long format.
    /// </summary>
    Long,

    /// <summary>
    /// The day name uses a narrow format.
    /// </summary>
    Narrow,

    /// <summary>
    /// The day name uses a short format.
    /// </summary>
    Short
}

/// <summary>
/// Defines the format of the months shown in a <see cref="FluentCalendar"/> component.
/// </summary>
public enum MonthFormat
{
    /// <summary>
    /// The month number is shown in 2 digits
    /// </summary>
    [Description("2-digit")]
    TwoDigit,

    /// <summary>
    /// The month name uses a long format.
    /// </summary>
    Long,

    /// <summary>
    /// The month names uses a narrow format.
    /// </summary>
    Narrow,

    /// <summary>
    /// The month is shown in a numeric format.
    /// </summary>
    Numeric,

    /// <summary>
    /// The month name uses a short format.
    /// </summary>
    Short
}

/// <summary>
/// Defines the format of the years shown in a <see cref="FluentCalendar"/> component.
/// </summary>
public enum YearFormat
{
    /// <summary>
    /// The year is shown in a 2 digit format.
    /// </summary>
    [Description("2-digit")]
    TwoDigit,

    /// <summary>
    /// The year is shwn in a numeric format (4 digits).
    /// </summary>
    Numeric
}