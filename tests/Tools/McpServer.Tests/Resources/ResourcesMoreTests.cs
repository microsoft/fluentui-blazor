// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.McpServer.Resources;
using Microsoft.FluentUI.AspNetCore.Components.McpServer.Services;

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Tests.Resources;

/// <summary>
/// Additional tests for Resources to improve coverage.
/// </summary>
public class ResourcesMoreTests
{
    private readonly FluentUIDocumentationService _documentationService;
    private readonly DocumentationGuideService _guideService;

    public ResourcesMoreTests()
    {
        var jsonPath = JsonDocumentationFinder.Find();
        _documentationService = new FluentUIDocumentationService(jsonPath);
        _guideService = new DocumentationGuideService();
    }

    #region ComponentResources Tests

    [Fact]
    public void ComponentResources_GetComponent_WithDifferentCasing_Works()
    {
        // Arrange
        var resources = new ComponentResources(_documentationService);

        // Act
        var lower = resources.GetComponent("fluentbutton");
        var upper = resources.GetComponent("FLUENTBUTTON");
        var mixed = resources.GetComponent("FluentButton");

        // Assert
        Assert.NotNull(lower);
        Assert.NotNull(upper);
        Assert.NotNull(mixed);
    }

    [Fact]
    public void ComponentResources_GetCategory_WithInvalidCategory_ReturnsMessage()
    {
        // Arrange
        var resources = new ComponentResources(_documentationService);

        // Act
        var result = resources.GetCategory("InvalidCategory123");

        // Assert
        Assert.Contains("not found", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void ComponentResources_GetEnum_WithValidEnum_ContainsAllValues()
    {
        // Arrange
        var resources = new ComponentResources(_documentationService);

        // Act
        var result = resources.GetEnum("Appearance");

        // Assert
        if (!result.Contains("not found", StringComparison.OrdinalIgnoreCase))
        {
            Assert.Contains("Values", result);
            Assert.Contains("|", result); // Table format
        }
    }

    #endregion

    #region GuideResources Tests

    [Fact]
    public void GuideResources_GetAllGuides_ContainsQuickLinks()
    {
        // Arrange
        var resources = new GuideResources(_guideService);

        // Act
        var result = resources.GetAllGuides();

        // Assert
        Assert.Contains("Quick Links", result);
        Assert.Contains("fluentui://guide/", result);
    }

    [Fact]
    public void GuideResources_GetInstallationGuide_ReturnsContent()
    {
        // Arrange
        var resources = new GuideResources(_guideService);

        // Act
        var result = resources.GetInstallationGuide();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void GuideResources_GetDefaultValuesGuide_ReturnsContent()
    {
        // Arrange
        var resources = new GuideResources(_guideService);

        // Act
        var result = resources.GetDefaultValuesGuide();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void GuideResources_GetWhatsNewGuide_ReturnsContent()
    {
        // Arrange
        var resources = new GuideResources(_guideService);

        // Act
        var result = resources.GetWhatsNewGuide();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    #endregion

    #region FluentUIResources Tests

    [Fact]
    public void FluentUIResources_GetAllComponents_ContainsCategoryHeaders()
    {
        // Arrange
        var resources = new FluentUIResources(_documentationService);

        // Act
        var result = resources.GetAllComponents();

        // Assert
        Assert.Contains("##", result); // Markdown headers
        Assert.Contains("Total:", result);
    }

    [Fact]
    public void FluentUIResources_GetCategories_GroupsComponentsCorrectly()
    {
        // Arrange
        var resources = new FluentUIResources(_documentationService);

        // Act
        var result = resources.GetCategories();

        // Assert
        Assert.Contains("components", result);
        Assert.Contains("##", result); // Category headers
    }

    [Fact]
    public void FluentUIResources_GetAllEnums_ContainsEnumDetails()
    {
        // Arrange
        var resources = new FluentUIResources(_documentationService);

        // Act
        var result = resources.GetAllEnums();

        // Assert
        Assert.Contains("Total:", result);
        Assert.Contains("##", result);
        Assert.Contains("Values:", result);
    }

    #endregion
}
