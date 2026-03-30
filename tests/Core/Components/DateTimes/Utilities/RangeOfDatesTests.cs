// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Globalization;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.DateTimes.Utilities;

public class RangeOfDatesTests
{
    [Fact]
    public void Constructor_Default_ShouldCreateEmptyRange()
    {
        // Arrange & Act
        var range = new RangeOfDates();

        // Assert
        Assert.Null(range.Start);
        Assert.Null(range.End);
    }

    [Fact]
    public void Constructor_WithStartAndEnd_ShouldSetValues()
    {
        // Arrange
        var start = new DateTime(2024, 1, 1);
        var end = new DateTime(2024, 1, 31);

        // Act
        var range = new RangeOfDates(start, end);

        // Assert
        Assert.Equal(start, range.Start);
        Assert.Equal(end, range.End);
    }

    [Fact]
    public void Constructor_WithNullValues_ShouldAcceptNulls()
    {
        // Arrange & Act
        var range = new RangeOfDates(null, null);

        // Assert
        Assert.Null(range.Start);
        Assert.Null(range.End);
    }

    [Fact]
    public void GetAllDates_WithValidRange_ShouldReturnAllDatesInRange()
    {
        // Arrange
        var start = new DateTime(2024, 1, 1);
        var end = new DateTime(2024, 1, 5);
        var range = new RangeOfDates(start, end);

        // Act
        var dates = range.GetAllDates();

        // Assert
        Assert.Equal(5, dates.Length);
        Assert.Equal(new DateTime(2024, 1, 1), dates[0]);
        Assert.Equal(new DateTime(2024, 1, 2), dates[1]);
        Assert.Equal(new DateTime(2024, 1, 3), dates[2]);
        Assert.Equal(new DateTime(2024, 1, 4), dates[3]);
        Assert.Equal(new DateTime(2024, 1, 5), dates[4]);
    }

    [Fact]
    public void GetAllDates_WithSingleDay_ShouldReturnOneDate()
    {
        // Arrange
        var date = new DateTime(2024, 1, 15);
        var range = new RangeOfDates(date, date);

        // Act
        var dates = range.GetAllDates();

        // Assert
        Assert.Single(dates);
        Assert.Equal(date, dates[0]);
    }

    [Fact]
    public void GetAllDates_WithNullStart_ShouldReturnOneDate()
    {
        // Arrange
        var end = new DateTime(2024, 1, 31);
        var range = new RangeOfDates(null, end);

        // Act
        var dates = range.GetAllDates();

        // Assert
        Assert.Single(dates);
        Assert.Equal(end, dates[0]);
    }

    [Fact]
    public void GetAllDates_WithNullEnd_ShouldReturnOneDate()
    {
        // Arrange
        var start = new DateTime(2024, 1, 1);
        var range = new RangeOfDates(start, null);

        // Act
        var dates = range.GetAllDates();

        // Assert
        Assert.Single(dates);
        Assert.Equal(start, dates[0]);
    }

    [Fact]
    public void GetAllDates_WithBothNull_ShouldReturnEmptyArray()
    {
        // Arrange
        var range = new RangeOfDates(null, null);

        // Act
        var dates = range.GetAllDates();

        // Assert
        Assert.Empty(dates);
    }

    [Fact]
    public void ToString_WithValidRange_ShouldFormatCorrectly()
    {
        // Arrange
        var start = new DateTime(2024, 1, 15);
        var end = new DateTime(2024, 12, 31);
        var range = new RangeOfDates(start, end);

        // Act
        var result = range.ToString();

        // Assert
        Assert.Equal("From 2024-01-15 to 2024-12-31.", result);
    }

    [Fact]
    public void ToString_WithNullStart_ShouldHandleGracefully()
    {
        // Arrange
        var end = new DateTime(2024, 12, 31);
        var range = new RangeOfDates(null, end);

        // Act
        var result = range.ToString();

        // Assert
        Assert.Equal("From <null> to 2024-12-31.", result);
    }

    [Fact]
    public void ToString_WithNullEnd_ShouldHandleGracefully()
    {
        // Arrange
        var start = new DateTime(2024, 1, 15);
        var range = new RangeOfDates(start, null);

        // Act
        var result = range.ToString();

        // Assert
        Assert.Equal("From 2024-01-15 to <null>.", result);
    }

    [Fact]
    public void ToString_WithBothNull_ShouldHandleGracefully()
    {
        // Arrange
        var range = new RangeOfDates(null, null);

        // Act
        var result = range.ToString();

        // Assert
        Assert.Equal("From <null> to <null>.", result);
    }

    // Base class method tests

    [Fact]
    public void IsEmpty_WithBothNull_ShouldReturnTrue()
    {
        // Arrange
        var range = new RangeOfDates(null, null);

        // Act
        var result = range.IsEmpty();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsEmpty_WithStartOnly_ShouldReturnFalse()
    {
        // Arrange
        var range = new RangeOfDates(new DateTime(2024, 1, 1), null);

        // Act
        var result = range.IsEmpty();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsEmpty_WithEndOnly_ShouldReturnFalse()
    {
        // Arrange
        var range = new RangeOfDates(null, new DateTime(2024, 1, 31));

        // Act
        var result = range.IsEmpty();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsEmpty_WithBothValues_ShouldReturnFalse()
    {
        // Arrange
        var range = new RangeOfDates(new DateTime(2024, 1, 1), new DateTime(2024, 1, 31));

        // Act
        var result = range.IsEmpty();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_WithDifferentStartAndEnd_ShouldReturnTrue()
    {
        // Arrange
        var range = new RangeOfDates(new DateTime(2024, 1, 1), new DateTime(2024, 1, 31));

        // Act
        var result = range.IsValid();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValid_WithSameStartAndEnd_ShouldReturnFalse()
    {
        // Arrange
        var date = new DateTime(2024, 1, 15);
        var range = new RangeOfDates(date, date);

        // Act
        var result = range.IsValid();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_WithNullStart_ShouldReturnFalse()
    {
        // Arrange
        var range = new RangeOfDates(null, new DateTime(2024, 1, 31));

        // Act
        var result = range.IsValid();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_WithNullEnd_ShouldReturnFalse()
    {
        // Arrange
        var range = new RangeOfDates(new DateTime(2024, 1, 1), null);

        // Act
        var result = range.IsValid();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsSingle_WithSameStartAndEnd_ShouldReturnTrue()
    {
        // Arrange
        var date = new DateTime(2024, 1, 15);
        var range = new RangeOfDates(date, date);

        // Act
        var result = range.IsSingle();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsSingle_WithDifferentStartAndEnd_ShouldReturnFalse()
    {
        // Arrange
        var range = new RangeOfDates(new DateTime(2024, 1, 1), new DateTime(2024, 1, 31));

        // Act
        var result = range.IsSingle();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsSingle_WithNullValues_ShouldReturnFalse()
    {
        // Arrange
        var range = new RangeOfDates(null, null);

        // Act
        var result = range.IsSingle();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Clear_ShouldSetBothValuesToNull()
    {
        // Arrange
        var range = new RangeOfDates(new DateTime(2024, 1, 1), new DateTime(2024, 1, 31));

        // Act
        range.Clear();

        // Assert
        Assert.Null(range.Start);
        Assert.Null(range.End);
        Assert.True(range.IsEmpty());
    }

    [Fact]
    public void Includes_WithDateInRange_ShouldReturnTrue()
    {
        // Arrange
        var range = new RangeOfDates(new DateTime(2024, 1, 1), new DateTime(2024, 1, 31));
        var date = new DateTime(2024, 1, 15);

        // Act
        var result = range.Includes(date);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Includes_WithStartDate_ShouldReturnTrue()
    {
        // Arrange
        var start = new DateTime(2024, 1, 1);
        var range = new RangeOfDates(start, new DateTime(2024, 1, 31));

        // Act
        var result = range.Includes(start);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Includes_WithEndDate_ShouldReturnTrue()
    {
        // Arrange
        var end = new DateTime(2024, 1, 31);
        var range = new RangeOfDates(new DateTime(2024, 1, 1), end);

        // Act
        var result = range.Includes(end);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Includes_WithDateBeforeRange_ShouldReturnFalse()
    {
        // Arrange
        var range = new RangeOfDates(new DateTime(2024, 1, 15), new DateTime(2024, 1, 31));
        var date = new DateTime(2024, 1, 1);

        // Act
        var result = range.Includes(date);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Includes_WithDateAfterRange_ShouldReturnFalse()
    {
        // Arrange
        var range = new RangeOfDates(new DateTime(2024, 1, 1), new DateTime(2024, 1, 15));
        var date = new DateTime(2024, 1, 31);

        // Act
        var result = range.Includes(date);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Includes_WithEmptyRange_ShouldReturnFalse()
    {
        // Arrange
        var range = new RangeOfDates(null, null);
        var date = new DateTime(2024, 1, 15);

        // Act
        var result = range.Includes(date);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Includes_WithReversedRange_ShouldStillWork()
    {
        // Arrange
        var range = new RangeOfDates(new DateTime(2024, 1, 31), new DateTime(2024, 1, 1));
        var date = new DateTime(2024, 1, 15);

        // Act
        var result = range.Includes(date);

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData(2024, 1, 1, true)]
    [InlineData(2024, 1, 15, false)]
    [InlineData(2024, 6, 1, false)]
    [InlineData(2024, 12, 15, false)]
    [InlineData(2024, 12, 16, true)]
    public void IsOutsideRange_ReturnsExpectedResult(int year, int month, int day, bool expected)
    {
        // Arrange
        var range = new RangeOfDates(new DateTime(2024, 1, 15), new DateTime(2024, 12, 15));
        var value = new DateTime(year, month, day);

        // Act
        var result = range.IsOutsideRange(value);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(2024, 1, 1, 2024, 1, 14, true)]
    [InlineData(2024, 12, 16, 2024, 12, 31, true)]
    [InlineData(2024, 1, 1, 2024, 1, 15, false)]
    [InlineData(2024, 12, 15, 2024, 12, 31, false)]
    [InlineData(2024, 6, 1, 2024, 6, 30, false)]
    public void IsPeriodOutsideRange_ReturnsExpectedResult(int startYear, int startMonth, int startDay, int endYear, int endMonth, int endDay, bool expected)
    {
        // Arrange
        var range = new RangeOfDates(new DateTime(2024, 1, 15), new DateTime(2024, 12, 15));
        var periodStart = new DateTime(startYear, startMonth, startDay);
        var periodEnd = new DateTime(endYear, endMonth, endDay);

        // Act
        var result = range.IsPeriodOutsideRange(periodStart, periodEnd);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(CalendarViews.Days, 2024, 1, 15, false)]
    [InlineData(CalendarViews.Days, 2024, 1, 1, true)]
    [InlineData(CalendarViews.Months, 2024, 2, 1, false)]
    [InlineData(CalendarViews.Years, 2025, 1, 1, true)]
    public void IsSelectionOutsideRange_ReturnsExpectedResult(CalendarViews view, int year, int month, int day, bool expected)
    {
        // Arrange
        var range = new RangeOfDates(new DateTime(2024, 1, 15), new DateTime(2024, 12, 15));
        var value = new DateTime(year, month, day);

        // Act
        var result = range.IsSelectionOutsideRange(value, view, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expected, result);
    }
}
