// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.McpServer.Services;
using Microsoft.FluentUI.AspNetCore.McpServer.Tools;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tests.Tools;

/// <summary>
/// Tests for the <see cref="MigrationTools"/> class.
/// </summary>
public class MigrationToolsTests
{
    private readonly MigrationService _migrationService;
    private readonly MigrationTools _tools;

    public MigrationToolsTests()
    {
        _migrationService = new MigrationService();
        _tools = new MigrationTools(_migrationService);
    }

    #region GetMigrationOverview Tests

    [Fact]
    public void GetMigrationOverview_ShouldReturnNonNullResult()
    {
        // Act
        var result = _tools.GetMigrationOverview();

        // Assert
        Assert.NotNull(result);
        Assert.False(string.IsNullOrEmpty(result));
    }

    [Fact]
    public void GetMigrationOverview_ShouldContainMigrationHeader()
    {
        // Act
        var result = _tools.GetMigrationOverview();

        // Assert
        Assert.Contains("Migration Overview", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetMigrationOverview_ShouldContainHintAboutComponentMigrations()
    {
        // Act
        var result = _tools.GetMigrationOverview();

        // Assert
        Assert.Contains("ListComponentMigrations", result, StringComparison.Ordinal);
    }

    #endregion

    #region ListComponentMigrations Tests

    [Fact]
    public void ListComponentMigrations_ShouldReturnNonNullResult()
    {
        // Act
        var result = _tools.ListComponentMigrations();

        // Assert
        Assert.NotNull(result);
        Assert.False(string.IsNullOrEmpty(result));
    }

    [Fact]
    public void ListComponentMigrations_ShouldContainComponentCount()
    {
        // Act
        var result = _tools.ListComponentMigrations();

        // Assert
        Assert.Contains("Component Migration Guides", result, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("available", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void ListComponentMigrations_ShouldContainFluentButton()
    {
        // Act
        var result = _tools.ListComponentMigrations();

        // Assert
        Assert.Contains("FluentButton", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void ListComponentMigrations_ShouldContainUsageHint()
    {
        // Act
        var result = _tools.ListComponentMigrations();

        // Assert
        Assert.Contains("GetComponentMigration", result, StringComparison.Ordinal);
    }

    #endregion

    #region GetComponentMigration Tests

    [Fact]
    public void GetComponentMigration_WithEmptyName_ShouldReturnErrorMessage()
    {
        // Act
        var result = _tools.GetComponentMigration(string.Empty);

        // Assert
        Assert.Contains("Please provide a component name", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetComponentMigration_WithWhitespace_ShouldReturnErrorMessage()
    {
        // Act
        var result = _tools.GetComponentMigration("   ");

        // Assert
        Assert.Contains("Please provide a component name", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetComponentMigration_WithNonExistentComponent_ShouldReturnNotFoundMessage()
    {
        // Act
        var result = _tools.GetComponentMigration("NonExistentComponent12345");

        // Assert
        Assert.Contains("No migration guide found", result, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("Available components", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetComponentMigration_WithFluentButton_ShouldReturnContent()
    {
        // Act
        var result = _tools.GetComponentMigration("FluentButton");

        // Assert
        Assert.NotNull(result);
        Assert.Contains("FluentButton", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetComponentMigration_WithButtonShorthand_ShouldReturnContent()
    {
        // Act
        var result = _tools.GetComponentMigration("Button");

        // Assert
        Assert.NotNull(result);
        Assert.Contains("FluentButton", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetComponentMigration_WithDataGrid_ShouldReturnContent()
    {
        // Act
        var result = _tools.GetComponentMigration("DataGrid");

        // Assert
        Assert.NotNull(result);
        Assert.False(string.IsNullOrEmpty(result));
    }

    [Fact]
    public void GetComponentMigration_CaseInsensitive_ShouldReturnContent()
    {
        // Act
        var result = _tools.GetComponentMigration("fluentbutton");

        // Assert
        Assert.NotNull(result);
        Assert.Contains("FluentButton", result, StringComparison.OrdinalIgnoreCase);
    }

    #endregion
}
