// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using FluentUI.Demo.DocApiGen.Models.McpMode;
using Xunit;

namespace FluentUI.Demo.DocApiGen.Tests.Models.McpMode;

/// <summary>
/// Unit tests for <see cref="McpDocumentationRoot"/>.
/// </summary>
public class McpDocumentationRootTests
{
    [Fact]
    public void Constructor_ShouldInitializeCollections()
    {
        // Arrange & Act
        var root = new McpDocumentationRoot
        {
            Metadata = new McpDocumentationMetadata
            {
                AssemblyVersion = "1.0.0",
                GeneratedDateUtc = "2024-01-01 00:00"
            }
        };

        // Assert
        Assert.NotNull(root.Tools);
        Assert.NotNull(root.Resources);
        Assert.NotNull(root.Prompts);
        Assert.Empty(root.Tools);
        Assert.Empty(root.Resources);
        Assert.Empty(root.Prompts);
    }

    [Fact]
    public void Metadata_ShouldBeRequired()
    {
        // Arrange & Act
        var root = new McpDocumentationRoot
        {
            Metadata = new McpDocumentationMetadata
            {
                AssemblyVersion = "5.0.0+abc123",
                GeneratedDateUtc = "2024-06-15 12:30"
            }
        };

        // Assert
        Assert.NotNull(root.Metadata);
        Assert.Equal("5.0.0+abc123", root.Metadata.AssemblyVersion);
        Assert.Equal("2024-06-15 12:30", root.Metadata.GeneratedDateUtc);
    }

    [Fact]
    public void Tools_ShouldBeAddable()
    {
        // Arrange
        var root = new McpDocumentationRoot
        {
            Metadata = new McpDocumentationMetadata
            {
                AssemblyVersion = "1.0.0",
                GeneratedDateUtc = "2024-01-01 00:00"
            }
        };

        // Act
        root.Tools.Add(new McpToolInfo
        {
            Name = "ListComponents",
            ClassName = "ComponentListTools"
        });

        // Assert
        Assert.Single(root.Tools);
        Assert.Equal("ListComponents", root.Tools[0].Name);
    }

    [Fact]
    public void Resources_ShouldBeAddable()
    {
        // Arrange
        var root = new McpDocumentationRoot
        {
            Metadata = new McpDocumentationMetadata
            {
                AssemblyVersion = "1.0.0",
                GeneratedDateUtc = "2024-01-01 00:00"
            }
        };

        // Act
        root.Resources.Add(new McpResourceInfo
        {
            Name = "components",
            ClassName = "FluentUIResources",
            MethodName = "GetAllComponents",
            UriTemplate = "fluentui://components"
        });

        // Assert
        Assert.Single(root.Resources);
        Assert.Equal("components", root.Resources[0].Name);
    }

    [Fact]
    public void Prompts_ShouldBeAddable()
    {
        // Arrange
        var root = new McpDocumentationRoot
        {
            Metadata = new McpDocumentationMetadata
            {
                AssemblyVersion = "1.0.0",
                GeneratedDateUtc = "2024-01-01 00:00"
            }
        };

        // Act
        root.Prompts.Add(new McpPromptInfo
        {
            Name = "CreateComponent",
            ClassName = "ComponentPrompts"
        });

        // Assert
        Assert.Single(root.Prompts);
        Assert.Equal("CreateComponent", root.Prompts[0].Name);
    }

    [Fact]
    public void CompleteDocumentation_ShouldBeConstructedProperly()
    {
        // Arrange & Act
        var root = new McpDocumentationRoot
        {
            Metadata = new McpDocumentationMetadata
            {
                AssemblyVersion = "5.0.0",
                GeneratedDateUtc = "2024-06-15 12:30",
                ToolCount = 5,
                ResourceCount = 5,
                PromptCount = 0
            },
            Tools =
            [
                new McpToolInfo { Name = "ListComponents", ClassName = "ComponentListTools" },
                new McpToolInfo { Name = "SearchComponents", ClassName = "ComponentListTools" },
                new McpToolInfo { Name = "GetComponentDetails", ClassName = "ComponentDetailTools" },
                new McpToolInfo { Name = "GetEnumValues", ClassName = "EnumTools" },
                new McpToolInfo { Name = "GetComponentEnums", ClassName = "EnumTools" }
            ],
            Resources =
            [
                new McpResourceInfo { Name = "components", ClassName = "FluentUIResources", MethodName = "GetAllComponents", UriTemplate = "fluentui://components" },
                new McpResourceInfo { Name = "enums", ClassName = "FluentUIResources", MethodName = "GetAllEnums", UriTemplate = "fluentui://enums" },
                new McpResourceInfo { Name = "component", ClassName = "ComponentResources", MethodName = "GetComponent", UriTemplate = "fluentui://component/{name}" },
                new McpResourceInfo { Name = "category", ClassName = "ComponentResources", MethodName = "GetCategory", UriTemplate = "fluentui://category/{name}" },
                new McpResourceInfo { Name = "enum", ClassName = "ComponentResources", MethodName = "GetEnum", UriTemplate = "fluentui://enum/{name}" }
            ],
            Prompts = []
        };

        // Assert
        Assert.Equal(5, root.Tools.Count);
        Assert.Equal(5, root.Resources.Count);
        Assert.Empty(root.Prompts);
        Assert.Equal(5, root.Metadata.ToolCount);
        Assert.Equal(5, root.Metadata.ResourceCount);
        Assert.Equal(0, root.Metadata.PromptCount);
    }
}
