// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.McpServer.Resources;
using Microsoft.FluentUI.AspNetCore.McpServer.Services;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tests.Resources;

/// <summary>
/// Tests for the <see cref="DocumentationResources"/> class.
/// </summary>
public class DocumentationResourcesTests
{
    private readonly DocumentationService _documentationService;
    private readonly DocumentationResources _resources;

    public DocumentationResourcesTests()
    {
        _documentationService = new DocumentationService();
        _resources = new DocumentationResources(_documentationService);
    }

    #region GetAllTopics Tests

    [Fact]
    public void GetAllTopics_ShouldReturnNonNullResult()
    {
        // Act
        var result = _resources.GetAllTopics();

        // Assert
        Assert.NotNull(result);
        Assert.False(string.IsNullOrEmpty(result));
    }

    [Fact]
    public void GetAllTopics_ShouldContainHeader()
    {
        // Act
        var result = _resources.GetAllTopics();

        // Assert
        Assert.Contains("Fluent UI Blazor - Documentation", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetAllTopics_ShouldContainTotalCount()
    {
        // Act
        var result = _resources.GetAllTopics();

        // Assert
        Assert.Contains("Total:", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetAllTopics_ShouldContainUsageInstructions()
    {
        // Act
        var result = _resources.GetAllTopics();

        // Assert
        Assert.Contains("fluentui://documentation/", result, StringComparison.OrdinalIgnoreCase);
    }

    #endregion

    #region GetTopic Tests

    [Fact]
    public void GetTopic_WithNonExistentTopic_ShouldReturnNotFoundMessage()
    {
        // Act
        var result = _resources.GetTopic("NonExistentTopic12345");

        // Assert
        Assert.Contains("Topic Not Found", result, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("was not found", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetTopic_WithEmptyTopic_ShouldReturnNotFoundMessage()
    {
        // Act
        var result = _resources.GetTopic(string.Empty);

        // Assert
        Assert.Contains("Not Found", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetTopic_ShouldSuggestAvailableTopics()
    {
        // Act
        var result = _resources.GetTopic("NonExistentTopic");

        // Assert
        Assert.Contains("Available topics", result, StringComparison.OrdinalIgnoreCase);
    }

    #endregion

    #region GetMigrationGuide Tests

    [Fact]
    public void GetMigrationGuide_ShouldReturnNonNullContent()
    {
        // Act
        var result = _resources.GetMigrationGuide();

        // Assert
        Assert.NotNull(result);
        Assert.Contains("Migration", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetMigrationGuide_ShouldContainV5Header()
    {
        // Act
        var result = _resources.GetMigrationGuide();

        // Assert
        Assert.Contains("v5", result, StringComparison.OrdinalIgnoreCase);
    }

    #endregion
}
