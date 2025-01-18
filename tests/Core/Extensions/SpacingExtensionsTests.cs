// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
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
    public void SpacingExtensions_Style(string value, string expectedStyle, string expectedClass)
    {
        // Arrange & Act
        var converted = value.SpacingToStyle();

        // Assert
        Assert.Equal(expectedStyle, converted.Style);
        Assert.Equal(expectedClass, converted.Class);
    }

    [Theory]
    [InlineData("", "", "")]
    [InlineData("mr-0", "", "mr-0")]
    [InlineData("my-class", "", "my-class")]
    [InlineData("mr-0 my-class", "", "mr-0 my-class")]
    public void SpacingExtensions_Class(string value, string expectedStyle, string expectedClass)
    {
        // Arrange & Act
        var converted = value.SpacingToStyle();

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
    public void SpacingExtensions_Keywords(string value, string expectedStyle, string expectedClass)
    {
        // Arrange & Act
        var converted = value.SpacingToStyle();

        // Assert
        Assert.Equal(expectedStyle, converted.Style);
        Assert.Equal(expectedClass, converted.Class);
    }
}
