// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.McpServer.Resources;
using Microsoft.FluentUI.AspNetCore.McpServer.Services;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tests.Resources;

/// <summary>
/// Tests for the <see cref="MigrationResources"/> class.
/// </summary>
public class MigrationResourcesTests
{
    private readonly MigrationService _migrationService;
    private readonly MigrationResources _resources;

    public MigrationResourcesTests()
    {
        _migrationService = new MigrationService();
        _resources = new MigrationResources(_migrationService);
    }

    #region GetMigrationOverview Tests

    [Fact]
    public void GetMigrationOverview_ShouldReturnNonNullResult()
    {
        // Act
        var result = _resources.GetMigrationOverview();

        // Assert
        Assert.NotNull(result);
        Assert.False(string.IsNullOrEmpty(result));
    }

    [Fact]
    public void GetMigrationOverview_ShouldContainMarkdownContent()
    {
        // Act
        var result = _resources.GetMigrationOverview();

        // Assert
        // Should contain YAML front matter or markdown content
        Assert.Contains("Migration", result, StringComparison.OrdinalIgnoreCase);
    }

    #endregion

    #region GetComponentMigrationList Tests

    [Fact]
    public void GetComponentMigrationList_ShouldReturnNonNullResult()
    {
        // Act
        var result = _resources.GetComponentMigrationList();

        // Assert
        Assert.NotNull(result);
        Assert.False(string.IsNullOrEmpty(result));
    }

    [Fact]
    public void GetComponentMigrationList_ShouldContainHeader()
    {
        // Act
        var result = _resources.GetComponentMigrationList();

        // Assert
        Assert.Contains("Component Migration Guides", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetComponentMigrationList_ShouldContainUriFormat()
    {
        // Act
        var result = _resources.GetComponentMigrationList();

        // Assert
        Assert.Contains("fluentui://migration/components/", result, StringComparison.OrdinalIgnoreCase);
    }

    #endregion

    #region GetComponentMigration Tests

    [Fact]
    public void GetComponentMigration_WithValidComponent_ShouldReturnContent()
    {
        // Act
        var result = _resources.GetComponentMigration("FluentButton");

        // Assert
        Assert.NotNull(result);
        Assert.Contains("FluentButton", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetComponentMigration_WithNonExistentComponent_ShouldReturnNotFound()
    {
        // Act
        var result = _resources.GetComponentMigration("NonExistentComponent12345");

        // Assert
        Assert.Contains("Not Found", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetComponentMigration_WithShorthand_ShouldReturnContent()
    {
        // Act
        var result = _resources.GetComponentMigration("Button");

        // Assert
        Assert.NotNull(result);
        Assert.DoesNotContain("Not Found", result, StringComparison.OrdinalIgnoreCase);
    }

    #endregion
}
