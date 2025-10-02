// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Globalization;
using Microsoft.FluentUI.AspNetCore.Components.Calendar;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.DateTimes.Utilities;

public class CalendarTValueTests
{
    [Theory]
    [InlineData(typeof(DateTime))]
    [InlineData(typeof(DateTime?))]
    [InlineData(typeof(DateOnly))]
    [InlineData(typeof(DateOnly?))]
    [InlineData(typeof(DateTimeOffset))]
    [InlineData(typeof(DateTimeOffset?))]
    public void IsNotDateType_SupportedTypes_ReturnsFalse(Type type)
    {
        // Act
        var result = type.IsNotDateType();

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData(typeof(string))]
    [InlineData(typeof(int))]
    [InlineData(typeof(double))]
    [InlineData(typeof(bool))]
    [InlineData(typeof(object))]
    [InlineData(typeof(decimal))]
    [InlineData(typeof(float))]
    [InlineData(typeof(long))]
    public void IsNotDateType_UnsupportedTypes_ReturnsTrue(Type type)
    {
        // Act
        var result = type.IsNotDateType();

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData("2023-10-26 14:30:00", typeof(DateTime), true, "2023-10-26 14:30:00")]
    [InlineData("2023-10-26 00:00:00", typeof(DateOnly), true, "2023-10-26 00:00:00")]
    [InlineData("2023-10-26 14:30:00", typeof(DateTimeOffset), true, "2023-10-26 14:30:00")]
    [InlineData("2023-10-26 14:30:00", typeof(DateTime), false, "2023-10-26 14:30:00")]
    [InlineData("2023-10-26 00:00:00", typeof(DateOnly), false, "2023-10-26 00:00:00")]
    [InlineData("2023-10-26 14:30:00", typeof(DateTimeOffset), false, "2023-10-26 14:30:00")]
    public void ConvertToDateTime_ValidValues_ReturnsCorrectDateTime(string inputDate, Type inputType, bool isNullOrDefault, string expectedDate)
    {
        // Arrange
        var expectedDateTime = DateTime.Parse(expectedDate, CultureInfo.InvariantCulture);
        object inputValue = inputType switch
        {
            Type t when t == typeof(DateTime) => DateTime.Parse(inputDate, CultureInfo.InvariantCulture),
            Type t when t == typeof(DateOnly) => DateOnly.Parse(inputDate.Split(' ')[0], CultureInfo.InvariantCulture),
            Type t when t == typeof(DateTimeOffset) => new DateTimeOffset(DateTime.Parse(inputDate, CultureInfo.InvariantCulture), TimeSpan.FromHours(2)),
            _ => throw new ArgumentException($"Unsupported type: {inputType}")
        };

        // Act
        var result = inputType switch
        {
            Type t when t == typeof(DateTime) => ((DateTime)inputValue).ConvertToDateTime(isNullOrDefault),
            Type t when t == typeof(DateOnly) => ((DateOnly)inputValue).ConvertToDateTime(isNullOrDefault),
            Type t when t == typeof(DateTimeOffset) => ((DateTimeOffset)inputValue).ConvertToDateTime(isNullOrDefault),
            _ => throw new ArgumentException($"Unsupported type: {inputType}")
        };

        // Assert
        Assert.Equal(expectedDateTime, result);
    }

    [Theory]
    [InlineData(typeof(DateTime?), true, null)]
    [InlineData(typeof(DateOnly?), true, null)]
    [InlineData(typeof(DateTimeOffset?), true, null)]
    [InlineData(typeof(DateTime?), false, null)]
    [InlineData(typeof(DateOnly?), false, null)]
    [InlineData(typeof(DateTimeOffset?), false, null)]
    [InlineData(typeof(DateTime), false, "0001-01-01")]
    [InlineData(typeof(DateOnly), false, "0001-01-01")]
    [InlineData(typeof(DateTimeOffset), false, "0001-01-01")]
    [InlineData(typeof(int), true, null)]
    [InlineData(typeof(int), false, null)]
    public void ConvertToDateTime_NullValues_ReturnsNull(Type nullableType, bool isNullOrDefault, string? expectedDateTimeString)
    {
        DateTime? expectedDateTime = expectedDateTimeString != null ? DateTime.Parse(expectedDateTimeString, CultureInfo.InvariantCulture) : null;

        // Arrange & Act
        var result = nullableType switch
        {
            Type t when t == typeof(DateTime?) => ((DateTime?)null).ConvertToDateTime(isNullOrDefault),
            Type t when t == typeof(DateOnly?) => ((DateOnly?)null).ConvertToDateTime(isNullOrDefault),
            Type t when t == typeof(DateTimeOffset?) => ((DateTimeOffset?)null).ConvertToDateTime(isNullOrDefault),
            Type t when t == typeof(DateTime) => DateTime.MinValue.ConvertToDateTime(isNullOrDefault),
            Type t when t == typeof(DateOnly) => DateOnly.MinValue.ConvertToDateTime(isNullOrDefault),
            Type t when t == typeof(DateTimeOffset) => DateTimeOffset.MinValue.ConvertToDateTime(isNullOrDefault),
            _ => 0.ConvertToDateTime(isNullOrDefault)
        };

        // Assert
        Assert.Equal(expectedDateTime, result);
    }

    [Theory]
    [InlineData(typeof(DateTime))]
    [InlineData(typeof(DateOnly))]
    [InlineData(typeof(DateTimeOffset))]
    public void ConvertToDateTime_DefaultValues_ReturnsNull(Type defaultType)
    {
        // Act & Assert
        if (defaultType == typeof(DateTime))
        {
            var defaultDateTime = default(DateTime);
            var result = defaultDateTime.ConvertToDateTime();
            Assert.Null(result);
        }
        else if (defaultType == typeof(DateOnly))
        {
            var defaultDateOnly = default(DateOnly);
            var result = defaultDateOnly.ConvertToDateTime();
            Assert.Null(result);
        }
        else if (defaultType == typeof(DateTimeOffset))
        {
            var defaultDateTimeOffset = default(DateTimeOffset);
            var result = defaultDateTimeOffset.ConvertToDateTime();
            Assert.Null(result);
        }
    }

    #region ConvertToRequiredDateTime Tests

    [Fact]
    public void ConvertToRequiredDateTime_DateTime_ReturnsCorrectValue()
    {
        // Arrange
        var dateTime = new DateTime(2023, 10, 26, 14, 30, 0);

        // Act
        var result = dateTime.ConvertToRequiredDateTime();

        // Assert
        Assert.Equal(dateTime, result);
    }

    [Fact]
    public void ConvertToRequiredDateTime_DateOnly_ReturnsCorrectDateTime()
    {
        // Arrange
        var dateOnly = new DateOnly(2023, 10, 26);
        var expectedDateTime = new DateTime(2023, 10, 26);

        // Act
        var result = dateOnly.ConvertToRequiredDateTime();

        // Assert
        Assert.Equal(expectedDateTime, result);
    }

    [Fact]
    public void ConvertToRequiredDateTime_NullValue_ThrowsArgumentNullException()
    {
        // Arrange
        DateTime? nullDateTime = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => nullDateTime.ConvertToRequiredDateTime());
    }

    [Fact]
    public void ConvertToRequiredDateTime_DefaultDateTime_ReturnsToday()
    {
        using var context = new DateTimeProviderContext(DateTime.Today);

        // Arrange
        var defaultDateTime = default(DateTime);

        // Act
        var result = defaultDateTime.ConvertToRequiredDateTime();

        // Assert
        Assert.Equal(DateTimeProvider.Today, result);
    }

    #endregion

    #region ConvertToTValue Tests

    [Fact]
    public void ConvertToTValue_DateTime_ReturnsCorrectValue()
    {
        // Arrange
        var dateTime = new DateTime(2023, 10, 26, 14, 30, 0);

        // Act
        var result = dateTime.ConvertToTValue<DateTime>();

        // Assert
        Assert.Equal(dateTime, result);
    }

    [Fact]
    public void ConvertToTValue_NullableDateTime_ReturnsCorrectValue()
    {
        // Arrange
        var dateTime = new DateTime(2023, 10, 26, 14, 30, 0);

        // Act
        var result = dateTime.ConvertToTValue<DateTime?>();

        // Assert
        Assert.Equal(dateTime, result);
    }

    [Fact]
    public void ConvertToTValue_DateOnly_ReturnsCorrectValue()
    {
        // Arrange
        var dateTime = new DateTime(2023, 10, 26, 14, 30, 0);
        var expectedDateOnly = DateOnly.FromDateTime(dateTime);

        // Act
        var result = dateTime.ConvertToTValue<DateOnly>();

        // Assert
        Assert.Equal(expectedDateOnly, result);
    }

    [Fact]
    public void ConvertToTValue_NullableDateOnly_ReturnsCorrectValue()
    {
        // Arrange
        var dateTime = new DateTime(2023, 10, 26, 14, 30, 0);
        var expectedDateOnly = DateOnly.FromDateTime(dateTime);

        // Act
        var result = dateTime.ConvertToTValue<DateOnly?>();

        // Assert
        Assert.Equal(expectedDateOnly, result);
    }

    [Fact]
    public void ConvertToTValue_UnsupportedType_ThrowsArgumentException()
    {
        // Arrange
        var dateTime = new DateTime(2023, 10, 26, 14, 30, 0);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => dateTime.ConvertToTValue<string>());
        Assert.Contains("Unsupported type", exception.Message);
    }

    #endregion

    #region IsNullOrDefault Tests

    [Fact]
    public void IsNullOrDefault_NullValue_ReturnsTrue()
    {
        // Arrange
        DateTime? nullValue = null;

        // Act
        var result = nullValue.IsNullOrDefault();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsNullOrDefault_DefaultDateTime_ReturnsTrue()
    {
        // Arrange
        var defaultValue = default(DateTime);

        // Act
        var result = defaultValue.IsNullOrDefault();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsNullOrDefault_DefaultDateOnly_ReturnsTrue()
    {
        // Arrange
        var defaultValue = default(DateOnly);

        // Act
        var result = defaultValue.IsNullOrDefault();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsNullOrDefault_ValidDateTime_ReturnsFalse()
    {
        // Arrange
        var validValue = new DateTime(2023, 10, 26);

        // Act
        var result = validValue.IsNullOrDefault();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsNullOrDefault_ValidDateOnly_ReturnsFalse()
    {
        // Arrange
        var validValue = new DateOnly(2023, 10, 26);

        // Act
        var result = validValue.IsNullOrDefault();

        // Assert
        Assert.False(result);
    }

    #endregion

    #region IsNotNull Tests

    [Fact]
    public void IsNotNull_NullValue_ReturnsFalse()
    {
        // Arrange
        DateTime? nullValue = null;

        // Act
        var result = nullValue.IsNotNull();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsNotNull_DefaultValue_ReturnsFalse()
    {
        // Arrange
        var defaultValue = default(DateTime);

        // Act
        var result = defaultValue.IsNotNull();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsNotNull_ValidValue_ReturnsTrue()
    {
        // Arrange
        var validValue = new DateTime(2023, 10, 26);

        // Act
        var result = validValue.IsNotNull();

        // Assert
        Assert.True(result);
    }

    #endregion

    #region AddMonths Tests

    [Fact]
    public void AddMonths_DateTime_ReturnsCorrectResult()
    {
        // Arrange
        var dateTime = new DateTime(2023, 10, 26);
        var culture = CultureInfo.InvariantCulture;
        var expectedResult = new DateTime(2023, 12, 26);

        // Act
        var result = dateTime.AddMonths(2, culture);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void AddMonths_DateOnly_ReturnsCorrectResult()
    {
        // Arrange
        var dateOnly = new DateOnly(2023, 10, 26);
        var culture = CultureInfo.InvariantCulture;
        var expectedResult = new DateOnly(2023, 12, 26);

        // Act
        var result = dateOnly.AddMonths(2, culture);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void AddMonths_NullValue_ReturnsDefault()
    {
        // Arrange
        DateTime? nullValue = null;
        var culture = CultureInfo.InvariantCulture;

        // Act
        var result = nullValue.AddMonths<DateTime?>(2, culture);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void AddMonths_NegativeMonths_ReturnsCorrectResult()
    {
        // Arrange
        var dateTime = new DateTime(2023, 10, 26);
        var culture = CultureInfo.InvariantCulture;
        var expectedResult = new DateTime(2023, 8, 26);

        // Act
        var result = dateTime.AddMonths(-2, culture);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    #endregion

    #region AddYears Tests

    [Fact]
    public void AddYears_DateTime_ReturnsCorrectResult()
    {
        // Arrange
        var dateTime = new DateTime(2023, 10, 26);
        var culture = CultureInfo.InvariantCulture;
        var expectedResult = new DateTime(2025, 10, 26);

        // Act
        var result = dateTime.AddYears(2, culture);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void AddYears_DateOnly_ReturnsCorrectResult()
    {
        // Arrange
        var dateOnly = new DateOnly(2023, 10, 26);
        var culture = CultureInfo.InvariantCulture;
        var expectedResult = new DateOnly(2025, 10, 26);

        // Act
        var result = dateOnly.AddYears(2, culture);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void AddYears_NullValue_ReturnsDefault()
    {
        // Arrange
        DateTime? nullValue = null;
        var culture = CultureInfo.InvariantCulture;

        // Act
        var result = nullValue.AddYears<DateTime?>(2, culture);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void AddYears_NegativeYears_ReturnsCorrectResult()
    {
        // Arrange
        var dateTime = new DateTime(2023, 10, 26);
        var culture = CultureInfo.InvariantCulture;
        var expectedResult = new DateTime(2021, 10, 26);

        // Act
        var result = dateTime.AddYears(-2, culture);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    #endregion

    #region GetYear Tests

    [Fact]
    public void GetYear_DateTime_ReturnsCorrectYear()
    {
        // Arrange
        var dateTime = new DateTime(2023, 10, 26);
        var culture = CultureInfo.InvariantCulture;

        // Act
        var result = dateTime.GetYear(culture);

        // Assert
        Assert.Equal(2023, result);
    }

    [Fact]
    public void GetYear_DateOnly_ReturnsCorrectYear()
    {
        // Arrange
        var dateOnly = new DateOnly(2023, 10, 26);
        var culture = CultureInfo.InvariantCulture;

        // Act
        var result = dateOnly.GetYear(culture);

        // Assert
        Assert.Equal(2023, result);
    }

    [Fact]
    public void GetYear_DefaultValue_ReturnsMinValueYear()
    {
        // Arrange
        var defaultDateTime = default(DateTime);
        var culture = CultureInfo.InvariantCulture;

        // Act
        var result = defaultDateTime.GetYear(culture);

        // Assert
        Assert.Equal(DateTime.MinValue.Year, result);
    }

    #endregion

    #region MinDateTime Tests

    [Fact]
    public void MinDateTime_DateTimeCollection_ReturnsMinimum()
    {
        // Arrange
        var dates = new[]
        {
            new DateTime(2023, 10, 26),
            new DateTime(2023, 5, 15),
            new DateTime(2023, 12, 31)
        };

        // Act
        var result = dates.MinDateTime();

        // Assert
        Assert.Equal(new DateTime(2023, 5, 15), result);
    }

    [Fact]
    public void MinDateTime_DateOnlyCollection_ReturnsMinimum()
    {
        // Arrange
        var dates = new[]
        {
            new DateOnly(2023, 10, 26),
            new DateOnly(2023, 5, 15),
            new DateOnly(2023, 12, 31)
        };

        // Act
        var result = dates.MinDateTime();

        // Assert
        Assert.Equal(new DateTime(2023, 5, 15), result);
    }

    [Fact]
    public void MinDateTime_SingleItem_ReturnsThatItem()
    {
        // Arrange
        var dates = new[] { new DateTime(2023, 10, 26) };

        // Act
        var result = dates.MinDateTime();

        // Assert
        Assert.Equal(new DateTime(2023, 10, 26), result);
    }

    [Fact]
    public void MinDateTime_EmptyCollection_ThrowsInvalidOperationException()
    {
        // Arrange
        var dates = Array.Empty<DateTime>();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => dates.MinDateTime());
    }

    #endregion

    #region MaxDateTime Tests

    [Fact]
    public void MaxDateTime_DateTimeCollection_ReturnsMaximum()
    {
        // Arrange
        var dates = new[]
        {
            new DateTime(2023, 10, 26),
            new DateTime(2023, 5, 15),
            new DateTime(2023, 12, 31)
        };

        // Act
        var result = dates.MaxDateTime();

        // Assert
        Assert.Equal(new DateTime(2023, 12, 31), result);
    }

    [Fact]
    public void MaxDateTime_DateOnlyCollection_ReturnsMaximum()
    {
        // Arrange
        var dates = new[]
        {
            new DateOnly(2023, 10, 26),
            new DateOnly(2023, 5, 15),
            new DateOnly(2023, 12, 31)
        };

        // Act
        var result = dates.MaxDateTime();

        // Assert
        Assert.Equal(new DateTime(2023, 12, 31), result);
    }

    [Fact]
    public void MaxDateTime_SingleItem_ReturnsThatItem()
    {
        // Arrange
        var dates = new[] { new DateTime(2023, 10, 26) };

        // Act
        var result = dates.MaxDateTime();

        // Assert
        Assert.Equal(new DateTime(2023, 10, 26), result);
    }

    [Fact]
    public void MaxDateTime_EmptyCollection_ThrowsInvalidOperationException()
    {
        // Arrange
        var dates = Array.Empty<DateTime>();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => dates.MaxDateTime());
    }

    #endregion

    #region Mixed Type Tests

    [Fact]
    public void ConvertToDateTime_MixedDateTimeOffset_HandlesTimeZoneCorrectly()
    {
        // Arrange
        var dateTime = new DateTime(2023, 10, 26, 12, 0, 0, DateTimeKind.Unspecified);
        var offset = TimeSpan.FromHours(5);
        var dateTimeOffset = new DateTimeOffset(dateTime, offset);

        // Act
        var result = dateTimeOffset.ConvertToDateTime();

        // Assert
        Assert.Equal(dateTimeOffset.DateTime, result);
    }

    [Fact]
    public void AddMonths_CrossYearBoundary_HandlesCorrectly()
    {
        // Arrange
        var dateTime = new DateTime(2023, 11, 15);
        var culture = CultureInfo.InvariantCulture;
        var expectedResult = new DateTime(2024, 2, 15);

        // Act
        var result = dateTime.AddMonths(3, culture);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void AddYears_LeapYear_HandlesCorrectly()
    {
        // Arrange
        var dateTime = new DateTime(2020, 2, 29); // Leap year
        var culture = CultureInfo.InvariantCulture;
        var expectedResult = new DateTime(2024, 2, 29); // Another leap year

        // Act
        var result = dateTime.AddYears(4, culture);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    #endregion
}
