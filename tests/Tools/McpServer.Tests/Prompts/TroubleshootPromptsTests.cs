// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.Extensions.AI;
using Microsoft.FluentUI.AspNetCore.McpServer.Prompts;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tests.Prompts;

/// <summary>
/// Tests for the <see cref="TroubleshootPrompts"/> class.
/// </summary>
public class TroubleshootPromptsTests
{
    [Fact]
    public void TroubleshootIssue_WithIssue_ReturnsValidChatMessage()
    {
        // Act
        var result = TroubleshootPrompts.TroubleshootIssue("Components not rendering");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(ChatRole.User, result.Role);
        Assert.NotNull(result.Text);
        Assert.Contains("Components not rendering", result.Text, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("styling")]
    [InlineData("rendering")]
    [InlineData("behavior")]
    [InlineData("performance")]
    [InlineData("general")]
    public void TroubleshootIssue_WithCategory_IncludesCategory(string category)
    {
        // Act
        var result = TroubleshootPrompts.TroubleshootIssue("Test issue", category);

        // Assert
        Assert.Contains(category, result.Text, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void TroubleshootIssue_IncludesHeader()
    {
        // Act
        var result = TroubleshootPrompts.TroubleshootIssue("Styling issues");

        // Assert
        Assert.Contains("Troubleshoot Fluent UI Blazor Issue", result.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void TroubleshootIssue_IncludesCommonTroubleshootingSteps()
    {
        // Act
        var result = TroubleshootPrompts.TroubleshootIssue("Dialog not opening");

        // Assert
        Assert.Contains("Common Troubleshooting Steps", result.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void TroubleshootIssue_HasSubstantialContent()
    {
        // Act
        var result = TroubleshootPrompts.TroubleshootIssue("JavaScript interop error");

        // Assert
        Assert.NotNull(result.Text);
        Assert.True(result.Text.Length > 100); // Should have substantial content
    }
}
