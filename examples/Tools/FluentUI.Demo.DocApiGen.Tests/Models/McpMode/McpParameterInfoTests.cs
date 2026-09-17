// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using FluentUI.Demo.DocApiGen.Models.McpMode;
using Xunit;

namespace FluentUI.Demo.DocApiGen.Tests.Models.McpMode;

/// <summary>
/// Unit tests for <see cref="McpParameterInfo"/>.
/// </summary>
public class McpParameterInfoTests
{
    [Fact]
    public void Constructor_ShouldRequireNameAndType()
    {
        // Arrange & Act
        var param = new McpParameterInfo
        {
            Name = "componentName",
            Type = "string"
        };

        // Assert
        Assert.Equal("componentName", param.Name);
        Assert.Equal("string", param.Type);
    }

    [Fact]
    public void IsRequired_ShouldDefaultToFalse()
    {
        // Arrange & Act
        var param = new McpParameterInfo
        {
            Name = "test",
            Type = "string"
        };

        // Assert
        Assert.False(param.IsRequired);
    }

    [Fact]
    public void IsRequired_ShouldBeSettable()
    {
        // Arrange & Act
        var param = new McpParameterInfo
        {
            Name = "componentName",
            Type = "string",
            IsRequired = true
        };

        // Assert
        Assert.True(param.IsRequired);
    }

    [Fact]
    public void Description_ShouldBeSettable()
    {
        // Arrange & Act
        var param = new McpParameterInfo
        {
            Name = "category",
            Type = "string?",
            Description = "Filter components by category"
        };

        // Assert
        Assert.Equal("Filter components by category", param.Description);
    }

    [Fact]
    public void DefaultValue_ShouldBeNullByDefault()
    {
        // Arrange & Act
        var param = new McpParameterInfo
        {
            Name = "test",
            Type = "string"
        };

        // Assert
        Assert.Null(param.DefaultValue);
    }

    [Fact]
    public void DefaultValue_ShouldBeSettable()
    {
        // Arrange & Act
        var param = new McpParameterInfo
        {
            Name = "category",
            Type = "string?",
            DefaultValue = "null"
        };

        // Assert
        Assert.Equal("null", param.DefaultValue);
    }

    [Fact]
    public void CompleteOptionalParameter_ShouldBeConstructedProperly()
    {
        // Arrange & Act
        var param = new McpParameterInfo
        {
            Name = "filter",
            Type = "string?",
            Description = "Optional filter term",
            IsRequired = false,
            DefaultValue = "null"
        };

        // Assert
        Assert.Equal("filter", param.Name);
        Assert.Equal("string?", param.Type);
        Assert.Equal("Optional filter term", param.Description);
        Assert.False(param.IsRequired);
        Assert.Equal("null", param.DefaultValue);
    }

    [Fact]
    public void CompleteRequiredParameter_ShouldBeConstructedProperly()
    {
        // Arrange & Act
        var param = new McpParameterInfo
        {
            Name = "componentName",
            Type = "string",
            Description = "The name of the component",
            IsRequired = true,
            DefaultValue = null
        };

        // Assert
        Assert.Equal("componentName", param.Name);
        Assert.Equal("string", param.Type);
        Assert.Equal("The name of the component", param.Description);
        Assert.True(param.IsRequired);
        Assert.Null(param.DefaultValue);
    }
}
