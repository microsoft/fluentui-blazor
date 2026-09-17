// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.McpServer.Services;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tests.Services;

/// <summary>
/// Tests for the <see cref="MigrationService"/> class.
/// </summary>
public class MigrationServiceTests
{
    #region Constructor Tests

    [Fact]
    public void Constructor_ShouldNotThrow()
    {
        // Act
        var exception = Record.Exception(() => new MigrationService());

        // Assert
        Assert.Null(exception);
    }

    #endregion

    #region GetMigrationOverview Tests

    [Fact]
    public void GetMigrationOverview_ShouldReturnNonNullResult()
    {
        // Arrange
        var service = new MigrationService();

        // Act
        var result = service.GetMigrationOverview();

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void GetMigrationOverview_ShouldHaveTitle()
    {
        // Arrange
        var service = new MigrationService();

        // Act
        var result = service.GetMigrationOverview();

        // Assert
        Assert.NotNull(result);
        Assert.False(string.IsNullOrEmpty(result.Title));
    }

    [Fact]
    public void GetMigrationOverview_ShouldHaveContent()
    {
        // Arrange
        var service = new MigrationService();

        // Act
        var result = service.GetMigrationOverview();

        // Assert
        Assert.NotNull(result);
        Assert.False(string.IsNullOrEmpty(result.Content));
    }

    [Fact]
    public void GetMigrationOverview_ShouldHaveMigrationGeneralFileName()
    {
        // Arrange
        var service = new MigrationService();

        // Act
        var result = service.GetMigrationOverview();

        // Assert
        Assert.NotNull(result);
        Assert.Equal("MigrationGeneral", result.FileName);
    }

    #endregion

    #region GetAllComponentMigrations Tests

    [Fact]
    public void GetAllComponentMigrations_ShouldReturnNonNullList()
    {
        // Arrange
        var service = new MigrationService();

        // Act
        var result = service.GetAllComponentMigrations();

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void GetAllComponentMigrations_ShouldReturnMultipleItems()
    {
        // Arrange
        var service = new MigrationService();

        // Act
        var result = service.GetAllComponentMigrations();

        // Assert
        Assert.True(result.Count > 0, "Expected at least one component migration guide");
    }

    [Fact]
    public void GetAllComponentMigrations_ShouldBeOrderedByTitle()
    {
        // Arrange
        var service = new MigrationService();

        // Act
        var result = service.GetAllComponentMigrations();

        // Assert
        if (result.Count > 1)
        {
            for (var i = 0; i < result.Count - 1; i++)
            {
                Assert.True(
                    string.Compare(result[i].Title, result[i + 1].Title, StringComparison.Ordinal) <= 0,
                    $"Expected '{result[i].Title}' to come before '{result[i + 1].Title}'");
            }
        }
    }

    [Fact]
    public void GetAllComponentMigrations_ShouldNotContainGeneralMigration()
    {
        // Arrange
        var service = new MigrationService();

        // Act
        var result = service.GetAllComponentMigrations();

        // Assert
        Assert.DoesNotContain(result, m => m.FileName.Equals("MigrationGeneral", StringComparison.OrdinalIgnoreCase));
    }

    #endregion

    #region GetComponentMigration Tests

    [Fact]
    public void GetComponentMigration_WithNull_ShouldReturnNull()
    {
        // Arrange
        var service = new MigrationService();

        // Act
        var result = service.GetComponentMigration(null!);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetComponentMigration_WithEmpty_ShouldReturnNull()
    {
        // Arrange
        var service = new MigrationService();

        // Act
        var result = service.GetComponentMigration(string.Empty);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetComponentMigration_WithWhitespace_ShouldReturnNull()
    {
        // Arrange
        var service = new MigrationService();

        // Act
        var result = service.GetComponentMigration("   ");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetComponentMigration_WithNonExistentComponent_ShouldReturnNull()
    {
        // Arrange
        var service = new MigrationService();

        // Act
        var result = service.GetComponentMigration("NonExistentComponent12345");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetComponentMigration_WithFluentPrefix_ShouldReturnResult()
    {
        // Arrange
        var service = new MigrationService();

        // Act
        var result = service.GetComponentMigration("FluentButton");

        // Assert
        Assert.NotNull(result);
        Assert.False(string.IsNullOrEmpty(result.Content));
    }

    [Fact]
    public void GetComponentMigration_WithoutFluentPrefix_ShouldReturnResult()
    {
        // Arrange
        var service = new MigrationService();

        // Act
        var result = service.GetComponentMigration("Button");

        // Assert
        Assert.NotNull(result);
        Assert.False(string.IsNullOrEmpty(result.Content));
    }

    [Fact]
    public void GetComponentMigration_CaseInsensitive_ShouldReturnResult()
    {
        // Arrange
        var service = new MigrationService();

        // Act
        var result = service.GetComponentMigration("fluentbutton");

        // Assert
        Assert.NotNull(result);
        Assert.False(string.IsNullOrEmpty(result.Content));
    }

    #endregion

    #region GetAvailableComponentNames Tests

    [Fact]
    public void GetAvailableComponentNames_ShouldReturnNonNullList()
    {
        // Arrange
        var service = new MigrationService();

        // Act
        var result = service.GetAvailableComponentNames();

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void GetAvailableComponentNames_ShouldReturnMultipleItems()
    {
        // Arrange
        var service = new MigrationService();

        // Act
        var result = service.GetAvailableComponentNames();

        // Assert
        Assert.True(result.Count > 0, "Expected at least one component name");
    }

    [Fact]
    public void GetAvailableComponentNames_ShouldBeSorted()
    {
        // Arrange
        var service = new MigrationService();

        // Act
        var result = service.GetAvailableComponentNames();

        // Assert
        if (result.Count > 1)
        {
            for (var i = 0; i < result.Count - 1; i++)
            {
                Assert.True(
                    string.Compare(result[i], result[i + 1], StringComparison.OrdinalIgnoreCase) <= 0,
                    $"Expected '{result[i]}' to come before '{result[i + 1]}'");
            }
        }
    }

    [Fact]
    public void GetAvailableComponentNames_ShouldContainFluentButton()
    {
        // Arrange
        var service = new MigrationService();

        // Act
        var result = service.GetAvailableComponentNames();

        // Assert
        Assert.Contains(result, name => name.Equals("FluentButton", StringComparison.OrdinalIgnoreCase));
    }

    #endregion

    #region ExtractComponentName Tests

    [Theory]
    [InlineData("MigrationFluentButton", "FluentButton")]
    [InlineData("MigrationFluentDataGrid", "FluentDataGrid")]
    [InlineData("MigrationColor", "Color")]
    [InlineData("Migration", "Migration")]
    [InlineData("FluentButton", "FluentButton")]
    public void ExtractComponentName_ShouldReturnExpectedValue(string fileName, string expected)
    {
        // Act
        var result = MigrationService.ExtractComponentName(fileName);

        // Assert
        Assert.Equal(expected, result);
    }

    #endregion
}
