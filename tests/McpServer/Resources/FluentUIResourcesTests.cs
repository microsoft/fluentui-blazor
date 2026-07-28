// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.McpServer.Resources;
using Microsoft.FluentUI.AspNetCore.McpServer.Services;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tests.Resources;

/// <summary>
/// Tests for the <see cref="FluentUIResources"/> class.
/// </summary>
public class FluentUIResourcesTests
{
    private readonly string _testJsonPath;
    private readonly bool _jsonFileExists;

    public FluentUIResourcesTests()
    {
        _testJsonPath = Path.Combine(AppContext.BaseDirectory, "FluentUIComponentsDocumentation.json");
        _jsonFileExists = File.Exists(_testJsonPath);
    }

    private FluentUIResources CreateResources()
    {
        var service = new FluentUIDocumentationService(_testJsonPath);
        return new FluentUIResources(service);
    }

    #region GetAllComponents Tests

    [Fact]
    public void GetAllComponents_ShouldReturnAllComponents()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var resources = CreateResources();

        // Act
        var result = resources.GetAllComponents();

        // Assert
        Assert.False(string.IsNullOrEmpty(result));
        Assert.Contains("# Fluent UI Blazor Components", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetAllComponents_ShouldShowTotalCount()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var resources = CreateResources();

        // Act
        var result = resources.GetAllComponents();

        // Assert
        Assert.Contains("Total:", result, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("components", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetAllComponents_ShouldGroupByCategory()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var resources = CreateResources();

        // Act
        var result = resources.GetAllComponents();

        // Assert
        Assert.Contains("## ", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetAllComponents_ShouldShowGenericIndicator()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var resources = CreateResources();

        // Act
        var result = resources.GetAllComponents();

        // Assert
        Assert.Contains("<T>", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetAllComponents_ShouldFormatAsMarkdown()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var resources = CreateResources();

        // Act
        var result = resources.GetAllComponents();

        // Assert
        Assert.Contains("# ", result, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("## ", result, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("- **", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetAllComponents_ShouldContainExpectedComponents()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var resources = CreateResources();

        // Act
        var result = resources.GetAllComponents();

        // Assert
        Assert.Contains("FluentButton", result, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("FluentGrid", result, StringComparison.OrdinalIgnoreCase);
    }

    #endregion

    #region GetAllEnums Tests

    [Fact]
    public void GetAllEnums_ShouldReturnAllEnums()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var resources = CreateResources();

        // Act
        var result = resources.GetAllEnums();

        // Assert
        Assert.False(string.IsNullOrEmpty(result));
        Assert.Contains("# Fluent UI Blazor Enum Types", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetAllEnums_ShouldShowTotalCount()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var resources = CreateResources();

        // Act
        var result = resources.GetAllEnums();

        // Assert
        Assert.Contains("Total:", result, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("enums", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetAllEnums_ShouldShowEnumValues()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var resources = CreateResources();

        // Act
        var result = resources.GetAllEnums();

        // Assert
        Assert.Contains("**Values:**", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetAllEnums_ShouldFormatAsMarkdown()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var resources = CreateResources();

        // Act
        var result = resources.GetAllEnums();

        // Assert
        Assert.Contains("# ", result, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("## ", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetAllEnums_ShouldContainExpectedEnums()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var resources = CreateResources();

        // Act
        var result = resources.GetAllEnums();

        // Assert
        Assert.Contains("Appearance", result, StringComparison.OrdinalIgnoreCase);
    }

    #endregion

    #region Constructor Tests

    [Fact]
    public void Constructor_ShouldInitializeCorrectly()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Act
        var resources = CreateResources();

        // Assert
        Assert.NotNull(resources);
    }

    #endregion
}
