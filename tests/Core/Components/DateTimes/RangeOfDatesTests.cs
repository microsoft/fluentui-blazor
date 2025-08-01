// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.DateTimes;

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
}
