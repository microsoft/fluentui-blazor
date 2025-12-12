// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Xunit;
using FluentUI.Demo.DocApiGen.Models.McpDocumentation;

namespace FluentUI.Demo.DocApiGen.Tests.Models.McpDocumentation;

/// <summary>
/// Unit tests for <see cref="McpDocumentationMetadata"/>.
/// </summary>
public class McpDocumentationMetadataTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithDefaults()
    {
        // Arrange & Act
        var metadata = new McpDocumentationMetadata();

        // Assert
        Assert.Equal(string.Empty, metadata.AssemblyVersion);
        Assert.Equal(string.Empty, metadata.GeneratedDateUtc);
        Assert.Equal(0, metadata.ComponentCount);
        Assert.Equal(0, metadata.EnumCount);
    }

    [Fact]
    public void AssemblyVersion_ShouldBeSettable()
    {
        // Arrange
        var metadata = new McpDocumentationMetadata();

        // Act
        metadata.AssemblyVersion = "1.2.3";

        // Assert
        Assert.Equal("1.2.3", metadata.AssemblyVersion);
    }

    [Fact]
    public void GeneratedDateUtc_ShouldBeSettable()
    {
        // Arrange
        var metadata = new McpDocumentationMetadata();
        var date = "2024-12-01T10:30:00Z";

        // Act
        metadata.GeneratedDateUtc = date;

        // Assert
        Assert.Equal(date, metadata.GeneratedDateUtc);
    }

    [Fact]
    public void ComponentCount_ShouldBeSettable()
    {
        // Arrange
        var metadata = new McpDocumentationMetadata();

        // Act
        metadata.ComponentCount = 42;

        // Assert
        Assert.Equal(42, metadata.ComponentCount);
    }

    [Fact]
    public void EnumCount_ShouldBeSettable()
    {
        // Arrange
        var metadata = new McpDocumentationMetadata();

        // Act
        metadata.EnumCount = 15;

        // Assert
        Assert.Equal(15, metadata.EnumCount);
    }

    [Fact]
    public void AllProperties_CanBeSetTogether()
    {
        // Arrange & Act
        var metadata = new McpDocumentationMetadata
        {
            AssemblyVersion = "2.0.0",
            GeneratedDateUtc = "2024-12-01T12:00:00Z",
            ComponentCount = 100,
            EnumCount = 20
        };

        // Assert
        Assert.Equal("2.0.0", metadata.AssemblyVersion);
        Assert.Equal("2024-12-01T12:00:00Z", metadata.GeneratedDateUtc);
        Assert.Equal(100, metadata.ComponentCount);
        Assert.Equal(20, metadata.EnumCount);
    }
}
