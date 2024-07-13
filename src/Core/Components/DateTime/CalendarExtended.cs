using System.Globalization;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;

namespace Microsoft.FluentUI.AspNetCore.Components;

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

        var monthFirst = Date.AddMonths(monthOffset, Culture);
        var maxDate = Culture.Calendar.MaxSupportedDateTime;
        var maxLimit = monthFirst.Year == maxDate.Year && monthFirst.Month == maxDate.Month && weekNumber > 3;

        if (!maxLimit)
        {
            var weekFirst = monthFirst.AddDays(weekNumber * 7, Culture).StartOfWeek(Culture);
            for (var i = 0; i < 7; i++)
            {
                yield return weekFirst.AddDays(i, Culture);
            }
        }
    }

    /// <summary>
    /// Returns the name of the month in the right culture.
    /// </summary>
    /// <returns></returns>
    public string GetMonthName()
    {
        return GetMonthName(Date);
    }

    /// <summary>
    /// Returns the name of the month in the right culture.
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public string GetMonthName(DateTime date)
    {
        return ToTitleCase(date.GetMonthName(Culture));
    }

    /// <summary>
    /// Returns the list of month names in the right culture.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<(int Index, string Abbreviated, string Name)> GetMonthNames()
    {
        var max = Culture.Calendar.GetMonthsInYear(Date.GetYear(Culture));
        for (var i = 0; i < max; i++)
        {
            yield return (i + 1, ToTitleCase(Culture.DateTimeFormat.AbbreviatedMonthNames[i]), ToTitleCase(Culture.DateTimeFormat.MonthNames[i]));
        }
    }

    /// <summary>
    /// Returns the list of 12 years.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<(int Index, int Year)> GetYearsRange()
    {
        var maxCount = 12;
        var maxYear = Culture.Calendar.MaxSupportedDateTime.GetYear(Culture);
        var year = Date.GetYear(Culture);

        if (year + maxCount > maxYear)
        {
            maxCount = maxYear - year + 1;
        }

        for (var i = 0; i < maxCount; i++)
        {
            yield return (i, year + i);
        }
    }

    /// <summary>
    /// Returns the name of the month in the right culture, followed by the year.
    /// </summary>
    /// <returns></returns>
    public string GetMonthNameAndYear()
    {
        var result = $"{GetMonthName(Date)} {Date.GetYear(Culture)}";
        return result;
    }

    /// <summary>
    /// Returns the year value.
    /// </summary>
    /// <returns></returns>
    public string GetYear()
    {
        return GetYear(Date);
    }

    /// <summary>
    /// Returns the year value.
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public string GetYear(DateTime date)
    {
        return date.GetYear(Culture).ToString();
    }

    /// <summary>
    /// Returns the year value.
    /// </summary>
    /// <returns></returns>
    public string GetYearsRangeLabel(int fromYear)
    {
        var min = fromYear;
        var max = fromYear + 11;

        var minSupportedYear = Culture.Calendar.MinSupportedDateTime.GetYear(Culture);
        var maxSupportedYear = Culture.Calendar.MaxSupportedDateTime.GetYear(Culture);

        if (min < minSupportedYear)
        {
            min = minSupportedYear;
        }

        if (max > maxSupportedYear)
        {
            max = maxSupportedYear;
        }

        return min == max ? $"{min}" : $"{min} - {max}";
    }

    /// <summary>
    /// Returns a list of days, abbreviated and complete (Mon, Monday), ...,(Sun, Sunday) in the correct culture.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<(string Abbreviated, string Shorted, string Name)> GetDayNames()
    {
        var firstDayOfWeek = (int)Culture.DateTimeFormat.FirstDayOfWeek;
        var abbreviated = Culture.DateTimeFormat.AbbreviatedDayNames;
        var names = Culture.DateTimeFormat.DayNames;
        var shorted = Culture.DateTimeFormat.ShortestDayNames;
        var dayNames = new (string Abbreviated, string Shorted, string Name)[7];

        for (var i = 0; i < 7; i++)
        {
            dayNames[i].Name = ToTitleCase(names[i]);
            dayNames[i].Shorted = shorted[i];
            dayNames[i].Abbreviated = ToTitleCase(abbreviated[i]);
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
        return date >= Date.StartOfMonth(Culture) && date <= Date.EndOfMonth(Culture);
    }

    /// <summary>
    /// Returns the number of day of the month in the current Culture.
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public int GetCalendarDayOfMonth(DateTime date)
    {
        return date.GetDay(Culture);
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
}
