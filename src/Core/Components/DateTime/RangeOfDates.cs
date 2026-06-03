// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Globalization;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents a range of dates with a defined start and end, providing functionality to retrieve all dates within the range.
/// </summary>
internal class RangeOfDates : RangeOf<System.DateTime>
{
    /// <inheritdoc cref="RangeOf{T}"/>
    public RangeOfDates() : base() { }

    /// <inheritdoc cref="RangeOf{T}"/>
    public RangeOfDates(System.DateTime? start, System.DateTime? end) : base(start, end) { }

    /// <summary>
    /// Returns all values between <see cref="RangeOf{T}.Start"/> and <see cref="RangeOf{T}.End"/>
    /// </summary>
    /// <returns></returns>
    public System.DateTime[] GetAllDates()
    {
        return GetRangeValues((min, max) =>
        {
            return [.. Enumerable.Range(0, (max - min).Days + 1).Select(offset => min.AddDays(offset))];
        });
    }

    /// <summary>
    /// Checks if a given date is outside the range defined by <see cref="RangeOf{T}.Start"/> and <see cref="RangeOf{T}.End"/>.
    /// </summary>
    /// <param name="value">The date to check.</param>
    /// <returns>True if the date is outside the range; otherwise, false.</returns>
    public bool IsOutside(DateTime value)
    {
        var min = Start?.Date;
        var max = End?.Date;
        var date = value.Date;

        if (min.HasValue && date < min.Value)
        {
            return true;
        }

        if (max.HasValue && date > max.Value)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Checks if a given period is outside the range defined by <see cref="RangeOf{T}.Start"/> and <see cref="RangeOf{T}.End"/>.
    /// </summary>
    /// <param name="periodStart">The start date of the period.</param>
    /// <param name="periodEnd">The end date of the period.</param>
    /// <returns>True if the period is outside the range; otherwise, false.</returns>
    public bool IsOutside(DateTime periodStart, DateTime periodEnd)
    {
        var min = Start?.Date;
        var max = End?.Date;

        if (min.HasValue && periodEnd.Date < min.Value)
        {
            return true;
        }

        if (max.HasValue && periodStart.Date > max.Value)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Checks if a given date is outside the range defined by <see cref="RangeOf{T}.Start"/> and <see cref="RangeOf{T}.End"/>, based on the specified calendar view.
    /// </summary>
    /// <param name="value">The date to check.</param>
    /// <param name="view">The calendar view to consider.</param>
    /// <param name="culture">The culture to use for date calculations.</param>
    /// <returns>True if the date is outside the range; otherwise, false.</returns>
    public bool IsOutside(DateTime value, CalendarViews view, CultureInfo culture)
    {
        return view switch
        {
            CalendarViews.Days => IsOutside(value),
            CalendarViews.Months => IsOutside(value.StartOfMonth(culture), value.EndOfMonth(culture)),
            CalendarViews.Years => IsOutside(value.StartOfYear(culture), value.EndOfYear(culture)),
            _ => false,
        };
    }

    /// <summary>
    /// Returns a string representation of the date range, formatted as "From {Start} to {End}".
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return $"From {Start?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) ?? "<null>"} to {End?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) ?? "<null>"}.";
    }
}
