using System.Globalization;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Computes the properties of a day in the calendar, depending on the current culture.
/// </summary>
public class FluentCalendarDay
{
    private readonly FluentCalendar _calendar;
    private readonly bool _isInDisabledList;
    private readonly bool _isOutsideCurrentMonth;

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentCalendarDay"/> class.
    /// </summary>
    /// <param name="calendar"></param>
    /// <param name="day"></param>
    internal FluentCalendarDay(FluentCalendar calendar, DateTime day)
    {
        _calendar = calendar;
        Date = day;

        _isInDisabledList = calendar.DisabledDateFunc?.Invoke(day) ?? false;
        _isOutsideCurrentMonth = !calendar.CalendarExtended.IsInCurrentMonth(day);
    }

    /// <summary>
    /// Current day
    /// </summary>
    public DateTime Date { get; }

    /// <summary>
    /// Gets a value indicating whether the day is disabled by the user.
    /// </summary>
    public bool IsDisabled => IsInactive ? false : _isInDisabledList && _calendar.DisabledSelectable;

    /// <summary>
    /// Gets a value indicating whether the day is inactive (out of the current month).
    /// </summary>
    public bool IsInactive => _isOutsideCurrentMonth || (_isInDisabledList && !_calendar.DisabledSelectable);

    /// <summary>
    /// Gets a value indicating whether the day is set to Today.
    /// </summary>
    public bool IsToday => Date == DateTime.Today && !_isOutsideCurrentMonth;

    /// <summary>
    /// Gets a value indicating whether the day is selected by the user.
    /// </summary>
    public bool IsSelected => Date.Date == _calendar.Value?.Date;

    /// <summary>
    /// Gets a value indicating whether the day is selected by the user, using <see cref="FluentCalendar.SelectMode"/>.
    /// </summary>
    public bool IsMultiDaySelected => _calendar.SelectMode != CalendarSelectMode.Single && _calendar.SelectedDates.Contains(Date) && !IsDisabled;

    /// <summary>
    /// Gets the name of the day and month in current culture.
    /// </summary>
    public string Title => _calendar.CalendarExtended.GetCalendarDayWithMonthName(Date);

    /// <summary>
    /// Gets the number of day of the month in the current Culture.
    /// </summary>
    public string DayNumber
    {
        get
        {
            var day = _calendar.CalendarExtended.GetCalendarDayOfMonth(Date);

            return _calendar.DayFormat switch
            {
                DayFormat.TwoDigit => day.ToString("00", CultureInfo.InvariantCulture),
                _ => Convert.ToString(day),
            };
        }
    }

    /// <summary>
    /// Gets the identifier of the day in the format yyyy-MM-dd.
    /// </summary>
    public string DayIdentifier => Date.ToString("yyyy-MM-dd");
}
