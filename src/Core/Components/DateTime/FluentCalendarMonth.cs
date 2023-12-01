namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Computes the properties of a month in the calendar.
/// </summary>
internal class FluentCalendarMonth
{
    FluentCalendar _calendar;
    private bool _isInDisabledList;

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentCalendarMonth"/> class.
    /// </summary>
    /// <param name="calendar"></param>
    /// <param name="month"></param>
    internal FluentCalendarMonth(FluentCalendar calendar, DateTime month)
    {
        _calendar = calendar;
        Month = month.Day == 1 ? month : new DateTime(month.Year, month.Day, 1);

        _isInDisabledList = calendar.DisabledDateFunc?.Invoke(Month) ?? false;
    }

    /// <summary>
    /// Current month (day is always 1)
    /// </summary>
    public DateTime Month { get; }

    /// <summary>
    /// Whether the month is readonly.
    /// </summary>
    public bool IsReadOnly => _isInDisabledList || _calendar.ReadOnly;

    /// <summary>
    /// Whether the month is disabled.
    /// </summary>
    public bool IsDisabled => _isInDisabledList;

    /// <summary>
    /// Whether the month is selected by the user
    /// </summary>
    public bool IsSelected => Month.Year == _calendar.Value?.Year && Month.Month == _calendar.Value?.Month;

    /// <summary>
    /// Gets the title of the month in the format [month] [year].
    /// </summary>
    public string Title => $"{_calendar.CalendarExtended.GetMonthName(Month)} {Month.Year:0000}";

    /// <summary>
    /// Gets the identifier of the month in the format yyyy-MM.
    /// </summary>
    public string MonthIdentifier => this.Month.ToString("yyyy-MM");
}
