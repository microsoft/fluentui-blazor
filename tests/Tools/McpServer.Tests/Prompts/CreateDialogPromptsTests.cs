// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.Extensions.AI;
using Microsoft.FluentUI.AspNetCore.McpServer.Prompts;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tests.Prompts;

/// <summary>
/// Tests for the <see cref="CreateDialogPrompts"/> class.
/// </summary>
public class CreateDialogPromptsTests
{
    [Fact]
    public void CreateDialog_WithDefaultParameters_ReturnsValidChatMessage()
    {
        // Act
        var result = CreateDialogPrompts.CreateDialog("Confirm deletion");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(ChatRole.User, result.Role);
        Assert.NotNull(result.Text);
        Assert.Contains("Confirm deletion", result.Text, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("confirmation")]
    [InlineData("form")]
    [InlineData("information")]
    public void CreateDialog_WithDialogType_IncludesTypeInMessage(string dialogType)
    {
        // Act
        var result = CreateDialogPrompts.CreateDialog("Test dialog", dialogType);

        // Assert
        Assert.Contains(dialogType, result.Text, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void CreateDialog_IncludesDialogServicePattern()
    {
        // Act
        var result = CreateDialogPrompts.CreateDialog("Test dialog");

        // Assert
        Assert.Contains("IDialogService", result.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void CreateDialog_IncludesDialogComponent()
    {
        // Act
        var result = CreateDialogPrompts.CreateDialog("Settings dialog");

        // Assert
        Assert.Contains("FluentDialog", result.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void CreateDialog_IncludesHeader()
    {
        // Act
        var result = CreateDialogPrompts.CreateDialog("Info dialog");

        // Assert
        Assert.Contains("Create a Fluent UI Blazor Dialog", result.Text, StringComparison.Ordinal);
    }
}
