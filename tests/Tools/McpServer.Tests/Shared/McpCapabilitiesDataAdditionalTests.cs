// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.McpServer.Services;
using Microsoft.FluentUI.AspNetCore.Components.McpServer.Shared;
using Microsoft.FluentUI.AspNetCore.Components.McpServer.Shared.Models;

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Tests.Shared;

/// <summary>
/// Additional tests for <see cref="McpCapabilitiesData"/> to improve coverage.
/// </summary>
public class McpCapabilitiesDataAdditionalTests
{
    public McpCapabilitiesDataAdditionalTests()
    {
        var mcpServerAssembly = typeof(FluentUIDocumentationService).Assembly;
        McpCapabilitiesData.Initialize(mcpServerAssembly);
    }

    [Fact]
    public void Initialize_WithCustomProvider_Works()
    {
        // Arrange
        var customSummary = new McpSummary(
            new[] { new McpToolInfo("TestTool", "Test", "TestClass", new McpParameterInfo[0]) },
            new[] { new McpPromptInfo("TestPrompt", "Test", "TestClass", new McpParameterInfo[0]) },
            new[] { new McpResourceInfo("test://resource", "test", "Test", "Test", "text/plain", false, "TestClass") }
        );

        // Act
        McpCapabilitiesData.Initialize(() => customSummary);
        var summary = McpCapabilitiesData.GetSummary();

        // Assert
        Assert.Single(summary.Tools);
        Assert.Single(summary.Prompts);
        Assert.Single(summary.Resources);

        // Cleanup - reinitialize with actual assembly
        var mcpServerAssembly = typeof(FluentUIDocumentationService).Assembly;
        McpCapabilitiesData.Initialize(mcpServerAssembly);
    }

    [Fact]
    public void ClearCache_AllowsReinitialization()
    {
        // Act
        var summary1 = McpCapabilitiesData.GetSummary();
        McpCapabilitiesData.ClearCache();
        var summary2 = McpCapabilitiesData.GetSummary();

        // Assert - Should get fresh data
        Assert.Equal(summary1.Tools.Count, summary2.Tools.Count);
    }

    [Fact]
    public void IsInitialized_ReturnsTrueAfterInitialization()
    {
        // Assert
        Assert.True(McpCapabilitiesData.IsInitialized);
    }

    [Fact]
    public void Tools_AreAccessibleMultipleTimes()
    {
        // Act
        var tools1 = McpCapabilitiesData.Tools;
        var tools2 = McpCapabilitiesData.Tools;

        // Assert
        Assert.Equal(tools1.Count, tools2.Count);
    }

    [Fact]
    public void Prompts_AreAccessibleMultipleTimes()
    {
        // Act
        var prompts1 = McpCapabilitiesData.Prompts;
        var prompts2 = McpCapabilitiesData.Prompts;

        // Assert
        Assert.Equal(prompts1.Count, prompts2.Count);
    }

    [Fact]
    public void Resources_AreAccessibleMultipleTimes()
    {
        // Act
        var resources1 = McpCapabilitiesData.Resources;
        var resources2 = McpCapabilitiesData.Resources;

        // Assert
        Assert.Equal(resources1.Count, resources2.Count);
    }
}
