using Microsoft.FluentUI.AspNetCore.Components.Resources;

namespace Microsoft.FluentUI.AspNetCore.Components;

public record TimeAgoOptions
{
    /// <summary>
    /// Gets the resource string for the format "{0} day ago"
    /// </summary>
    public string DayAgo { get; init; } = TimeAgoResource.DayAgo;

    /// <summary>
    /// Gets the resource string for the format "{0} days ago"
    /// </summary>
    public string DaysAgo { get; init; } = TimeAgoResource.DaysAgo;

    /// <summary>
    /// Gets the resource string for the format "{0} hour ago"
    /// </summary>
    public string HourAgo { get; init; } = TimeAgoResource.HourAgo;

    /// <summary>
    /// Gets the resource string for the format "{0} hours ago"
    /// </summary>
    public string HoursAgo { get; init; } = TimeAgoResource.HoursAgo;

    /// <summary>
    /// Gets the resource string for the format "{0} minute ago"
    /// </summary>
    public string MinuteAgo { get; init; } = TimeAgoResource.MinuteAgo;

    /// <summary>
    /// Gets the resource string for the format "{0} minutes ago"
    /// </summary>
    public string MinutesAgo { get; init; } = TimeAgoResource.MinutesAgo;

    /// <summary>
    /// Gets the resource string for the format "{0} month ago"
    /// </summary>
    public string MonthAgo { get; init; } = TimeAgoResource.MonthAgo;

    /// <summary>
    /// Gets the resource string for the format "{0} months ago"
    /// </summary>
    public string MonthsAgo { get; init; } = TimeAgoResource.MonthsAgo;

    /// <summary>
    /// Gets the resource string for the format "Just now"
    /// </summary>
    public string SecondAgo { get; init; } = TimeAgoResource.SecondAgo;

    /// <summary>
    /// Gets the resource string for the format "{0} seconds ago"
    /// </summary>
    public string SecondsAgo { get; init; } = TimeAgoResource.SecondsAgo;

    /// <summary>
    /// Gets the resource string for the format "{0} year ago"
    /// </summary>
    public string YearAgo { get; init; } = TimeAgoResource.YearAgo;

    /// <summary>
    /// Gets the resource string for the format "{0} years ago"
    /// </summary>
    public string YearsAgo { get; init; } = TimeAgoResource.YearsAgo;
}
