// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace FluentUI.Demo.SampleData;

/// <summary>
/// Returns a list of dates and times with random data.
/// </summary>
public class DateAndTime
{
    /// <summary>
    /// Get a list of all months.
    /// </summary>
    public static IEnumerable<MonthItem> AllMonths => Enumerable.Range(0, 12)
                                                                .Select(i => new MonthItem
                                                                {
                                                                    Index = $"{i + 1:00}",
                                                                    Name = GetMonthName(i)
                                                                });
    /// <summary />
    private static string GetMonthName(int index)
    {
        return System.Globalization
                     .DateTimeFormatInfo
                     .CurrentInfo
                     .MonthNames
                     .ElementAt(index % 12);
    }

    /// <summary>
    /// Represents a month item.
    /// </summary>
    public class MonthItem
    {
        /// <summary>
        /// Gets or sets the index of the month, formatted as "00".
        /// </summary>
        public string Index { get; set; } = "00";

        /// <summary>
        /// Gets or sets the name of the month.
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// Returns the month index and name.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"{Index:00} {Name}";
    }
}
