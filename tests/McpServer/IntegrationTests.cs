// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.McpServer.Resources;
using Microsoft.FluentUI.AspNetCore.McpServer.Services;
using Microsoft.FluentUI.AspNetCore.McpServer.Tools;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tests;

/// <summary>
/// Integration tests that verify the complete workflow of the MCP server components.
/// </summary>
public class IntegrationTests
{
    private readonly string _testJsonPath;
    private readonly bool _jsonFileExists;

    public IntegrationTests()
    {
        _testJsonPath = Path.Combine(AppContext.BaseDirectory, "FluentUIComponentsDocumentation.json");
        _jsonFileExists = File.Exists(_testJsonPath);
    }

    [Fact]
    public void FullWorkflow_ShouldWorkEndToEnd()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var service = new FluentUIDocumentationService(_testJsonPath);
        var componentDocService = new ComponentDocumentationService();
        var listTools = new ComponentListTools(service, componentDocService);
        var detailTools = new ComponentDetailTools(service, componentDocService);
        var enumTools = new EnumTools(service);
        var fluentUIResources = new FluentUIResources(service);
        var componentResources = new ComponentResources(service, componentDocService);

        // Act & Assert - List all components
        var allComponents = listTools.ListComponents();
        Assert.Contains("FluentButton", allComponents, StringComparison.OrdinalIgnoreCase);

        // Act & Assert - Search for a component
        var searchResults = listTools.SearchComponents("Button");
        Assert.Contains("FluentButton", searchResults, StringComparison.OrdinalIgnoreCase);

        // Act & Assert - Get component details
        var details = detailTools.GetComponentDetails("FluentButton");
        Assert.Contains("# FluentButton", details, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("## Parameters", details, StringComparison.OrdinalIgnoreCase);

        // Act & Assert - Get enum values
        var enumValues = enumTools.GetEnumValues("Appearance");
        Assert.Contains("# Appearance", enumValues, StringComparison.OrdinalIgnoreCase);

        // Act & Assert - Get component enums
        var componentEnums = enumTools.GetComponentEnums("FluentButton");
        Assert.Contains("Enum Types for", componentEnums, StringComparison.OrdinalIgnoreCase);

        // Act & Assert - Resources
        var allComponentsResource = fluentUIResources.GetAllComponents();
        Assert.Contains("# Fluent UI Blazor Components", allComponentsResource, StringComparison.OrdinalIgnoreCase);

        var allEnumsResource = fluentUIResources.GetAllEnums();
        Assert.Contains("# Fluent UI Blazor Enum Types", allEnumsResource, StringComparison.OrdinalIgnoreCase);

        var componentResource = componentResources.GetComponent("FluentButton");
        Assert.Contains("# FluentButton", componentResource, StringComparison.OrdinalIgnoreCase);

        var categoryResource = componentResources.GetCategory("Components");
        Assert.Contains("Components", categoryResource, StringComparison.OrdinalIgnoreCase);

        var enumResource = componentResources.GetEnum("Appearance");
        Assert.Contains("# Appearance", enumResource, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void ServiceConsistency_AllComponentsShouldHaveDetails()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var service = new FluentUIDocumentationService(_testJsonPath);
        var componentDocService = new ComponentDocumentationService();
        var detailTools = new ComponentDetailTools(service, componentDocService);

        // Act
        var allComponents = service.GetAllComponents();

        // Assert
        foreach (var component in allComponents.Take(10)) // Test first 10 for speed
        {
            var details = detailTools.GetComponentDetails(component.Name);
            Assert.Contains($"# {component.Name}", details, StringComparison.OrdinalIgnoreCase);
        }
    }

    [Fact]
    public void ServiceConsistency_AllEnumsShouldHaveDetails()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var service = new FluentUIDocumentationService(_testJsonPath);
        var enumTools = new EnumTools(service);

        // Act
        var allEnums = service.GetAllEnums();

        // Assert
        foreach (var enumInfo in allEnums.Take(10)) // Test first 10 for speed
        {
            var details = enumTools.GetEnumValues(enumInfo.Name);
            Assert.Contains($"# {enumInfo.Name}", details, StringComparison.OrdinalIgnoreCase);
        }
    }

    [Fact]
    public void SearchAndDetails_ShouldBeConsistent()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var service = new FluentUIDocumentationService(_testJsonPath);
        var componentDocService = new ComponentDocumentationService();
        var listTools = new ComponentListTools(service, componentDocService);
        var detailTools = new ComponentDetailTools(service, componentDocService);

        // Act
        _ = listTools.SearchComponents("Button");

        // Assert - All search results should have valid details
        var components = service.SearchComponents("Button");
        foreach (var details in components.Select(component => detailTools.GetComponentDetails(component.Name)))
        {
            Assert.DoesNotContain("not found", details, StringComparison.OrdinalIgnoreCase);
        }
    }

    [Fact]
    public void CategoryFilter_ShouldReturnValidComponents()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var service = new FluentUIDocumentationService(_testJsonPath);
        var componentDocService = new ComponentDocumentationService();
        var listTools = new ComponentListTools(service, componentDocService);
        var detailTools = new ComponentDetailTools(service, componentDocService);

        // Act
        _ = listTools.ListComponents(category: "Components");
        var components = service.GetComponentsByCategory("Components");

        // Assert
        foreach (var component in components.Take(5))
        {
            var details = detailTools.GetComponentDetails(component.Name);
            Assert.Contains($"**Category:** {component.Category}", details, StringComparison.OrdinalIgnoreCase);
        }
    }

    [Fact]
    public void EnumsForComponent_ShouldHaveValidEnumDetails()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var service = new FluentUIDocumentationService(_testJsonPath);
        var enumTools = new EnumTools(service);

        // Act
        var componentEnums = service.GetEnumsForComponent("FluentButton");

        // Assert
        foreach (var kvp in componentEnums)
        {
            var enumDetails = enumTools.GetEnumValues(kvp.Value.Name);
            Assert.Contains($"# {kvp.Value.Name}", enumDetails, StringComparison.OrdinalIgnoreCase);
        }
    }

    [Fact]
    public void MultipleServicesFromSameFile_ShouldBeConsistent()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var service1 = new FluentUIDocumentationService(_testJsonPath);
        var service2 = new FluentUIDocumentationService(_testJsonPath);

        // Act
        var components1 = service1.GetAllComponents();
        var components2 = service2.GetAllComponents();

        // Assert
        Assert.Equal(components1.Count, components2.Count);
        Assert.Equal(
            components1.Select(c => c.Name).Order(StringComparer.OrdinalIgnoreCase),
            components2.Select(c => c.Name).Order(StringComparer.OrdinalIgnoreCase));
    }
}
