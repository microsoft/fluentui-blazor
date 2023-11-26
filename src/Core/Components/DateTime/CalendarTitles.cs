namespace Microsoft.FluentUI.AspNetCore.Components;

internal class CalendarTitles
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CalendarTitles"/> class.
    /// </summary>
    /// <param name="calendar"></param>
    public CalendarTitles(FluentCalendar calendar)
    {
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
    public bool ReadOnly => View switch
    {
        CalendarViews.Days => false,
        CalendarViews.DaysMonths => true,
        CalendarViews.Months => false,
        CalendarViews.Years => true,
        _ => true
    };

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
                CalendarViews.Years => CalendarExtended.GetYearsRangeLabel(Date.Year),
                _ => string.Empty
            };
        }
    }

    public string PreviousTitle
    {
        get
        {
            return View switch
            {
                CalendarViews.Days => CalendarExtended.GetMonthName(Date.AddMonths(-1)),
                CalendarViews.Months => CalendarExtended.GetYear(Date.AddYears(-1)),
                CalendarViews.Years => CalendarExtended.GetYearsRangeLabel(Date.Year - 12),
                _ => string.Empty
            };
        }
    }

    public bool PreviousDisabled
    {
        get
        {
            return View switch
            {
                CalendarViews.Days => Date.Year == DateTime.MinValue.Year && Date.Month == DateTime.MinValue.Month,
                CalendarViews.Months => Date.Year == DateTime.MinValue.Year,
                CalendarViews.Years => Date.Year == DateTime.MinValue.Year,
                _ => false
            };
        }
    }

    public string NextTitle
    {
        get
        {
            return View switch
            {
                CalendarViews.Days => CalendarExtended.GetMonthName(Date.AddMonths(+1)),
                CalendarViews.Months => CalendarExtended.GetYear(Date.AddYears(+1)),
                CalendarViews.Years => CalendarExtended.GetYearsRangeLabel(Date.Year + 12),
                _ => string.Empty
            };
        }
    }

    public bool NextDisabled
    {
        get
        {
            return View switch
            {
                CalendarViews.Days => Date.Year == DateTime.MaxValue.Year && Date.Month == DateTime.MaxValue.Month,
                CalendarViews.Months => Date.Year == DateTime.MaxValue.Year,
                CalendarViews.Years => Date.Year == DateTime.MaxValue.Year,
                _ => false
            };
        }
    }
}
