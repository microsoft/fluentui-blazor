// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.Calendar;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Computes the properties of a month in the calendar.
/// </summary>
/// <typeparam name="TValue">The type of value handled by the calendar.</typeparam>
internal class FluentCalendarMonth<TValue>
{
    private readonly FluentCalendar<TValue> _calendar;
    private readonly bool _isInDisabledList;

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentCalendarMonth{TValue}"/> class.
    /// </summary>
    /// <param name="calendar"></param>
    /// <param name="month"></param>
    internal FluentCalendarMonth(FluentCalendar<TValue> calendar, DateTime month)
    {
        _calendar = calendar;
        Month = month.GetDay(_calendar.Culture) == 1 ? month : month.StartOfMonth(_calendar.Culture);

        if (calendar.DisabledCheckAllDaysOfMonthYear)
        {
            _isInDisabledList = calendar.AllDaysAreDisabled(month.StartOfMonth(_calendar.Culture), month.EndOfMonth(_calendar.Culture));
        }
        else
        {
            _isInDisabledList = calendar.DisabledDateFunc?.Invoke(Month.ConvertToTValue<TValue>()) ?? false;
        }
    }

    /// <summary>
    /// Current month (day is always 1)
    /// </summary>
    public DateTime Month { get; }

    /// <summary>
    /// Whether the month is readonly.
    /// </summary>
    public bool IsReadOnly => _isInDisabledList || _calendar.IsReadOnlyOrDisabled;

    /// <summary>
    /// Whether the month is disabled.
    /// </summary>
    public bool IsDisabled => _isInDisabledList;

    /// <summary>
    /// Whether the month is selected by the user
    /// </summary>
    public bool IsSelected => Month.GetYear(_calendar.Culture) == _calendar.ValueAsDateTime?.GetYear(_calendar.Culture) && Month.GetMonth(_calendar.Culture) == _calendar.ValueAsDateTime?.GetMonth(_calendar.Culture);

    /// <summary>
    /// Gets the title of the month in the format [month] [year].
    /// </summary>
    public string Title => $"{_calendar.CalendarExtended.GetMonthName(Month)} {Month.GetYear(_calendar.Culture):0000}";

    /// <summary>
    /// Gets the identifier of the month in the format yyyy-MM.
    /// </summary>
    public string MonthIdentifier => Month.ToString("yyyy-MM", _calendar.Culture);
}
