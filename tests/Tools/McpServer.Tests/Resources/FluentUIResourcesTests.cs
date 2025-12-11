// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.McpServer.Resources;
using Microsoft.FluentUI.AspNetCore.Components.McpServer.Services;

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Tests.Resources;

/// <summary>
/// Tests for <see cref="FluentUIResources"/>.
/// </summary>
public class FluentUIResourcesTests
{
    private readonly FluentUIResources _resources;
    private readonly FluentUIDocumentationService _documentationService;

    public FluentUIResourcesTests()
    {
        var jsonPath = JsonDocumentationFinder.Find();
        _documentationService = new FluentUIDocumentationService(jsonPath);
        _resources = new FluentUIResources(_documentationService);
    }

    [Fact]
    public void GetAllComponents_ReturnsNonEmptyString()
    {
        // Act
        var result = _resources.GetAllComponents();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void GetAllComponents_ContainsFluentButton()
    {
        // Act
        var result = _resources.GetAllComponents();

        // Assert
        Assert.Contains("FluentButton", result);
    }

    [Fact]
    public void GetAllComponents_ContainsMarkdownHeaders()
    {
        // Act
        var result = _resources.GetAllComponents();

        // Assert
        Assert.Contains("#", result);
        Assert.Contains("Fluent UI Blazor Components", result);
    }

    [Fact]
    public void GetAllComponents_ContainsTotalCount()
    {
        // Act
        var result = _resources.GetAllComponents();

        // Assert
        Assert.Contains("Total:", result);
    }

    [Fact]
    public void GetCategories_ReturnsNonEmptyString()
    {
        // Act
        var result = _resources.GetCategories();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void GetCategories_ContainsButtonCategory()
    {
        // Act
        var result = _resources.GetCategories();

        // Assert
        Assert.Contains("Button", result);
    }

    [Fact]
    public void GetCategories_ContainsComponentCounts()
    {
        // Act
        var result = _resources.GetCategories();

        // Assert
        Assert.Contains("components", result);
    }

    [Fact]
    public void GetAllEnums_ReturnsNonEmptyString()
    {
        // Act
        var result = _resources.GetAllEnums();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void GetAllEnums_ContainsTotalCount()
    {
        // Act
        var result = _resources.GetAllEnums();

        // Assert
        Assert.Contains("Total:", result);
    }

    [Fact]
    public void GetAllEnums_ContainsEnumValues()
    {
        // Act
        var result = _resources.GetAllEnums();

        // Assert
        Assert.Contains("Values:", result);
    }

    [Fact]
    public void GetAllEnums_ContainsMarkdownHeaders()
    {
        // Act
        var result = _resources.GetAllEnums();

        // Assert
        Assert.Contains("# Fluent UI Blazor Enum Types", result);
    }
}
