// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Xunit;
using FluentUI.Demo.DocApiGen.Models.AllMode;

namespace FluentUI.Demo.DocApiGen.Tests.Models.AllMode;

/// <summary>
/// Unit tests for <see cref="EnumInfo"/>.
/// </summary>
public class EnumInfoTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithDefaults()
    {
        // Arrange & Act
        var enumInfo = new EnumInfo();

        // Assert
        Assert.Equal(string.Empty, enumInfo.Name);
        Assert.Equal(string.Empty, enumInfo.FullName);
        Assert.Equal(string.Empty, enumInfo.Description);
        Assert.NotNull(enumInfo.Values);
        Assert.Empty(enumInfo.Values);
    }

    [Fact]
    public void Name_ShouldBeSettable()
    {
        // Arrange
        var enumInfo = new EnumInfo();

        // Act
        enumInfo.Name = "Appearance";

        // Assert
        Assert.Equal("Appearance", enumInfo.Name);
    }

    [Fact]
    public void FullName_ShouldBeSettable()
    {
        // Arrange
        var enumInfo = new EnumInfo();

        // Act
        enumInfo.FullName = "Microsoft.FluentUI.AspNetCore.Components.Appearance";

        // Assert
        Assert.Equal("Microsoft.FluentUI.AspNetCore.Components.Appearance", enumInfo.FullName);
    }

    [Fact]
    public void Description_ShouldBeSettable()
    {
        // Arrange
        var enumInfo = new EnumInfo();

        // Act
        enumInfo.Description = "Defines button appearance styles";

        // Assert
        Assert.Equal("Defines button appearance styles", enumInfo.Description);
    }

    [Fact]
    public void Values_ShouldBeSettable()
    {
        // Arrange
        var enumInfo = new EnumInfo();
        var values = new List<EnumValueInfo>
        {
            new() { Name = "Neutral", Value = 0 },
            new() { Name = "Accent", Value = 1 }
        };

        // Act
        enumInfo.Values = values;

        // Assert
        Assert.Same(values, enumInfo.Values);
        Assert.Equal(2, enumInfo.Values.Count);
    }

    [Fact]
    public void CompleteObject_ShouldBeConstructedProperly()
    {
        // Arrange & Act
        var enumInfo = new EnumInfo
        {
            Name = "Appearance",
            FullName = "Microsoft.FluentUI.AspNetCore.Components.Appearance",
            Description = "Defines button appearance styles",
            Values =
            [
                new EnumValueInfo
                {
                    Name = "Neutral",
                    Value = 0,
                    Description = "Neutral appearance"
                },
                new EnumValueInfo
                {
                    Name = "Accent",
                    Value = 1,
                    Description = "Accent appearance"
                },
                new EnumValueInfo
                {
                    Name = "Lightweight",
                    Value = 2,
                    Description = "Lightweight appearance"
                }
            ]
        };

        // Assert
        Assert.Equal("Appearance", enumInfo.Name);
        Assert.Equal("Microsoft.FluentUI.AspNetCore.Components.Appearance", enumInfo.FullName);
        Assert.Equal("Defines button appearance styles", enumInfo.Description);
        Assert.Equal(3, enumInfo.Values.Count);
        Assert.Equal("Neutral", enumInfo.Values[0].Name);
        Assert.Equal(0, enumInfo.Values[0].Value);
        Assert.Equal("Accent", enumInfo.Values[1].Name);
        Assert.Equal(1, enumInfo.Values[1].Value);
    }

    [Fact]
    public void Values_CanBeModifiedAfterConstruction()
    {
        // Arrange
        var enumInfo = new EnumInfo();

        // Act
        enumInfo.Values.Add(new EnumValueInfo { Name = "Value1", Value = 0 });
        enumInfo.Values.Add(new EnumValueInfo { Name = "Value2", Value = 1 });

        // Assert
        Assert.Equal(2, enumInfo.Values.Count);
        Assert.Equal("Value1", enumInfo.Values[0].Name);
        Assert.Equal("Value2", enumInfo.Values[1].Name);
    }
}
