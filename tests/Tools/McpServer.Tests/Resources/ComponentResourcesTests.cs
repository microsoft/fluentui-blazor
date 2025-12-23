// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.McpServer.Resources;
using Microsoft.FluentUI.AspNetCore.McpServer.Services;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tests.Resources;

/// <summary>
/// Tests for the <see cref="ComponentResources"/> class.
/// </summary>
public class ComponentResourcesTests
{
    private readonly string _testJsonPath;
    private readonly bool _jsonFileExists;

    public ComponentResourcesTests()
    {
        _testJsonPath = Path.Combine(AppContext.BaseDirectory, "FluentUIComponentsDocumentation.json");
        _jsonFileExists = File.Exists(_testJsonPath);
    }

    private ComponentResources CreateResources()
    {
        var service = new FluentUIDocumentationService(_testJsonPath);
        return new ComponentResources(service);
    }

    #region GetComponent Tests

    [Fact]
    public void GetComponent_WithValidName_ShouldReturnDetails()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var resources = CreateResources();

        // Act
        var result = resources.GetComponent("FluentButton");

        // Assert
        Assert.False(string.IsNullOrEmpty(result));
        Assert.Contains("# FluentButton", result);
    }

    [Fact]
    public void GetComponent_WithNonExistentName_ShouldReturnNotFound()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var resources = CreateResources();

        // Act
        var result = resources.GetComponent("NonExistentComponent");

        // Assert
        Assert.Contains("# Component Not Found", result);
        Assert.Contains("was not found", result);
    }

    [Fact]
    public void GetComponent_ShouldShowCategory()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var resources = CreateResources();

        // Act
        var result = resources.GetComponent("FluentButton");

        // Assert
        Assert.Contains("**Category:**", result);
    }

    [Fact]
    public void GetComponent_ShouldShowParameters()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var resources = CreateResources();

        // Act
        var result = resources.GetComponent("FluentButton");

        // Assert
        Assert.Contains("## Parameters", result);
    }

    [Fact]
    public void GetComponent_ForGenericComponent_ShouldShowGenericIndicator()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var resources = CreateResources();

        // Act
        var result = resources.GetComponent("FluentDataGrid");

        // Assert
        Assert.Contains("**Generic Component:** Yes", result);
    }

    [Fact]
    public void GetComponent_ShouldShowEvents()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var resources = CreateResources();

        // Act
        var result = resources.GetComponent("FluentButton");

        // Assert
        Assert.Contains("## Events", result);
    }

    [Fact]
    public void GetComponent_ShouldFormatAsMarkdown()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var resources = CreateResources();

        // Act
        var result = resources.GetComponent("FluentButton");

        // Assert
        Assert.Contains("# ", result);
        Assert.Contains("## ", result);
        Assert.Contains("|", result);
    }

    #endregion

    #region GetCategory Tests

    [Fact]
    public void GetCategory_WithValidCategory_ShouldReturnComponents()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var resources = CreateResources();

        // Act
        var result = resources.GetCategory("Components");

        // Assert
        Assert.False(string.IsNullOrEmpty(result));
        Assert.Contains("# Components Components", result);
        Assert.Contains("Total:", result);
    }

    [Fact]
    public void GetCategory_WithInvalidCategory_ShouldReturnNotFound()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var resources = CreateResources();

        // Act
        var result = resources.GetCategory("NonExistentCategory");

        // Assert
        Assert.Contains("# Category Not Found", result);
        Assert.Contains("No components found in category", result);
    }

    [Fact]
    public void GetCategory_ShouldShowCount()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var resources = CreateResources();

        // Act
        var result = resources.GetCategory("Components");

        // Assert
        Assert.Contains("Total:", result);
        Assert.Contains("components", result);
    }

    [Fact]
    public void GetCategory_ShouldFormatAsMarkdown()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var resources = CreateResources();

        // Act
        var result = resources.GetCategory("Components");

        // Assert
        Assert.Contains("# ", result);
        Assert.Contains("## ", result);
    }

    [Fact]
    public void GetCategory_ShouldShowGenericIndicator()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var resources = CreateResources();

        // Act
        var result = resources.GetCategory("Components");

        // Assert
        Assert.Contains("<T>", result);
    }

    #endregion

    #region GetEnum Tests

    [Fact]
    public void GetEnum_WithValidName_ShouldReturnDetails()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var resources = CreateResources();

        // Act
        var result = resources.GetEnum("Appearance");

        // Assert
        Assert.False(string.IsNullOrEmpty(result));
        Assert.Contains("# Appearance", result);
    }

    [Fact]
    public void GetEnum_WithNonExistentName_ShouldReturnNotFound()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var resources = CreateResources();

        // Act
        var result = resources.GetEnum("NonExistentEnum");

        // Assert
        Assert.Contains("# Enum Not Found", result);
        Assert.Contains("was not found", result);
    }

    [Fact]
    public void GetEnum_ShouldShowValues()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var resources = CreateResources();

        // Act
        var result = resources.GetEnum("Appearance");

        // Assert
        Assert.Contains("## Values", result);
        Assert.Contains("| Name | Value | Description |", result);
    }

    [Fact]
    public void GetEnum_ShouldFormatAsMarkdown()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var resources = CreateResources();

        // Act
        var result = resources.GetEnum("Appearance");

        // Assert
        Assert.Contains("# ", result);
        Assert.Contains("## ", result);
        Assert.Contains("|", result);
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
