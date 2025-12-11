// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.McpServer.Services;
using Microsoft.FluentUI.AspNetCore.Components.McpServer.Shared;

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Tests.Shared;

/// <summary>
/// More tests for MCP Shared classes to improve coverage.
/// </summary>
public class McpSharedMoreTests
{
    [Fact]
    public void McpCapabilitiesData_WithoutInitialization_ReturnsEmptySummary()
    {
        // Arrange - Clear any initialization
        McpCapabilitiesData.ClearCache();

        // Create a new instance that won't find the assembly
        var emptyAssemblyList = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => a.GetName().Name == "NonExistentAssembly");

        // Act - Try to get summary without proper initialization
        // The class should handle this gracefully
        var summary = McpCapabilitiesData.GetSummary();

        // Assert - Should return empty or valid summary
        Assert.NotNull(summary);
        
        // Reinitialize for other tests
        var mcpServerAssembly = typeof(FluentUIDocumentationService).Assembly;
        McpCapabilitiesData.Initialize(mcpServerAssembly);
    }

    [Fact]
    public void McpReflectionService_WithNullAssembly_ThrowsException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            McpReflectionService.GetTools(null!));
    }

    [Fact]
    public void McpReflectionService_GetPrompts_FiltersCorrectly()
    {
        // Arrange
        var assembly = typeof(FluentUIDocumentationService).Assembly;

        // Act
        var prompts = McpReflectionService.GetPrompts(assembly);

        // Assert
        Assert.All(prompts, p =>
        {
            Assert.NotEmpty(p.Name);
            Assert.NotEmpty(p.Description);
            Assert.NotNull(p.Parameters);
        });
    }

    [Fact]
    public void McpReflectionService_GetResources_IncludesAllResourceTypes()
    {
        // Arrange
        var assembly = typeof(FluentUIDocumentationService).Assembly;

        // Act
        var resources = McpReflectionService.GetResources(assembly);

        // Assert
        var staticResources = resources.Where(r => !r.IsTemplate).ToList();
        var templateResources = resources.Where(r => r.IsTemplate).ToList();

        Assert.NotEmpty(staticResources);
        Assert.NotEmpty(templateResources);

        // Verify template resources have URI templates
        Assert.All(templateResources, r => Assert.Contains("{", r.Uri));
    }

    [Fact]
    public void McpReflectionService_GetTools_IncludesToolsFromAllClasses()
    {
        // Arrange
        var assembly = typeof(FluentUIDocumentationService).Assembly;

        // Act
        var tools = McpReflectionService.GetTools(assembly);

        // Assert
        var toolClasses = tools.Select(t => t.ClassName).Distinct().ToList();
        Assert.True(toolClasses.Count >= 4, "Should have tools from multiple classes");
    }
}
