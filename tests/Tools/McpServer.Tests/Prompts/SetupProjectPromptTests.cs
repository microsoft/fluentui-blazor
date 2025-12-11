// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.McpServer.Prompts;
using Microsoft.FluentUI.AspNetCore.Components.McpServer.Services;

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Tests.Prompts;

/// <summary>
/// Tests for <see cref="SetupProjectPrompt"/>.
/// </summary>
public class SetupProjectPromptTests
{
    private readonly SetupProjectPrompt _prompt;
    private readonly FluentUIDocumentationService _documentationService;

    public SetupProjectPromptTests()
    {
        var jsonPath = JsonDocumentationFinder.Find();
        _documentationService = new FluentUIDocumentationService(jsonPath);
        var guideService = new DocumentationGuideService();
        _prompt = new SetupProjectPrompt(guideService, _documentationService);
    }

    [Theory]
    [InlineData("server")]
    [InlineData("wasm")]
    [InlineData("hybrid")]
    [InlineData("maui")]
    public void SetupProject_WithVariousProjectTypes_ReturnsValidPrompt(string projectType)
    {
        // Act
        var result = _prompt.SetupProject(projectType);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Text);
        Assert.Contains(projectType.ToUpperInvariant(), result.Text);
    }

    [Fact]
    public void SetupProject_ContainsVersionInformation()
    {
        // Act
        var result = _prompt.SetupProject("server");

        // Assert
        Assert.Contains("Version Information", result.Text);
        Assert.Contains("MCP Server Version", result.Text);
        Assert.Contains("Compatible Components Version", result.Text);
    }

    [Fact]
    public void SetupProject_ContainsNuGetCommands()
    {
        // Act
        var result = _prompt.SetupProject("server");

        // Assert
        Assert.Contains("dotnet add package", result.Text);
        Assert.Contains("Microsoft.FluentUI.AspNetCore.Components", result.Text);
    }

    [Fact]
    public void SetupProject_ContainsVersionInNuGetCommand()
    {
        // Arrange
        var expectedVersion = _documentationService.ComponentsVersion;

        // Act
        var result = _prompt.SetupProject("server");

        // Assert
        Assert.Contains($"--version {expectedVersion}", result.Text);
    }

    [Fact]
    public void SetupProject_WithIconsFeature_IncludesIconsPackage()
    {
        // Act
        var result = _prompt.SetupProject("server", "icons");

        // Assert
        Assert.Contains("Microsoft.FluentUI.AspNetCore.Components.Icons", result.Text);
    }

    [Fact]
    public void SetupProject_WithEmojiFeature_IncludesEmojiPackage()
    {
        // Act
        var result = _prompt.SetupProject("server", "emoji");

        // Assert
        Assert.Contains("Microsoft.FluentUI.AspNetCore.Components.Emoji", result.Text);
    }

    [Fact]
    public void SetupProject_WithFeatures_IncludesFeaturesInTask()
    {
        // Arrange
        var features = "icons, datagrid";

        // Act
        var result = _prompt.SetupProject("server", features);

        // Assert
        Assert.Contains(features, result.Text);
        Assert.Contains("Features", result.Text);
    }

    [Fact]
    public void SetupProject_ContainsTaskSection()
    {
        // Act
        var result = _prompt.SetupProject("server");

        // Assert
        Assert.Contains("Task", result.Text);
        Assert.Contains("step-by-step instructions", result.Text);
    }

    [Fact]
    public void SetupProject_ContainsSetupSteps()
    {
        // Act
        var result = _prompt.SetupProject("server");

        // Assert
        Assert.Contains("Program.cs", result.Text);
        Assert.Contains("_Imports.razor", result.Text);
        Assert.Contains("Layout", result.Text);
    }

    [Fact]
    public void SetupProject_ContainsCompatibilityWarning()
    {
        // Act
        var result = _prompt.SetupProject("server");

        // Assert
        Assert.Contains("Important", result.Text);
        Assert.Contains("compatibility", result.Text, StringComparison.OrdinalIgnoreCase);
    }
}
