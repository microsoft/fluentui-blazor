namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Computes the properties of a year in the calendar.
/// </summary>
internal class FluentCalendarYear
{
    FluentCalendar _calendar;

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentCalendarYear"/> class.
    /// </summary>
    /// <param name="calendar"></param>
    /// <param name="year"></param>
    internal FluentCalendarYear(FluentCalendar calendar, DateTime year)
    {
        _calendar = calendar;
        Year = year.Day == 1 && year.Month == 1 ? year : new DateTime(year.Year, 1, 1);
    }

    /// <summary>
    /// Current Year (month and day are always 1)
    /// </summary>
    public DateTime Year { get; }

    /// <summary>
    /// Whether the year is disabled by the user.
    /// </summary>
    public bool IsDisabled => IsInactive ? true : false;  // TODO

    /// <summary>
    /// Whether the year is inactive (out of the current year).
    /// </summary>
    public bool IsInactive => false; // TODO

    /// <summary>
    /// Whether the year is selected by the user
    /// </summary>
    public bool IsSelected => Year == _calendar.Value;

    /// <summary>
    /// Gets the title of the year in the format [year].
    /// </summary>
    public string Title => this.Year.ToString();

    /// <summary>
    /// Gets the identifier of the year in the format yyyy.
    /// </summary>
    public string YearIdentifier => this.Year.ToString("yyyy");
}
