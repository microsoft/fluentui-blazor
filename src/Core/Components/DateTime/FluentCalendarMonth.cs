namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Computes the properties of a month in the calendar.
/// </summary>
internal class FluentCalendarMonth
{
    FluentCalendar _calendar;

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentCalendarMonth"/> class.
    /// </summary>
    /// <param name="calendar"></param>
    /// <param name="month"></param>
    internal FluentCalendarMonth(FluentCalendar calendar, DateTime month)
    {
        _calendar = calendar;
        Month = month.Day == 1 ? month : new DateTime(month.Year, month.Day, 1);
    }

    /// <summary>
    /// Current month (day is always 1)
    /// </summary>
    public DateTime Month { get; }

    /// <summary>
    /// Whether the month is disabled by the user.
    /// </summary>
    public bool IsDisabled => IsInactive ? true : false;  // TODO

    /// <summary>
    /// Whether the month is inactive (out of the current year).
    /// </summary>
    public bool IsInactive => false; // TODO

    /// <summary>
    /// Whether the month is selected by the user
    /// </summary>
    public bool IsSelected => Month == _calendar.Value;

    /// <summary>
    /// Gets the identifier of the month in the format yyyy-MM.
    /// </summary>
    public string MonthIdentifier => this.Month.ToString("yyyy-MM");
}
