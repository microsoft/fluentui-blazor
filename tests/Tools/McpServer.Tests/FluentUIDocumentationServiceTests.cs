// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.McpServer.Services;

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Tests.Services;

/// <summary>
/// Tests for <see cref="FluentUIDocumentationService"/>.
/// </summary>
public class FluentUIDocumentationServiceTests
{
    private readonly FluentUIDocumentationService _service;

    public FluentUIDocumentationServiceTests()
    {
        var assembly = typeof(Microsoft.FluentUI.AspNetCore.Components._Imports).Assembly;
        var xmlPath = XmlDocumentationFinder.Find();
        _service = new FluentUIDocumentationService(assembly, xmlPath);
    }

    [Fact]
    public void GetAllComponents_ReturnsNonEmptyList()
    {
        // Act
        var components = _service.GetAllComponents();

        // Assert
        Assert.NotEmpty(components);
    }

    [Fact]
    public void GetAllComponents_ContainsFluentButton()
    {
        // Act
        var components = _service.GetAllComponents();

        // Assert
        Assert.Contains(components, c => c.Name == "FluentButton");
    }

    [Fact]
    public void GetAllComponents_ContainsFluentCard()
    {
        // Act
        var components = _service.GetAllComponents();

        // Assert
        Assert.Contains(components, c => c.Name == "FluentCard");
    }

    [Theory]
    [InlineData("FluentButton")]
    [InlineData("FluentTextInput")]
    [InlineData("FluentAccordion")]
    [InlineData("FluentDialog")]
    [InlineData("FluentCard")]
    public void GetComponentDetails_ReturnsDetailsForKnownComponents(string componentName)
    {
        // Act
        var details = _service.GetComponentDetails(componentName);

        // Assert
        Assert.NotNull(details);
        Assert.Equal(componentName, details.Component.Name);
    }

    [Fact]
    public void GetComponentDetails_ReturnsNullForUnknownComponent()
    {
        // Act
        var details = _service.GetComponentDetails("NonExistentComponent");

        // Assert
        Assert.Null(details);
    }

    [Fact]
    public void GetComponentDetails_FluentButton_HasParameters()
    {
        // Act
        var details = _service.GetComponentDetails("FluentButton");

        // Assert
        Assert.NotNull(details);
        Assert.NotEmpty(details.Parameters);
    }

    [Fact]
    public void GetComponentDetails_FluentButton_HasAppearanceParameter()
    {
        // Act
        var details = _service.GetComponentDetails("FluentButton");

        // Assert
        Assert.NotNull(details);
        Assert.Contains(details.Parameters, p => p.Name == "Appearance");
    }

    [Fact]
    public void GetComponentsByCategory_ReturnsComponentsInCategory()
    {
        // Act
        var components = _service.GetComponentsByCategory("Button");

        // Assert
        Assert.NotEmpty(components);
        Assert.All(components, c => Assert.Equal("Button", c.Category));
    }

    [Fact]
    public void GetComponentsByCategory_IsCaseInsensitive()
    {
        // Act
        var componentsLower = _service.GetComponentsByCategory("button");
        var componentsUpper = _service.GetComponentsByCategory("BUTTON");

        // Assert
        Assert.Equal(componentsLower.Count, componentsUpper.Count);
    }

    [Fact]
    public void SearchComponents_FindsFluentButtonByName()
    {
        // Act
        var components = _service.SearchComponents("button");

        // Assert
        Assert.NotEmpty(components);
        Assert.Contains(components, c => c.Name == "FluentButton");
    }

    [Fact]
    public void SearchComponents_IsCaseInsensitive()
    {
        // Act
        var componentsLower = _service.SearchComponents("button");
        var componentsUpper = _service.SearchComponents("BUTTON");

        // Assert
        Assert.Equal(componentsLower.Count, componentsUpper.Count);
    }

    [Fact]
    public void GetCategories_ReturnsNonEmptyList()
    {
        // Act
        var categories = _service.GetCategories();

        // Assert
        Assert.NotEmpty(categories);
    }

    [Fact]
    public void GetCategories_ContainsButtonCategory()
    {
        // Act
        var categories = _service.GetCategories();

        // Assert
        Assert.Contains("Button", categories);
    }

    [Fact]
    public void GetAllEnums_ReturnsNonEmptyList()
    {
        // Act
        var enums = _service.GetAllEnums();

        // Assert
        Assert.NotEmpty(enums);
    }

    [Theory]
    [InlineData("ButtonAppearance")]
    [InlineData("Color")]
    [InlineData("Orientation")]
    public void GetEnumDetails_ReturnsInfoForKnownEnums(string enumName)
    {
        // Act
        var enumInfo = _service.GetEnumDetails(enumName);

        // Assert
        Assert.NotNull(enumInfo);
        Assert.Equal(enumName, enumInfo.Name);
        Assert.NotEmpty(enumInfo.Values);
    }

    [Fact]
    public void GetEnumDetails_ReturnsNullForUnknownEnum()
    {
        // Act
        var enumInfo = _service.GetEnumDetails("NonExistentEnum");

        // Assert
        Assert.Null(enumInfo);
    }

    [Fact]
    public void GetEnumsForComponent_FluentButton_ReturnsEnums()
    {
        // Act
        var enums = _service.GetEnumsForComponent("FluentButton");

        // Assert
        Assert.NotEmpty(enums);
    }
}
