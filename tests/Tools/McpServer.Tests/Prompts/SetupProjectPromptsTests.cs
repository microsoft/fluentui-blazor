// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.Extensions.AI;
using Microsoft.FluentUI.AspNetCore.McpServer.Prompts;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tests.Prompts;

/// <summary>
/// Tests for the <see cref="SetupProjectPrompts"/> class.
/// </summary>
public class SetupProjectPromptsTests
{
    [Fact]
    public void SetupProject_WithDefaultParameters_ReturnsValidChatMessage()
    {
        // Act
        var result = SetupProjectPrompts.SetupProject();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(ChatRole.User, result.Role);
        Assert.NotNull(result.Text);
        Assert.Contains("Fluent UI Blazor Project Setup Guide", result.Text, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("blazor-server")]
    [InlineData("blazor-webassembly")]
    [InlineData("blazor-webapp")]
    public void SetupProject_WithProjectType_IncludesProjectTypeInMessage(string projectType)
    {
        // Act
        var result = SetupProjectPrompts.SetupProject(projectType);

        // Assert
        Assert.Contains(projectType, result.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void SetupProject_WithIconsEnabled_IncludesIconsPackage()
    {
        // Act
        var result = SetupProjectPrompts.SetupProject(includeIcons: true);

        // Assert
        Assert.Contains("Microsoft.FluentUI.AspNetCore.Components.Icons", result.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void SetupProject_WithIconsDisabled_ExcludesIconsPackage()
    {
        // Act
        var result = SetupProjectPrompts.SetupProject(includeIcons: false);

        // Assert
        Assert.DoesNotContain("Microsoft.FluentUI.AspNetCore.Components.Icons", result.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void SetupProject_IncludesNuGetInstallation()
    {
        // Act
        var result = SetupProjectPrompts.SetupProject();

        // Assert
        Assert.Contains("dotnet add package Microsoft.FluentUI.AspNetCore.Components", result.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void SetupProject_IncludesServiceRegistration()
    {
        // Act
        var result = SetupProjectPrompts.SetupProject();

        // Assert
        Assert.Contains("AddFluentUIComponents", result.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void SetupProject_IncludesImportsSection()
    {
        // Act
        var result = SetupProjectPrompts.SetupProject();

        // Assert
        Assert.Contains("_Imports.razor", result.Text, StringComparison.Ordinal);
        Assert.Contains("@using Microsoft.FluentUI.AspNetCore.Components", result.Text, StringComparison.Ordinal);
    }
}
