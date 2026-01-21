// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.McpServer.Services;
using Microsoft.FluentUI.AspNetCore.McpServer.Tools;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tests.Tools;

/// <summary>
/// Tests for the <see cref="GetStartedTools"/> class.
/// </summary>
public class GetStartedToolsTests
{
    private readonly GetStartedDocumentationService _documentationService;
    private readonly GetStartedTools _tools;

    public GetStartedToolsTests()
    {
        _documentationService = new GetStartedDocumentationService();
        _tools = new GetStartedTools(_documentationService);
    }

    #region ListGetStartedTopics Tests

    [Fact]
    public void ListGetStartedTopics_ShouldReturnNonNullResult()
    {
        // Act
        var result = _tools.ListGetStartedTopics();

        // Assert
        Assert.NotNull(result);
        Assert.False(string.IsNullOrEmpty(result));
    }

    [Fact]
    public void ListGetStartedTopics_WhenNoDocumentation_ShouldReturnAppropriateMessage()
    {
        // Act
        var result = _tools.ListGetStartedTopics();

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
        var result = _tools.GetGetStartedTopic(string.Empty);

        // Assert
        Assert.Contains("Please provide a topic name", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetGetStartedTopic_WithWhitespace_ShouldReturnErrorMessage()
    {
        // Act
        var result = _tools.GetGetStartedTopic("   ");

        // Assert
        Assert.Contains("Please provide a topic name", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetGetStartedTopic_WithNonExistentTopic_ShouldReturnNotFoundMessage()
    {
        // Act
        var result = _tools.GetGetStartedTopic("NonExistentTopic12345");

        // Assert
        Assert.Contains("was not found", result, StringComparison.OrdinalIgnoreCase);
    }

    #endregion

    #region SearchGetStartedDocumentation Tests

    [Fact]
    public void SearchGetStartedDocumentation_WithEmptyTerm_ShouldReturnErrorMessage()
    {
        // Act
        var result = _tools.SearchGetStartedDocumentation(string.Empty);

        // Assert
        Assert.Contains("Please provide a search term", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void SearchGetStartedDocumentation_WithWhitespace_ShouldReturnErrorMessage()
    {
        // Act
        var result = _tools.SearchGetStartedDocumentation("   ");

        // Assert
        Assert.Contains("Please provide a search term", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void SearchGetStartedDocumentation_WithNonMatchingTerm_ShouldReturnNoResultsMessage()
    {
        // Act
        var result = _tools.SearchGetStartedDocumentation("XYZ123NonExistent456");

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
