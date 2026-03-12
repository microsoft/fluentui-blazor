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
            return Enumerable.Range(0, (max - min).Days + 1)
                             .Select(offset => min.AddDays(offset))
                             .ToArray();
        });
    }

    /// <summary>
    /// Returns a string representation of the date range, formatted as "From {Start} to {End}".
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return $"From {Start?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) ?? "<null>"} to {End?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) ?? "<null>"}.";
    }

    internal bool IsOutsideRange(DateTime value)
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

    internal bool IsPeriodOutsideRange(DateTime periodStart, DateTime periodEnd)
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

    internal bool IsSelectionOutsideRange(DateTime value, CalendarViews view, CultureInfo culture)
        => view switch
        {
            CalendarViews.Days => IsOutsideRange(value),
            CalendarViews.Months => IsPeriodOutsideRange(value.StartOfMonth(culture), value.EndOfMonth(culture)),
            CalendarViews.Years => IsPeriodOutsideRange(value.StartOfYear(culture), value.EndOfYear(culture)),
            _ => false,
        };
}
