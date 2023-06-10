using System.Globalization;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary>
/// Computes the properties of a day in the calendar, depending on the current culture.
/// </summary>
internal class DayProperties
{
    FluentCalendar _calendar;
    private bool _isInDisabledList;
    private bool _isOutsideCurrentMonth;

    /// <summary>
    /// Initializes a new instance of the <see cref="DayProperties"/> class.
    /// </summary>
    /// <param name="calendar"></param>
    /// <param name="day"></param>
    internal DayProperties(FluentCalendar calendar, DateTime day)
    {
        _calendar = calendar;
        Day = day;

        _isInDisabledList = calendar.DisabledDateFunc?.Invoke(day) ?? false;
        _isOutsideCurrentMonth = !calendar.CalendarExtended.IsInCurrentMonth(day);
    }

    /// <summary>
    /// Current day
    /// </summary>
    public DateTime Day { get; }

    /// <summary>
    /// Whether the day is disabled by the user.
    /// </summary>
    public bool IsDisabled => IsInactive ? false : _isInDisabledList && _calendar.DisabledSelectable;

    /// <summary>
    /// Whether the day is inactive (out of the current month).
    /// </summary>
    public bool IsInactive => _isOutsideCurrentMonth || (_isInDisabledList && !_calendar.DisabledSelectable);

    /// <summary>
    /// Whether the day is now
    /// </summary>
    public bool IsToday => Day == DateTime.Today && !_isOutsideCurrentMonth;

    /// <summary>
    /// Whether the day is selected by the user
    /// </summary>
    public bool IsSelected => Day == _calendar.SelectedDate;

    /// <summary>
    /// Gets the name of the day and month in current culture.
    /// </summary>
    public string Title => _calendar.CalendarExtended.GetCalendarDayWithMonthName(this.Day);

    /// <summary>
    /// Gets the number of day of the month in the current Culture.
    /// </summary>
    public int DayNumber => _calendar.CalendarExtended.GetCalendarDayOfMonth(this.Day);

    /// <summary>
    /// Gets the identifier of the day in the format yyyy-MM-dd.
    /// </summary>
    public string DayIdentifier => this.Day.ToString("yyyy-MM-dd");
}
