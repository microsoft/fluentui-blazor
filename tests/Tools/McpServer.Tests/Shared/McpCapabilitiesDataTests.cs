// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.McpServer.Services;
using Microsoft.FluentUI.AspNetCore.Components.McpServer.Shared;

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Tests.Shared;

/// <summary>
/// Tests for <see cref="McpCapabilitiesData"/>.
/// </summary>
public class McpCapabilitiesDataTests
{
    public McpCapabilitiesDataTests()
    {
        // Initialize with the MCP Server assembly
        var mcpServerAssembly = typeof(FluentUIDocumentationService).Assembly;
        McpCapabilitiesData.Initialize(mcpServerAssembly);
    }

    [Fact]
    public void Tools_ReturnsNonEmptyList()
    {
        // Act
        var tools = McpCapabilitiesData.Tools;

        // Assert
        Assert.NotEmpty(tools);
    }

    [Fact]
    public void Tools_ContainsExpectedTools()
    {
        // Act
        var tools = McpCapabilitiesData.Tools;

        // Assert
        Assert.Contains(tools, t => t.Name == "ListComponents");
        Assert.Contains(tools, t => t.Name == "GetComponentDetails");
        Assert.Contains(tools, t => t.Name == "SearchComponents");
        Assert.Contains(tools, t => t.Name == "GetEnumValues");
        Assert.Contains(tools, t => t.Name == "GetGuide");
    }

    [Fact]
    public void Prompts_ReturnsNonEmptyList()
    {
        // Act
        var prompts = McpCapabilitiesData.Prompts;

        // Assert
        Assert.NotEmpty(prompts);
    }

    [Fact]
    public void Prompts_ContainsExpectedPrompts()
    {
        // Act
        var prompts = McpCapabilitiesData.Prompts;

        // Assert
        Assert.Contains(prompts, p => p.Name == "create_component");
        Assert.Contains(prompts, p => p.Name == "create_form");
        Assert.Contains(prompts, p => p.Name == "migrate_to_v5");
        Assert.Contains(prompts, p => p.Name == "setup_project");
    }

    [Fact]
    public void Resources_ReturnsNonEmptyList()
    {
        // Act
        var resources = McpCapabilitiesData.Resources;

        // Assert
        Assert.NotEmpty(resources);
    }

    [Fact]
    public void Resources_ContainsStaticResources()
    {
        // Act
        var resources = McpCapabilitiesData.Resources;

        // Assert
        Assert.Contains(resources, r => r.Uri == "fluentui://components" && !r.IsTemplate);
        Assert.Contains(resources, r => r.Uri == "fluentui://categories" && !r.IsTemplate);
        Assert.Contains(resources, r => r.Uri == "fluentui://enums" && !r.IsTemplate);
    }

    [Fact]
    public void Resources_ContainsTemplateResources()
    {
        // Act
        var resources = McpCapabilitiesData.Resources;

        // Assert
        Assert.Contains(resources, r => r.Uri == "fluentui://component/{name}" && r.IsTemplate);
        Assert.Contains(resources, r => r.Uri == "fluentui://category/{name}" && r.IsTemplate);
        Assert.Contains(resources, r => r.Uri == "fluentui://enum/{name}" && r.IsTemplate);
    }

    [Fact]
    public void GetSummary_ReturnsSummaryWithAllData()
    {
        // Act
        var summary = McpCapabilitiesData.GetSummary();

        // Assert
        Assert.NotNull(summary);
        Assert.Equal(McpCapabilitiesData.Tools.Count, summary.Tools.Count);
        Assert.Equal(McpCapabilitiesData.Prompts.Count, summary.Prompts.Count);
        Assert.Equal(McpCapabilitiesData.Resources.Count, summary.Resources.Count);
    }

    [Fact]
    public void Tools_HaveDescriptions()
    {
        // Act
        var tools = McpCapabilitiesData.Tools;

        // Assert
        Assert.All(tools, t => Assert.NotEmpty(t.Description));
    }

    [Fact]
    public void Prompts_HaveDescriptions()
    {
        // Act
        var prompts = McpCapabilitiesData.Prompts;

        // Assert
        Assert.All(prompts, p => Assert.NotEmpty(p.Description));
    }

    [Fact]
    public void Resources_HaveDescriptions()
    {
        // Act
        var resources = McpCapabilitiesData.Resources;

        // Assert
        Assert.All(resources, r => Assert.NotEmpty(r.Description));
    }

    [Fact]
    public void Tools_HaveValidParameters()
    {
        // Act
        var tools = McpCapabilitiesData.Tools;

        // Assert
        foreach (var tool in tools)
        {
            foreach (var param in tool.Parameters)
            {
                Assert.NotEmpty(param.Name);
                Assert.NotEmpty(param.Type);
            }
        }
    }

    [Fact]
    public void Prompts_HaveValidParameters()
    {
        // Act
        var prompts = McpCapabilitiesData.Prompts;

        // Assert
        foreach (var prompt in prompts)
        {
            foreach (var param in prompt.Parameters)
            {
                Assert.NotEmpty(param.Name);
                Assert.NotEmpty(param.Type);
            }
        }
    }

    [Fact]
    public void IsInitialized_ReturnsTrue()
    {
        // Assert
        Assert.True(McpCapabilitiesData.IsInitialized);
    }

    [Fact]
    public void ClearCache_ClearsCache()
    {
        // Act
        McpCapabilitiesData.ClearCache();
        var summary = McpCapabilitiesData.GetSummary();

        // Assert
        Assert.NotNull(summary);
    }
}
