// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Globalization;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents a range of dates with a defined start and end, providing functionality to retrieve all dates within the range.
/// </summary>
public class RangeOfDates : RangeOf<System.DateTime>
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
        return $"From {Start?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)} to {End?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}.";
    }
}
