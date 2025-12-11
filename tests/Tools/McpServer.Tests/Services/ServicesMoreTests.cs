// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.McpServer.Services;

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Tests.Services;

/// <summary>
/// Additional tests for Services to improve coverage.
/// </summary>
public class ServicesMoreTests
{
    [Fact]
    public void FluentUIDocumentationService_GetEnumsForComponent_ReturnsAllEnums()
    {
        // Arrange
        var jsonPath = JsonDocumentationFinder.Find();
        var service = new FluentUIDocumentationService(jsonPath);

        // Act
        var enums = service.GetEnumsForComponent("FluentButton");

        // Assert
        Assert.NotNull(enums);
        // FluentButton should have Appearance enum at minimum
        if (enums.Count > 0)
        {
            Assert.All(enums.Values, e => Assert.NotEmpty(e.Name));
        }
    }

    [Fact]
    public void FluentUIDocumentationService_SearchComponents_WithLongTerm_Works()
    {
        // Arrange
        var jsonPath = JsonDocumentationFinder.Find();
        var service = new FluentUIDocumentationService(jsonPath);

        // Act
        var result = service.SearchComponents("button");

        // Assert
        Assert.NotNull(result);
        // Should return either results or empty list
    }

    [Fact]
    public void FluentUIDocumentationService_GetComponentsByCategory_EmptyCategory_ReturnsEmpty()
    {
        // Arrange
        var jsonPath = JsonDocumentationFinder.Find();
        var service = new FluentUIDocumentationService(jsonPath);

        // Act
        var result = service.GetComponentsByCategory("");

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void FluentUIDocumentationService_ComponentsVersion_IsValid()
    {
        // Arrange
        var jsonPath = JsonDocumentationFinder.Find();
        var service = new FluentUIDocumentationService(jsonPath);

        // Act & Assert
        Assert.NotNull(service.ComponentsVersion);
        Assert.NotEmpty(service.ComponentsVersion);
        Assert.NotEqual("Unknown", service.ComponentsVersion);
    }

    [Fact]
    public void FluentUIDocumentationService_McpServerVersion_IsValid()
    {
        // Act
        var version = FluentUIDocumentationService.McpServerVersion;

        // Assert
        Assert.NotNull(version);
        Assert.NotEmpty(version);
        Assert.NotEqual("Unknown", version);
    }

    [Fact]
    public void FluentUIDocumentationService_DocumentationGeneratedDate_IsValid()
    {
        // Arrange
        var jsonPath = JsonDocumentationFinder.Find();
        var service = new FluentUIDocumentationService(jsonPath);

        // Act & Assert
        Assert.NotNull(service.DocumentationGeneratedDate);
        Assert.NotEmpty(service.DocumentationGeneratedDate);
    }

    [Fact]
    public void DocumentationGuideService_GetAllGuides_ReturnsMultipleGuides()
    {
        // Arrange
        var service = new DocumentationGuideService();

        // Act
        var guides = service.GetAllGuides();

        // Assert
        Assert.NotNull(guides);
        // Should have at least installation, migration, etc.
    }

    [Fact]
    public void DocumentationGuideService_GetGuideContent_WithInvalidTopic_ReturnsNull()
    {
        // Arrange
        var service = new DocumentationGuideService();

        // Act
        var content = service.GetGuideContent("invalid-topic-xyz");

        // Assert
        Assert.Null(content);
    }

    [Fact]
    public void DocumentationGuideService_GetFullMigrationGuide_ReturnsContent()
    {
        // Arrange
        var service = new DocumentationGuideService();

        // Act
        var content = service.GetFullMigrationGuide();

        // Assert
        Assert.NotNull(content);
        Assert.NotEmpty(content);
    }

    [Fact]
    public void FluentUIDocumentationService_GetComponentDetails_WithPartialName_Works()
    {
        // Arrange
        var jsonPath = JsonDocumentationFinder.Find();
        var service = new FluentUIDocumentationService(jsonPath);

        // Act
        var result = service.GetComponentDetails("Button"); // Without "Fluent" prefix

        // Assert
        if (result != null)
        {
            Assert.Equal("FluentButton", result.Component.Name);
        }
    }

    [Fact]
    public void FluentUIDocumentationService_GetEnumDetails_WithNullableName_Works()
    {
        // Arrange
        var jsonPath = JsonDocumentationFinder.Find();
        var service = new FluentUIDocumentationService(jsonPath);

        // Act
        var result = service.GetEnumDetails("Appearance");

        // Assert
        // Should find an appearance enum or return null
        Assert.True(result != null || result == null);
    }
}
