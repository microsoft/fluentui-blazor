// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.Extensions;

namespace Microsoft.FluentUI.AspNetCore.Components;

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
        CalendarExtended = new CalendarExtended(calendar.Culture, calendar.PickerMonth);
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
                CalendarViews.Days => CalendarExtended.GetMonthNameAndYear(),
                CalendarViews.Months => CalendarExtended.GetYear(),
                CalendarViews.Years => CalendarExtended.GetYearsRangeLabel(Date.GetYear(_calendar.Culture)),
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
                CalendarViews.Years => CalendarExtended.GetYearsRangeLabel(Date.GetYear(_calendar.Culture) - 12),
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
            var minDate = _calendar.Culture.Calendar.MinSupportedDateTime;

            return View switch
            {
                CalendarViews.Days => Date.Year == minDate.Year && Date.Month == minDate.Month,
                CalendarViews.Months => Date.Year == minDate.Year,
                CalendarViews.Years => Date.Year == minDate.Year,
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
                CalendarViews.Years => CalendarExtended.GetYearsRangeLabel(Date.GetYear(_calendar.Culture) + 12),
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
            var maxDate = _calendar.Culture.Calendar.MaxSupportedDateTime;

            return View switch
            {
                CalendarViews.Days => Date.Year == maxDate.Year && Date.Month == maxDate.Month,
                CalendarViews.Months => Date.Year == maxDate.Year,
                CalendarViews.Years => Date.Year == maxDate.Year,
                _ => false
            };
        }
    }
}
