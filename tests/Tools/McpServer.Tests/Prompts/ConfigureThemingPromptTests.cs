// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.McpServer.Prompts;
using Microsoft.FluentUI.AspNetCore.Components.McpServer.Services;

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Tests.Prompts;

/// <summary>
/// Tests for <see cref="ConfigureThemingPrompt"/>.
/// </summary>
public class ConfigureThemingPromptTests
{
    private readonly ConfigureThemingPrompt _prompt;

    public ConfigureThemingPromptTests()
    {
        var guideService = new DocumentationGuideService();
        _prompt = new ConfigureThemingPrompt(guideService);
    }

    [Fact]
    public void ConfigureTheming_WithTheme_ReturnsNonEmptyMessage()
    {
        // Arrange
        var themeType = "dark";

        // Act
        var result = _prompt.ConfigureTheming(themeType);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Text);
        Assert.NotEmpty(result.Text);
    }

    [Fact]
    public void ConfigureTheming_ContainsThemingTitle()
    {
        // Act
        var result = _prompt.ConfigureTheming("light");

        // Assert
        Assert.Contains("Theme", result.Text);
    }

    [Fact]
    public void ConfigureTheming_IncludesThemeType()
    {
        // Arrange
        var themeType = "dark";

        // Act
        var result = _prompt.ConfigureTheming(themeType);

        // Assert
        Assert.Contains(themeType.ToUpperInvariant(), result.Text);
    }

    [Fact]
    public void ConfigureTheming_ContainsTaskSection()
    {
        // Act
        var result = _prompt.ConfigureTheming("light");

        // Assert
        Assert.Contains("Task", result.Text);
    }

    [Theory]
    [InlineData("light")]
    [InlineData("dark")]
    [InlineData("custom")]
    [InlineData("dynamic")]
    public void ConfigureTheming_WithVariousThemes_GeneratesValidPrompt(string themeType)
    {
        // Act
        var result = _prompt.ConfigureTheming(themeType);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Text);
        Assert.Contains("Theme", result.Text);
    }

    [Fact]
    public void ConfigureTheming_WithCustomizations_IncludesCustomizations()
    {
        // Arrange
        var customizations = "#0078D4, #106EBE";

        // Act
        var result = _prompt.ConfigureTheming("custom", customizations);

        // Assert
        Assert.Contains(customizations, result.Text);
        Assert.Contains("Customizations", result.Text);
    }

    [Fact]
    public void ConfigureTheming_ContainsDesignTokenInfo()
    {
        // Act
        var result = _prompt.ConfigureTheming("custom");

        // Assert
        Assert.Contains("Design token", result.Text);
    }
}
