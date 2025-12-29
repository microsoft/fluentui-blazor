// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using FluentUI.Demo.DocApiGen.Models.McpMode;
using Xunit;

namespace FluentUI.Demo.DocApiGen.Tests.Models.McpMode;

/// <summary>
/// Unit tests for <see cref="McpToolInfo"/>.
/// </summary>
public class McpToolInfoTests
{
    [Fact]
    public void Constructor_ShouldRequireNameAndClassName()
    {
        // Arrange & Act
        var tool = new McpToolInfo
        {
            Name = "ListComponents",
            ClassName = "ComponentListTools"
        };

        // Assert
        Assert.Equal("ListComponents", tool.Name);
        Assert.Equal("ComponentListTools", tool.ClassName);
    }

    [Fact]
    public void OptionalProperties_ShouldBeNullByDefault()
    {
        // Arrange & Act
        var tool = new McpToolInfo
        {
            Name = "TestTool",
            ClassName = "TestClass"
        };

        // Assert
        Assert.Null(tool.Description);
        Assert.Null(tool.Summary);
        Assert.Null(tool.ReturnType);
        Assert.Null(tool.Parameters);
    }

    [Fact]
    public void Description_ShouldBeSettable()
    {
        // Arrange & Act
        var tool = new McpToolInfo
        {
            Name = "ListComponents",
            ClassName = "ComponentListTools",
            Description = "Lists all available components"
        };

        // Assert
        Assert.Equal("Lists all available components", tool.Description);
    }

    [Fact]
    public void Summary_ShouldBeSettable()
    {
        // Arrange & Act
        var tool = new McpToolInfo
        {
            Name = "ListComponents",
            ClassName = "ComponentListTools",
            Summary = "XML documentation summary"
        };

        // Assert
        Assert.Equal("XML documentation summary", tool.Summary);
    }

    [Fact]
    public void ReturnType_ShouldBeSettable()
    {
        // Arrange & Act
        var tool = new McpToolInfo
        {
            Name = "ListComponents",
            ClassName = "ComponentListTools",
            ReturnType = "string"
        };

        // Assert
        Assert.Equal("string", tool.ReturnType);
    }

    [Fact]
    public void Parameters_ShouldBeSettable()
    {
        // Arrange
        var parameters = new List<McpParameterInfo>
        {
            new()
            {
                Name = "category",
                Type = "string?",
                Description = "Filter by category",
                IsRequired = false
            }
        };

        // Act
        var tool = new McpToolInfo
        {
            Name = "ListComponents",
            ClassName = "ComponentListTools",
            Parameters = parameters
        };

        // Assert
        Assert.NotNull(tool.Parameters);
        Assert.Single(tool.Parameters);
        Assert.Equal("category", tool.Parameters[0].Name);
    }

    [Fact]
    public void CompleteObject_ShouldBeConstructedProperly()
    {
        // Arrange & Act
        var tool = new McpToolInfo
        {
            Name = "GetComponentDetails",
            ClassName = "ComponentDetailTools",
            Description = "Gets detailed documentation for a component",
            Summary = "Returns full component documentation including parameters, events, and methods.",
            ReturnType = "string",
            Parameters =
            [
                new McpParameterInfo
                {
                    Name = "componentName",
                    Type = "string",
                    Description = "The name of the component",
                    IsRequired = true
                }
            ]
        };

        // Assert
        Assert.Equal("GetComponentDetails", tool.Name);
        Assert.Equal("ComponentDetailTools", tool.ClassName);
        Assert.Equal("Gets detailed documentation for a component", tool.Description);
        Assert.NotNull(tool.Summary);
        Assert.Equal("string", tool.ReturnType);
        Assert.Single(tool.Parameters!);
        Assert.True(tool.Parameters![0].IsRequired);
    }
}
