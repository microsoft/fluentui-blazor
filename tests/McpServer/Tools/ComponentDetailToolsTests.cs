// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.McpServer.Services;
using Microsoft.FluentUI.AspNetCore.McpServer.Tools;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tests.Tools;

/// <summary>
/// Tests for the <see cref="ComponentDetailTools"/> class.
/// </summary>
public class ComponentDetailToolsTests
{
    private readonly string _testJsonPath;
    private readonly bool _jsonFileExists;

    public ComponentDetailToolsTests()
    {
        _testJsonPath = Path.Combine(AppContext.BaseDirectory, "FluentUIComponentsDocumentation.json");
        _jsonFileExists = File.Exists(_testJsonPath);
    }

    private ComponentDetailTools CreateTools()
    {
        var service = new FluentUIDocumentationService(_testJsonPath);
        var componentDocService = new ComponentDocumentationService();
        return new ComponentDetailTools(service, componentDocService);
    }

    #region GetComponentDetails Tests

    [Fact]
    public void GetComponentDetails_WithExactName_ShouldReturnDetails()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var tools = CreateTools();

        // Act
        var result = tools.GetComponentDetails("FluentButton");

        // Assert
        Assert.False(string.IsNullOrEmpty(result));
        Assert.Contains("# FluentButton", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetComponentDetails_WithoutFluentPrefix_ShouldReturnDetails()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var tools = CreateTools();

        // Act
        var result = tools.GetComponentDetails("Button");

        // Assert
        Assert.False(string.IsNullOrEmpty(result));
        Assert.Contains("# FluentButton", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetComponentDetails_WithNonExistentName_ShouldReturnNotFoundMessage()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var tools = CreateTools();

        // Act
        var result = tools.GetComponentDetails("NonExistentComponent");

        // Assert
        Assert.Contains("not found", result, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("ListComponents()", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetComponentDetails_ShouldShowCategory()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var tools = CreateTools();

        // Act
        var result = tools.GetComponentDetails("FluentButton");

        // Assert
        Assert.Contains("**Category:**", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetComponentDetails_ShouldShowParameters()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var tools = CreateTools();

        // Act
        var result = tools.GetComponentDetails("FluentButton");

        // Assert
        Assert.Contains("## Parameters", result, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("| Name | Type | Default | Description |", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetComponentDetails_ShouldShowBaseClass()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var tools = CreateTools();

        // Act
        var result = tools.GetComponentDetails("FluentButton");

        // Assert
        Assert.Contains("**Base Class:**", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetComponentDetails_ForGenericComponent_ShouldShowGenericIndicator()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var tools = CreateTools();

        // Act - FluentDataGrid is a generic component with type parameter TGridItem
        var result = tools.GetComponentDetails("FluentDataGrid");

        // Assert
        Assert.Contains("**Generic Component:** Yes", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetComponentDetails_CaseInsensitive_ShouldWork()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var tools = CreateTools();

        // Act
        var result = tools.GetComponentDetails("fluentbutton");

        // Assert
        Assert.Contains("# FluentButton", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetComponentDetails_ShouldFormatAsMarkdown()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var tools = CreateTools();

        // Act
        var result = tools.GetComponentDetails("FluentButton");

        // Assert
        Assert.Contains("# ", result, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("## ", result, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("|", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetComponentDetails_ShouldShowEnumValues()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var tools = CreateTools();

        // Act
        var result = tools.GetComponentDetails("FluentButton");

        // Assert
        Assert.Contains("Values:", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetComponentDetails_WithEvents_ShouldShowEvents()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var tools = CreateTools();

        // Act
        var result = tools.GetComponentDetails("FluentButton");

        // Assert
        Assert.Contains("## Events", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetComponentDetails_WithMethods_ShouldShowMethods()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var tools = CreateTools();

        // Act
        var result = tools.GetComponentDetails("FluentTextField");

        // Assert
        // If the component has methods, they should be shown
        if (result.Contains("## Methods", StringComparison.OrdinalIgnoreCase))
        {
            Assert.Contains("```csharp", result, StringComparison.OrdinalIgnoreCase);
        }
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
