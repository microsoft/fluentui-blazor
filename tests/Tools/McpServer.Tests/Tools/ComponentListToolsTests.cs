// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.McpServer.Services;
using Microsoft.FluentUI.AspNetCore.McpServer.Tools;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tests.Tools;

/// <summary>
/// Tests for the <see cref="ComponentListTools"/> class.
/// </summary>
public class ComponentListToolsTests
{
    private readonly string _testJsonPath;
    private readonly bool _jsonFileExists;

    public ComponentListToolsTests()
    {
        _testJsonPath = Path.Combine(AppContext.BaseDirectory, "FluentUIComponentsDocumentation.json");
        _jsonFileExists = File.Exists(_testJsonPath);
    }

    private ComponentListTools CreateTools()
    {
        var service = new FluentUIDocumentationService(_testJsonPath);
        return new ComponentListTools(service);
    }

    #region ListComponents Tests

    [Fact]
    public void ListComponents_WithNoCategory_ShouldReturnAllComponents()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var tools = CreateTools();

        // Act
        var result = tools.ListComponents();

        // Assert
        Assert.False(string.IsNullOrEmpty(result));
        Assert.Contains("# Fluent UI Blazor Components", result);
        Assert.Contains("FluentButton", result);
    }

    [Fact]
    public void ListComponents_WithNoCategory_ShouldShowCount()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var tools = CreateTools();

        // Act
        var result = tools.ListComponents();

        // Assert
        Assert.Contains("found)", result);
    }

    [Fact]
    public void ListComponents_WithValidCategory_ShouldReturnFilteredComponents()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var tools = CreateTools();

        // Act
        var result = tools.ListComponents(category: "Components");

        // Assert
        Assert.False(string.IsNullOrEmpty(result));
        Assert.Contains("Components", result);
    }

    [Fact]
    public void ListComponents_WithInvalidCategory_ShouldReturnNotFoundMessage()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var tools = CreateTools();

        // Act
        var result = tools.ListComponents(category: "NonExistentCategory");

        // Assert
        Assert.Contains("No components found in category", result);
        Assert.Contains("NonExistentCategory", result);
    }

    [Fact]
    public void ListComponents_ShouldGroupByCategory()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var tools = CreateTools();

        // Act
        var result = tools.ListComponents();

        // Assert
        Assert.Contains("## ", result);
    }

    [Fact]
    public void ListComponents_ShouldShowGenericIndicator()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var tools = CreateTools();

        // Act
        var result = tools.ListComponents();

        // Assert
        Assert.Contains("<T>", result);
    }

    [Fact]
    public void ListComponents_ShouldFormatAsMarkdown()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var tools = CreateTools();

        // Act
        var result = tools.ListComponents();

        // Assert
        Assert.Contains("# ", result);
        Assert.Contains("## ", result);
        Assert.Contains("**", result);
    }

    #endregion

    #region SearchComponents Tests

    [Fact]
    public void SearchComponents_WithValidTerm_ShouldReturnMatches()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var tools = CreateTools();

        // Act
        var result = tools.SearchComponents("Button");

        // Assert
        Assert.False(string.IsNullOrEmpty(result));
        Assert.Contains("Search Results", result);
        Assert.Contains("Button", result);
    }

    [Fact]
    public void SearchComponents_WithEmptyTerm_ShouldReturnError()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var tools = CreateTools();

        // Act
        var result = tools.SearchComponents("");

        // Assert
        Assert.Contains("Please provide a search term", result);
    }

    [Fact]
    public void SearchComponents_WithWhitespaceTerm_ShouldReturnError()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var tools = CreateTools();

        // Act
        var result = tools.SearchComponents("   ");

        // Assert
        Assert.Contains("Please provide a search term", result);
    }

    [Fact]
    public void SearchComponents_WithNoMatches_ShouldReturnNotFoundMessage()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var tools = CreateTools();

        // Act
        var result = tools.SearchComponents("XyzNonExistentTerm123");

        // Assert
        Assert.Contains("No components found matching", result);
    }

    [Fact]
    public void SearchComponents_ShouldShowCount()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var tools = CreateTools();

        // Act
        var result = tools.SearchComponents("Button");

        // Assert
        Assert.Contains("found)", result);
    }

    [Fact]
    public void SearchComponents_ShouldShowCategory()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var tools = CreateTools();

        // Act
        var result = tools.SearchComponents("Button");

        // Assert
        Assert.Contains("**Category:**", result);
    }

    [Fact]
    public void SearchComponents_CaseInsensitive_ShouldReturnMatches()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var tools = CreateTools();

        // Act
        var resultLower = tools.SearchComponents("button");
        var resultUpper = tools.SearchComponents("BUTTON");

        // Assert
        Assert.Contains("Button", resultLower);
        Assert.Contains("Button", resultUpper);
    }

    [Fact]
    public void SearchComponents_ShouldFormatAsMarkdown()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var tools = CreateTools();

        // Act
        var result = tools.SearchComponents("Button");

        // Assert
        Assert.Contains("# ", result);
        Assert.Contains("## ", result);
    }

    #endregion

    #region Constructor Tests

    [Fact]
    public void Constructor_ShouldInitializeCorrectly()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Act
        var tools = CreateTools();

        // Assert
        Assert.NotNull(tools);
    }

    #endregion
}
