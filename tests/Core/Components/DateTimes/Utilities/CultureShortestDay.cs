// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.DateTimes.Utilities;
internal class CultureShortestDay
{
    public static readonly CultureInfo InvariantCulture = GetCultureWithShortestNames(CultureInfo.InvariantCulture, ["S", "M", "T", "W", "T", "F", "S"]);

    public static readonly CultureInfo UnitedStatesCulture = GetCultureWithShortestNames(CultureInfo.GetCultureInfo("en-US"), ["S", "M", "T", "W", "T", "F", "S"]);

    public static readonly CultureInfo FrenchCulture = GetCultureWithShortestNames(CultureInfo.GetCultureInfo("fr-FR"), ["D", "L", "M", "M", "J", "V", "S"]);

    public static readonly CultureInfo IranianCulture = GetCultureWithShortestNames(CultureInfo.GetCultureInfo("fa-IR"), ["ش", "ی", "د", "س", "چ", "پ", "ج"]);

    private static CultureInfo GetCultureWithShortestNames(CultureInfo culture, string[] shortestDayNames)
    {
        var clone = (CultureInfo)culture.Clone();
        clone.DateTimeFormat.ShortestDayNames = shortestDayNames;
        return clone;
    }
}
