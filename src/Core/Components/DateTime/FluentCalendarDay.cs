// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Globalization;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Computes the properties of a day in the calendar, depending on the current culture.
/// </summary>
/// <typeparam name="TValue">The type of value handled by the calendar.</typeparam>
public class FluentCalendarDay<TValue>
{
    private readonly FluentCalendar<TValue> _calendar;
    private readonly bool _isInDisabledList;
    private readonly bool _isOutsideCurrentMonth;

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentCalendarDay{TValue}"/> class.
    /// </summary>
    /// <param name="calendar"></param>
    /// <param name="day"></param>
    internal FluentCalendarDay(FluentCalendar<TValue> calendar, DateTime day)
    {
        _calendar = calendar;
        Date = day.Date.ConvertToTValue<TValue>();
        DateTime = day.Date;

        _isInDisabledList = calendar.DisabledDateFunc?.Invoke(day.ConvertToTValue<TValue>()) ?? false;
        _isOutsideCurrentMonth = !calendar.CalendarExtended.IsInCurrentMonth(day);
    }

    /// <summary>
    /// Current day
    /// </summary>
    internal DateTime DateTime { get; }

    /// <summary>
    /// Current day converted to TValue
    /// </summary>
    public TValue Date { get; }

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
    public bool IsToday => DateTime == DateTimeProvider.Today && !_isOutsideCurrentMonth;

    /// <summary>
    /// Gets a value indicating whether the day is selected by the user.
    /// </summary>
    public bool IsSelected => DateTime.Date == _calendar.ValueAsDateTime?.Date;

    /// <summary>
    /// Gets a value indicating whether the day is selected by the user, using <see cref="FluentCalendar{TValue}.SelectMode"/>.
    /// </summary>
    public bool IsMultiDaySelected => _calendar.SelectMode != CalendarSelectMode.Single && _calendar.SelectedDates.Contains(Date) && !IsDisabled;

    /// <summary>
    /// Gets the name of the day and month in current culture.
    /// </summary>
    public string Title => _calendar.CalendarExtended.GetCalendarDayWithMonthName(DateTime);

    /// <summary>
    /// Gets the number of day of the month in the current Culture.
    /// </summary>
    public string DayNumber
    {
        get
        {
            var day = _calendar.CalendarExtended.GetCalendarDayOfMonth(DateTime);

            return _calendar.DayFormat switch
            {
                CalendarDayFormat.TwoDigit => day.ToString("00", CultureInfo.InvariantCulture),
                _ => Convert.ToString(day, _calendar.Culture),
            };
        }
    }

    /// <summary>
    /// Gets the identifier of the day in the format yyyy-MM-dd.
    /// </summary>
    public string DayIdentifier => DateTime.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
}
