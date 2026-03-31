// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.Extensions.AI;
using Microsoft.FluentUI.AspNetCore.McpServer.Prompts;
using Microsoft.FluentUI.AspNetCore.McpServer.Services;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tests.Prompts;

/// <summary>
/// Tests for the <see cref="ExplainComponentPrompts"/> class.
/// </summary>
public class ExplainComponentPromptsTests
{
    private readonly string _testJsonPath;
    private readonly bool _jsonFileExists;

    public ExplainComponentPromptsTests()
    {
        _testJsonPath = Path.Combine(AppContext.BaseDirectory, "FluentUIComponentsDocumentation.json");
        _jsonFileExists = File.Exists(_testJsonPath);
    }

    [Fact]
    public void ExplainComponent_WithComponentName_ReturnsValidChatMessage()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var service = new FluentUIDocumentationService(_testJsonPath);
        var prompts = new ExplainComponentPrompts(service);

        // Act
        var result = prompts.ExplainComponent("FluentButton");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(ChatRole.User, result.Role);
        Assert.NotNull(result.Text);
        Assert.Contains("FluentButton", result.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void ExplainComponent_WithExamplesEnabled_IncludesExampleRequest()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var service = new FluentUIDocumentationService(_testJsonPath);
        var prompts = new ExplainComponentPrompts(service);

        // Act
        var result = prompts.ExplainComponent("FluentButton", includeExamples: true);

        // Assert
        Assert.Contains("example", result.Text, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData("basic")]
    [InlineData("intermediate")]
    [InlineData("advanced")]
    public void ExplainComponent_WithDetailLevel_IncludesDetailLevel(string detailLevel)
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var service = new FluentUIDocumentationService(_testJsonPath);
        var prompts = new ExplainComponentPrompts(service);

        // Act
        var result = prompts.ExplainComponent("FluentButton", detailLevel: detailLevel);

        // Assert
        Assert.Contains(detailLevel, result.Text, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void ExplainComponent_WithUnknownComponent_StillReturnsMessage()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var service = new FluentUIDocumentationService(_testJsonPath);
        var prompts = new ExplainComponentPrompts(service);

        // Act
        var result = prompts.ExplainComponent("NonExistentComponent");

        // Assert
        Assert.NotNull(result);
        Assert.Contains("NonExistentComponent", result.Text, StringComparison.Ordinal);
    }
}
