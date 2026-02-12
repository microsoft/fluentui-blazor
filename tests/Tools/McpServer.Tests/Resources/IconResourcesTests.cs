// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.McpServer.Resources;
using Microsoft.FluentUI.AspNetCore.McpServer.Services;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tests.Resources;

/// <summary>
/// Tests for the <see cref="IconResources"/> class.
/// </summary>
public class IconResourcesTests
{
    private static IconResources CreateResources()
    {
        var synonymService = new IconSynonymService();
        var iconService = new IconService(synonymService);
        return new IconResources(iconService);
    }

    #region GetIconCatalog

    [Fact]
    public void GetIconCatalog_ContainsTitle()
    {
        var resources = CreateResources();
        var result = resources.GetIconCatalog();

        Assert.Contains("# Fluent UI Icon Catalog", result, StringComparison.Ordinal);
    }

    [Fact]
    public void GetIconCatalog_ContainsTotalCount()
    {
        var resources = CreateResources();
        var result = resources.GetIconCatalog();

        Assert.Contains("Total icons", result, StringComparison.Ordinal);
    }

    [Fact]
    public void GetIconCatalog_ContainsVariantTable()
    {
        var resources = CreateResources();
        var result = resources.GetIconCatalog();

        Assert.Contains("## Variants", result, StringComparison.Ordinal);
        Assert.Contains("Regular", result, StringComparison.Ordinal);
        Assert.Contains("Filled", result, StringComparison.Ordinal);
        Assert.Contains("Light", result, StringComparison.Ordinal);
        Assert.Contains("Color", result, StringComparison.Ordinal);
    }

    [Fact]
    public void GetIconCatalog_ContainsSizeTable()
    {
        var resources = CreateResources();
        var result = resources.GetIconCatalog();

        Assert.Contains("## Sizes", result, StringComparison.Ordinal);
        Assert.Contains("20", result, StringComparison.Ordinal);
        Assert.Contains("Default for most components", result, StringComparison.Ordinal);
    }

    [Fact]
    public void GetIconCatalog_ContainsUsageGuidance()
    {
        var resources = CreateResources();
        var result = resources.GetIconCatalog();

        Assert.Contains("## Usage", result, StringComparison.Ordinal);
        Assert.Contains("FluentIcon", result, StringComparison.Ordinal);
        Assert.Contains("SearchIcons", result, StringComparison.Ordinal);
    }

    [Fact]
    public void GetIconCatalog_ContainsSampleIcons()
    {
        var resources = CreateResources();
        var result = resources.GetIconCatalog();

        Assert.Contains("## Sample Icon Names", result, StringComparison.Ordinal);
        Assert.Contains("more", result, StringComparison.OrdinalIgnoreCase);
    }

    #endregion

    #region GetIconVariants

    [Fact]
    public void GetIconVariants_ContainsTitle()
    {
        var resources = CreateResources();
        var result = resources.GetIconVariants();

        Assert.Contains("# Fluent UI Icon Variants", result, StringComparison.Ordinal);
    }

    [Fact]
    public void GetIconVariants_ContainsAllVariantSections()
    {
        var resources = CreateResources();
        var result = resources.GetIconVariants();

        Assert.Contains("## Regular (Outlined)", result, StringComparison.Ordinal);
        Assert.Contains("## Filled (Solid)", result, StringComparison.Ordinal);
        Assert.Contains("## Light (Thin)", result, StringComparison.Ordinal);
        Assert.Contains("## Color (Multi-color)", result, StringComparison.Ordinal);
    }

    [Fact]
    public void GetIconVariants_ContainsChoosingGuide()
    {
        var resources = CreateResources();
        var result = resources.GetIconVariants();

        Assert.Contains("## Choosing the Right Variant", result, StringComparison.Ordinal);
        Assert.Contains("Context", result, StringComparison.Ordinal);
        Assert.Contains("Recommended Variant", result, StringComparison.Ordinal);
    }

    [Fact]
    public void GetIconVariants_ContainsCodeExamples()
    {
        var resources = CreateResources();
        var result = resources.GetIconVariants();

        Assert.Contains("```razor", result, StringComparison.Ordinal);
        Assert.Contains("Icons.Regular.Size20.Settings()", result, StringComparison.Ordinal);
    }

    [Fact]
    public void GetIconVariants_ContainsIconCounts()
    {
        var resources = CreateResources();
        var result = resources.GetIconVariants();

        // Should contain icon count numbers
        Assert.Contains("icons** available", result, StringComparison.Ordinal);
    }

    #endregion

    #region GetIconByName

    [Fact]
    public void GetIconByName_ExistingIcon_ContainsDetails()
    {
        var resources = CreateResources();
        var result = resources.GetIconByName("Bookmark");

        Assert.Contains("# Icon: Bookmark", result, StringComparison.Ordinal);
        Assert.Contains("Recommended", result, StringComparison.Ordinal);
        Assert.Contains("Availability Matrix", result, StringComparison.Ordinal);
        Assert.Contains("Code Examples", result, StringComparison.Ordinal);
    }

    [Fact]
    public void GetIconByName_ExistingIcon_ContainsCodeSnippets()
    {
        var resources = CreateResources();
        var result = resources.GetIconByName("Bookmark");

        Assert.Contains("FluentIcon", result, StringComparison.Ordinal);
        Assert.Contains("FluentButton", result, StringComparison.Ordinal);
    }

    [Fact]
    public void GetIconByName_NonExistentIcon_ReturnsNotFound()
    {
        var resources = CreateResources();
        var result = resources.GetIconByName("NonExistentXyz");

        Assert.Contains("not found", result, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("SearchIcons", result, StringComparison.Ordinal);
    }

    #endregion
}
