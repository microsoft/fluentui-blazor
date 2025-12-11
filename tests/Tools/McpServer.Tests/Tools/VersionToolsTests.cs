// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.McpServer.Services;
using Microsoft.FluentUI.AspNetCore.Components.McpServer.Tools;

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Tests.Tools;

/// <summary>
/// Tests for <see cref="VersionTools"/>.
/// </summary>
public class VersionToolsTests
{
    private readonly VersionTools _tools;
    private readonly FluentUIDocumentationService _documentationService;

    public VersionToolsTests()
    {
        var jsonPath = JsonDocumentationFinder.Find();
        _documentationService = new FluentUIDocumentationService(jsonPath);
        _tools = new VersionTools(_documentationService);
    }

    [Fact]
    public void GetVersionInfo_ReturnsNonEmptyString()
    {
        // Act
        var result = _tools.GetVersionInfo();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void GetVersionInfo_ContainsMcpServerVersion()
    {
        // Act
        var result = _tools.GetVersionInfo();

        // Assert
        Assert.Contains("MCP Server Version", result);
    }

    [Fact]
    public void GetVersionInfo_ContainsComponentsVersion()
    {
        // Act
        var result = _tools.GetVersionInfo();

        // Assert
        Assert.Contains("Components Version", result);
    }

    [Fact]
    public void GetVersionInfo_ContainsDocumentationStatistics()
    {
        // Act
        var result = _tools.GetVersionInfo();

        // Assert
        Assert.Contains("Documentation Statistics", result);
        Assert.Contains("Components", result);
        Assert.Contains("Enums", result);
    }

    [Fact]
    public void GetVersionInfo_ContainsInstallCommand()
    {
        // Act
        var result = _tools.GetVersionInfo();

        // Assert
        Assert.Contains("dotnet add package", result);
        Assert.Contains("Microsoft.FluentUI.AspNetCore.Components", result);
    }

    [Fact]
    public void CheckVersionCompatibility_WithExactMatch_ReturnsCompatible()
    {
        // Arrange
        var expectedVersion = _documentationService.ComponentsVersion;

        // Act
        var result = _tools.CheckVersionCompatibility(expectedVersion);

        // Assert
        Assert.Contains("Compatible", result);
        Assert.Contains("✅", result);
    }

    [Fact]
    public void CheckVersionCompatibility_WithMinorDifference_ReturnsWarning()
    {
        // Arrange - Use a version with same major but different minor
        var expectedVersion = _documentationService.ComponentsVersion;
        var parts = expectedVersion.Split('.');
        if (parts.Length >= 2 && int.TryParse(parts[1], out var minor))
        {
            var differentMinor = $"{parts[0]}.{minor + 1}.0";

            // Act
            var result = _tools.CheckVersionCompatibility(differentMinor);

            // Assert
            Assert.Contains("Version Compatibility Check", result);
        }
    }

    [Fact]
    public void CheckVersionCompatibility_WithMajorDifference_ReturnsWarning()
    {
        // Arrange
        var differentMajor = "99.0.0";

        // Act
        var result = _tools.CheckVersionCompatibility(differentMajor);

        // Assert
        Assert.Contains("⚠️", result);
        Assert.Contains("Major version mismatch", result);
    }

    [Fact]
    public void CheckVersionCompatibility_WithInvalidVersion_ReturnsError()
    {
        // Arrange
        var invalidVersion = "not-a-version";

        // Act
        var result = _tools.CheckVersionCompatibility(invalidVersion);

        // Assert
        Assert.Contains("Unable to parse", result);
    }

    [Fact]
    public void CheckVersionCompatibility_ContainsRecommendedActions()
    {
        // Arrange
        var differentVersion = "1.0.0";

        // Act
        var result = _tools.CheckVersionCompatibility(differentVersion);

        // Assert
        Assert.Contains("Recommended Actions", result);
        Assert.Contains("dotnet add package", result);
        Assert.Contains("dotnet tool update", result);
    }

    [Theory]
    [InlineData("5.0.0")]
    [InlineData("4.10.3")]
    [InlineData("5.0.0-preview.1")]
    public void CheckVersionCompatibility_WithVariousVersions_ReturnsResult(string version)
    {
        // Act
        var result = _tools.CheckVersionCompatibility(version);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Contains("Version Compatibility Check", result);
    }
}
