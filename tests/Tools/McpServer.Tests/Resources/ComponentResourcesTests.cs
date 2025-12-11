// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.McpServer.Resources;
using Microsoft.FluentUI.AspNetCore.Components.McpServer.Services;

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Tests.Resources;

/// <summary>
/// Tests for <see cref="ComponentResources"/>.
/// </summary>
public class ComponentResourcesTests
{
    private readonly ComponentResources _resources;
    private readonly FluentUIDocumentationService _documentationService;

    public ComponentResourcesTests()
    {
        var jsonPath = JsonDocumentationFinder.Find();
        _documentationService = new FluentUIDocumentationService(jsonPath);
        _resources = new ComponentResources(_documentationService);
    }

    [Fact]
    public void GetComponent_WithValidName_ReturnsNonEmptyString()
    {
        // Act
        var result = _resources.GetComponent("FluentButton");

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void GetComponent_WithValidName_ContainsComponentInfo()
    {
        // Act
        var result = _resources.GetComponent("FluentButton");

        // Assert
        Assert.Contains("FluentButton", result);
    }

    [Fact]
    public void GetComponent_WithInvalidName_ReturnsNotFoundMessage()
    {
        // Act
        var result = _resources.GetComponent("NonExistentComponent");

        // Assert
        Assert.Contains("not found", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetComponent_ContainsParameters()
    {
        // Act
        var result = _resources.GetComponent("FluentButton");

        // Assert
        Assert.Contains("Parameter", result);
    }

    [Fact]
    public void GetComponent_ContainsCategory()
    {
        // Act
        var result = _resources.GetComponent("FluentButton");

        // Assert
        Assert.Contains("Category", result);
    }

    [Theory]
    [InlineData("FluentButton")]
    [InlineData("FluentCard")]
    [InlineData("FluentTextField")]
    public void GetComponent_WithVariousComponents_ReturnsValidInfo(string componentName)
    {
        // Act
        var result = _resources.GetComponent(componentName);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Contains(componentName, result);
    }

    [Fact]
    public void GetCategory_WithValidCategory_ReturnsNonEmptyString()
    {
        // Act
        var result = _resources.GetCategory("Button");

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void GetCategory_WithValidCategory_ContainsComponents()
    {
        // Act
        var result = _resources.GetCategory("Button");

        // Assert
        Assert.Contains("FluentButton", result);
    }

    [Fact]
    public void GetCategory_WithInvalidCategory_ReturnsNotFoundMessage()
    {
        // Act
        var result = _resources.GetCategory("NonExistentCategory");

        // Assert
        Assert.Contains("not found", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetCategory_ContainsTotalCount()
    {
        // Act
        var result = _resources.GetCategory("Button");

        // Assert
        Assert.Contains("Total:", result);
    }

    [Fact]
    public void GetEnum_WithValidName_ReturnsNonEmptyString()
    {
        // Act
        var result = _resources.GetEnum("Appearance");

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void GetEnum_WithInvalidName_ReturnsNotFoundMessage()
    {
        // Act
        var result = _resources.GetEnum("NonExistentEnum");

        // Assert
        Assert.Contains("not found", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetEnum_ContainsValuesSection()
    {
        // Act
        var result = _resources.GetEnum("Appearance");

        // Assert
        Assert.Contains("Values", result);
    }

    [Fact]
    public void GetEnum_ContainsTable()
    {
        // Act
        var result = _resources.GetEnum("Appearance");

        // Assert
        Assert.Contains("|", result);
        Assert.Contains("Name", result);
    }
}
