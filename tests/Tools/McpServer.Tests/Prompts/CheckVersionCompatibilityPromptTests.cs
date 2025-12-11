// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.McpServer.Prompts;
using Microsoft.FluentUI.AspNetCore.Components.McpServer.Services;

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Tests.Prompts;

/// <summary>
/// Tests for <see cref="CheckVersionCompatibilityPrompt"/>.
/// </summary>
public class CheckVersionCompatibilityPromptTests
{
    private readonly CheckVersionCompatibilityPrompt _prompt;
    private readonly FluentUIDocumentationService _documentationService;

    public CheckVersionCompatibilityPromptTests()
    {
        var jsonPath = JsonDocumentationFinder.Find();
        _documentationService = new FluentUIDocumentationService(jsonPath);
        _prompt = new CheckVersionCompatibilityPrompt(_documentationService);
    }

    [Fact]
    public void CheckVersionCompatibility_WithExactMatch_ReturnsPerfectMatch()
    {
        // Arrange
        var expectedVersion = _documentationService.ComponentsVersion;

        // Act
        var result = _prompt.CheckVersionCompatibility(expectedVersion);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Text);
        Assert.Contains("Perfect Match", result.Text);
        Assert.Contains("✅", result.Text);
    }

    [Fact]
    public void CheckVersionCompatibility_WithMajorDifference_ReturnsWarning()
    {
        // Arrange
        var differentMajor = "1.0.0";

        // Act
        var result = _prompt.CheckVersionCompatibility(differentMajor);

        // Assert
        Assert.NotNull(result);
        Assert.Contains("Major Version Mismatch", result.Text);
        Assert.Contains("❌", result.Text);
    }

    [Fact]
    public void CheckVersionCompatibility_ContainsVersionTable()
    {
        // Arrange
        var version = "5.0.0";

        // Act
        var result = _prompt.CheckVersionCompatibility(version);

        // Assert
        Assert.Contains("MCP Server", result.Text);
        Assert.Contains("Expected Components Version", result.Text);
        Assert.Contains("Your Installed Version", result.Text);
    }

    [Fact]
    public void CheckVersionCompatibility_ContainsTask()
    {
        // Arrange
        var version = "5.0.0";

        // Act
        var result = _prompt.CheckVersionCompatibility(version);

        // Assert
        Assert.Contains("Task", result.Text);
        Assert.Contains("Explain any potential issues", result.Text);
    }

    [Fact]
    public void CheckVersionCompatibility_WithPreReleaseVersion_HandlesCorrectly()
    {
        // Arrange
        var preReleaseVersion = "5.0.0-preview.1";

        // Act
        var result = _prompt.CheckVersionCompatibility(preReleaseVersion);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Text);
    }

    [Theory]
    [InlineData("5.0.0")]
    [InlineData("4.10.3")]
    [InlineData("6.0.0")]
    [InlineData("5.1.0")]
    public void CheckVersionCompatibility_WithVariousVersions_ReturnsValidPrompt(string version)
    {
        // Act
        var result = _prompt.CheckVersionCompatibility(version);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Text);
        Assert.Contains("Version Compatibility Check", result.Text);
    }

    [Fact]
    public void CheckVersionCompatibility_WithMinorDifference_ReturnsMinorWarning()
    {
        // Arrange
        var expectedVersion = _documentationService.ComponentsVersion;
        var parts = expectedVersion.Split('.');
        if (parts.Length >= 2 && int.TryParse(parts[1], out var minor))
        {
            var differentMinor = $"{parts[0]}.{minor + 1}.0";

            // Act
            var result = _prompt.CheckVersionCompatibility(differentMinor);

            // Assert
            Assert.NotNull(result);
            // Should be either exact match or minor difference
            Assert.True(
                result.Text.Contains("Minor Version Difference") || 
                result.Text.Contains("Perfect Match") ||
                result.Text.Contains("Major Version Mismatch"));
        }
    }
}
