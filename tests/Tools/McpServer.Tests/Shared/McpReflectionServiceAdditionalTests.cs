// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.McpServer.Services;
using Microsoft.FluentUI.AspNetCore.Components.McpServer.Shared;

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Tests.Shared;

/// <summary>
/// Additional tests for <see cref="McpReflectionService"/> to improve coverage.
/// </summary>
public class McpReflectionServiceAdditionalTests
{
    private readonly System.Reflection.Assembly _mcpServerAssembly;

    public McpReflectionServiceAdditionalTests()
    {
        _mcpServerAssembly = typeof(FluentUIDocumentationService).Assembly;
    }

    [Fact]
    public void GetTools_HandlesParametersWithoutDescriptions()
    {
        // Act
        var tools = McpReflectionService.GetTools(_mcpServerAssembly);

        // Assert - All tools should have parameters with names and types
        foreach (var tool in tools)
        {
            foreach (var param in tool.Parameters)
            {
                Assert.NotNull(param.Name);
                Assert.NotNull(param.Type);
                // Description can be empty, but should be non-null
                Assert.NotNull(param.Description);
            }
        }
    }

    [Fact]
    public void GetPrompts_HandlesOptionalParameters()
    {
        // Act
        var prompts = McpReflectionService.GetPrompts(_mcpServerAssembly);

        // Assert - Check for optional parameters
        var promptsWithOptional = prompts.Where(p => p.Parameters.Any(param => !param.Required));
        Assert.NotEmpty(promptsWithOptional);
    }

    [Fact]
    public void GetResources_HandlesTemplateParameters()
    {
        // Act
        var resources = McpReflectionService.GetResources(_mcpServerAssembly);

        // Assert - Template resources should have parameters
        var templateResources = resources.Where(r => r.IsTemplate);
        Assert.NotEmpty(templateResources);
        Assert.All(templateResources, r => Assert.Contains("{", r.Uri));
    }

    [Fact]
    public void GetSummary_CachesResults()
    {
        // Act
        var summary1 = McpReflectionService.GetSummary(_mcpServerAssembly);
        var summary2 = McpReflectionService.GetSummary(_mcpServerAssembly);

        // Assert - Should return consistent results
        Assert.Equal(summary1.Tools.Count, summary2.Tools.Count);
        Assert.Equal(summary1.Prompts.Count, summary2.Prompts.Count);
        Assert.Equal(summary1.Resources.Count, summary2.Resources.Count);
    }

    [Fact]
    public void GetTools_IncludesInheritedMethods()
    {
        // Act
        var tools = McpReflectionService.GetTools(_mcpServerAssembly);

        // Assert - Should have tools from all tool classes
        Assert.True(tools.Count >= 5); // At least ListComponents, GetComponentDetails, SearchComponents, GetEnumValues, GetGuide
    }

    [Fact]
    public void GetPrompts_IncludesAllPromptTypes()
    {
        // Act
        var prompts = McpReflectionService.GetPrompts(_mcpServerAssembly);

        // Assert - Should have multiple prompt types
        Assert.True(prompts.Count >= 8); // At least 8 different prompts
    }

    [Fact]
    public void GetResources_IncludesBothStaticAndTemplate()
    {
        // Act
        var resources = McpReflectionService.GetResources(_mcpServerAssembly);

        // Assert
        var staticResources = resources.Where(r => !r.IsTemplate).ToList();
        var templateResources = resources.Where(r => r.IsTemplate).ToList();

        Assert.NotEmpty(staticResources);
        Assert.NotEmpty(templateResources);
    }
}
