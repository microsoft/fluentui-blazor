// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.McpServer.Models;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tests.Models;

/// <summary>
/// Tests for the <see cref="EnumInfo"/> record.
/// </summary>
public class EnumInfoTests
{
    [Fact]
    public void EnumInfo_WithRequiredProperties_ShouldBeCreatedSuccessfully()
    {
        // Arrange & Act
        var enumInfo = new EnumInfo
        {
            Name = "Appearance",
            FullName = "Microsoft.FluentUI.AspNetCore.Components.Appearance"
        };

        // Assert
        Assert.Equal("Appearance", enumInfo.Name);
        Assert.Equal("Microsoft.FluentUI.AspNetCore.Components.Appearance", enumInfo.FullName);
    }

    [Fact]
    public void EnumInfo_DefaultValues_ShouldBeCorrect()
    {
        // Arrange & Act
        var enumInfo = new EnumInfo
        {
            Name = "Appearance",
            FullName = "Microsoft.FluentUI.AspNetCore.Components.Appearance"
        };

        // Assert
        Assert.Equal(string.Empty, enumInfo.Description);
        Assert.Empty(enumInfo.Values);
    }

    [Fact]
    public void EnumInfo_WithAllProperties_ShouldBeCreatedSuccessfully()
    {
        // Arrange
        var values = new List<EnumValueInfo>
        {
            new() { Name = "Accent", Value = 0, Description = "The accent appearance" },
            new() { Name = "Lightweight", Value = 1, Description = "The lightweight appearance" },
            new() { Name = "Neutral", Value = 2, Description = "The neutral appearance" }
        };

        // Act
        var enumInfo = new EnumInfo
        {
            Name = "Appearance",
            FullName = "Microsoft.FluentUI.AspNetCore.Components.Appearance",
            Description = "Defines the appearance of a button",
            Values = values
        };

        // Assert
        Assert.Equal("Appearance", enumInfo.Name);
        Assert.Equal("Microsoft.FluentUI.AspNetCore.Components.Appearance", enumInfo.FullName);
        Assert.Equal("Defines the appearance of a button", enumInfo.Description);
        Assert.Equal(3, enumInfo.Values.Count);
    }

    [Fact]
    public void EnumInfo_Values_ShouldContainCorrectData()
    {
        // Arrange
        var values = new List<EnumValueInfo>
        {
            new() { Name = "Small", Value = 0 },
            new() { Name = "Medium", Value = 1 },
            new() { Name = "Large", Value = 2 }
        };

        // Act
        var enumInfo = new EnumInfo
        {
            Name = "Size",
            FullName = "Microsoft.FluentUI.AspNetCore.Components.Size",
            Values = values
        };

        // Assert
        Assert.Equal("Small", enumInfo.Values[0].Name);
        Assert.Equal(0, enumInfo.Values[0].Value);
        Assert.Equal("Medium", enumInfo.Values[1].Name);
        Assert.Equal(1, enumInfo.Values[1].Value);
        Assert.Equal("Large", enumInfo.Values[2].Name);
        Assert.Equal(2, enumInfo.Values[2].Value);
    }

    [Fact]
    public void EnumInfo_RecordEquality_ShouldWorkCorrectly()
    {
        // Arrange
        var values = new List<EnumValueInfo>
        {
            new() { Name = "Accent", Value = 0 }
        };

        var enumInfo1 = new EnumInfo
        {
            Name = "Appearance",
            FullName = "Microsoft.FluentUI.AspNetCore.Components.Appearance",
            Values = values
        };

        var enumInfo2 = new EnumInfo
        {
            Name = "Appearance",
            FullName = "Microsoft.FluentUI.AspNetCore.Components.Appearance",
            Values = values
        };

        // Act & Assert
        Assert.Equal(enumInfo1, enumInfo2);
    }
}
