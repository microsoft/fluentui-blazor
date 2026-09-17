// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.Extensions.AI;
using Microsoft.FluentUI.AspNetCore.McpServer.Prompts;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tests.Prompts;

/// <summary>
/// Tests for the <see cref="CreateFormPrompts"/> class.
/// </summary>
public class CreateFormPromptsTests
{
    [Fact]
    public void CreateForm_WithDefaultParameters_ReturnsValidChatMessage()
    {
        // Act
        var result = CreateFormPrompts.CreateForm("User registration form");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(ChatRole.User, result.Role);
        Assert.NotNull(result.Text);
        Assert.Contains("User registration form", result.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void CreateForm_WithValidationEnabled_IncludesValidationSection()
    {
        // Act
        var result = CreateFormPrompts.CreateForm("Test form", includeValidation: true);

        // Assert
        Assert.Contains("validation", result.Text, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("DataAnnotations", result.Text, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void CreateForm_IncludesEditFormPattern()
    {
        // Act
        var result = CreateFormPrompts.CreateForm("Contact form");

        // Assert
        Assert.Contains("EditForm", result.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void CreateForm_IncludesEditFormGuidance()
    {
        // Act
        var result = CreateFormPrompts.CreateForm("Login form");

        // Assert
        Assert.Contains("EditForm", result.Text, StringComparison.Ordinal);
        Assert.Contains("OnValidSubmit", result.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void CreateForm_IncludesFormHeader()
    {
        // Act
        var result = CreateFormPrompts.CreateForm("Registration");

        // Assert
        Assert.Contains("Create a Fluent UI Blazor Form", result.Text, StringComparison.Ordinal);
    }
}
