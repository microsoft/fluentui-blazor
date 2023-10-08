using System.Globalization;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary>
/// Gets few calendar details in the right culture.
/// </summary>
internal struct CalendarExtended
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CalendarExtended"/> class.
    /// </summary>
    /// <param name="culture"></param>
    /// <param name="currentDate"></param>
    public CalendarExtended(CultureInfo culture, DateTime currentDate)
    {
        Culture = culture;
        Date = currentDate;
    }

    /// <summary>
    /// Gets the culture to use for the calendar.
    /// </summary>
    public CultureInfo Culture { get; }

    /// <summary>
    /// Gets the currently date.
    /// </summary>
    public DateTime Date { get; }

    /// <summary>
    /// Returns the n-th week of the currently displayed month.
    /// </summary>
    /// <param name="weekNumber">between 0 and 4.</param>
    /// <param name="monthOffset">offset from _picker_month.</param>
    /// <returns></returns>
    public IEnumerable<DateTime> GetDaysOfWeek(int weekNumber, int monthOffset = 0)
    {
        if (weekNumber is < 0 or > 5)
        {
            throw new ArgumentException("Index must be between 0 and 5");
        }

        var monthFirst = GetFirstDayToDisplay(monthOffset);
        var weekFirst = monthFirst.AddDays(weekNumber * 7).StartOfWeek(GetFirstDayOfWeek());
        for (var i = 0; i < 7; i++)
        {
            yield return weekFirst.AddDays(i);
        }
    }

    /// <summary>
    /// Gets the first day of the n-th month.
    /// </summary>
    /// <returns></returns>
    public DateTime GetFirstDayToDisplay(int monthOffset = 0)
    {
        return Culture.Calendar.AddMonths(Date, monthOffset);
    }

    /// <summary>
    /// Returns the last day of the n-th month.
    /// </summary>
    /// <returns></returns>
    public DateTime GetLastDayToDisplay(int monthOffset = 0)
    {
        return Culture.Calendar.AddMonths(Date, monthOffset).EndOfMonth(Culture);
    }

    /// <summary>
    /// Returns the first day of the week.
    /// </summary>
    /// <returns></returns>
    public DayOfWeek GetFirstDayOfWeek()
    {
        return Culture.DateTimeFormat.FirstDayOfWeek;
    }

    /// <summary>
    /// Returns the name of the month in the right culture.
    /// </summary>
    /// <returns></returns>
    public string GetMonthName()
    {
        return GetMonthName(this.Date);
    }

    /// <summary>
    /// Returns the name of the month in the right culture.
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public string GetMonthName(DateTime date)
    {
        var monthIndex = date.Month <= 1 ? 0 : date.Month - 1;
        return ToTitleCase(Culture.DateTimeFormat.MonthNames[monthIndex]);
    }

    /// <summary>
    /// Returns the name of the month in the right culture, followed by the year.
    /// </summary>
    /// <returns></returns>
    public string GetMonthNameAndYear()
    {
        var result = $"{GetMonthName(Date)} {Date.Year}";
        return result;
    }

    /// <summary>
    /// Returns a list of days, abbreviated and complete (Mon, Monday), ...,(Sun, Sunday) in the correct culture.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<(string Abbreviated, string Name)> GetDayNames()
    {
        int firstDayOfWeek = (int)GetFirstDayOfWeek();
        var abbreviated = Culture.DateTimeFormat.AbbreviatedDayNames;
        var names = Culture.DateTimeFormat.DayNames;
        var dayNames = new (string Abbreviated, string Name)[7];

        for (int i = 0; i < 7; i++)
        {
            dayNames[i].Name = ToTitleCase(names[i]);
            dayNames[i].Abbreviated = ToAbbreviatedDisplay(ToTitleCase(abbreviated[i]));
        }

        return Shift(dayNames, firstDayOfWeek);
    }


    /// <summary>
    /// Returns True if the specified date is in the current month of the <see cref="Date"/>.
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public bool IsInCurrentMonth(DateTime date)
    {
        return date.Year == this.Date.Year &&
               date.Month == this.Date.Month;
    }

    /// <summary>
    /// Returns the number of day of the month in the current Culture.
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public int GetCalendarDayOfMonth(DateTime date)
    {
        return Culture.Calendar.GetDayOfMonth(date);
    }

    /// <summary>
    /// Returns the name of the day and month in current culture.
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public string GetCalendarDayWithMonthName(DateTime date)
    {
        return $"{GetMonthName(date)} {GetCalendarDayOfMonth(date)}";
    }

    /// <summary>
    /// Shift array and cycle around from the end.
    /// </summary>
    private static T[] Shift<T>(T[] array, int positions)
    {
        var copy = new T[array.Length];
        Array.Copy(array, 0, copy, array.Length - positions, positions);
        Array.Copy(array, positions, copy, 0, array.Length - positions);
        return copy;
    }

    /// <summary>
    /// Returns the string in title case (the first letter of a word is an uppercase letter).
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private string ToTitleCase(string value)
    {
        return Culture.TextInfo.ToTitleCase(value);
    }

    /// <summary>
    /// Returns the string according to different cultures
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private string ToAbbreviatedDisplay(string value)
    {
        switch (Culture.Name)
        {
            case string x when x.StartsWith("zh-"):
                return value[1].ToString();
            default: return value[0].ToString();
        }
    }
}
