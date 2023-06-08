namespace Microsoft.Fast.Components.FluentUI;

/// <summary>
/// Computes the properties of a day in the calendar, depending on the current culture.
/// </summary>
internal class DayProperties
{
    private readonly CalendarExtended _calendarData;

    /// <summary>
    /// Initializes a new instance of the <see cref="DayProperties"/> class.
    /// </summary>
    /// <param name="calendarData"></param>
    /// <param name="day"></param>
    internal DayProperties(CalendarExtended calendarData, DateTime day)
    {
        _calendarData = calendarData;
        Day = day;
    }

    /// <summary>
    /// Current day
    /// </summary>
    public DateTime Day { get; }

    /// <summary>
    /// CSS classes to apply to the day: day, inactive, today, selected.
    /// </summary>
    public IEnumerable<string> CssClasses { get; set; } = default!;

    /// <summary>
    /// Whether the day is active or not.
    /// </summary>
    public bool IsDisabled { get; set; } = false;

    /// <summary>
    /// Gets the name of the day and month in current culture.
    /// </summary>
    public string Title => _calendarData.GetCalendarDayWithMonthName(this.Day);

    /// <summary>
    /// Gets the number of day of the month in the current Culture.
    /// </summary>
    public int DayNumber => _calendarData.GetCalendarDayOfMonth(this.Day);

    /// <summary>
    /// Gets the identifier of the day in the format yyyy-MM-dd.
    /// </summary>
    public string DayIdentifier => this.Day.ToString("yyyy-MM-dd");
}
