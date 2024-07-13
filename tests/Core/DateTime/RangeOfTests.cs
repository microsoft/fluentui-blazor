// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.Components.DateTime;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.DateTime;

public class RangeOfTests
{
    private static readonly System.DateTime START_DATE = new System.DateTime(2021, 3, 15);
    private static readonly System.DateTime END_DATE = new System.DateTime(2021, 10, 10);

    private static readonly System.DateTime INCLUDED_DATE = new System.DateTime(2021, 07, 02);
    private static readonly System.DateTime EXCLUDED_BEFORE_DATE = new System.DateTime(2020, 10, 01);
    private static readonly System.DateTime EXCLUDED_AFTER_DATE = new System.DateTime(2022, 10, 01);

    [Fact]
    public void RangeOfDates_Default_Includes()
    {
        // Arrange
        var range = new RangeOfDates(START_DATE, END_DATE);

        // Assert
        Assert.True(range.Includes(INCLUDED_DATE));
        Assert.True(range.Includes(START_DATE));
        Assert.True(range.Includes(END_DATE));

        Assert.False(range.Includes(EXCLUDED_BEFORE_DATE));
        Assert.False(range.Includes(EXCLUDED_AFTER_DATE));
    }

    [Fact]
    public void RangeOfDates_Inversed_Includes()
    {
        // Arrange (Start is greater than End)
        var range = new RangeOfDates(END_DATE, START_DATE);

        // Assert
        Assert.True(range.Includes(INCLUDED_DATE));
        Assert.True(range.Includes(START_DATE));
        Assert.True(range.Includes(END_DATE));

        Assert.False(range.Includes(EXCLUDED_BEFORE_DATE));
        Assert.False(range.Includes(EXCLUDED_AFTER_DATE));
    }

    [Fact]
    public void RangeOfDates_IsEmpty()
    {
        // Arrange
        var rangeNull = new RangeOfDates();
        var rangeStartNull = new RangeOfDates(null, END_DATE);
        var rangeEndNull = new RangeOfDates(START_DATE, null);
        var rangeNotNull = new RangeOfDates(START_DATE, END_DATE);

        // Assert
        Assert.True(rangeNull.IsEmpty());

        Assert.False(rangeStartNull.IsEmpty());
        Assert.False(rangeEndNull.IsEmpty());
        Assert.False(rangeNotNull.IsEmpty());
    }

    [Fact]
    public void RangeOfDates_IsValid()
    {
        // Arrange
        var rangeNull = new RangeOfDates();
        var rangeStartNull = new RangeOfDates(null, END_DATE);
        var rangeEndNull = new RangeOfDates(START_DATE, null);
        var rangeSameDates = new RangeOfDates(START_DATE, START_DATE);
        var rangeValid = new RangeOfDates(START_DATE, END_DATE);

        // Assert
        Assert.True(rangeValid.IsValid());

        Assert.False(rangeNull.IsValid());
        Assert.False(rangeStartNull.IsValid());
        Assert.False(rangeEndNull.IsValid());
        Assert.False(rangeSameDates.IsValid());
    }

    [Fact]
    public void RangeOfDates_GetAllDates()
    {
        // Arrange
        var range = new RangeOfDates(START_DATE, END_DATE);
        var rangeInversed = new RangeOfDates(END_DATE, START_DATE);
        var rangeNull = new RangeOfDates();
        var rangeStartNull = new RangeOfDates(null, START_DATE);
        var rangeEndNull = new RangeOfDates(START_DATE, null);

        // Assert
        Assert.Equal(210, range.GetAllDates().Count());
        Assert.Equal(210, rangeInversed.GetAllDates().Count());

        Assert.Empty(rangeNull.GetAllDates());
        Assert.Equal(START_DATE, rangeStartNull.GetAllDates().Single());
        Assert.Equal(START_DATE, rangeEndNull.GetAllDates().Single());
    }

    [Fact]
    public void RangeOfDates_Default_Clear()
    {
        // Arrange
        var range = new RangeOfDates(START_DATE, END_DATE);

        // Act
        range.Clear();

        // Assert
        Assert.False(range.Includes(START_DATE));
        Assert.False(range.Includes(END_DATE));

        Assert.Null(range.Start);
        Assert.Null(range.End);
    }

    [Fact]
    public void RangeOfDates_Default_ToString()
    {
        // Arrange
        var range = new RangeOfDates(START_DATE, END_DATE);

        // Assert
        Assert.Equal("From 2021-03-15 to 2021-10-10.", range.ToString());
    }
}
