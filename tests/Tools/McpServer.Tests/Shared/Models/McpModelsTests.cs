// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.McpServer.Shared.Models;

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Tests.Shared.Models;

/// <summary>
/// Tests for MCP model classes to improve coverage.
/// </summary>
public class McpModelsTests
{
    [Fact]
    public void McpToolInfo_AllPropertiesWork()
    {
        // Arrange & Act
        var tool = new McpToolInfo(
            "TestTool",
            "Test Description",
            "TestClass",
            [
                new McpParameterInfo("param1", "string", "Description", true)
            ]);

        // Assert
        Assert.Equal("TestTool", tool.Name);
        Assert.Equal("Test Description", tool.Description);
        Assert.Equal("TestClass", tool.ClassName);
        Assert.Single(tool.Parameters);
    }

    [Fact]
    public void McpPromptInfo_AllPropertiesWork()
    {
        // Arrange & Act
        var prompt = new McpPromptInfo(
            "TestPrompt",
            "Test Description",
            "TestClass",
            [
                new McpParameterInfo("param1", "string", "Description", false)
            ]);

        // Assert
        Assert.Equal("TestPrompt", prompt.Name);
        Assert.Equal("Test Description", prompt.Description);
        Assert.Single(prompt.Parameters);
    }

    [Fact]
    public void McpResourceInfo_AllPropertiesWork()
    {
        // Arrange & Act
        var resource = new McpResourceInfo(
            "test://resource/{id}",
            "test-resource",
            "Test Resource",
            "Test Description",
            "text/plain",
            true,
            "TestClass");

        // Assert
        Assert.Equal("test://resource/{id}", resource.Uri);
        Assert.Equal("Test Resource", resource.Title);
        Assert.Equal("Test Description", resource.Description);
        Assert.True(resource.IsTemplate);
    }

    [Fact]
    public void McpParameterInfo_AllPropertiesWork()
    {
        // Arrange & Act
        var param = new McpParameterInfo(
            "testParam",
            "string",
            "Test parameter description",
            true);

        // Assert
        Assert.Equal("testParam", param.Name);
        Assert.Equal("string", param.Type);
        Assert.Equal("Test parameter description", param.Description);
        Assert.True(param.Required);
    }

    [Fact]
    public void McpSummary_AllPropertiesWork()
    {
        // Arrange
        var tools = new[] { new McpToolInfo("Tool1", "Desc", "Class1", []) };
        var prompts = new[] { new McpPromptInfo("Prompt1", "Desc", "Class1", []) };
        var resources = new[] { new McpResourceInfo("uri://test", "test", "Title", "Desc", "text/plain", false, "Class1") };

        // Act
        var summary = new McpSummary(tools, prompts, resources);

        // Assert
        Assert.Single(summary.Tools);
        Assert.Single(summary.Prompts);
        Assert.Single(summary.Resources);
    }

    [Fact]
    public void McpToolInfo_WithEmptyParameters_Works()
    {
        // Act
        var tool = new McpToolInfo("TestTool", "Description", "TestClass", []);

        // Assert
        Assert.Empty(tool.Parameters);
    }

    [Fact]
    public void McpPromptInfo_WithMultipleParameters_Works()
    {
        // Arrange
        var parameters = new[]
        {
            new McpParameterInfo("param1", "string", "Desc1", true),
            new McpParameterInfo("param2", "int", "Desc2", false)
        };

        // Act
        var prompt = new McpPromptInfo("TestPrompt", "Description", "TestClass", parameters);

        // Assert
        Assert.Equal(2, prompt.Parameters.Count);
    }

    [Fact]
    public void McpResourceInfo_NonTemplate_WorksCorrectly()
    {
        // Act
        var resource = new McpResourceInfo(
            "test://static/resource",
            "test-static",
            "Static Resource",
            "A static resource",
            "text/plain",
            false,
            "TestClass");

        // Assert
        Assert.False(resource.IsTemplate);
        Assert.DoesNotContain("{", resource.Uri);
    }
}
