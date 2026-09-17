// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.McpServer.Tools;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tests.Tools;

/// <summary>
/// Tests for the <see cref="VersionTools"/> class.
/// </summary>
public class VersionToolsTests
{
    #region GetVersionInfo Tests

    [Fact]
    public void GetVersionInfo_ReturnsNonEmptyResultWithVersionHeader()
    {
        // Act
        var result = VersionTools.GetVersionInfo();

        // Assert
        Assert.False(string.IsNullOrEmpty(result));
        Assert.Contains("# Fluent UI Blazor - Version Information", result, StringComparison.Ordinal);
    }

    [Fact]
    public void GetVersionInfo_ShouldContainMcpServerSection()
    {
        // Act
        var result = VersionTools.GetVersionInfo();

        // Assert
        Assert.Contains("## MCP Server", result, StringComparison.Ordinal);
        Assert.Contains("Microsoft.FluentUI.AspNetCore.McpServer", result, StringComparison.Ordinal);
    }

    [Fact]
    public void GetVersionInfo_ShouldContainRequiredComponentVersion()
    {
        // Act
        var result = VersionTools.GetVersionInfo();

        // Assert
        Assert.Contains("## Required Component Library Version", result, StringComparison.Ordinal);
        Assert.Contains("Microsoft.FluentUI.AspNetCore.Components", result, StringComparison.Ordinal);
    }

    [Fact]
    public void GetVersionInfo_ShouldContainExpectedPackageReference()
    {
        // Act
        var result = VersionTools.GetVersionInfo();
        var expectedVersion = VersionTools.GetMcpSemanticVersion();

        // Assert
        Assert.Contains("### Expected PackageReference", result, StringComparison.Ordinal);
        Assert.Contains(
            $"<PackageReference Include=\"Microsoft.FluentUI.AspNetCore.Components\" Version=\"{expectedVersion}\" />",
            result,
            StringComparison.Ordinal);
    }

    [Fact]
    public void GetVersionInfo_ShouldContainNextStepInstructions()
    {
        // Act
        var result = VersionTools.GetVersionInfo();

        // Assert
        Assert.Contains("### Next Step", result, StringComparison.Ordinal);
        Assert.Contains("CheckProjectVersion", result, StringComparison.Ordinal);
    }

    #endregion

    #region CheckProjectVersion Tests

    [Fact]
    public void CheckProjectVersion_WithMatchingVersion_ShouldReportCompatible()
    {
        // Arrange
        var mcpVersion = VersionTools.GetMcpSemanticVersion();

        // Act
        var result = VersionTools.CheckProjectVersion(mcpVersion);

        // Assert
        Assert.Contains("## Result: COMPATIBLE", result, StringComparison.Ordinal);
        Assert.DoesNotContain("INCOMPATIBLE", result, StringComparison.Ordinal);
    }

    [Fact]
    public void CheckProjectVersion_WithMismatchedVersion_ShouldReportIncompatible()
    {
        // Act
        var result = VersionTools.CheckProjectVersion("999.0.0");

        // Assert
        Assert.Contains("## Result: INCOMPATIBLE", result, StringComparison.Ordinal);
        Assert.Contains("WARNING", result, StringComparison.Ordinal);
    }

    [Fact]
    public void CheckProjectVersion_WithMismatchedVersion_ShouldShowRisks()
    {
        // Act
        var result = VersionTools.CheckProjectVersion("4.0.0");

        // Assert
        Assert.Contains("### Risks", result, StringComparison.Ordinal);
        Assert.Contains("parameters, events, or methods may have changed", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void CheckProjectVersion_WithMismatchedVersion_ShouldShowUpgradeInstructions()
    {
        // Arrange
        var mcpVersion = VersionTools.GetMcpSemanticVersion();

        // Act
        var result = VersionTools.CheckProjectVersion("4.0.0");

        // Assert
        Assert.Contains("### Recommended Action", result, StringComparison.Ordinal);
        Assert.Contains(
            $"<PackageReference Include=\"Microsoft.FluentUI.AspNetCore.Components\" Version=\"{mcpVersion}\" />",
            result,
            StringComparison.Ordinal);
        Assert.Contains(
            $"dotnet add package Microsoft.FluentUI.AspNetCore.Components --version {mcpVersion}",
            result,
            StringComparison.Ordinal);
    }

    [Fact]
    public void CheckProjectVersion_WithBuildMetadata_ShouldStripAndCompare()
    {
        // Arrange
        var mcpVersion = VersionTools.GetMcpSemanticVersion();

        // Act - version with build metadata appended
        var result = VersionTools.CheckProjectVersion(mcpVersion + "+abc123");

        // Assert - should still be compatible after stripping metadata
        Assert.Contains("## Result: COMPATIBLE", result, StringComparison.Ordinal);
    }

    [Fact]
    public void CheckProjectVersion_WithWhitespace_ShouldTrimAndCompare()
    {
        // Arrange
        var mcpVersion = VersionTools.GetMcpSemanticVersion();

        // Act
        var result = VersionTools.CheckProjectVersion("  " + mcpVersion + "  ");

        // Assert
        Assert.Contains("## Result: COMPATIBLE", result, StringComparison.Ordinal);
    }

    [Fact]
    public void CheckProjectVersion_ShouldShowBothVersions()
    {
        // Arrange
        var mcpVersion = VersionTools.GetMcpSemanticVersion();

        // Act
        var result = VersionTools.CheckProjectVersion("4.9.0");

        // Assert
        Assert.Contains($"**MCP Server version**: `{mcpVersion}`", result, StringComparison.Ordinal);
        Assert.Contains("**Project version**: `4.9.0`", result, StringComparison.Ordinal);
    }

    [Fact]
    public void CheckProjectVersion_WithNullInput_DoesNotThrow()
    {
        // Act
        var exception = Record.Exception(() => VersionTools.CheckProjectVersion(null!));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void CheckProjectVersion_WithEmptyStringInput_DoesNotThrow()
    {
        // Act
        var exception = Record.Exception(() => VersionTools.CheckProjectVersion(string.Empty));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void CheckProjectVersion_WithWhitespaceOnlyInput_DoesNotThrow()
    {
        // Act
        var exception = Record.Exception(() => VersionTools.CheckProjectVersion("   "));

        // Assert
        Assert.Null(exception);
    }

    #endregion

    #region GetMcpSemanticVersion Tests

    [Fact]
    public void GetMcpSemanticVersion_ShouldNotContainBuildMetadata()
    {
        // Act
        var version = VersionTools.GetMcpSemanticVersion();

        // Assert
        Assert.DoesNotContain("+", version, StringComparison.Ordinal);
    }

    [Fact]
    public void GetMcpSemanticVersion_ShouldNotBeEmpty()
    {
        // Act
        var version = VersionTools.GetMcpSemanticVersion();

        // Assert
        Assert.False(string.IsNullOrWhiteSpace(version));
    }

    #endregion
}
