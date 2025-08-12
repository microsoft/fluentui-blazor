// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Globalization;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.DateTimes;

public class CalendarExtendedTests
{
    [Fact]
    public void Constructor_InitializesProperties()
    {
        // Arrange
        var culture = new CultureInfo("en-US");
        var date = new DateTime(2023, 1, 1);

        // Act
        var calendar = new CalendarExtended(culture, date);

        // Assert
        Assert.Equal(culture, calendar.Culture);
        Assert.Equal(date, calendar.Date);
    }

    [Theory]
    [InlineData("en-US", 2023, 10, 26, 0, "10/22/2023")]
    [InlineData("fr-FR", 2023, 10, 26, 0, "23/10/2023")]
    [InlineData("en-US", 2023, 10, 26, 1, "10/29/2023")] // With month offset
    public void GetDaysOfWeek_ReturnsCorrectDays(string cultureName, int year, int month, int day, int weekNumber, string expectedStartDate)
    {
        // Arrange
        var culture = new CultureInfo(cultureName);
        var date = new DateTime(year, month, day);
        var calendar = new CalendarExtended(culture, date);
        var expected = DateTime.Parse(expectedStartDate, culture);

        // Act
        var week = calendar.GetDaysOfWeek(weekNumber).ToList();

        // Assert
        Assert.Equal(7, week.Count);
        Assert.Equal(expected.Date, week.First().Date);
    }

    [Fact]
    public void GetDaysOfWeek_ThrowsOnInvalidWeekNumber()
    {
        // Arrange
        var calendar = new CalendarExtended(new CultureInfo("en-US"), new DateTime(2025, 08, 12));

        // Act & Assert
        Assert.Throws<ArgumentException>(() => calendar.GetDaysOfWeek(-1).ToArray());
        Assert.Throws<ArgumentException>(() => calendar.GetDaysOfWeek(6).ToArray());
    }

    [Theory]
    [InlineData("en-US", 2023, 10, 26, "October")]
    [InlineData("fr-FR", 2023, 10, 26, "Octobre")]
    public void GetMonthName_ReturnsCorrectName(string cultureName, int year, int month, int day, string expectedMonth)
    {
        // Arrange
        var culture = new CultureInfo(cultureName);
        var date = new DateTime(year, month, day);
        var calendar = new CalendarExtended(culture, date);

        // Act
        var monthName = calendar.GetMonthName();

        // Assert
        Assert.Equal(expectedMonth, monthName);
    }

    [Theory]
    [InlineData("en-US", 12, "January", "December")]
    [InlineData("fr-FR", 12, "Janvier", "Décembre")]
    public void GetMonthNames_ReturnsAllMonths(string cultureName, int expectedCount, string firstMonth, string lastMonth)
    {
        // Arrange
        var culture = new CultureInfo(cultureName);
        var calendar = new CalendarExtended(culture, new DateTime(2025, 08, 12));

        // Act
        var months = calendar.GetMonthNames().ToList();

        // Assert
        Assert.Equal(expectedCount, months.Count);
        Assert.Equal(firstMonth, months.First().Name);
        Assert.Equal(lastMonth, months.Last().Name);
    }

    [Fact]
    public void GetYearsRange_Returns12Years()
    {
        // Arrange
        var calendar = new CalendarExtended(new CultureInfo("en-US"), new DateTime(2020, 1, 1));

        // Act
        var years = calendar.GetYearsRange().ToList();

        // Assert
        Assert.Equal(12, years.Count);
        Assert.Equal(2020, years.First().Year);
        Assert.Equal(2031, years.Last().Year);
    }

    [Fact]
    public void GetMonthNameAndYear_ReturnsCorrectFormat()
    {
        // Arrange
        var calendar = new CalendarExtended(new CultureInfo("en-US"), new DateTime(2023, 10, 1));

        // Act
        var result = calendar.GetMonthNameAndYear();

        // Assert
        Assert.Equal("October 2023", result);
    }

    [Fact]
    public void GetYear_ReturnsCorrectYear()
    {
        // Arrange
        var calendar = new CalendarExtended(new CultureInfo("en-US"), new DateTime(2023, 1, 1));

        // Act
        var year = calendar.GetYear();

        // Assert
        Assert.Equal("2023", year);
    }

    [Theory]
    [InlineData(2020, "2020 - 2031")]
    [InlineData(1, "1 - 12")]
    [InlineData(-5, "1 - 12")]
    [InlineData(9998, "9998 - 9999")]
    [InlineData(9999, "9999")]
    public void GetYearsRangeLabel_ReturnsCorrectRange(int fromYear, string expectedLabel)
    {
        // Arrange
        var calendar = new CalendarExtended(new CultureInfo("en-US"), new DateTime(2025, 08, 12));

        // Act
        var label = calendar.GetYearsRangeLabel(fromYear);

        // Assert
        Assert.Equal(expectedLabel, label);
    }

    [Theory]
    [InlineData("en-US", "Sunday", "Saturday")]
    [InlineData("fr-FR", "Lundi", "Dimanche")]
    public void GetDayNames_ReturnsCorrectlyOrderedDays(string cultureName, string firstDay, string lastDay)
    {
        // Arrange
        var culture = new CultureInfo(cultureName);
        var calendar = new CalendarExtended(culture, new DateTime(2025, 08, 12));

        // Act
        var dayNames = calendar.GetDayNames().ToList();

        // Assert
        Assert.Equal(7, dayNames.Count);
        Assert.Equal(firstDay, dayNames.First().Name);
        Assert.Equal(lastDay, dayNames.Last().Name);
    }

    [Theory]
    [InlineData(2023, 10, 15, true)]  // Middle of the month
    [InlineData(2023, 10, 1, true)]   // Start of the month
    [InlineData(2023, 10, 31, true)]  // End of the month
    [InlineData(2023, 9, 30, false)]  // Day before
    [InlineData(2023, 11, 1, false)]  // Day after
    public void IsInCurrentMonth_WorksCorrectly(int year, int month, int day, bool expected)
    {
        // Arrange
        var calendar = new CalendarExtended(new CultureInfo("en-US"), new DateTime(2023, 10, 15));
        var dateToCheck = new DateTime(year, month, day);

        // Act
        var result = calendar.IsInCurrentMonth(dateToCheck);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetCalendarDayOfMonth_ReturnsCorrectDay()
    {
        // Arrange
        var date = new DateTime(2023, 10, 26);
        var calendar = new CalendarExtended(new CultureInfo("en-US"), date);

        // Act
        var day = calendar.GetCalendarDayOfMonth(date);

        // Assert
        Assert.Equal(26, day);
    }

    [Fact]
    public void GetCalendarDayWithMonthName_ReturnsCorrectFormat()
    {
        // Arrange
        var date = new DateTime(2023, 10, 26);
        var calendar = new CalendarExtended(new CultureInfo("en-US"), date);

        // Act
        var result = calendar.GetCalendarDayWithMonthName(date);

        // Assert
        Assert.Equal("October 26", result);
    }
}
