// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Themes;

public partial class ThemeCustomTests
{
    [Fact]
    public void ThemesCustom_Serialization()
    {
        // Arrange
        var theme = new Theme();

        // Act
        theme.Borders.Radius.Medium = "0px";
        theme.Borders.Radius.Small = "2px";

        // Assert
        var jsonResult = theme.ToJson();
        Assert.Equal(2, jsonResult.Split(",").Length);
        Assert.Contains("\"borderRadiusMedium\": \"0px\"", jsonResult);
        Assert.Contains("\"borderRadiusSmall\": \"2px\"", jsonResult);
    }

    [Fact]
    public void ThemeCustom_Deserialization()
    {
        // Arrange
        var json = "{ \"borderRadiusLarge\": \"4px\", \"borderRadius2XLarge\": \"8px\" }";
        var theme = new Theme(json);

        // Act
        var dictionary = theme.ToDictionary()
                              .Where(i => !string.IsNullOrEmpty(i.Value))
                              .ToDictionary();

        // Assert
        Assert.Equal(2, dictionary.Count());
        Assert.Equal("4px", dictionary["borderRadiusLarge"]);
        Assert.Equal("8px", dictionary["borderRadius2XLarge"]);
    }

    [Fact]
    public void ThemeCustom_Combine()
    {
        // Arrange
        var json = "{ \"borderRadiusLarge\": \"4px\", \"borderRadius2XLarge\": \"8px\" }";
        var theme = new Theme();
        theme.FromJson(json);

        // Act
        theme.Borders.Radius.Medium = "3px";
        theme.Borders.Radius.Small = "2px";

        // Assert
        var jsonResult = theme.ToJson();
        Assert.Equal(4, jsonResult.Split(",").Length);
        Assert.Contains("\"borderRadiusLarge\": \"4px\"", jsonResult);
        Assert.Contains("\"borderRadius2XLarge\": \"8px\"", jsonResult);
        Assert.Contains("\"borderRadiusMedium\": \"3px\"", jsonResult);
        Assert.Contains("\"borderRadiusSmall\": \"2px\"", jsonResult);
    }
}