// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components.Calendar;

/// <summary>
/// Provides titles and navigation-related properties for a calendar,  based on the current view and culture settings.
/// </summary>
/// <typeparam name="TValue">The type of value handled by the calendar.</typeparam>
internal class CalendarTitles<TValue>
{
    private readonly FluentCalendar<TValue> _calendar;

    /// <summary>
    /// Initializes a new instance of the <see cref="CalendarTitles{TValue}"/> class.
    /// </summary>
    /// <param name="calendar"></param>
    public CalendarTitles(FluentCalendar<TValue> calendar)
    {
        _calendar = calendar;
        CalendarExtended = new CalendarExtended(calendar.Culture, calendar.PickerMonth.ConvertToRequiredDateTime());
        View = calendar.View;
    }

    /// <summary>
    /// Gets the CalendarExtended to use for the calendar.
    /// </summary>
    public CalendarExtended CalendarExtended { get; }

    /// <summary>
    /// Gets the currently View.
    /// </summary>
    public CalendarViews View { get; }

    /// <summary>
    /// Gets the current date.
    /// </summary>
    private DateTime Date => CalendarExtended.Date;

    /// <summary>
    /// Gets a value indicating whether the calendar Title is not clickable.
    /// </summary>
    public bool ReadOnly
    {
        get
        {
            if (_calendar.IsReadOnlyOrDisabled)
            {
                return true;
            }

            if (PreviousDisabled && NextDisabled)
            {
                return true;
            }

            return View switch
            {
                CalendarViews.Days => false,
                CalendarViews.Months => false,
                CalendarViews.Years => true,
                _ => true
            };
        }
    }

    /// <summary>
    /// Gets the main calendar title.
    /// </summary>
    public string Label
    {
        get
        {
            return View switch
            {
#pragma warning disable MA0011
                CalendarViews.Days => CalendarExtended.GetMonthNameAndYear(),
                CalendarViews.Months => CalendarExtended.GetYear(),
                CalendarViews.Years => CalendarExtended.GetYearsRangeLabel(Date.GetYear(_calendar.Culture) - CalendarExtended.YearShiftCentered),
#pragma warning restore MA0011
                _ => string.Empty
            };
        }
    }

    /// <summary>
    /// Gets the title representing the previous period based on the current calendar view.
    /// </summary>
    public string PreviousTitle
    {
        get
        {
            return View switch
            {
                CalendarViews.Days => CalendarExtended.GetMonthName(Date.AddMonths(-1, _calendar.Culture)),
                CalendarViews.Months => CalendarExtended.GetYear(Date.AddYears(-1, _calendar.Culture)),
                CalendarViews.Years => CalendarExtended.GetYearsRangeLabel(Date.GetYear(_calendar.Culture) - 12 - CalendarExtended.YearShiftCentered),
                _ => string.Empty
            };
        }
    }

    /// <summary>
    /// Gets a value indicating whether the "Previous" navigation button is disabled.
    /// </summary>
    public bool PreviousDisabled
    {
        get
        {
#pragma warning disable MA0011
            var userMinDate = _calendar.MinDate.ConvertToDateTime();
            var calendarMinDate = _calendar.Culture.Calendar.MinSupportedDateTime;
            var minDate = userMinDate is null || userMinDate.Value < calendarMinDate ? calendarMinDate.AddMonths(1) : userMinDate.Value;
#pragma warning restore MA0011
            var previousRangeLastYear = Date.GetYear(_calendar.Culture) - CalendarExtended.YearShiftCentered - 1;
            var minYear = minDate.GetYear(_calendar.Culture);
            var minimumPreviousNavigationDate = calendarMinDate.AddYears(12, _calendar.Culture);

            return View switch
            {
                CalendarViews.Days => Date.Year == minDate.Year && Date.Month == minDate.Month,
                CalendarViews.Months => Date.Year == minDate.Year,
                CalendarViews.Years => Date < minimumPreviousNavigationDate || previousRangeLastYear < minYear,
                _ => false
            };
        }
    }

    /// <summary>
    /// Gets the title representing the next time period based on the current calendar view.
    /// </summary>
    public string NextTitle
    {
        get
        {
            return View switch
            {
                CalendarViews.Days => CalendarExtended.GetMonthName(Date.AddMonths(+1, _calendar.Culture)),
                CalendarViews.Months => CalendarExtended.GetYear(Date.AddYears(+1, _calendar.Culture)),
                CalendarViews.Years => CalendarExtended.GetYearsRangeLabel(Date.GetYear(_calendar.Culture) + 12 - CalendarExtended.YearShiftCentered),
                _ => string.Empty
            };
        }
    }

    /// <summary>
    /// Gets a value indicating whether the "Next" navigation option is disabled.
    /// </summary>
    public bool NextDisabled
    {
        get
        {
            var userMaxDate = _calendar.MaxDate.ConvertToDateTime();
            var calendarMaxDate = _calendar.Culture.Calendar.MaxSupportedDateTime;
            var maxDate = userMaxDate is null || userMaxDate.Value > calendarMaxDate ? calendarMaxDate : userMaxDate.Value;
            var nextRangeFirstYear = Date.GetYear(_calendar.Culture) + 12 - CalendarExtended.YearShiftCentered;
            var maxYear = maxDate.GetYear(_calendar.Culture);

            return View switch
            {
                CalendarViews.Days => Date.Year == maxDate.Year && Date.Month == maxDate.Month,
                CalendarViews.Months => Date.Year == maxDate.Year,
                CalendarViews.Years => nextRangeFirstYear > maxYear,
                _ => false
            };
        }
    }
}
