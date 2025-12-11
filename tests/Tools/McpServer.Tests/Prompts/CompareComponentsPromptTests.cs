// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.McpServer.Prompts;
using Microsoft.FluentUI.AspNetCore.Components.McpServer.Services;

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Tests.Prompts;

/// <summary>
/// Tests for <see cref="CompareComponentsPrompt"/>.
/// </summary>
public class CompareComponentsPromptTests
{
    private readonly CompareComponentsPrompt _prompt;
    private readonly FluentUIDocumentationService _documentationService;

    public CompareComponentsPromptTests()
    {
        var jsonPath = JsonDocumentationFinder.Find();
        _documentationService = new FluentUIDocumentationService(jsonPath);
        _prompt = new CompareComponentsPrompt(_documentationService);
    }

    [Fact]
    public void CompareComponents_WithTwoComponents_ReturnsNonEmptyMessage()
    {
        // Arrange
        var componentNames = "FluentButton,FluentAnchor";

        // Act
        var result = _prompt.CompareComponents(componentNames);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Text);
        Assert.NotEmpty(result.Text);
    }

    [Fact]
    public void CompareComponents_ContainsBothComponents()
    {
        // Arrange
        var componentNames = "FluentButton,FluentCard";

        // Act
        var result = _prompt.CompareComponents(componentNames);

        // Assert
        Assert.Contains("FluentButton", result.Text);
        Assert.Contains("FluentCard", result.Text);
    }

    [Fact]
    public void CompareComponents_ContainsComparisonTitle()
    {
        // Act
        var result = _prompt.CompareComponents("FluentButton,FluentAnchor");

        // Assert
        Assert.Contains("Component Comparison", result.Text);
    }

    [Fact]
    public void CompareComponents_ContainsTaskSection()
    {
        // Act
        var result = _prompt.CompareComponents("FluentButton,FluentAnchor");

        // Assert
        Assert.Contains("Task", result.Text);
        Assert.Contains("key differences", result.Text);
    }

    [Fact]
    public void CompareComponents_WithUnknownComponent_HandlesGracefully()
    {
        // Arrange
        var componentNames = "FluentButton,NonExistentComponent";

        // Act
        var result = _prompt.CompareComponents(componentNames);

        // Assert
        Assert.NotNull(result);
        Assert.Contains("not found", result.Text);
    }

    [Fact]
    public void CompareComponents_IncludesCategory()
    {
        // Act
        var result = _prompt.CompareComponents("FluentButton,FluentCard");

        // Assert
        Assert.Contains("Category", result.Text);
    }

    [Fact]
    public void CompareComponents_IncludesParametersCount()
    {
        // Act
        var result = _prompt.CompareComponents("FluentButton,FluentCard");

        // Assert
        Assert.Contains("Parameters", result.Text);
    }

    [Theory]
    [InlineData("FluentButton,FluentAnchor")]
    [InlineData("FluentTextField,FluentTextArea")]
    [InlineData("FluentDialog,FluentDrawer")]
    public void CompareComponents_WithVariousPairs_GeneratesValidPrompt(string componentNames)
    {
        // Act
        var result = _prompt.CompareComponents(componentNames);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Text);
        Assert.Contains("Comparison", result.Text);
    }

    [Fact]
    public void CompareComponents_WithThreeComponents_ComparesAll()
    {
        // Arrange
        var componentNames = "FluentButton,FluentAnchor,FluentCard";

        // Act
        var result = _prompt.CompareComponents(componentNames);

        // Assert
        Assert.Contains("FluentButton", result.Text);
        Assert.Contains("FluentAnchor", result.Text);
        Assert.Contains("FluentCard", result.Text);
    }
}
