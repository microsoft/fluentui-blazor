// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.McpServer.Models;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tests.Models;

/// <summary>
/// Tests for the <see cref="PropertyInfo"/> record.
/// </summary>
public class PropertyInfoTests
{
    [Fact]
    public void PropertyInfo_WithRequiredProperties_ShouldBeCreatedSuccessfully()
    {
        // Arrange & Act
        var propertyInfo = new PropertyInfo
        {
            Name = "Label",
            Type = "string"
        };

        // Assert
        Assert.Equal("Label", propertyInfo.Name);
        Assert.Equal("string", propertyInfo.Type);
    }

    [Fact]
    public void PropertyInfo_DefaultValues_ShouldBeCorrect()
    {
        // Arrange & Act
        var propertyInfo = new PropertyInfo
        {
            Name = "Label",
            Type = "string"
        };

        // Assert
        Assert.Equal(string.Empty, propertyInfo.Description);
        Assert.False(propertyInfo.IsParameter);
        Assert.False(propertyInfo.IsInherited);
        Assert.Null(propertyInfo.DefaultValue);
        Assert.Empty(propertyInfo.EnumValues);
    }

    [Fact]
    public void PropertyInfo_WithAllProperties_ShouldBeCreatedSuccessfully()
    {
        // Arrange & Act
        var propertyInfo = new PropertyInfo
        {
            Name = "Appearance",
            Type = "Appearance?",
            Description = "The appearance of the button",
            IsParameter = true,
            IsInherited = false,
            DefaultValue = "Appearance.Neutral",
            EnumValues = ["Accent", "Lightweight", "Neutral", "Outline", "Stealth"]
        };

        // Assert
        Assert.Equal("Appearance", propertyInfo.Name);
        Assert.Equal("Appearance?", propertyInfo.Type);
        Assert.Equal("The appearance of the button", propertyInfo.Description);
        Assert.True(propertyInfo.IsParameter);
        Assert.False(propertyInfo.IsInherited);
        Assert.Equal("Appearance.Neutral", propertyInfo.DefaultValue);
        Assert.Equal(5, propertyInfo.EnumValues.Length);
        Assert.Contains("Accent", propertyInfo.EnumValues);
    }

    [Fact]
    public void PropertyInfo_WithIsInherited_ShouldBeCreatedSuccessfully()
    {
        // Arrange & Act
        var propertyInfo = new PropertyInfo
        {
            Name = "Class",
            Type = "string?",
            Description = "CSS class names",
            IsParameter = true,
            IsInherited = true
        };

        // Assert
        Assert.True(propertyInfo.IsInherited);
    }

    [Fact]
    public void PropertyInfo_RecordEquality_ShouldWorkCorrectly()
    {
        // Arrange
        var propertyInfo1 = new PropertyInfo
        {
            Name = "Label",
            Type = "string",
            Description = "The label text"
        };

        var propertyInfo2 = new PropertyInfo
        {
            Name = "Label",
            Type = "string",
            Description = "The label text"
        };

        // Act & Assert
        Assert.Equal(propertyInfo1, propertyInfo2);
    }

    [Fact]
    public void PropertyInfo_WithEnumValues_ShouldContainAllValues()
    {
        // Arrange
        var enumValues = new[] { "Value1", "Value2", "Value3" };

        // Act
        var propertyInfo = new PropertyInfo
        {
            Name = "Size",
            Type = "Size?",
            EnumValues = enumValues
        };

        // Assert
        Assert.Equal(enumValues, propertyInfo.EnumValues);
    }
}
