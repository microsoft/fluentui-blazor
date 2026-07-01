// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.McpServer.Services;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tests.Services;

/// <summary>
/// Tests for the <see cref="DocumentationService"/> class.
/// </summary>
public class DocumentationServiceTests
{
    #region Constructor Tests

    [Fact]
    public void Constructor_WithNullExcludedFolders_ShouldDefaultToMcp()
    {
        // Act
        var exception = Record.Exception(() => new DocumentationService(excludedFolders: null));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void Constructor_WithEmptyExcludedFolders_ShouldNotThrow()
    {
        // Act
        var exception = Record.Exception(() => new DocumentationService([]));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void Constructor_WithCustomExcludedFolders_ShouldNotThrow()
    {
        // Act
        var exception = Record.Exception(() => new DocumentationService(["mcp", "custom"]));

        // Assert
        Assert.Null(exception);
    }

    #endregion

    #region GetAllDocumentation Tests

    [Fact]
    public void GetAllDocumentation_ShouldReturnNonNullList()
    {
        // Arrange
        var service = new DocumentationService();

        // Act
        var result = service.GetAllDocumentation();

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void GetAllDocumentation_ShouldExcludeHiddenDocuments()
    {
        // Arrange
        var service = new DocumentationService();

        // Act
        var result = service.GetAllDocumentation();

        // Assert
        Assert.DoesNotContain(result, d => d.Hidden);
    }

    [Fact]
    public void GetAllDocumentation_ShouldBeOrderedByOrder()
    {
        // Arrange
        var service = new DocumentationService();

        // Act
        var result = service.GetAllDocumentation();

        // Assert
        if (result.Count > 1)
        {
            for (var i = 0; i < result.Count - 1; i++)
            {
                Assert.True(result[i].Order <= result[i + 1].Order);
            }
        }
    }

    #endregion

    #region GetDocumentation Tests

    [Fact]
    public void GetDocumentation_WithNonExistentTitle_ShouldReturnNull()
    {
        // Arrange
        var service = new DocumentationService();

        // Act
        var result = service.GetDocumentation("NonExistentTitle12345");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetDocumentation_WithEmptyString_ShouldReturnNull()
    {
        // Arrange
        var service = new DocumentationService();

        // Act
        var result = service.GetDocumentation(string.Empty);

        // Assert
        Assert.Null(result);
    }

    #endregion

    #region SearchDocumentation Tests

    [Fact]
    public void SearchDocumentation_WithEmptyTerm_ShouldReturnEmptyList()
    {
        // Arrange
        var service = new DocumentationService();

        // Act
        var result = service.SearchDocumentation(string.Empty);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void SearchDocumentation_WithNonMatchingTerm_ShouldReturnEmptyList()
    {
        // Arrange
        var service = new DocumentationService();

        // Act
        var result = service.SearchDocumentation("XYZ123NonExistent456");

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void SearchDocumentation_ShouldBeCaseInsensitive()
    {
        // Arrange
        var service = new DocumentationService();

        // Act
        var lowerResult = service.SearchDocumentation("install");
        var upperResult = service.SearchDocumentation("INSTALL");

        // Assert
        Assert.Equal(lowerResult.Count, upperResult.Count);
    }

    #endregion

    #region GetMigrationDocumentation Tests

    [Fact]
    public void GetMigrationDocumentation_ShouldReturnNonNullList()
    {
        // Arrange
        var service = new DocumentationService();

        // Act
        var result = service.GetMigrationDocumentation();

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void GetMigrationDocumentation_ShouldOnlyReturnMigrationRelatedDocs()
    {
        // Arrange
        var service = new DocumentationService();

        // Act
        var result = service.GetMigrationDocumentation();

        // Assert
        foreach (var doc in result)
        {
            var isMigrationRelated = doc.FileName.Contains("Migration", StringComparison.OrdinalIgnoreCase) ||
                                     doc.Title.Contains("Migration", StringComparison.OrdinalIgnoreCase) ||
                                     doc.Title.Contains("Migrating", StringComparison.OrdinalIgnoreCase);
            Assert.True(isMigrationRelated, $"Document '{doc.Title}' should be migration-related");
        }
    }

    #endregion

    #region GetTopics Tests

    [Fact]
    public void GetTopics_ShouldReturnNonNullList()
    {
        // Arrange
        var service = new DocumentationService();

        // Act
        var result = service.GetTopics();

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void GetTopics_ShouldReturnDistinctTitles()
    {
        // Arrange
        var service = new DocumentationService();

        // Act
        var result = service.GetTopics();

        // Assert
        Assert.Equal(result.Count, result.Distinct(StringComparer.OrdinalIgnoreCase).Count());
    }

    [Fact]
    public void GetTopics_ShouldBeOrderedAlphabetically()
    {
        // Arrange
        var service = new DocumentationService();

        // Act
        var result = service.GetTopics();

        // Assert
        if (result.Count > 1)
        {
            var sorted = result.Order(StringComparer.OrdinalIgnoreCase).ToList();
            Assert.Equal(sorted, result);
        }
    }

    #endregion
}
