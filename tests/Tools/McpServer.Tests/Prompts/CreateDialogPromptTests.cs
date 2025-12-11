// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.McpServer.Prompts;
using Microsoft.FluentUI.AspNetCore.Components.McpServer.Services;

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Tests.Prompts;

/// <summary>
/// Tests for <see cref="CreateDialogPrompt"/>.
/// </summary>
public class CreateDialogPromptTests
{
    private readonly CreateDialogPrompt _prompt;
    private readonly FluentUIDocumentationService _documentationService;

    public CreateDialogPromptTests()
    {
        var jsonPath = JsonDocumentationFinder.Find();
        _documentationService = new FluentUIDocumentationService(jsonPath);
        _prompt = new CreateDialogPrompt(_documentationService);
    }

    [Fact]
    public void CreateDialog_WithPurpose_ReturnsNonEmptyMessage()
    {
        // Arrange
        var purpose = "confirm delete";

        // Act
        var result = _prompt.CreateDialog(purpose);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Text);
        Assert.NotEmpty(result.Text);
        Assert.Contains(purpose, result.Text);
    }

    [Fact]
    public void CreateDialog_ContainsFluentDialogInfo()
    {
        // Act
        var result = _prompt.CreateDialog("edit user");

        // Assert
        Assert.Contains("FluentDialog", result.Text);
    }

    [Fact]
    public void CreateDialog_WithContent_IncludesContent()
    {
        // Arrange
        var content = "form with name and email";

        // Act
        var result = _prompt.CreateDialog("edit user", content);

        // Assert
        Assert.Contains(content, result.Text);
        Assert.Contains("Content", result.Text);
    }

    [Fact]
    public void CreateDialog_ContainsTaskSection()
    {
        // Act
        var result = _prompt.CreateDialog("confirm action");

        // Assert
        Assert.Contains("Task", result.Text);
        Assert.Contains("title", result.Text);
        Assert.Contains("Action buttons", result.Text);
    }

    [Fact]
    public void CreateDialog_ContainsServiceBasedOpening()
    {
        // Act
        var result = _prompt.CreateDialog("display details");

        // Assert
        Assert.Contains("Service-based", result.Text);
    }

    [Theory]
    [InlineData("confirm delete")]
    [InlineData("edit user")]
    [InlineData("display details")]
    [InlineData("warning message")]
    public void CreateDialog_WithVariousPurposes_GeneratesValidPrompt(string purpose)
    {
        // Act
        var result = _prompt.CreateDialog(purpose);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Text);
        Assert.Contains("FluentDialog", result.Text);
    }
}
