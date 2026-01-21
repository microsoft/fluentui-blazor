// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.McpServer.Services;
using Microsoft.FluentUI.AspNetCore.McpServer.Tools;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tests.Tools;

/// <summary>
/// Tests for the <see cref="DocumentationTools"/> class.
/// </summary>
public class DocumentationToolsTests
{
    private readonly DocumentationService _documentationService;
    private readonly DocumentationTools _tools;

    public DocumentationToolsTests()
    {
        _documentationService = new DocumentationService();
        _tools = new DocumentationTools(_documentationService);
    }

    #region ListGetStartedTopics Tests

    [Fact]
    public void ListGetStartedTopics_ShouldReturnNonNullResult()
    {
        // Act
        var result = _tools.ListDocumentation();

        // Assert
        Assert.NotNull(result);
        Assert.False(string.IsNullOrEmpty(result));
    }

    [Fact]
    public void ListGetStartedTopics_WhenNoDocumentation_ShouldReturnAppropriateMessage()
    {
        // Act
        var result = _tools.ListDocumentation();

        // Assert
        // Either contains documentation or indicates no documentation found
        var hasContent = result.Contains('#', StringComparison.Ordinal) ||
                         result.Contains("No GetStarted documentation found", StringComparison.OrdinalIgnoreCase);
        Assert.True(hasContent);
    }

    #endregion

    #region GetGetStartedTopic Tests

    [Fact]
    public void GetGetStartedTopic_WithEmptyName_ShouldReturnErrorMessage()
    {
        // Act
        var result = _tools.GetDocumentationTopic(string.Empty);

        // Assert
        Assert.Contains("Please provide a topic name", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetGetStartedTopic_WithWhitespace_ShouldReturnErrorMessage()
    {
        // Act
        var result = _tools.GetDocumentationTopic("   ");

        // Assert
        Assert.Contains("Please provide a topic name", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetGetStartedTopic_WithNonExistentTopic_ShouldReturnNotFoundMessage()
    {
        // Act
        var result = _tools.GetDocumentationTopic("NonExistentTopic12345");

        // Assert
        Assert.Contains("was not found", result, StringComparison.OrdinalIgnoreCase);
    }

    #endregion

    #region SearchGetStartedDocumentation Tests

    [Fact]
    public void SearchGetStartedDocumentation_WithEmptyTerm_ShouldReturnErrorMessage()
    {
        // Act
        var result = _tools.SearchDocumentation(string.Empty);

        // Assert
        Assert.Contains("Please provide a search term", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void SearchGetStartedDocumentation_WithWhitespace_ShouldReturnErrorMessage()
    {
        // Act
        var result = _tools.SearchDocumentation("   ");

        // Assert
        Assert.Contains("Please provide a search term", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void SearchGetStartedDocumentation_WithNonMatchingTerm_ShouldReturnNoResultsMessage()
    {
        // Act
        var result = _tools.SearchDocumentation("XYZ123NonExistent456");

        // Assert
        Assert.Contains("No documentation found", result, StringComparison.OrdinalIgnoreCase);
    }

    #endregion

    #region GetMigrationGuide Tests

    [Fact]
    public void GetMigrationGuide_ShouldReturnNonNullResult()
    {
        // Act
        var result = _tools.GetMigrationGuide();

        // Assert
        Assert.NotNull(result);
        Assert.False(string.IsNullOrEmpty(result));
    }

    [Fact]
    public void GetMigrationGuide_WhenNoDocumentation_ShouldReturnAppropriateMessage()
    {
        // Act
        var result = _tools.GetMigrationGuide();

        // Assert
        // Either contains migration content or indicates no documentation found
        var hasContent = result.Contains("Migration", StringComparison.OrdinalIgnoreCase) ||
                         result.Contains("No migration documentation found", StringComparison.OrdinalIgnoreCase);
        Assert.True(hasContent);
    }

    #endregion
}
