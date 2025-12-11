// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.McpServer.Prompts;
using Microsoft.FluentUI.AspNetCore.Components.McpServer.Services;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Tests.Prompts;

public class CreateComponentPromptTests
{
    private readonly CreateComponentPrompt _prompt;
    private readonly FluentUIDocumentationService _documentationService;

    public CreateComponentPromptTests()
    {
        var jsonPath = JsonDocumentationFinder.Find();
        _documentationService = new FluentUIDocumentationService(jsonPath);
        _prompt = new CreateComponentPrompt(_documentationService);
    }

    [Fact]
    public void CreateComponent_WithValidComponentName_ReturnsNonEmptyMessage()
    {
        // Arrange
        var componentName = "FluentButton";

        // Act
        var result = _prompt.CreateComponent(componentName);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Text);
        Assert.NotEmpty(result.Text);
        Assert.Contains(componentName, result.Text);
    }

    [Fact]
    public void CreateComponent_WithRequirements_IncludesRequirementsInMessage()
    {
        // Arrange
        var componentName = "FluentButton";
        var requirements = "Make it red with large text";

        // Act
        var result = _prompt.CreateComponent(componentName, requirements);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Text);
        Assert.Contains(requirements, result.Text);
        Assert.Contains("Requirements", result.Text);
    }

    [Fact]
    public void CreateComponent_WithInvalidComponentName_ReturnsErrorMessage()
    {
        // Arrange
        var componentName = "NonExistentComponent";

        // Act
        var result = _prompt.CreateComponent(componentName);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Text);
        Assert.Contains("not found", result.Text);
    }

    [Theory]
    [InlineData("FluentButton")]
    [InlineData("FluentTextField")]
    [InlineData("FluentDataGrid")]
    public void CreateComponent_WithVariousComponents_GeneratesValidPrompt(string componentName)
    {
        // Act
        var result = _prompt.CreateComponent(componentName);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Text);
        Assert.Contains("Task", result.Text);
        Assert.Contains("Uses proper Fluent UI Blazor syntax", result.Text);
    }
}
