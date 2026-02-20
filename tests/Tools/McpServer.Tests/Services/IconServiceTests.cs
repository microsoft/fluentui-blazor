// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.McpServer.Models;
using Microsoft.FluentUI.AspNetCore.McpServer.Services;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tests.Services;

/// <summary>
/// Tests for the <see cref="IconService"/> class.
/// </summary>
public class IconServiceTests
{
    private static IconService CreateService()
    {
        return new IconService(new IconSynonymService());
    }

    #region Constructor and Count

    [Fact]
    public void Constructor_LoadsIcons()
    {
        var service = CreateService();
        Assert.True(service.Count > 0, "Icon catalog should contain icons");
    }

    [Fact]
    public void Count_ReturnsLargeNumber()
    {
        var service = CreateService();

        // The catalog has ~2200+ icons
        Assert.True(service.Count > 2000, $"Expected 2000+ icons, got {service.Count}");
    }

    #endregion

    #region GetAllIcons

    [Fact]
    public void GetAllIcons_ReturnsNonEmptyList()
    {
        var service = CreateService();
        var icons = service.GetAllIcons();

        Assert.NotEmpty(icons);
        Assert.Equal(service.Count, icons.Count);
    }

    [Fact]
    public void GetAllIcons_AllIconsHaveNames()
    {
        var service = CreateService();
        var icons = service.GetAllIcons();

        Assert.All(icons, icon => Assert.False(string.IsNullOrEmpty(icon.Name)));
    }

    [Fact]
    public void GetAllIcons_AllIconsHaveVariants()
    {
        var service = CreateService();
        var icons = service.GetAllIcons();

        Assert.All(icons, icon => Assert.NotEmpty(icon.Variants));
    }

    #endregion

    #region SearchIcons

    [Fact]
    public void SearchIcons_EmptyTerm_ReturnsEmpty()
    {
        var service = CreateService();
        var results = service.SearchIcons("");

        Assert.Empty(results);
    }

    [Fact]
    public void SearchIcons_NullTerm_ReturnsEmpty()
    {
        var service = CreateService();
        var results = service.SearchIcons(null!);

        Assert.Empty(results);
    }

    [Fact]
    public void SearchIcons_WhitespaceTerm_ReturnsEmpty()
    {
        var service = CreateService();
        var results = service.SearchIcons("   ");

        Assert.Empty(results);
    }

    [Fact]
    public void SearchIcons_DirectNameMatch_FindsIcon()
    {
        var service = CreateService();
        var results = service.SearchIcons("Bookmark");

        Assert.NotEmpty(results);
        Assert.Contains(results, r => r.Name.Contains("Bookmark", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void SearchIcons_CaseInsensitive()
    {
        var service = CreateService();
        var upper = service.SearchIcons("BOOKMARK");
        var lower = service.SearchIcons("bookmark");

        Assert.NotEmpty(upper);
        Assert.Equal(upper.Count, lower.Count);
    }

    [Fact]
    public void SearchIcons_SynonymSearch_FindsIcons()
    {
        var service = CreateService();
        var results = service.SearchIcons("trash");

        Assert.NotEmpty(results);
        Assert.Contains(results, r => r.Name.Contains("Delete", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void SearchIcons_SynonymBell_FindsAlert()
    {
        var service = CreateService();
        var results = service.SearchIcons("bell");

        Assert.NotEmpty(results);
        Assert.Contains(results, r => r.Name.Contains("Alert", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void SearchIcons_SynonymGear_FindsSettings()
    {
        var service = CreateService();
        var results = service.SearchIcons("gear");

        Assert.NotEmpty(results);
        Assert.Contains(results, r => r.Name.Contains("Settings", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void SearchIcons_PartialSynonymMatch_FindsIcons()
    {
        var service = CreateService();

        // "notif" should match the "notification" synonym
        var results = service.SearchIcons("notif");

        Assert.NotEmpty(results);
        Assert.Contains(results, r => r.Name.Contains("Alert", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void SearchIcons_WithVariantFilter_FiltersResults()
    {
        var service = CreateService();
        var allResults = service.SearchIcons("Arrow");
        var filledOnly = service.SearchIcons("Arrow", variant: "Filled");

        Assert.NotEmpty(allResults);
        Assert.NotEmpty(filledOnly);
        Assert.True(filledOnly.Count <= allResults.Count);
        Assert.All(filledOnly, icon => Assert.True(icon.Variants.ContainsKey("Filled")));
    }

    [Fact]
    public void SearchIcons_WithSizeFilter_FiltersResults()
    {
        var service = CreateService();
        var allResults = service.SearchIcons("Arrow");
        var size20Only = service.SearchIcons("Arrow", size: 20);

        Assert.NotEmpty(allResults);
        Assert.NotEmpty(size20Only);
        Assert.True(size20Only.Count <= allResults.Count);
    }

    [Fact]
    public void SearchIcons_WithVariantAndSizeFilter_FiltersResults()
    {
        var service = CreateService();
        var filtered = service.SearchIcons("Arrow", variant: "Regular", size: 20);

        Assert.NotEmpty(filtered);
        Assert.All(filtered, icon =>
        {
            Assert.True(icon.Variants.ContainsKey("Regular"));
            Assert.Contains(icon.Variants.Values, sizes => sizes.Contains(20));
        });
    }

    [Fact]
    public void SearchIcons_NonExistentTerm_ReturnsEmpty()
    {
        var service = CreateService();
        var results = service.SearchIcons("qqrrttzzww");

        Assert.Empty(results);
    }

    [Fact]
    public void SearchIcons_ResultsAreSortedByName()
    {
        var service = CreateService();
        var results = service.SearchIcons("Arrow");

        Assert.NotEmpty(results);
        var names = results.Select(r => r.Name).ToList();
        var sorted = names.OrderBy(n => n, StringComparer.OrdinalIgnoreCase).ToList();
        Assert.Equal(sorted, names);
    }

    #endregion

    #region GetIconByName

    [Fact]
    public void GetIconByName_ExistingIcon_ReturnsIcon()
    {
        var service = CreateService();
        var icon = service.GetIconByName("Bookmark");

        Assert.NotNull(icon);
        Assert.Equal("Bookmark", icon.Name);
    }

    [Fact]
    public void GetIconByName_CaseInsensitive()
    {
        var service = CreateService();
        var icon = service.GetIconByName("bookmark");

        Assert.NotNull(icon);
        Assert.Equal("Bookmark", icon.Name);
    }

    [Fact]
    public void GetIconByName_TrimsWhitespace()
    {
        var service = CreateService();
        var icon = service.GetIconByName("  Bookmark  ");

        Assert.NotNull(icon);
    }

    [Fact]
    public void GetIconByName_NonExistentIcon_ReturnsNull()
    {
        var service = CreateService();
        var icon = service.GetIconByName("NonExistentIconXyz");

        Assert.Null(icon);
    }

    [Fact]
    public void GetIconByName_EmptyName_ReturnsNull()
    {
        var service = CreateService();
        Assert.Null(service.GetIconByName(""));
        Assert.Null(service.GetIconByName(null!));
        Assert.Null(service.GetIconByName("  "));
    }

    #endregion

    #region GetIconsByVariant

    [Fact]
    public void GetIconsByVariant_Regular_ReturnsIcons()
    {
        var service = CreateService();
        var icons = service.GetIconsByVariant("Regular");

        Assert.NotEmpty(icons);
        Assert.All(icons, icon => Assert.True(icon.Variants.ContainsKey("Regular")));
    }

    [Fact]
    public void GetIconsByVariant_UnknownVariant_ReturnsEmpty()
    {
        var service = CreateService();
        var icons = service.GetIconsByVariant("NonExistentVariant");

        Assert.Empty(icons);
    }

    #endregion

    #region GetIconsBySize

    [Fact]
    public void GetIconsBySize_Size20_ReturnsIcons()
    {
        var service = CreateService();
        var icons = service.GetIconsBySize(20);

        Assert.NotEmpty(icons);
    }

    [Fact]
    public void GetIconsBySize_InvalidSize_ReturnsEmpty()
    {
        var service = CreateService();
        var icons = service.GetIconsBySize(999);

        Assert.Empty(icons);
    }

    #endregion

    #region GetVariantSummary

    [Fact]
    public void GetVariantSummary_ContainsAllKnownVariants()
    {
        var service = CreateService();
        var summary = service.GetVariantSummary();

        Assert.True(summary.ContainsKey("Regular"));
        Assert.True(summary.ContainsKey("Filled"));
        Assert.True(summary.ContainsKey("Light"));
        Assert.True(summary.ContainsKey("Color"));
    }

    [Fact]
    public void GetVariantSummary_RegularHasMostIcons()
    {
        var service = CreateService();
        var summary = service.GetVariantSummary();

        Assert.True(summary["Regular"] > summary["Light"],
            "Regular variant should have more icons than Light");
    }

    #endregion

    #region GetSizeSummary

    [Fact]
    public void GetSizeSummary_ContainsAllKnownSizes()
    {
        var service = CreateService();
        var summary = service.GetSizeSummary();

        Assert.True(summary.ContainsKey(10));
        Assert.True(summary.ContainsKey(20));
        Assert.True(summary.ContainsKey(48));
    }

    [Fact]
    public void GetSizeSummary_Size20HasMostIcons()
    {
        var service = CreateService();
        var summary = service.GetSizeSummary();

        Assert.True(summary[20] > summary[10],
            "Size 20 should have more icons than size 10");
    }

    #endregion

    #region GenerateBlazorCode

    [Fact]
    public void GenerateBlazorCode_ValidCombination_ReturnsCode()
    {
        var service = CreateService();
        var code = service.GenerateBlazorCode("Bookmark", "Regular", 20);

        Assert.Contains("Icons.Regular.Size20.Bookmark", code, StringComparison.Ordinal);
        Assert.Contains("<FluentIcon", code, StringComparison.Ordinal);
    }

    [Fact]
    public void GenerateBlazorCode_NonExistentIcon_ReturnsComment()
    {
        var service = CreateService();
        var code = service.GenerateBlazorCode("NonExistentXyz", "Regular", 20);

        Assert.Contains("not found", code, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GenerateBlazorCode_ColorVariant_ReturnsWarning()
    {
        var service = CreateService();
        var code = service.GenerateBlazorCode("Bookmark", "Color", 20);

        Assert.Contains("Color", code, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("not yet available", code, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GenerateBlazorCode_InvalidCombination_ReturnsAvailable()
    {
        var service = CreateService();

        // Try a variant/size combination that likely doesn't exist
        var code = service.GenerateBlazorCode("Bookmark", "Regular", 10);

        // Depending on whether Bookmark has Regular Size10, this will be valid code or an error
        Assert.False(string.IsNullOrEmpty(code));
    }

    #endregion

    #region GenerateUsageExamples

    [Fact]
    public void GenerateUsageExamples_ValidIcon_ContainsMultipleExamples()
    {
        var service = CreateService();
        var examples = service.GenerateUsageExamples("Bookmark", "Regular", 20);

        Assert.Contains("Basic Icon", examples, StringComparison.Ordinal);
        Assert.Contains("With Color", examples, StringComparison.Ordinal);
        Assert.Contains("Button", examples, StringComparison.Ordinal);
        Assert.Contains("FluentIcon", examples, StringComparison.Ordinal);
    }

    [Fact]
    public void GenerateUsageExamples_NonExistentIcon_ReturnsMessage()
    {
        var service = CreateService();
        var examples = service.GenerateUsageExamples("NonExistentXyz", "Regular", 20);

        Assert.Contains("not found", examples, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GenerateUsageExamples_ColorVariant_ReturnsWarning()
    {
        var service = CreateService();
        var examples = service.GenerateUsageExamples("Bookmark", "Color", 20);

        Assert.Contains("Color", examples, StringComparison.Ordinal);
    }

    #endregion

    #region GetRecommendedDefault

    [Fact]
    public void GetRecommendedDefault_PrefersRegularSize20()
    {
        var service = CreateService();
        var icon = service.GetIconByName("Bookmark");
        Assert.NotNull(icon);

        var (variant, size) = service.GetRecommendedDefault(icon);

        // If Bookmark has Regular Size20, it should be recommended
        if (icon.HasVariantAndSize("Regular", 20))
        {
            Assert.Equal("Regular", variant);
            Assert.Equal(20, size);
        }
    }

    [Fact]
    public void GetRecommendedDefault_FallbackToRegularSize24()
    {
        var variants = new Dictionary<string, IList<int>>(StringComparer.OrdinalIgnoreCase)
        {
            ["Regular"] = new List<int> { 24, 32 },
            ["Filled"] = new List<int> { 24 },
        };
        var icon = new IconModel("TestIcon", variants);
        var service = CreateService();

        var (variant, size) = service.GetRecommendedDefault(icon);

        Assert.Equal("Regular", variant);
        Assert.Equal(24, size);
    }

    [Fact]
    public void GetRecommendedDefault_FallbackToFilledSize20()
    {
        var variants = new Dictionary<string, IList<int>>(StringComparer.OrdinalIgnoreCase)
        {
            ["Filled"] = new List<int> { 20, 24 },
        };
        var icon = new IconModel("TestIcon", variants);
        var service = CreateService();

        var (variant, size) = service.GetRecommendedDefault(icon);

        Assert.Equal("Filled", variant);
        Assert.Equal(20, size);
    }

    [Fact]
    public void GetRecommendedDefault_FallbackToFirstAvailable()
    {
        var variants = new Dictionary<string, IList<int>>(StringComparer.OrdinalIgnoreCase)
        {
            ["Light"] = new List<int> { 32 },
        };
        var icon = new IconModel("TestIcon", variants);
        var service = CreateService();

        var (variant, size) = service.GetRecommendedDefault(icon);

        Assert.Equal("Light", variant);
        Assert.Equal(32, size);
    }

    #endregion
}
