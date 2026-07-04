// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.McpServer.Models;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tests.Models;

/// <summary>
/// Tests for the <see cref="EnumValueInfo"/> record.
/// </summary>
public class EnumValueInfoTests
{
    [Fact]
    public void EnumValueInfo_WithRequiredProperties_ShouldBeCreatedSuccessfully()
    {
        // Arrange & Act
        var enumValueInfo = new EnumValueInfo
        {
            Name = "Accent"
        };

        // Assert
        Assert.Equal("Accent", enumValueInfo.Name);
    }

    [Fact]
    public void EnumValueInfo_DefaultValues_ShouldBeCorrect()
    {
        // Arrange & Act
        var enumValueInfo = new EnumValueInfo
        {
            Name = "Accent"
        };

        // Assert
        Assert.Equal(0, enumValueInfo.Value);
        Assert.Equal(string.Empty, enumValueInfo.Description);
    }

    [Fact]
    public void EnumValueInfo_WithAllProperties_ShouldBeCreatedSuccessfully()
    {
        // Arrange & Act
        var enumValueInfo = new EnumValueInfo
        {
            Name = "Accent",
            Value = 1,
            Description = "The accent color style"
        };

        // Assert
        Assert.Equal("Accent", enumValueInfo.Name);
        Assert.Equal(1, enumValueInfo.Value);
        Assert.Equal("The accent color style", enumValueInfo.Description);
    }

    [Fact]
    public void EnumValueInfo_RecordEquality_ShouldWorkCorrectly()
    {
        // Arrange
        var enumValueInfo1 = new EnumValueInfo
        {
            Name = "Accent",
            Value = 0,
            Description = "Accent style"
        };

        var enumValueInfo2 = new EnumValueInfo
        {
            Name = "Accent",
            Value = 0,
            Description = "Accent style"
        };

        // Act & Assert
        Assert.Equal(enumValueInfo1, enumValueInfo2);
    }

    [Fact]
    public void EnumValueInfo_RecordInequality_ShouldWorkCorrectly()
    {
        // Arrange
        var enumValueInfo1 = new EnumValueInfo
        {
            Name = "Accent",
            Value = 0
        };

        var enumValueInfo2 = new EnumValueInfo
        {
            Name = "Neutral",
            Value = 1
        };

        // Act & Assert
        Assert.NotEqual(enumValueInfo1, enumValueInfo2);
    }

    [Fact]
    public void EnumValueInfo_WithNegativeValue_ShouldBeCreatedSuccessfully()
    {
        // Arrange & Act
        var enumValueInfo = new EnumValueInfo
        {
            Name = "None",
            Value = -1,
            Description = "No value selected"
        };

        // Assert
        Assert.Equal(-1, enumValueInfo.Value);
    }

    [Fact]
    public void EnumValueInfo_WithLargeValue_ShouldBeCreatedSuccessfully()
    {
        // Arrange & Act
        var enumValueInfo = new EnumValueInfo
        {
            Name = "MaxValue",
            Value = int.MaxValue
        };

        // Assert
        Assert.Equal(int.MaxValue, enumValueInfo.Value);
    }
}
