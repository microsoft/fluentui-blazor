// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.McpServer.Resources;
using Microsoft.FluentUI.AspNetCore.Components.McpServer.Services;

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Tests.Resources;

/// <summary>
/// Tests for <see cref="GuideResources"/>.
/// </summary>
public class GuideResourcesTests
{
    private readonly GuideResources _resources;

    public GuideResourcesTests()
    {
        var guideService = new DocumentationGuideService();
        _resources = new GuideResources(guideService);
    }

    [Fact]
    public void GetAllGuides_ReturnsNonEmptyString()
    {
        // Act
        var result = _resources.GetAllGuides();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void GetAllGuides_ContainsGuideLinks()
    {
        // Act
        var result = _resources.GetAllGuides();

        // Assert
        Assert.Contains("fluentui://guide/", result);
    }

    [Fact]
    public void GetInstallationGuide_ReturnsNonEmptyString()
    {
        // Act
        var result = _resources.GetInstallationGuide();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void GetInstallationGuide_ContainsInstallationInfo()
    {
        // Act
        var result = _resources.GetInstallationGuide();

        // Assert
        Assert.True(
            result.Contains("install", StringComparison.OrdinalIgnoreCase) ||
            result.Contains("Installation", StringComparison.OrdinalIgnoreCase) ||
            result.Contains("package", StringComparison.OrdinalIgnoreCase) ||
            result.Contains("fluentui-blazor.net", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void GetMigrationGuide_ReturnsNonEmptyString()
    {
        // Act
        var result = _resources.GetMigrationGuide();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void GetMigrationGuide_ContainsMigrationInfo()
    {
        // Act
        var result = _resources.GetMigrationGuide();

        // Assert
        Assert.True(
            result.Contains("migrat", StringComparison.OrdinalIgnoreCase) ||
            result.Contains("Migration", StringComparison.OrdinalIgnoreCase) ||
            result.Contains("upgrade", StringComparison.OrdinalIgnoreCase) ||
            result.Contains("v5", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void GetStylesGuide_ReturnsNonEmptyString()
    {
        // Act
        var result = _resources.GetStylesGuide();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void GetStylesGuide_ContainsStylesInfo()
    {
        // Act
        var result = _resources.GetStylesGuide();

        // Assert
        Assert.True(
            result.Contains("style", StringComparison.OrdinalIgnoreCase) ||
            result.Contains("Styles", StringComparison.OrdinalIgnoreCase) ||
            result.Contains("CSS", StringComparison.OrdinalIgnoreCase) ||
            result.Contains("fluentui-blazor.net", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void GetLocalizationGuide_ReturnsNonEmptyString()
    {
        // Act
        var result = _resources.GetLocalizationGuide();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void GetLocalizationGuide_ContainsLocalizationInfo()
    {
        // Act
        var result = _resources.GetLocalizationGuide();

        // Assert
        Assert.True(
            result.Contains("local", StringComparison.OrdinalIgnoreCase) ||
            result.Contains("Localization", StringComparison.OrdinalIgnoreCase) ||
            result.Contains("language", StringComparison.OrdinalIgnoreCase) ||
            result.Contains("fluentui-blazor.net", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void GetDefaultValuesGuide_ReturnsNonEmptyString()
    {
        // Act
        var result = _resources.GetDefaultValuesGuide();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void GetWhatsNewGuide_ReturnsNonEmptyString()
    {
        // Act
        var result = _resources.GetWhatsNewGuide();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }
}
