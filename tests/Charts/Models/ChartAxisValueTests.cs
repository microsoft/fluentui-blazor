// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.Charts;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Charts.Models;

public class ChartAxisValueTests
{
    [Fact]
    public void DefaultValue_IsNumericWithZeroNumber()
    {
        // Arrange
        var value = default(ChartAxisValue);

        // Assert
        Assert.False(value.IsDate);
        Assert.Equal(0.0, value.NumberValue);
        Assert.Equal(default, value.DateValue);
    }

    [Fact]
    public void ImplicitFromDouble_SetsNumericValue()
    {
        ChartAxisValue value = 42.5;

        Assert.False(value.IsDate);
        Assert.Equal(42.5, value.NumberValue);
    }

    [Fact]
    public void ImplicitFromDateTimeOffset_SetsDateValue()
    {
        var dto = new DateTimeOffset(2024, 6, 1, 10, 20, 30, TimeSpan.FromHours(2));
        ChartAxisValue value = dto;

        Assert.True(value.IsDate);
        Assert.Equal(dto, value.DateValue);
    }

    [Fact]
    public void ImplicitFromDateTime_Unspecified_TreatedAsUtc()
    {
        var dt = new DateTime(2024, 1, 2, 3, 4, 5, DateTimeKind.Unspecified);

        ChartAxisValue value = dt;

        Assert.True(value.IsDate);
        Assert.Equal(TimeSpan.Zero, value.DateValue.Offset);
        Assert.Equal(2024, value.DateValue.Year);
        Assert.Equal(1, value.DateValue.Month);
        Assert.Equal(2, value.DateValue.Day);
    }

    [Fact]
    public void ImplicitFromDateTime_Utc_PreservedAsUtc()
    {
        var dt = new DateTime(2024, 3, 4, 5, 6, 7, DateTimeKind.Utc);

        ChartAxisValue value = dt;

        Assert.True(value.IsDate);
        Assert.Equal(TimeSpan.Zero, value.DateValue.Offset);
        Assert.Equal(dt, value.DateValue.UtcDateTime);
    }

    [Fact]
    public void Equals_WithSameNumericValue_ReturnsTrue()
    {
        ChartAxisValue left = 12.25;
        ChartAxisValue right = 12.25;

        Assert.True(left.Equals(right));
        Assert.True(left == right);
        Assert.False(left != right);
    }

    [Fact]
    public void Equals_WithDifferentNumericValue_ReturnsFalse()
    {
        ChartAxisValue left = 12.25;
        ChartAxisValue right = 13.25;

        Assert.False(left.Equals(right));
        Assert.False(left == right);
        Assert.True(left != right);
    }

    [Fact]
    public void Equals_WithSameDateValue_ReturnsTrue()
    {
        var dto = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero);
        ChartAxisValue left = dto;
        ChartAxisValue right = dto;

        Assert.True(left.Equals(right));
        Assert.True(left == right);
        Assert.False(left != right);
    }

    [Fact]
    public void Equals_NumberAndDate_AreNotEqual()
    {
        ChartAxisValue number = 10;
        ChartAxisValue date = new DateTimeOffset(1970, 1, 1, 0, 0, 10, TimeSpan.Zero);

        Assert.False(number.Equals(date));
        Assert.False(number == date);
        Assert.True(number != date);
    }

    [Fact]
    public void Equals_Object_WithSameValueType_ReturnsTrue()
    {
        ChartAxisValue value = 5.5;
        object boxed = (ChartAxisValue)5.5;

        Assert.True(value.Equals(boxed));
    }

    [Fact]
    public void Equals_Object_WithNullOrDifferentType_ReturnsFalse()
    {
        ChartAxisValue value = 5.5;

        Assert.False(value.Equals(null!));
        Assert.False(value.Equals("5.5"));
    }

    [Fact]
    public void GetHashCode_SameValues_AreEqual()
    {
        ChartAxisValue left = 42;
        ChartAxisValue right = 42;

        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }

    [Fact]
    public void GetHashCode_DifferentKinds_AreDifferent()
    {
        ChartAxisValue number = 42;
        ChartAxisValue date = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero);

        Assert.NotEqual(number.GetHashCode(), date.GetHashCode());
    }
}
