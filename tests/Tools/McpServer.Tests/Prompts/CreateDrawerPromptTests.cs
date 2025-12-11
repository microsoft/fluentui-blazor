// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.McpServer.Prompts;
using Microsoft.FluentUI.AspNetCore.Components.McpServer.Services;

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Tests.Prompts;

/// <summary>
/// Tests for <see cref="CreateDrawerPrompt"/>.
/// </summary>
public class CreateDrawerPromptTests
{
    private readonly CreateDrawerPrompt _prompt;
    private readonly FluentUIDocumentationService _documentationService;

    public CreateDrawerPromptTests()
    {
        var jsonPath = JsonDocumentationFinder.Find();
        _documentationService = new FluentUIDocumentationService(jsonPath);
        _prompt = new CreateDrawerPrompt(_documentationService);
    }

    [Fact]
    public void CreateDrawer_WithPurpose_ReturnsNonEmptyMessage()
    {
        // Arrange
        var purpose = "navigation menu";

        // Act
        var result = _prompt.CreateDrawer(purpose);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Text);
        Assert.NotEmpty(result.Text);
        Assert.Contains(purpose, result.Text);
    }

    [Fact]
    public void CreateDrawer_ContainsFluentDrawerInfo()
    {
        // Act
        var result = _prompt.CreateDrawer("settings panel");

        // Assert
        Assert.Contains("FluentDrawer", result.Text);
    }

    [Fact]
    public void CreateDrawer_WithPosition_IncludesPosition()
    {
        // Arrange
        var position = "end";

        // Act
        var result = _prompt.CreateDrawer("settings", position);

        // Assert
        Assert.Contains(position, result.Text);
        Assert.Contains("Position", result.Text);
    }

    [Fact]
    public void CreateDrawer_WithContent_IncludesContent()
    {
        // Arrange
        var content = "navigation links";

        // Act
        var result = _prompt.CreateDrawer("navigation", null, content);

        // Assert
        Assert.Contains(content, result.Text);
        Assert.Contains("Content", result.Text);
    }

    [Fact]
    public void CreateDrawer_ContainsTaskSection()
    {
        // Act
        var result = _prompt.CreateDrawer("filters");

        // Assert
        Assert.Contains("Task", result.Text);
        Assert.Contains("Header", result.Text);
        Assert.Contains("close button", result.Text);
    }

    [Fact]
    public void CreateDrawer_ContainsStateManagement()
    {
        // Act
        var result = _prompt.CreateDrawer("details view");

        // Assert
        Assert.Contains("state", result.Text, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData("navigation menu")]
    [InlineData("settings panel")]
    [InlineData("details view")]
    [InlineData("filters")]
    public void CreateDrawer_WithVariousPurposes_GeneratesValidPrompt(string purpose)
    {
        // Act
        var result = _prompt.CreateDrawer(purpose);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Text);
        Assert.Contains("FluentDrawer", result.Text);
    }

    [Theory]
    [InlineData("start")]
    [InlineData("end")]
    [InlineData("top")]
    [InlineData("bottom")]
    public void CreateDrawer_WithVariousPositions_IncludesPosition(string position)
    {
        // Act
        var result = _prompt.CreateDrawer("panel", position);

        // Assert
        Assert.Contains(position, result.Text);
    }
}
