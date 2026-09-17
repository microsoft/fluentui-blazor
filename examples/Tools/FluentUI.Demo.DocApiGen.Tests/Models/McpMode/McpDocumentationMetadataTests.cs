// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using FluentUI.Demo.DocApiGen.Models.McpMode;
using Xunit;

namespace FluentUI.Demo.DocApiGen.Tests.Models.McpMode;

/// <summary>
/// Unit tests for <see cref="McpDocumentationMetadata"/>.
/// </summary>
public class McpDocumentationMetadataTests
{
    [Fact]
    public void Constructor_ShouldRequireAssemblyVersionAndGeneratedDateUtc()
    {
        // Arrange & Act
        var metadata = new McpDocumentationMetadata
        {
            AssemblyVersion = "5.0.0",
            GeneratedDateUtc = "2024-06-15 12:30"
        };

        // Assert
        Assert.Equal("5.0.0", metadata.AssemblyVersion);
        Assert.Equal("2024-06-15 12:30", metadata.GeneratedDateUtc);
    }

    [Fact]
    public void Counts_ShouldDefaultToZero()
    {
        // Arrange & Act
        var metadata = new McpDocumentationMetadata
        {
            AssemblyVersion = "1.0.0",
            GeneratedDateUtc = "2024-01-01 00:00"
        };

        // Assert
        Assert.Equal(0, metadata.ToolCount);
        Assert.Equal(0, metadata.ResourceCount);
        Assert.Equal(0, metadata.PromptCount);
    }

    [Fact]
    public void ToolCount_ShouldBeSettable()
    {
        // Arrange & Act
        var metadata = new McpDocumentationMetadata
        {
            AssemblyVersion = "1.0.0",
            GeneratedDateUtc = "2024-01-01 00:00",
            ToolCount = 5
        };

        // Assert
        Assert.Equal(5, metadata.ToolCount);
    }

    [Fact]
    public void ResourceCount_ShouldBeSettable()
    {
        // Arrange & Act
        var metadata = new McpDocumentationMetadata
        {
            AssemblyVersion = "1.0.0",
            GeneratedDateUtc = "2024-01-01 00:00",
            ResourceCount = 6
        };

        // Assert
        Assert.Equal(6, metadata.ResourceCount);
    }

    [Fact]
    public void PromptCount_ShouldBeSettable()
    {
        // Arrange & Act
        var metadata = new McpDocumentationMetadata
        {
            AssemblyVersion = "1.0.0",
            GeneratedDateUtc = "2024-01-01 00:00",
            PromptCount = 3
        };

        // Assert
        Assert.Equal(3, metadata.PromptCount);
    }

    [Fact]
    public void AssemblyVersion_ShouldSupportVersionWithCommitHash()
    {
        // Arrange & Act
        var metadata = new McpDocumentationMetadata
        {
            AssemblyVersion = "5.0.0+abc12345",
            GeneratedDateUtc = "2024-06-15 12:30"
        };

        // Assert
        Assert.Equal("5.0.0+abc12345", metadata.AssemblyVersion);
    }

    [Fact]
    public void CompleteMetadata_ShouldBeConstructedProperly()
    {
        // Arrange & Act
        var metadata = new McpDocumentationMetadata
        {
            AssemblyVersion = "5.0.0+abc12345",
            GeneratedDateUtc = "2024-06-15 12:30",
            ToolCount = 5,
            ResourceCount = 6,
            PromptCount = 0
        };

        // Assert
        Assert.Equal("5.0.0+abc12345", metadata.AssemblyVersion);
        Assert.Equal("2024-06-15 12:30", metadata.GeneratedDateUtc);
        Assert.Equal(5, metadata.ToolCount);
        Assert.Equal(6, metadata.ResourceCount);
        Assert.Equal(0, metadata.PromptCount);
    }
}
