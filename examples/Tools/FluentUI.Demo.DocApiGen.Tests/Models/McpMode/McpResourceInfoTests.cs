// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using FluentUI.Demo.DocApiGen.Models.McpMode;
using Xunit;

namespace FluentUI.Demo.DocApiGen.Tests.Models.McpMode;

/// <summary>
/// Unit tests for <see cref="McpResourceInfo"/>.
/// </summary>
public class McpResourceInfoTests
{
    [Fact]
    public void Constructor_ShouldRequireNameClassNameMethodNameAndUriTemplate()
    {
        // Arrange & Act
        var resource = new McpResourceInfo
        {
            Name = "components",
            ClassName = "FluentUIResources",
            MethodName = "GetAllComponents",
            UriTemplate = "fluentui://components"
        };

        // Assert
        Assert.Equal("components", resource.Name);
        Assert.Equal("FluentUIResources", resource.ClassName);
        Assert.Equal("GetAllComponents", resource.MethodName);
        Assert.Equal("fluentui://components", resource.UriTemplate);
    }

    [Fact]
    public void OptionalProperties_ShouldBeNullByDefault()
    {
        // Arrange & Act
        var resource = new McpResourceInfo
        {
            Name = "test",
            ClassName = "TestClass",
            MethodName = "TestMethod",
            UriTemplate = "test://uri"
        };

        // Assert
        Assert.Null(resource.Title);
        Assert.Null(resource.MimeType);
        Assert.Null(resource.Description);
        Assert.Null(resource.Summary);
        Assert.Null(resource.Parameters);
    }

    [Fact]
    public void Title_ShouldBeSettable()
    {
        // Arrange & Act
        var resource = new McpResourceInfo
        {
            Name = "components",
            ClassName = "FluentUIResources",
            MethodName = "GetAllComponents",
            UriTemplate = "fluentui://components",
            Title = "All Fluent UI Blazor Components"
        };

        // Assert
        Assert.Equal("All Fluent UI Blazor Components", resource.Title);
    }

    [Fact]
    public void MimeType_ShouldBeSettable()
    {
        // Arrange & Act
        var resource = new McpResourceInfo
        {
            Name = "components",
            ClassName = "FluentUIResources",
            MethodName = "GetAllComponents",
            UriTemplate = "fluentui://components",
            MimeType = "text/markdown"
        };

        // Assert
        Assert.Equal("text/markdown", resource.MimeType);
    }

    [Fact]
    public void TemplatedResource_ShouldHaveParameters()
    {
        // Arrange
        var parameters = new List<McpParameterInfo>
        {
            new()
            {
                Name = "name",
                Type = "string",
                Description = "The component name",
                IsRequired = true
            }
        };

        // Act
        var resource = new McpResourceInfo
        {
            Name = "component",
            ClassName = "ComponentResources",
            MethodName = "GetComponent",
            UriTemplate = "fluentui://component/{name}",
            Parameters = parameters
        };

        // Assert
        Assert.NotNull(resource.Parameters);
        Assert.Single(resource.Parameters);
        Assert.Equal("name", resource.Parameters[0].Name);
    }

    [Fact]
    public void CompleteStaticResource_ShouldBeConstructedProperly()
    {
        // Arrange & Act
        var resource = new McpResourceInfo
        {
            Name = "components",
            ClassName = "FluentUIResources",
            MethodName = "GetAllComponents",
            UriTemplate = "fluentui://components",
            Title = "All Fluent UI Blazor Components",
            MimeType = "text/markdown",
            Description = "Complete list of all available Fluent UI Blazor components",
            Summary = "Gets the complete list of all Fluent UI Blazor components."
        };

        // Assert
        Assert.Equal("components", resource.Name);
        Assert.Equal("FluentUIResources", resource.ClassName);
        Assert.Equal("GetAllComponents", resource.MethodName);
        Assert.Equal("fluentui://components", resource.UriTemplate);
        Assert.Equal("All Fluent UI Blazor Components", resource.Title);
        Assert.Equal("text/markdown", resource.MimeType);
        Assert.Equal("Complete list of all available Fluent UI Blazor components", resource.Description);
        Assert.NotNull(resource.Summary);
        Assert.Null(resource.Parameters);
    }

    [Fact]
    public void CompleteTemplatedResource_ShouldBeConstructedProperly()
    {
        // Arrange & Act
        var resource = new McpResourceInfo
        {
            Name = "component",
            ClassName = "ComponentResources",
            MethodName = "GetComponent",
            UriTemplate = "fluentui://component/{name}",
            Title = "Component Documentation",
            MimeType = "text/markdown",
            Description = "Detailed documentation for a specific component",
            Summary = "Gets detailed documentation for a specific component.",
            Parameters =
            [
                new McpParameterInfo
                {
                    Name = "name",
                    Type = "string",
                    Description = "The component name",
                    IsRequired = true
                }
            ]
        };

        // Assert
        Assert.Equal("component", resource.Name);
        Assert.Equal("fluentui://component/{name}", resource.UriTemplate);
        Assert.NotNull(resource.Parameters);
        Assert.Single(resource.Parameters);
    }
}
