// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Extensions;

public class SpacingExtensionsTests
{

    [Theory]
    [InlineData("", "", "")]
    [InlineData("10", "10px", "")]
    [InlineData("10 15", "10px 15px", "")]
    [InlineData("10 15 20", "10px 15px 20px", "")]
    [InlineData("10 15 20 25", "10px 15px 20px 25px", "")]
    [InlineData("10% 15em 20 25px", "10% 15em 20px 25px", "")]
    [InlineData("-10 -15", "-10px -15px", "")]
    [InlineData("+10 +15%", "+10px +15%", "")]
    [InlineData("margin: 10px", "", "")]
    [InlineData("border: 1px", "", "")]
    public void SpacingExtensions_Style(string value, string expectedStyle, string expectedClass)
    {
        // Arrange & Act
        var converted = value.ConvertSpacing();

        // Assert
        Assert.Equal(expectedStyle, converted.Style);
        Assert.Equal(expectedClass, converted.Class);
    }

    [Theory]
    [InlineData("", "", "")]
    [InlineData("mr-0", "", "mr-0")]
    [InlineData("my-class", "", "my-class")]
    [InlineData("mr-0 my-class", "", "mr-0 my-class")]
    [InlineData("Inv@lid", "", "")]
    public void SpacingExtensions_Class(string value, string expectedStyle, string expectedClass)
    {
        // Arrange & Act
        var converted = value.ConvertSpacing();

        // Assert
        Assert.Equal(expectedStyle, converted.Style);
        Assert.Equal(expectedClass, converted.Class);
    }

    [Theory]
    [InlineData("auto", "auto", "")]
    [InlineData("inherit", "inherit", "")]
    [InlineData("initial", "initial", "")]
    [InlineData("revert", "revert", "")]
    [InlineData("revert-layer", "revert-layer", "")]
    [InlineData("unset", "unset", "")]
    [InlineData("calc(100px - 10px)", "calc(100px - 10px)", "")]
    public void SpacingExtensions_Keywords(string value, string expectedStyle, string expectedClass)
    {
        // Arrange & Act
        var converted = value.ConvertSpacing();

        // Assert
        Assert.Equal(expectedStyle, converted.Style);
        Assert.Equal(expectedClass, converted.Class);
    }

    [Theory]
    [InlineData("auto 0px")]
    [InlineData("revert 10px")]
    public void SpacingExtensions_Invalid(string value)
    {
        // Arrange & Act
        var ex = Assert.Throws<ArgumentException>(() => value.ConvertSpacing());

        // Assert
        Assert.Equal("The value cannot contain a CSS keyword and a class name or style value. (Parameter 'value')", ex.Message);
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData("10", "10px")]
    [InlineData("10px", "10px")]
    [InlineData("10%", "10%")]
    [InlineData("10.0", "10.0")]
    [InlineData("abc", "abc")]
    public void SpacingExtensions_AddMissingPx(string? value, string? expectedResult)
    {
        // Arrange & Act
        var converted = value.AddMissingPx();

        // Assert
        Assert.Equal(expectedResult, converted);
    }
}
