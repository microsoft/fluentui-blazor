// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.Extensions.AI;
using Microsoft.FluentUI.AspNetCore.McpServer.Prompts;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tests.Prompts;

/// <summary>
/// Tests for the <see cref="CreateDrawerPrompts"/> class.
/// </summary>
public class CreateDrawerPromptsTests
{
    [Fact]
    public void CreateDrawer_WithDefaultParameters_ReturnsValidChatMessage()
    {
        // Act
        var result = CreateDrawerPrompts.CreateDrawer("User settings panel");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(ChatRole.User, result.Role);
        Assert.NotNull(result.Text);
        Assert.Contains("User settings panel", result.Text, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("start", "Left (Start)")]
    [InlineData("end", "Right (End)")]
    public void CreateDrawer_WithPosition_IncludesPositionInMessage(string position, string expectedText)
    {
        // Act
        var result = CreateDrawerPrompts.CreateDrawer("Test drawer", position);

        // Assert
        Assert.Contains(expectedText, result.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void CreateDrawer_IncludesDialogServicePattern()
    {
        // Act
        var result = CreateDrawerPrompts.CreateDrawer("Side panel");

        // Assert
        Assert.Contains("IDialogService", result.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void CreateDrawer_IncludesDrawerComponent()
    {
        // Act
        var result = CreateDrawerPrompts.CreateDrawer("Navigation drawer");

        // Assert
        Assert.Contains("FluentDialogInstance", result.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void CreateDrawer_IncludesHeader()
    {
        // Act
        var result = CreateDrawerPrompts.CreateDrawer("Filter panel");

        // Assert
        Assert.Contains("Create a Fluent UI Blazor Drawer", result.Text, StringComparison.Ordinal);
    }
}
