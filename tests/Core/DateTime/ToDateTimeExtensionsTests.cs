using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.DateTime;

public class ToDateTimeExtensionsTests : TestBase
{
    [Theory]
    [InlineData("2024-02-11", "2024-02-11 00:00:00")]
    public void DateOnly_ToDateTime(string dateOnly, string expected)
    {
        var value = DateOnly.Parse(dateOnly);
        var dateTime = DateTimeExtensions.ToDateTime(value);

        Assert.Equal(expected, dateTime.ToString("yyyy-MM-dd HH:mm:ss"));
    }

    [Theory]
    [InlineData("14:23:45", "0001-01-01 14:23:45")]
    public void TimeOnly_ToDateTime(string timeOnly, string expected)
    {
        var value = TimeOnly.Parse(timeOnly);
        var dateTime = DateTimeExtensions.ToDateTime(value);

        Assert.Equal(expected, dateTime.ToString("yyyy-MM-dd HH:mm:ss"));
    }

    [Theory]
    [InlineData("2024-02-11", "2024-02-11 00:00:00")]
    [InlineData(null, "0001-01-01 00:00:00")]
    public void DateOnlyNullable_ToDateTime(string? dateOnly, string expected)
    {
        var value = string.IsNullOrEmpty(dateOnly) ? (DateOnly?)null : DateOnly.Parse(dateOnly);
        var dateTime = DateTimeExtensions.ToDateTime(value);

        Assert.Equal(expected, dateTime.ToString("yyyy-MM-dd HH:mm:ss"));
    }

    [Theory]
    [InlineData("14:23:45", "0001-01-01 14:23:45")]
    [InlineData(null, "0001-01-01 00:00:00")]
    public void TimeOnlyNullable_ToDateTime(string? timeOnly, string expected)
    {
        var value = string.IsNullOrEmpty(timeOnly) ? (TimeOnly?)null : TimeOnly.Parse(timeOnly);
        var dateTime = DateTimeExtensions.ToDateTime(value);

        Assert.Equal(expected, dateTime.ToString("yyyy-MM-dd HH:mm:ss"));
    }

    [Theory]
    [InlineData("2024-02-11", "2024-02-11 00:00:00")]
    [InlineData(null, null)]
    public void DateOnlyNullable_ToDateTimeNullable(string? dateOnly, string? expected)
    {
        var value = string.IsNullOrEmpty(dateOnly) ? (DateOnly?)null : DateOnly.Parse(dateOnly);
        var dateTime = DateTimeExtensions.ToDateTimeNullable(value);

        Assert.Equal(expected, dateTime?.ToString("yyyy-MM-dd HH:mm:ss"));
    }

    [Theory]
    [InlineData("14:23:45", "0001-01-01 14:23:45")]
    [InlineData(null, null)]
    public void TimeOnlyNullable_ToDateTimeNullable(string? timeOnly, string? expected)
    {
        var value = string.IsNullOrEmpty(timeOnly) ? (TimeOnly?)null : TimeOnly.Parse(timeOnly);
        var dateTime = DateTimeExtensions.ToDateTimeNullable(value);

        Assert.Equal(expected, dateTime?.ToString("yyyy-MM-dd HH:mm:ss"));
    }

    [Theory]
    [InlineData("2024-02-11", "2024-02-11 00:00:00")]
    [InlineData("2024-02-11 14:23:45", "2024-02-11 14:23:45")]
    [InlineData(null, "0001-01-01 00:00:00")]
    public void DateTimeNullable_ToDateTime(string? dateTime, string expected)
    {
        var value = string.IsNullOrEmpty(dateTime) ? (System.DateTime?)null : System.DateTime.Parse(dateTime);
        var newDateTime = DateTimeExtensions.ToDateTime(value);

        Assert.Equal(expected, newDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
    }

    [Theory]
    [InlineData("2024-02-11", "2024-02-11")]
    [InlineData("2024-02-11 14:23:45", "2024-02-11")]
    [InlineData(null, "0001-01-01")]
    public void DateTimeNullable_ToDateOnly(string? dateTime, string expected)
    {
        var value = string.IsNullOrEmpty(dateTime) ? (System.DateTime?)null : System.DateTime.Parse(dateTime);
        var newDateTime = DateTimeExtensions.ToDateOnly(value);

        Assert.Equal(expected, newDateTime.ToString("yyyy-MM-dd"));
    }

    [Theory]
    [InlineData("2024-02-11", "00:00:00")]
    [InlineData("2024-02-11 14:23:45", "14:23:45")]
    [InlineData(null, "00:00:00")]
    public void DateTimeNullable_ToTimeOnly(string? dateTime, string expected)
    {
        var value = string.IsNullOrEmpty(dateTime) ? (System.DateTime?)null : System.DateTime.Parse(dateTime);
        var newDateTime = DateTimeExtensions.ToTimeOnly(value);

        Assert.Equal(expected, newDateTime.ToString("HH:mm:ss"));
    }

    [Theory]
    [InlineData("2024-02-11", "2024-02-11")]
    [InlineData("2024-02-11 14:23:45", "2024-02-11")]
    [InlineData(null, null)]
    public void DateTimeNullable_ToDateOnlyNullable(string? dateTime, string? expected)
    {
        var value = string.IsNullOrEmpty(dateTime) ? (System.DateTime?)null : System.DateTime.Parse(dateTime);
        var newDateTime = DateTimeExtensions.ToDateOnlyNullable(value);

        Assert.Equal(expected, newDateTime?.ToString("yyyy-MM-dd"));
    }

    [Theory]
    [InlineData("2024-02-11", "00:00:00")]
    [InlineData("2024-02-11 14:23:45", "14:23:45")]
    [InlineData(null, null)]
    public void DateTimeNullable_ToTimeOnlyNullable(string? dateTime, string? expected)
    {
        var value = string.IsNullOrEmpty(dateTime) ? (System.DateTime?)null : System.DateTime.Parse(dateTime);
        var newDateTime = DateTimeExtensions.ToTimeOnlyNullable(value);

        Assert.Equal(expected, newDateTime?.ToString("HH:mm:ss"));
    }
}
