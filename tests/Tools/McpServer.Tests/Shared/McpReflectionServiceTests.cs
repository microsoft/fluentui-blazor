// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Reflection;
using Microsoft.FluentUI.AspNetCore.Components.McpServer.Shared;
using Microsoft.FluentUI.AspNetCore.Components.McpServer.Services;

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Tests.Shared;

/// <summary>
/// Tests for <see cref="McpReflectionService"/>.
/// </summary>
public class McpReflectionServiceTests
{
    private readonly Assembly _mcpServerAssembly;

    public McpReflectionServiceTests()
    {
        // Get the MCP Server assembly using the service type
        _mcpServerAssembly = typeof(FluentUIDocumentationService).Assembly;
    }

    [Fact]
    public void GetTools_ReturnsNonEmptyList()
    {
        // Act
        var tools = McpReflectionService.GetTools(_mcpServerAssembly);

        // Assert
        Assert.NotEmpty(tools);
    }

    [Fact]
    public void GetTools_ContainsListComponents()
    {
        // Act
        var tools = McpReflectionService.GetTools(_mcpServerAssembly);

        // Assert
        Assert.Contains(tools, t => t.Name == "ListComponents");
    }

    [Fact]
    public void GetTools_ContainsGetComponentDetails()
    {
        // Act
        var tools = McpReflectionService.GetTools(_mcpServerAssembly);

        // Assert
        Assert.Contains(tools, t => t.Name == "GetComponentDetails");
    }

    [Fact]
    public void GetTools_ToolsHaveDescriptions()
    {
        // Act
        var tools = McpReflectionService.GetTools(_mcpServerAssembly);

        // Assert
        Assert.All(tools, t => Assert.NotEmpty(t.Description));
    }

    [Fact]
    public void GetPrompts_ReturnsNonEmptyList()
    {
        // Act
        var prompts = McpReflectionService.GetPrompts(_mcpServerAssembly);

        // Assert
        Assert.NotEmpty(prompts);
    }

    [Fact]
    public void GetPrompts_ContainsCreateComponent()
    {
        // Act
        var prompts = McpReflectionService.GetPrompts(_mcpServerAssembly);

        // Assert
        Assert.Contains(prompts, p => p.Name == "create_component");
    }

    [Fact]
    public void GetPrompts_ContainsMigrateToV5()
    {
        // Act
        var prompts = McpReflectionService.GetPrompts(_mcpServerAssembly);

        // Assert
        Assert.Contains(prompts, p => p.Name == "migrate_to_v5");
    }

    [Fact]
    public void GetPrompts_PromptsHaveDescriptions()
    {
        // Act
        var prompts = McpReflectionService.GetPrompts(_mcpServerAssembly);

        // Assert
        Assert.All(prompts, p => Assert.NotEmpty(p.Description));
    }

    [Fact]
    public void GetResources_ReturnsNonEmptyList()
    {
        // Act
        var resources = McpReflectionService.GetResources(_mcpServerAssembly);

        // Assert
        Assert.NotEmpty(resources);
    }

    [Fact]
    public void GetResources_ContainsComponentsResource()
    {
        // Act
        var resources = McpReflectionService.GetResources(_mcpServerAssembly);

        // Assert
        Assert.Contains(resources, r => r.Uri == "fluentui://components");
    }

    [Fact]
    public void GetResources_ResourcesHaveDescriptions()
    {
        // Act
        var resources = McpReflectionService.GetResources(_mcpServerAssembly);

        // Assert
        Assert.All(resources, r => Assert.NotEmpty(r.Description));
    }

    [Fact]
    public void GetSummary_ReturnsCompleteSummary()
    {
        // Act
        var summary = McpReflectionService.GetSummary(_mcpServerAssembly);

        // Assert
        Assert.NotNull(summary);
        Assert.NotEmpty(summary.Tools);
        Assert.NotEmpty(summary.Prompts);
        Assert.NotEmpty(summary.Resources);
    }

    [Fact]
    public void GetTools_ParametersHaveCorrectTypes()
    {
        // Act
        var tools = McpReflectionService.GetTools(_mcpServerAssembly);
        var listComponentsTool = tools.FirstOrDefault(t => t.Name == "ListComponents");

        // Assert
        Assert.NotNull(listComponentsTool);
        var categoryParam = listComponentsTool.Parameters.FirstOrDefault(p => p.Name == "category");
        Assert.NotNull(categoryParam);
        // Note: Nullable reference types (string?) are represented as "string" at runtime
        Assert.Equal("string", categoryParam.Type);
        Assert.False(categoryParam.Required);
    }

    [Fact]
    public void GetPrompts_RequiredParametersMarkedCorrectly()
    {
        // Act
        var prompts = McpReflectionService.GetPrompts(_mcpServerAssembly);
        var createComponentPrompt = prompts.FirstOrDefault(p => p.Name == "create_component");

        // Assert
        Assert.NotNull(createComponentPrompt);
        var componentNameParam = createComponentPrompt.Parameters.FirstOrDefault(p => p.Name == "componentName");
        Assert.NotNull(componentNameParam);
        Assert.True(componentNameParam.Required);
    }

    [Fact]
    public void GetResources_TemplatesIdentifiedCorrectly()
    {
        // Act
        var resources = McpReflectionService.GetResources(_mcpServerAssembly);

        // Assert
        var staticResources = resources.Where(r => !r.IsTemplate);
        var templateResources = resources.Where(r => r.IsTemplate);

        Assert.NotEmpty(staticResources);
        Assert.NotEmpty(templateResources);

        // Static resources should not contain {
        Assert.All(staticResources, r => Assert.DoesNotContain("{", r.Uri));

        // Template resources should contain {
        Assert.All(templateResources, r => Assert.Contains("{", r.Uri));
    }
}
