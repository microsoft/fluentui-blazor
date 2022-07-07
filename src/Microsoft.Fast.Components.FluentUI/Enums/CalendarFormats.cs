namespace Microsoft.Fast.Components.FluentUI;

/// <summary>
/// Enum representing the different day formats
/// </summary>
public enum DayFormat
{
    TwoDigit,
    Numeric
}

/// <summary>
/// Enum representing the different weekday formats
/// </summary>
public enum WeekdayFormat
{
    Long,
    Narrow,
    Short
}

/// <summary>
/// Enum representing the different month formats
/// </summary>
public enum MonthFormat
{
    TwoDigit,
    Long,
    Narrow,
    Numeric,
    Short
}

/// <summary>
/// Enum representing the different year formats
/// </summary>
public enum YearFormat
{
    TwoDigit,
    Numeric
}

public static class CalendarFormatsExtensions
{
    // DayFormat extension
    private static readonly Dictionary<DayFormat, string> _dayFormatValues =
        Enum.GetValues<DayFormat>().ToDictionary(id => id, id => Enum.GetName(id)!.ToLowerInvariant());

    public static string? ToAttributeValue(this DayFormat? value)
    {
        if (value is null) return null;
        return value switch
        {
            DayFormat.TwoDigit => "2-digit",
            _ => _dayFormatValues[value.Value],
        };
    }

    // WeekdayFormat extension
    private static readonly Dictionary<WeekdayFormat, string> _weekdayFormatValues =
        Enum.GetValues<WeekdayFormat>().ToDictionary(id => id, id => Enum.GetName(id)!.ToLowerInvariant());

    public static string? ToAttributeValue(this WeekdayFormat? value) => value == null ? null : _weekdayFormatValues[value.Value];

    // MonthFormat extension
    private static readonly Dictionary<MonthFormat, string> _monthFormatValues =
        Enum.GetValues<MonthFormat>().ToDictionary(id => id, id => Enum.GetName(id)!.ToLowerInvariant());

    public static string? ToAttributeValue(this MonthFormat? value)
    {
        if (value is null) return null;
        return value switch
        {
            MonthFormat.TwoDigit => "2-digit",
            _ => _monthFormatValues[value.Value],
        };
    }

    // YearFormat extension 
    private static readonly Dictionary<YearFormat, string> _yearFormatValues =
        Enum.GetValues<YearFormat>().ToDictionary(id => id, id => Enum.GetName(id)!.ToLowerInvariant());

    public static string? ToAttributeValue(this YearFormat? value)
    {
        if (value is null) return null;
        return value switch
        {
            YearFormat.TwoDigit => "2-digit",
            _ => _yearFormatValues[value.Value],
        };
    }
}