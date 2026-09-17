// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.Extensions.AI;
using Microsoft.FluentUI.AspNetCore.McpServer.Prompts;
using Microsoft.FluentUI.AspNetCore.McpServer.Services;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tests.Prompts;

/// <summary>
/// Tests for the <see cref="CompareComponentsPrompts"/> class.
/// </summary>
public class CompareComponentsPromptsTests
{
    private readonly string _testJsonPath;
    private readonly bool _jsonFileExists;

    public CompareComponentsPromptsTests()
    {
        _testJsonPath = Path.Combine(AppContext.BaseDirectory, "FluentUIComponentsDocumentation.json");
        _jsonFileExists = File.Exists(_testJsonPath);
    }

    [Fact]
    public void CompareComponents_WithTwoComponents_ReturnsValidChatMessage()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var service = new FluentUIDocumentationService(_testJsonPath);
        var prompts = new CompareComponentsPrompts(service);

        // Act
        var result = prompts.CompareComponents("FluentSelect,FluentCombobox");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(ChatRole.User, result.Role);
        Assert.NotNull(result.Text);
        Assert.Contains("FluentSelect", result.Text, StringComparison.Ordinal);
        Assert.Contains("FluentCombobox", result.Text, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("features")]
    [InlineData("performance")]
    [InlineData("accessibility")]
    [InlineData("all")]
    public void CompareComponents_WithComparisonFocus_IncludesFocus(string focus)
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var service = new FluentUIDocumentationService(_testJsonPath);
        var prompts = new CompareComponentsPrompts(service);

        // Act
        var result = prompts.CompareComponents("FluentButton,FluentAnchor", focus);

        // Assert
        Assert.Contains("Focus", result.Text, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void CompareComponents_IncludesComparisonHeader()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var service = new FluentUIDocumentationService(_testJsonPath);
        var prompts = new CompareComponentsPrompts(service);

        // Act
        var result = prompts.CompareComponents("FluentTextField,FluentTextArea");

        // Assert
        Assert.Contains("Compare Fluent UI Blazor Components", result.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void CompareComponents_IncludesComponentsToCompareSection()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var service = new FluentUIDocumentationService(_testJsonPath);
        var prompts = new CompareComponentsPrompts(service);

        // Act
        var result = prompts.CompareComponents("FluentDialog,FluentDrawer");

        // Assert
        Assert.Contains("Components to Compare", result.Text, StringComparison.Ordinal);
    }
}
