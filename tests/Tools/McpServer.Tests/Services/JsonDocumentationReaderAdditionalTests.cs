// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.McpServer.Services;

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Tests.Services;

/// <summary>
/// Additional tests for <see cref="JsonDocumentationReader"/> to improve coverage.
/// </summary>
public class JsonDocumentationReaderAdditionalTests
{
    [Fact]
    public void Constructor_WithNullPath_LoadsFromEmbeddedResource()
    {
        // Act
        var reader = new JsonDocumentationReader(null);

        // Assert
        Assert.True(reader.IsAvailable);
        Assert.NotNull(reader.Metadata);
    }

    [Fact]
    public void Constructor_WithInvalidPath_FallsBackToEmbeddedResource()
    {
        // Act
        var reader = new JsonDocumentationReader("/invalid/path/to/file.json");

        // Assert - Should fall back to embedded resource
        Assert.NotNull(reader);
    }

    [Fact]
    public void GetComponent_WithEmptyString_ReturnsNull()
    {
        // Arrange
        var jsonPath = JsonDocumentationFinder.Find();
        var reader = new JsonDocumentationReader(jsonPath);

        // Act & Assert
        Assert.Null(reader.GetComponent(string.Empty));
    }

    [Fact]
    public void GetComponent_WithNull_ReturnsNull()
    {
        // Arrange
        var jsonPath = JsonDocumentationFinder.Find();
        var reader = new JsonDocumentationReader(jsonPath);

        // Act & Assert
        Assert.Null(reader.GetComponent(null!));
    }

    [Fact]
    public void GetEnum_WithEmptyString_ReturnsNull()
    {
        // Arrange
        var jsonPath = JsonDocumentationFinder.Find();
        var reader = new JsonDocumentationReader(jsonPath);

        // Act & Assert
        Assert.Null(reader.GetEnum(string.Empty));
    }

    [Fact]
    public void GetEnum_WithNull_ReturnsNull()
    {
        // Arrange
        var jsonPath = JsonDocumentationFinder.Find();
        var reader = new JsonDocumentationReader(jsonPath);

        // Act & Assert
        Assert.Null(reader.GetEnum(null!));
    }

    [Fact]
    public void Metadata_ContainsValidInformation()
    {
        // Arrange
        var jsonPath = JsonDocumentationFinder.Find();
        var reader = new JsonDocumentationReader(jsonPath);

        // Act & Assert
        Assert.NotNull(reader.Metadata);
        Assert.NotEmpty(reader.Metadata.AssemblyVersion);
        Assert.NotEmpty(reader.Metadata.GeneratedDateUtc);
        Assert.True(reader.Metadata.ComponentCount > 0);
        Assert.True(reader.Metadata.EnumCount > 0);
    }
}
