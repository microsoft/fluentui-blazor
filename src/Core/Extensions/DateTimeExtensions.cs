using System.Globalization;

namespace Microsoft.FluentUI.AspNetCore.Components.Extensions;

/// <summary>
/// Extension methods for <see cref="DateTime"/>.
/// </summary>
public static class DateTimeExtensions
{
    /// <summary>
    /// Returns a string in the ISO format yyyy-MM-dd.
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static string ToIsoDateString(this DateTime? self)
    {
        if (self == null)
        {
            return string.Empty;
        }

        return $"{self.Value.Year:D4}-{self.Value.Month:D2}-{self.Value.Day:D2}";
    }

    /// <summary>
    /// Returns the first day of the month.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public static DateTime StartOfMonth(this DateTime self, CultureInfo culture)
    {
        var month = culture.Calendar.GetMonth(self);
        var year = culture.Calendar.GetYear(self);
        return culture.Calendar.ToDateTime(year, month, 1, 0, 0, 0, 0);
    }

    /// <summary>
    /// Returns the first day of the year
    /// </summary>
    /// <param name="self"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public static DateTime StartOfYear(this DateTime self, CultureInfo culture)
    {
        var year = culture.Calendar.GetYear(self);
        return culture.Calendar.ToDateTime(year, 1, 1, 0, 0, 0, 0);
    }

    /// <summary>
    /// Returns the last day of the month.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public static DateTime EndOfMonth(this DateTime self, CultureInfo culture)
    {
        var month = culture.Calendar.GetMonth(self);
        var year = culture.Calendar.GetYear(self);
        var days = culture.Calendar.GetDaysInMonth(year, month);
        return culture.Calendar.ToDateTime(year, month, days, 0, 0, 0, 0);
    }
    /// <summary>
    /// Returns the first day of the week.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="firstDayOfWeek"></param>
    /// <returns></returns>
    public static DateTime StartOfWeek(this DateTime self, DayOfWeek firstDayOfWeek)
    {
        var diff = (7 + (self.DayOfWeek - firstDayOfWeek)) % 7;
        if (self.Year == 1 && self.Month == 1 && (self.Day - diff) < 1)
        {
            return self.Date;
        }

        return self.AddDays(-1 * diff).Date;
    }

    /// <summary>
    /// Returns the first day of the week.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public static DateTime StartOfWeek(this DateTime self, CultureInfo culture)
    {
        var minDate = culture.Calendar.MinSupportedDateTime;
        var firstDayOfWeek = culture.DateTimeFormat.FirstDayOfWeek;

        var diff = (7 + (self.DayOfWeek - firstDayOfWeek)) % 7;
        if (self.Year == minDate.Year && self.Month == minDate.Month && (self.Day - diff) < 1)
        {
            return self.Date;
        }

        return self.AddDays(-1 * diff, culture).Date;
    }

    /// <summary>
    /// Returns the first day of the week.
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static DateTime StartOfWeek(this DateTime self)
    {
        return StartOfWeek(self, CultureInfo.CurrentUICulture);
    }

    /// <summary>
    /// Get a string showing how long ago a DateTime was, for example '4 minutes ago'.
    /// </summary>
    /// <param name="delay"></param>
    /// <param name="resources"></param>
    /// <returns></returns>
    /// <remarks>Inspired from https://github.com/NickStrupat/TimeAgo.</remarks>
    public static string ToTimeAgo(this TimeSpan delay, TimeAgoOptions? resources = null)
    {
        const int MAX_SECONDS_FOR_JUST_NOW = 10;

        if (resources == null)
        {
            resources = new TimeAgoOptions();
        }

        if (delay.Days > 365)
        {
            var years = Math.Round(decimal.Divide(delay.Days, 365));
            return string.Format(years == 1 ? resources.YearAgo : resources.YearsAgo, years);
        }

        if (delay.Days > 30)
        {
            var months = delay.Days / 30;
            if (delay.Days % 31 != 0)
            {
                months += 1;
            }

            return string.Format(months == 1 ? resources.MonthAgo : resources.MonthsAgo, months);
        }

        if (delay.Days > 0)
        {
            return string.Format(delay.Days == 1 ? resources.DayAgo : resources.DaysAgo, delay.Days);
        }

        if (delay.Hours > 0)
        {
            return string.Format(delay.Hours == 1 ? resources.HourAgo : resources.HoursAgo, delay.Hours);
        }

        if (delay.Minutes > 0)
        {
            return string.Format(delay.Minutes == 1 ? resources.MinuteAgo : resources.MinutesAgo, delay.Minutes);
        }

        if (delay.Seconds > MAX_SECONDS_FOR_JUST_NOW)
        {
            return string.Format(resources.SecondsAgo, delay.Seconds);
        }

        if (delay.Seconds <= MAX_SECONDS_FOR_JUST_NOW)
        {
            return string.Format(resources.SecondAgo, delay.Seconds);
        }

        throw new NotSupportedException("The DateTime object does not have a supported value.");
    }

    /// <summary>
    /// Converts the DateOnly to an equivalent DateTime.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static DateTime ToDateTime(this DateOnly value)
    {
        return value.ToDateTime(TimeOnly.MinValue);
    }

    /// <summary>
    /// Converts the TimeOnly to an equivalent DateTime.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static DateTime ToDateTime(this TimeOnly value)
    {
        return new DateTime(value.Ticks);
    }

    /// <summary>
    /// Converts the nullable DateOnly to an equivalent DateTime.
    /// Returns <see cref="DateOnly.MinValue"/> if the <paramref name="value"/> is null.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static DateTime ToDateTime(this DateOnly? value)
    {
        return value == null ? DateTime.MinValue : value.Value.ToDateTime(TimeOnly.MinValue);
    }

    /// <summary>
    /// Converts the nullable TimeOnly to an equivalent DateTime.
    /// Returns <see cref="DateTime.MinValue"/> if the <paramref name="value"/> is null.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static DateTime ToDateTime(this TimeOnly? value)
    {
        return value == null ? DateTime.MinValue : value.Value.ToDateTime();
    }

    /// <summary>
    /// Converts the nullable DateOnly to an equivalent DateTime.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static DateTime? ToDateTimeNullable(this DateOnly? value)
    {
        return value == null ? (DateTime?)null : value.Value.ToDateTime(TimeOnly.MinValue);
    }

    /// <summary>
    /// Converts the nullable TimeOnly to an equivalent DateTime.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static DateTime? ToDateTimeNullable(this TimeOnly? value)
    {
        return value == null ? (DateTime?)null : value.Value.ToDateTime();
    }

    /// <summary>
    /// Converts the nullable DateTime to an equivalent DateTime.
    /// Returns <see cref="DateOnly.MinValue"/> if the <paramref name="value"/> is null.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static DateTime ToDateTime(this DateTime? value)
    {
        return value == null ? DateTime.MinValue : value.Value;
    }

    /// <summary>
    /// Converts the nullable DateTime to an equivalent DateOnly, removing the time part.
    /// Returns <see cref="DateOnly.MinValue"/> if the <paramref name="value"/> is null.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static DateOnly ToDateOnly(this DateTime? value)
    {
        return value == null ? DateOnly.MinValue : DateOnly.FromDateTime(value.Value);
    }

    /// <summary>
    /// Converts the nullable DateTime to an equivalent TimeOnly, removing the time part.
    /// Returns <see cref="TimeOnly.MinValue"/> if the <paramref name="value"/> is null.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static TimeOnly ToTimeOnly(this DateTime? value)
    {
        return value == null ? TimeOnly.MinValue : TimeOnly.FromDateTime(value.Value);
    }

    /// <summary>
    /// Converts the nullable DateTime to an equivalent DateOnly?, removing the time part.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static DateOnly? ToDateOnlyNullable(this DateTime? value)
    {
        return value == null ? (DateOnly?)null : DateOnly.FromDateTime(value.Value);
    }

    /// <summary>
    /// Converts the nullable DateTime to an equivalent TimeOnly?, removing the time part.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static TimeOnly? ToTimeOnlyNullable(this DateTime? value)
    {
        return value == null ? (TimeOnly?)null : TimeOnly.FromDateTime(value.Value);
    }

    /// <summary>
    /// Returns the year
    /// </summary>
    /// <param name="value"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public static int GetYear(this DateTime value, CultureInfo culture)
    {
        return culture.Calendar.GetYear(value);
    }

    /// <summary>
    /// Returns the month
    /// </summary>
    /// <param name="value"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public static int GetMonth(this DateTime value, CultureInfo culture)
    {
        return culture.Calendar.GetMonth(value);
    }

    /// <summary>
    /// Returns the day
    /// </summary>
    /// <param name="value"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public static int GetDay(this DateTime value, CultureInfo culture)
    {
        return culture.Calendar.GetDayOfMonth(value);
    }

    /// <summary>
    /// Returns the DateTime resulting from adding the given number of
    /// days to the specified DateTime.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="days"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public static DateTime AddDays(this DateTime value, int days, CultureInfo culture)
    {
        return culture.Calendar.AddDays(value, days);
    }

    /// <summary>
    /// Returns the DateTime resulting from adding the given number of
    /// months to the specified DateTime.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="months"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public static DateTime AddMonths(this DateTime value, int months, CultureInfo culture)
    {
        return culture.Calendar.AddMonths(value, months);
    }

    /// <summary>
    /// Returns the DateTime resulting from adding the given number of
    /// years to the specified DateTime.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="years"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public static DateTime AddYears(this DateTime value, int years, CultureInfo culture)
    {
        return culture.Calendar.AddYears(value, years);
    }

    /// <summary>
    /// Returns the name of the month
    /// </summary>
    /// <param name="value"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public static string GetMonthName(this DateTime value, CultureInfo culture)
    {
        var month = culture.Calendar.GetMonth(value);

        return culture.DateTimeFormat.MonthNames[month - 1];
    }
}
