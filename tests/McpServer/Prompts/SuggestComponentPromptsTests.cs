// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.Extensions.AI;
using Microsoft.FluentUI.AspNetCore.McpServer.Prompts;
using Microsoft.FluentUI.AspNetCore.McpServer.Services;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tests.Prompts;

/// <summary>
/// Tests for the <see cref="SuggestComponentPrompts"/> class.
/// </summary>
public class SuggestComponentPromptsTests
{
    private readonly string _testJsonPath;
    private readonly bool _jsonFileExists;

    public SuggestComponentPromptsTests()
    {
        _testJsonPath = Path.Combine(AppContext.BaseDirectory, "FluentUIComponentsDocumentation.json");
        _jsonFileExists = File.Exists(_testJsonPath);
    }

    [Fact]
    public void SuggestComponents_WithUseCase_ReturnsValidChatMessage()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var service = new FluentUIDocumentationService(_testJsonPath);
        var prompts = new SuggestComponentPrompts(service);

        // Act
        var result = prompts.SuggestComponents("I need a button with an icon");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(ChatRole.User, result.Role);
        Assert.NotNull(result.Text);
        Assert.Contains("button with an icon", result.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void SuggestComponents_WithContext_IncludesContext()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var service = new FluentUIDocumentationService(_testJsonPath);
        var prompts = new SuggestComponentPrompts(service);

        // Act
        var result = prompts.SuggestComponents("Display a list of items", "Building an e-commerce site");

        // Assert
        Assert.Contains("e-commerce", result.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void SuggestComponents_IncludesAvailableComponentsSection()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var service = new FluentUIDocumentationService(_testJsonPath);
        var prompts = new SuggestComponentPrompts(service);

        // Act
        var result = prompts.SuggestComponents("Display a list of items");

        // Assert
        Assert.Contains("Available Components", result.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void SuggestComponents_IncludesHeader()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var service = new FluentUIDocumentationService(_testJsonPath);
        var prompts = new SuggestComponentPrompts(service);

        // Act
        var result = prompts.SuggestComponents("Create a navigation menu");

        // Assert
        Assert.Contains("Suggest Fluent UI Blazor Components", result.Text, StringComparison.Ordinal);
    }
}
