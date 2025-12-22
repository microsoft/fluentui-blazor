// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Globalization;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Extensions;

public class DateTimeExtensionsTests
{
    [Fact]
    public void DateTimeExtensions_ToIsoDateString_NullDate_ReturnsEmptyString()
    {
        DateTime? date = null;
        var result = date.ToIsoDateString();
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void DateTimeExtensions_ToIsoDateString_ValidDate_ReturnsIsoFormattedString()
    {
        DateTime? date = new DateTime(2025, 4, 19);
        var result = date.ToIsoDateString();
        Assert.Equal("2025-04-19", result);
    }

    [Fact]
    public void DateTimeExtensions_StartOfMonth_ValidDate_ReturnsFirstDayOfMonth()
    {
        var date = new DateTime(2025, 4, 19);
        var culture = CultureInfo.InvariantCulture;
        var result = date.StartOfMonth(culture);
        Assert.Equal(new DateTime(2025, 4, 1), result);
    }

    [Fact]
    public void DateTimeExtensions_StartOfYear_ValidDate_ReturnsFirstDayOfYear()
    {
        var date = new DateTime(2025, 4, 19);
        var culture = CultureInfo.InvariantCulture;
        var result = date.StartOfYear(culture);
        Assert.Equal(new DateTime(2025, 1, 1), result);
    }

    [Fact]
    public void DateTimeExtensions_EndOfYear_ValidDate_ReturnsLastDayOfYear()
    {
        var date = new DateTime(2025, 4, 19);
        var culture = CultureInfo.InvariantCulture;
        var result = date.EndOfYear(culture);
        Assert.Equal(new DateTime(2025, 12, 31), result);
    }

    [Fact]
    public void DateTimeExtensions_EndOfMonth_ValidDate_ReturnsLastDayOfMonth()
    {
        var date = new DateTime(2025, 4, 19);
        var culture = CultureInfo.InvariantCulture;
        var result = date.EndOfMonth(culture);
        Assert.Equal(new DateTime(2025, 4, 30), result);
    }

    [Fact]
    public void DateTimeExtensions_StartOfWeek_ValidDate_ReturnsFirstDayOfWeek()
    {
        var date = new DateTime(2025, 4, 19); // Saturday
        var result = date.StartOfWeek(DayOfWeek.Monday);
        Assert.Equal(new DateTime(2025, 4, 14), result); // Monday
    }

    [Fact]
    public void DateTimeExtensions_StartOfWeek_WithCulture_ReturnsFirstDayOfWeek()
    {
        var date = new DateTime(2025, 4, 19); // Saturday
        var culture = CultureInfo.InvariantCulture;
        var result = date.StartOfWeek(culture);
        Assert.Equal(new DateTime(2025, 4, 13), result); // Sunday (default first day in InvariantCulture)
    }

    [Fact]
    public void DateTimeExtensions_StartOfWeek_Min()
    {
        var date = DateTime.MinValue; // January 1, 0001
        var result = date.StartOfWeek(DayOfWeek.Wednesday);
        Assert.Equal(new DateTime(1, 1, 1), result);
    }

    [Fact]
    public void DateTimeExtensions_StartOfWeek_WithCulture_Min()
    {
        var date = DateTime.MinValue; // January 1, 0001
        var culture = CultureInfo.InvariantCulture;
        var result = date.StartOfWeek(culture);
        Assert.Equal(new DateTime(1, 1, 1), result);
    }

    [Fact]
    public void DateTimeExtensions_StartOfWeek_NoParameters_ReturnsFirstDayOfWeek()
    {
        var date = new DateTime(2025, 4, 19); // Saturday
        var result = date.StartOfWeek();
        var expected = date.StartOfWeek(CultureInfo.CurrentUICulture);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void DateTimeExtensions_ToTimeAgo_ValidTimeSpan_ReturnsCorrectString()
    {
        var delay = TimeSpan.FromMinutes(5);
        var result = delay.ToTimeAgo();
        Assert.Contains("5 minutes ago", result);
    }

    [Fact]
    public void DateTimeExtensions_ToDateTime_DateOnly_ReturnsDateTime()
    {
        var dateOnly = new DateOnly(2025, 4, 19);
        var result = dateOnly.ToDateTime();
        Assert.Equal(new DateTime(2025, 4, 19), result);
    }

    [Fact]
    public void DateTimeExtensions_ToDateTime_TimeOnly_ReturnsDateTime()
    {
        var timeOnly = new TimeOnly(14, 30);
        var result = timeOnly.ToDateTime();
        Assert.Equal(new DateTime(1, 1, 1, 14, 30, 0), result);
    }

    [Fact]
    public void DateTimeExtensions_ToDateTimeNullable_NullDateOnly_ReturnsNull()
    {
        DateOnly? dateOnly = null;
        var result = dateOnly.ToDateTimeNullable();
        Assert.Null(result);
    }

    [Fact]
    public void DateTimeExtensions_ToDateOnly_NullDateTime_ReturnsMinValue()
    {
        DateTime? dateTime = null;
        var result = dateTime.ToDateOnly();
        Assert.Equal(DateOnly.MinValue, result);
    }

    [Fact]
    public void DateTimeExtensions_ToTimeOnly_NullDateTime_ReturnsMinValue()
    {
        DateTime? dateTime = null;
        var result = dateTime.ToTimeOnly();
        Assert.Equal(TimeOnly.MinValue, result);
    }

    [Fact]
    public void DateTimeExtensions_GetYear_ValidDate_ReturnsYear()
    {
        var date = new DateTime(2025, 4, 19);
        var culture = CultureInfo.InvariantCulture;
        var result = date.GetYear(culture);
        Assert.Equal(2025, result);
    }

    [Fact]
    public void DateTimeExtensions_GetMonth_ValidDate_ReturnsMonth()
    {
        var date = new DateTime(2025, 4, 19);
        var culture = CultureInfo.InvariantCulture;
        var result = date.GetMonth(culture);
        Assert.Equal(4, result);
    }

    [Fact]
    public void DateTimeExtensions_GetDay_ValidDate_ReturnsDay()
    {
        var date = new DateTime(2025, 4, 19);
        var culture = CultureInfo.InvariantCulture;
        var result = date.GetDay(culture);
        Assert.Equal(19, result);
    }

    [Fact]
    public void DateTimeExtensions_AddDays_ValidDate_AddsDays()
    {
        var date = new DateTime(2025, 4, 19);
        var culture = CultureInfo.InvariantCulture;
        var result = date.AddDays(5, culture);
        Assert.Equal(new DateTime(2025, 4, 24), result);
    }

    [Fact]
    public void DateTimeExtensions_AddMonths_ValidDate_AddsMonths()
    {
        var date = new DateTime(2025, 4, 19);
        var culture = CultureInfo.InvariantCulture;
        var result = date.AddMonths(2, culture);
        Assert.Equal(new DateTime(2025, 6, 19), result);
    }

    [Fact]
    public void DateTimeExtensions_AddYears_ValidDate_AddsYears()
    {
        var date = new DateTime(2025, 4, 19);
        var culture = CultureInfo.InvariantCulture;
        var result = date.AddYears(1, culture);
        Assert.Equal(new DateTime(2026, 4, 19), result);
    }

    [Fact]
    public void DateTimeExtensions_GetMonthName_ValidDate_ReturnsMonthName()
    {
        var date = new DateTime(2025, 4, 19);
        var culture = CultureInfo.InvariantCulture;
        var result = date.GetMonthName(culture);
        Assert.Equal("April", result);
    }

    [Fact]
    public void DateTimeExtensions_ToDateTime_NullTimeOnly_ReturnsMinValue()
    {
        TimeOnly? timeOnly = null;
        var result = timeOnly.ToDateTime();
        Assert.Equal(DateTime.MinValue, result);
    }

    [Fact]
    public void DateTimeExtensions_ToDateTime_NullableDateOnly_ReturnsCorrectDateTime()
    {
        // Test with null DateOnly  
        DateOnly? nullDateOnly = null;
        var result = nullDateOnly.ToDateTime();
        Assert.Equal(DateTime.MinValue, result);

        // Test with valid DateOnly  
        DateOnly? validDateOnly = new DateOnly(2025, 4, 19);
        result = validDateOnly.ToDateTime();
        Assert.Equal(new DateTime(2025, 4, 19), result);
    }

    [Fact]
    public void DateTimeExtensions_ToDateTimeNullable_ValidTimeOnly_ReturnsDateTime()
    {
        TimeOnly? timeOnly = new TimeOnly(14, 30);
        var result = timeOnly.ToDateTimeNullable();
        Assert.Equal(new DateTime(1, 1, 1, 14, 30, 0), result);
    }

    [Fact]
    public void DateTimeExtensions_ToDateTimeNullable_NullTimeOnly_ReturnsNull()
    {
        TimeOnly? timeOnly = null;
        var result = timeOnly.ToDateTimeNullable();
        Assert.Null(result);
    }

    [Fact]
    public void DateTimeExtensions_ToDateTime_NullableDateTime_ReturnsCorrectDateTime()
    {
        // Test with null DateTime
        DateTime? nullDateTime = null;
        var result = nullDateTime.ToDateTime();
        Assert.Equal(DateTime.MinValue, result);

        // Test with valid DateTime
        DateTime? validDateTime = new DateTime(2025, 4, 19);
        result = validDateTime.ToDateTime();
        Assert.Equal(new DateTime(2025, 4, 19), result);
    }

    [Fact]
    public void DateTimeExtensions_ToDateOnlyNullable_NullDateTime_ReturnsNull()
    {
        DateTime? dateTime = null;
        var result = dateTime.ToDateOnlyNullable();
        Assert.Null(result);
    }

    [Fact]
    public void DateTimeExtensions_ToDateOnlyNullable_ValidDateTime_ReturnsDateOnly()
    {
        DateTime? dateTime = new DateTime(2025, 4, 19);
        var result = dateTime.ToDateOnlyNullable();
        Assert.Equal(new DateOnly(2025, 4, 19), result);
    }

    [Fact]
    public void DateTimeExtensions_ToTimeOnlyNullable_NullDateTime_ReturnsNull()
    {
        DateTime? dateTime = null;
        var result = dateTime.ToTimeOnlyNullable();
        Assert.Null(result);
    }

    [Fact]
    public void DateTimeExtensions_ToTimeOnlyNullable_ValidDateTime_ReturnsTimeOnly()
    {
        DateTime? dateTime = new DateTime(2025, 4, 19, 14, 30, 0);
        var result = dateTime.ToTimeOnlyNullable();
        Assert.Equal(new TimeOnly(14, 30), result);
    }
}

