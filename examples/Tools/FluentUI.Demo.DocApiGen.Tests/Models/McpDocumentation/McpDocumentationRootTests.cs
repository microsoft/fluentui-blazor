// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Xunit;
using FluentUI.Demo.DocApiGen.Models.McpDocumentation;

namespace FluentUI.Demo.DocApiGen.Tests.Models.McpDocumentation;

/// <summary>
/// Unit tests for <see cref="McpDocumentationRoot"/>.
/// </summary>
public class McpDocumentationRootTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithDefaults()
    {
        // Arrange & Act
        var root = new McpDocumentationRoot();

        // Assert
        Assert.NotNull(root.Metadata);
        Assert.NotNull(root.Components);
        Assert.NotNull(root.Enums);
        Assert.Empty(root.Components);
        Assert.Empty(root.Enums);
    }

    [Fact]
    public void Metadata_ShouldBeSettable()
    {
        // Arrange
        var root = new McpDocumentationRoot();
        var metadata = new McpDocumentationMetadata
        {
            AssemblyVersion = "1.0.0",
            GeneratedDateUtc = "2024-01-01T00:00:00Z",
            ComponentCount = 10,
            EnumCount = 5
        };

        // Act
        root.Metadata = metadata;

        // Assert
        Assert.Same(metadata, root.Metadata);
        Assert.Equal("1.0.0", root.Metadata.AssemblyVersion);
        Assert.Equal("2024-01-01T00:00:00Z", root.Metadata.GeneratedDateUtc);
        Assert.Equal(10, root.Metadata.ComponentCount);
        Assert.Equal(5, root.Metadata.EnumCount);
    }

    [Fact]
    public void Components_ShouldBeSettable()
    {
        // Arrange
        var root = new McpDocumentationRoot();
        var components = new List<McpComponentInfo>
        {
            new() { Name = "FluentButton", FullName = "Microsoft.FluentUI.AspNetCore.Components.FluentButton" },
            new() { Name = "FluentCard", FullName = "Microsoft.FluentUI.AspNetCore.Components.FluentCard" }
        };

        // Act
        root.Components = components;

        // Assert
        Assert.Same(components, root.Components);
        Assert.Equal(2, root.Components.Count);
        Assert.Equal("FluentButton", root.Components[0].Name);
        Assert.Equal("FluentCard", root.Components[1].Name);
    }

    [Fact]
    public void Enums_ShouldBeSettable()
    {
        // Arrange
        var root = new McpDocumentationRoot();
        var enums = new List<McpEnumInfo>
        {
            new() { Name = "Appearance", FullName = "Microsoft.FluentUI.AspNetCore.Components.Appearance" },
            new() { Name = "Color", FullName = "Microsoft.FluentUI.AspNetCore.Components.Color" }
        };

        // Act
        root.Enums = enums;

        // Assert
        Assert.Same(enums, root.Enums);
        Assert.Equal(2, root.Enums.Count);
        Assert.Equal("Appearance", root.Enums[0].Name);
        Assert.Equal("Color", root.Enums[1].Name);
    }

    [Fact]
    public void CompleteObject_ShouldBeConstructedProperly()
    {
        // Arrange & Act
        var root = new McpDocumentationRoot
        {
            Metadata = new McpDocumentationMetadata
            {
                AssemblyVersion = "2.0.0",
                GeneratedDateUtc = "2024-12-01T10:30:00Z",
                ComponentCount = 2,
                EnumCount = 1
            },
            Components =
            [
                new McpComponentInfo
                {
                    Name = "FluentButton",
                    FullName = "Microsoft.FluentUI.AspNetCore.Components.FluentButton",
                    Summary = "A button component",
                    Category = "Forms",
                    IsGeneric = false,
                    BaseClass = "FluentComponentBase",
                    Properties =
                    [
                        new McpPropertyInfo
                        {
                            Name = "Appearance",
                            Type = "Appearance?",
                            Description = "The appearance of the button",
                            IsParameter = true
                        }
                    ],
                    Events =
                    [
                        new McpEventInfo
                        {
                            Name = "OnClick",
                            Type = "EventCallback<MouseEventArgs>",
                            Description = "Triggered when button is clicked"
                        }
                    ],
                    Methods =
                    [
                        new McpMethodInfo
                        {
                            Name = "Focus",
                            ReturnType = "Task",
                            Description = "Sets focus on the button"
                        }
                    ]
                }
            ],
            Enums =
            [
                new McpEnumInfo
                {
                    Name = "Appearance",
                    FullName = "Microsoft.FluentUI.AspNetCore.Components.Appearance",
                    Description = "Defines button appearance styles",
                    Values =
                    [
                        new McpEnumValueInfo
                        {
                            Name = "Neutral",
                            Value = 0,
                            Description = "Neutral appearance"
                        },
                        new McpEnumValueInfo
                        {
                            Name = "Accent",
                            Value = 1,
                            Description = "Accent appearance"
                        }
                    ]
                }
            ]
        };

        // Assert
        Assert.NotNull(root.Metadata);
        Assert.Equal("2.0.0", root.Metadata.AssemblyVersion);
        Assert.Equal(2, root.Metadata.ComponentCount);
        Assert.Equal(1, root.Metadata.EnumCount);

        Assert.Single(root.Components);
        Assert.Equal("FluentButton", root.Components[0].Name);
        Assert.Equal("Forms", root.Components[0].Category);
        Assert.Single(root.Components[0].Properties);
        Assert.Single(root.Components[0].Events);
        Assert.Single(root.Components[0].Methods);

        Assert.Single(root.Enums);
        Assert.Equal("Appearance", root.Enums[0].Name);
        Assert.Equal(2, root.Enums[0].Values.Count);
    }

    [Fact]
    public void Components_CanBeModifiedAfterConstruction()
    {
        // Arrange
        var root = new McpDocumentationRoot();

        // Act
        root.Components.Add(new McpComponentInfo { Name = "Component1" });
        root.Components.Add(new McpComponentInfo { Name = "Component2" });

        // Assert
        Assert.Equal(2, root.Components.Count);
        Assert.Equal("Component1", root.Components[0].Name);
        Assert.Equal("Component2", root.Components[1].Name);
    }

    [Fact]
    public void Enums_CanBeModifiedAfterConstruction()
    {
        // Arrange
        var root = new McpDocumentationRoot();

        // Act
        root.Enums.Add(new McpEnumInfo { Name = "Enum1" });
        root.Enums.Add(new McpEnumInfo { Name = "Enum2" });

        // Assert
        Assert.Equal(2, root.Enums.Count);
        Assert.Equal("Enum1", root.Enums[0].Name);
        Assert.Equal("Enum2", root.Enums[1].Name);
    }
}
