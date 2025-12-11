// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.McpServer.Prompts;
using Microsoft.FluentUI.AspNetCore.Components.McpServer.Services;

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Tests.Prompts;

/// <summary>
/// Tests for <see cref="MigrateToV5Prompt"/>.
/// </summary>
public class MigrateToV5PromptTests
{
    private readonly MigrateToV5Prompt _prompt;

    public MigrateToV5PromptTests()
    {
        var guideService = new DocumentationGuideService();
        _prompt = new MigrateToV5Prompt(guideService);
    }

    [Fact]
    public void MigrateToV5_WithExistingCode_ReturnsNonEmptyMessage()
    {
        // Arrange
        var existingCode = "<FluentButton Appearance=\"@Appearance.Accent\">Click me</FluentButton>";

        // Act
        var result = _prompt.MigrateToV5(existingCode);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Text);
        Assert.NotEmpty(result.Text);
    }

    [Fact]
    public void MigrateToV5_ContainsMigrationTitle()
    {
        // Arrange
        var existingCode = "<FluentButton>Test</FluentButton>";

        // Act
        var result = _prompt.MigrateToV5(existingCode);

        // Assert
        Assert.Contains("Migrate", result.Text);
        Assert.Contains("v5", result.Text, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void MigrateToV5_IncludesExistingCode()
    {
        // Arrange
        var existingCode = "<FluentCard><p>Hello World</p></FluentCard>";

        // Act
        var result = _prompt.MigrateToV5(existingCode);

        // Assert
        Assert.Contains(existingCode, result.Text);
    }

    [Fact]
    public void MigrateToV5_ContainsCodeBlock()
    {
        // Arrange
        var existingCode = "<FluentButton>Test</FluentButton>";

        // Act
        var result = _prompt.MigrateToV5(existingCode);

        // Assert
        Assert.Contains("```razor", result.Text);
        Assert.Contains("```", result.Text);
    }

    [Fact]
    public void MigrateToV5_ContainsTaskSection()
    {
        // Arrange
        var existingCode = "<FluentButton>Test</FluentButton>";

        // Act
        var result = _prompt.MigrateToV5(existingCode);

        // Assert
        Assert.Contains("Task", result.Text);
    }

    [Fact]
    public void MigrateToV5_WithFocus_IncludesFocusAreas()
    {
        // Arrange
        var existingCode = "<FluentButton>Test</FluentButton>";
        var focus = "Button appearance changes";

        // Act
        var result = _prompt.MigrateToV5(existingCode, focus);

        // Assert
        Assert.Contains(focus, result.Text);
        Assert.Contains("Focus", result.Text);
    }

    [Theory]
    [InlineData("<FluentButton>Click</FluentButton>")]
    [InlineData("<FluentCard><FluentButton>Nested</FluentButton></FluentCard>")]
    [InlineData("<FluentDataGrid Items=\"@items\"></FluentDataGrid>")]
    public void MigrateToV5_WithVariousCode_GeneratesValidPrompt(string existingCode)
    {
        // Act
        var result = _prompt.MigrateToV5(existingCode);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Text);
        Assert.Contains("Migrate", result.Text);
    }

    [Fact]
    public void MigrateToV5_ContainsBreakingChangesSection()
    {
        // Arrange
        var existingCode = "<FluentButton>Test</FluentButton>";

        // Act
        var result = _prompt.MigrateToV5(existingCode);

        // Assert
        // Should mention breaking changes
        Assert.True(
            result.Text.Contains("breaking", StringComparison.OrdinalIgnoreCase) ||
            result.Text.Contains("changes", StringComparison.OrdinalIgnoreCase));
    }
}
