// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.Extensions.AI;
using Microsoft.FluentUI.AspNetCore.McpServer.Prompts;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tests.Prompts;

/// <summary>
/// Tests for the <see cref="ConfigureThemingPrompts"/> class.
/// </summary>
public class ConfigureThemingPromptsTests
{
    [Fact]
    public void ConfigureTheming_WithThemingGoal_ReturnsValidChatMessage()
    {
        // Act
        var result = ConfigureThemingPrompts.ConfigureTheming("add company branding");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(ChatRole.User, result.Role);
        Assert.NotNull(result.Text);
        Assert.Contains("add company branding", result.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void ConfigureTheming_WithCustomColors_IncludesCustomColorsSection()
    {
        // Act
        var result = ConfigureThemingPrompts.ConfigureTheming("customize theme", includeCustomColors: true);

        // Assert
        Assert.Contains("Custom", result.Text, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void ConfigureTheming_WithDarkModeSupport_IncludesDarkModeSection()
    {
        // Act
        var result = ConfigureThemingPrompts.ConfigureTheming("implement dark mode", supportDarkMode: true);

        // Assert
        Assert.Contains("dark", result.Text, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void ConfigureTheming_IncludesFluentProviders()
    {
        // Act
        var result = ConfigureThemingPrompts.ConfigureTheming("setup theming");

        // Assert
        Assert.Contains("FluentProviders", result.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void ConfigureTheming_IncludesDesignTokens()
    {
        // Act
        var result = ConfigureThemingPrompts.ConfigureTheming("customize colors");

        // Assert
        Assert.Contains("Design Token", result.Text, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void ConfigureTheming_IncludesHeader()
    {
        // Act
        var result = ConfigureThemingPrompts.ConfigureTheming("brand colors");

        // Assert
        Assert.Contains("Configure Theming in Fluent UI Blazor", result.Text, StringComparison.Ordinal);
    }
}
