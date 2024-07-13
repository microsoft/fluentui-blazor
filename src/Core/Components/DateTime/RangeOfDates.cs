// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components.Components.DateTime;

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

    public override string ToString()
    {
        return $"From {Start?.ToString("yyyy-MM-dd")} to {End?.ToString("yyyy-MM-dd")}.";
    }
}
