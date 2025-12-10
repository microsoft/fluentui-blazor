// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.McpServer.Services;
using Microsoft.FluentUI.AspNetCore.Components.McpServer.Tools;

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Tests.Tools;

/// <summary>
/// Tests for <see cref="ComponentListTools"/>.
/// </summary>
public class ComponentListToolsTests
{
    private readonly ComponentListTools _tools;
    private readonly FluentUIDocumentationService _documentationService;

    public ComponentListToolsTests()
    {
        var assembly = typeof(Microsoft.FluentUI.AspNetCore.Components._Imports).Assembly;
        var xmlPath = XmlDocumentationFinder.Find();
        _documentationService = new FluentUIDocumentationService(assembly, xmlPath);
        _tools = new ComponentListTools(_documentationService);
    }

    [Fact]
    public void ListComponents_ReturnsMarkdownWithComponents()
    {
        // Act
        var result = _tools.ListComponents();

        // Assert
        Assert.NotEmpty(result);
        Assert.Contains("FluentButton", result);
        Assert.Contains("#", result); // Contains markdown headers
    }

    [Fact]
    public void ListComponents_WithCategory_FiltersResults()
    {
        // Act
        var result = _tools.ListComponents("Button");

        // Assert
        Assert.NotEmpty(result);
        Assert.Contains("FluentButton", result);
    }

    [Fact]
    public void ListComponents_WithInvalidCategory_ReturnsMessage()
    {
        // Act
        var result = _tools.ListComponents("NonExistentCategory");

        // Assert
        Assert.Contains("No components found", result);
    }

    [Fact]
    public void SearchComponents_FindsMatchingComponents()
    {
        // Act
        var result = _tools.SearchComponents("button");

        // Assert
        Assert.NotEmpty(result);
        Assert.Contains("FluentButton", result);
    }

    [Fact]
    public void SearchComponents_ReturnsMessageForNoMatches()
    {
        // Act
        var result = _tools.SearchComponents("xyz123nonexistent");

        // Assert
        Assert.Contains("No components found", result);
    }

    [Fact]
    public void ListCategories_ReturnsMarkdownWithCategories()
    {
        // Act
        var result = _tools.ListCategories();

        // Assert
        Assert.NotEmpty(result);
        Assert.Contains("Button", result);
        Assert.Contains("#", result); // Contains markdown headers
    }
}
