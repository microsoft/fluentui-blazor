// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.McpServer.Prompts;
using Microsoft.FluentUI.AspNetCore.Components.McpServer.Services;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Tests.Prompts;

public class ExplainComponentPromptTests
{
    private readonly ExplainComponentPrompt _prompt;
    private readonly FluentUIDocumentationService _documentationService;

    public ExplainComponentPromptTests()
    {
        var componentsAssembly = typeof(Microsoft.FluentUI.AspNetCore.Components._Imports).Assembly;
        _documentationService = new FluentUIDocumentationService(componentsAssembly);
        _prompt = new ExplainComponentPrompt(_documentationService);
    }

    [Fact]
    public void ExplainComponent_WithValidComponentName_ReturnsNonEmptyMessage()
    {
        // Arrange
        var componentName = "FluentButton";

        // Act
        var result = _prompt.ExplainComponent(componentName);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Text);
        Assert.NotEmpty(result.Text);
        Assert.Contains(componentName, result.Text);
    }

    [Fact]
    public void ExplainComponent_ReturnsStructuredExplanation()
    {
        // Arrange
        var componentName = "FluentButton";

        // Act
        var result = _prompt.ExplainComponent(componentName);

        // Assert
        Assert.Contains("Explain", result.Text);
        Assert.Contains("Task", result.Text);
        Assert.Contains("What the component is used for", result.Text);
        Assert.Contains("Common use cases", result.Text);
    }

    [Theory]
    [InlineData("FluentButton")]
    [InlineData("FluentTextField")]
    [InlineData("FluentCheckbox")]
    public void ExplainComponent_WithVariousComponents_GeneratesValidPrompt(string componentName)
    {
        // Act
        var result = _prompt.ExplainComponent(componentName);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Text);
        Assert.Contains("comprehensive explanation", result.Text);
        Assert.Contains("best practices", result.Text);
    }
}
