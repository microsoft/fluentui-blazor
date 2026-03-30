// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Themes;

public partial class ThemeTests
{
    [Fact]
    public void Themes_Serialization()
    {
        // Arrange
        var theme = new Theme();

        // Act
        theme.Borders.Radius.Medium = "0px";
        theme.Borders.Radius.Small = "2px";
        theme.Fonts.Weight.Bold = 700;

        // Assert
        var jsonResult = theme.ToJson();
        Assert.Equal(3, jsonResult.Split(",").Length);
        Assert.Contains("\"borderRadiusMedium\": \"0px\"", jsonResult);
        Assert.Contains("\"borderRadiusSmall\": \"2px\"", jsonResult);
        Assert.Contains("\"fontWeightBold\": 700", jsonResult);
    }

    [Fact]
    public void Theme_Deserialization()
    {
        // Arrange
        var json = "{ \"borderRadiusLarge\": \"4px\", \"borderRadius2XLarge\": \"8px\", \"fontWeightBold\": 700 }";
        var theme = new Theme(json);

        // Act
        var dictionary = theme.ToCompactDictionary();

        // Assert
        Assert.Equal(3, dictionary.Count());
        Assert.Equal("4px", dictionary["borderRadiusLarge"]);
        Assert.Equal("8px", dictionary["borderRadius2XLarge"]);
        Assert.Equal(700, dictionary["fontWeightBold"]);
    }

    [Fact]
    public void Theme_ToCompactDictionary_EmptyValuesAreRemoved()
    {
        // Arrange
        var theme = new Theme();

        // Act
        theme.Borders.Radius.Medium = "1px";
        theme.Borders.Radius.Small = string.Empty;
        theme.Fonts.Weight.Bold = 0;
        var dictionary = theme.ToCompactDictionary();

        // Assert
        Assert.Single(dictionary);
        Assert.Equal("1px", dictionary["borderRadiusMedium"]);
        Assert.False(dictionary.TryGetValue("borderRadiusSmall", out _));
        Assert.False(dictionary.TryGetValue("fontWeightBold", out _));
    }

    [Fact]
    public void Theme_Combine()
    {
        // Arrange
        var json = "{ \"borderRadiusLarge\": \"4px\", \"borderRadius2XLarge\": \"8px\", \"fontWeightBold\": 700 }";
        var theme = new Theme();
        theme.FromJson(json);

        // Act
        theme.Borders.Radius.Medium = "3px";
        theme.Borders.Radius.Small = "2px";
        theme.Fonts.Weight.Medium = 600;

        // Assert
        var jsonResult = theme.ToJson();
        Assert.Equal(6, jsonResult.Split(",").Length);
        Assert.Contains("\"borderRadiusLarge\": \"4px\"", jsonResult);
        Assert.Contains("\"borderRadius2XLarge\": \"8px\"", jsonResult);
        Assert.Contains("\"borderRadiusMedium\": \"3px\"", jsonResult);
        Assert.Contains("\"borderRadiusSmall\": \"2px\"", jsonResult);
        Assert.Contains("\"fontWeightBold\": 700", jsonResult);
        Assert.Contains("\"fontWeightMedium\": 600", jsonResult);
    }
}
