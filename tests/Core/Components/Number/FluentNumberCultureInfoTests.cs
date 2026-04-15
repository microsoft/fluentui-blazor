// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Globalization;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.Number;

public class FluentNumberCultureInfoTests
{
    [Fact]
    public void Invariant_HasDefaultSettings()
    {
        // Arrange & Act
        var culture = FluentNumberInputCultureInfo.Invariant;

        // Assert
        Assert.Equal(2, culture.NumberFormat.NumberDecimalDigits);
        Assert.Equal(".", culture.NumberFormat.NumberDecimalSeparator);
        Assert.Equal("", culture.NumberFormat.NumberGroupSeparator);
        Assert.Equal("fuib", culture.Name);
        Assert.Equal("FluentUI", culture.DisplayName);
    }

    [Fact]
    public void Constructor_Default_UsesCurrentCultureDecimalSeparator()
    {
        // Arrange & Act
        var culture = new FluentNumberInputCultureInfo();

        // Assert
        var expectedSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
        Assert.Equal(2, culture.NumberFormat.NumberDecimalDigits);
        Assert.Equal(expectedSeparator, culture.NumberFormat.NumberDecimalSeparator);
        Assert.Equal("", culture.NumberFormat.NumberGroupSeparator);
        Assert.Equal("fuib", culture.Name);
        Assert.Equal("FluentUI", culture.DisplayName);
    }

    [Fact]
    public void Constructor_WithDecimalDigits_UsesCurrentCultureDecimalSeparator()
    {
        // Arrange & Act
        var culture = new FluentNumberInputCultureInfo(4);

        // Assert
        var expectedSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
        Assert.Equal(4, culture.NumberFormat.NumberDecimalDigits);
        Assert.Equal(expectedSeparator, culture.NumberFormat.NumberDecimalSeparator);
        Assert.Equal("", culture.NumberFormat.NumberGroupSeparator);
    }

    [Fact]
    public void Constructor_WithDecimalDigitsAndSeparator_UsesEmptyGroupSeparator()
    {
        // Arrange & Act
        var culture = new FluentNumberInputCultureInfo(3, ",");

        // Assert
        Assert.Equal(3, culture.NumberFormat.NumberDecimalDigits);
        Assert.Equal(",", culture.NumberFormat.NumberDecimalSeparator);
        Assert.Equal("", culture.NumberFormat.NumberGroupSeparator);
    }

    [Fact]
    public void Constructor_WithAllParameters()
    {
        // Arrange & Act
        var culture = new FluentNumberInputCultureInfo(5, ".", " ");

        // Assert
        Assert.Equal(5, culture.NumberFormat.NumberDecimalDigits);
        Assert.Equal(".", culture.NumberFormat.NumberDecimalSeparator);
        Assert.Equal(" ", culture.NumberFormat.NumberGroupSeparator);
        Assert.Equal("fuib", culture.Name);
        Assert.Equal("FluentUI", culture.DisplayName);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(10)]
    public void Constructor_VariousDecimalDigits(int decimalDigits)
    {
        // Arrange & Act
        var culture = new FluentNumberInputCultureInfo(decimalDigits);

        // Assert
        Assert.Equal(decimalDigits, culture.NumberFormat.NumberDecimalDigits);
    }

    [Fact]
    public void IsInstanceOfCultureInfo()
    {
        // Arrange & Act
        var culture = new FluentNumberInputCultureInfo();

        // Assert
        Assert.IsAssignableFrom<CultureInfo>(culture);
    }
}
