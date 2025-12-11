// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.McpServer.Prompts;
using Microsoft.FluentUI.AspNetCore.Components.McpServer.Services;

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Tests.Prompts;

/// <summary>
/// Tests for <see cref="ConfigureLocalizationPrompt"/>.
/// </summary>
public class ConfigureLocalizationPromptTests
{
    private readonly ConfigureLocalizationPrompt _prompt;

    public ConfigureLocalizationPromptTests()
    {
        var guideService = new DocumentationGuideService();
        _prompt = new ConfigureLocalizationPrompt(guideService);
    }

    [Fact]
    public void ConfigureLocalization_WithLanguages_ReturnsNonEmptyMessage()
    {
        // Arrange
        var languages = "French,German";

        // Act
        var result = _prompt.ConfigureLocalization(languages);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Text);
        Assert.NotEmpty(result.Text);
    }

    [Fact]
    public void ConfigureLocalization_ContainsLocalizationTitle()
    {
        // Act
        var result = _prompt.ConfigureLocalization("English");

        // Assert
        Assert.Contains("Localization", result.Text);
    }

    [Fact]
    public void ConfigureLocalization_IncludesLanguages()
    {
        // Arrange
        var languages = "German,Spanish";

        // Act
        var result = _prompt.ConfigureLocalization(languages);

        // Assert
        Assert.Contains(languages, result.Text);
    }

    [Fact]
    public void ConfigureLocalization_ContainsTaskSection()
    {
        // Act
        var result = _prompt.ConfigureLocalization("French");

        // Assert
        Assert.Contains("Task", result.Text);
    }

    [Theory]
    [InlineData("English")]
    [InlineData("French")]
    [InlineData("German,Spanish")]
    [InlineData("Japanese,Korean,Chinese")]
    public void ConfigureLocalization_WithVariousLanguages_GeneratesValidPrompt(string languages)
    {
        // Act
        var result = _prompt.ConfigureLocalization(languages);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Text);
        Assert.Contains("Localization", result.Text);
    }

    [Fact]
    public void ConfigureLocalization_ContainsIFluentLocalizerInfo()
    {
        // Act
        var result = _prompt.ConfigureLocalization("French");

        // Assert
        Assert.Contains("IFluentLocalizer", result.Text);
    }

    [Fact]
    public void ConfigureLocalization_ContainsCultureSwitchingInfo()
    {
        // Act
        var result = _prompt.ConfigureLocalization("French");

        // Assert
        Assert.Contains("Culture switching", result.Text);
    }
}
