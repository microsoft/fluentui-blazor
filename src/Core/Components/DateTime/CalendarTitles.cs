// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.Extensions;

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
            var minDate = _calendar.Culture.Calendar.MinSupportedDateTime.AddMonths(1);
#pragma warning restore MA0011

            var rangeMinDate = _calendar.MinDate.ConvertToDateTime()?.Date;
            var rangePreviousDisabled = false;
            if (rangeMinDate.HasValue)
            {
                var candidate = View switch
                {
                    CalendarViews.Days => Date.AddMonths(-1, _calendar.Culture),
                    CalendarViews.Months => Date.AddYears(-1, _calendar.Culture),
                    CalendarViews.Years => Date.AddYears(-12, _calendar.Culture),
                    _ => Date,
                };

                var candidatePeriodEnd = View switch
                {
                    CalendarViews.Days
                        => candidate.StartOfMonth(_calendar.Culture)
                            .AddMonths(1, _calendar.Culture)
                            .AddDays(-1),

                    CalendarViews.Months
                        => candidate.StartOfYear(_calendar.Culture)
                            .AddYears(1, _calendar.Culture)
                            .AddDays(-1),

                    CalendarViews.Years
                        => candidate.StartOfYear(_calendar.Culture)
                            .AddYears(12, _calendar.Culture)
                            .AddDays(-1),

                    _ => candidate,
                };

                rangePreviousDisabled = candidatePeriodEnd.Date < rangeMinDate.Value;
            }

            return View switch
            {
                CalendarViews.Days => (Date.Year == minDate.Year && Date.Month == minDate.Month) || rangePreviousDisabled,
                CalendarViews.Months => Date.Year == minDate.Year || rangePreviousDisabled,
                CalendarViews.Years => Date.Year - CalendarExtended.YearShiftCentered <= minDate.Year + 12 || rangePreviousDisabled,
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
            var maxDate = _calendar.Culture.Calendar.MaxSupportedDateTime;

            var rangeMaxDate = _calendar.MaxDate.ConvertToDateTime()?.Date;
            var rangeNextDisabled = false;
            if (rangeMaxDate.HasValue)
            {
                var candidate = View switch
                {
                    CalendarViews.Days => Date.AddMonths(+1, _calendar.Culture),
                    CalendarViews.Months => Date.AddYears(+1, _calendar.Culture),
                    CalendarViews.Years => Date.AddYears(+12, _calendar.Culture),
                    _ => Date,
                };

                var candidatePeriodStart = View switch
                {
                    CalendarViews.Days
                        => candidate.StartOfMonth(_calendar.Culture),

                    CalendarViews.Months
                        => candidate.StartOfYear(_calendar.Culture),

                    CalendarViews.Years
                        => candidate.StartOfYear(_calendar.Culture),

                    _ => candidate,
                };

                rangeNextDisabled = candidatePeriodStart.Date > rangeMaxDate.Value;
            }

            return View switch
            {
                CalendarViews.Days => (Date.Year == maxDate.Year && Date.Month == maxDate.Month) || rangeNextDisabled,
                CalendarViews.Months => Date.Year == maxDate.Year || rangeNextDisabled,
                CalendarViews.Years => Date.Year + 12 - CalendarExtended.YearShiftCentered >= maxDate.Year || rangeNextDisabled,
                _ => false
            };
        }
    }
}
