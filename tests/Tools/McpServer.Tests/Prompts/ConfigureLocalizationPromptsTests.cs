// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.Extensions.AI;
using Microsoft.FluentUI.AspNetCore.McpServer.Prompts;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tests.Prompts;

/// <summary>
/// Tests for the <see cref="ConfigureLocalizationPrompts"/> class.
/// </summary>
public class ConfigureLocalizationPromptsTests
{
    [Fact]
    public void ConfigureLocalization_WithDefaultParameters_ReturnsValidChatMessage()
    {
        // Act
        var result = ConfigureLocalizationPrompts.ConfigureLocalization();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(ChatRole.User, result.Role);
        Assert.NotNull(result.Text);
    }

    [Theory]
    [InlineData("en,fr")]
    [InlineData("en,de,es")]
    [InlineData("fr,it,pt")]
    public void ConfigureLocalization_WithLanguages_IncludesLanguages(string languages)
    {
        // Act
        var result = ConfigureLocalizationPrompts.ConfigureLocalization(languages);

        // Assert
        foreach (var lang in languages.Split(','))
        {
            Assert.Contains(lang, result.Text, StringComparison.Ordinal);
        }
    }

    [Fact]
    public void ConfigureLocalization_IncludesLocalizationService()
    {
        // Act
        var result = ConfigureLocalizationPrompts.ConfigureLocalization();

        // Assert
        Assert.Contains("IFluentLocalizer", result.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void ConfigureLocalization_IncludesResourceFileApproach()
    {
        // Act
        var result = ConfigureLocalizationPrompts.ConfigureLocalization();

        // Assert
        Assert.Contains("Resource", result.Text, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void ConfigureLocalization_IncludesHeader()
    {
        // Act
        var result = ConfigureLocalizationPrompts.ConfigureLocalization();

        // Assert
        Assert.Contains("Configure Localization in Fluent UI Blazor", result.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void ConfigureLocalization_IncludesServiceRegistration()
    {
        // Act
        var result = ConfigureLocalizationPrompts.ConfigureLocalization();

        // Assert
        Assert.Contains("AddLocalization", result.Text, StringComparison.Ordinal);
    }
}
