// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.McpServer.Services;
using Microsoft.FluentUI.AspNetCore.McpServer.Tools;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tests.Tools;

/// <summary>
/// Tests for the <see cref="IconTools"/> class.
/// </summary>
public class IconToolsTests
{
    private static IconTools CreateTools()
    {
        var synonymService = new IconSynonymService();
        var iconService = new IconService(synonymService);
        return new IconTools(iconService);
    }

    #region SearchIcons Tool

    [Fact]
    public void SearchIcons_EmptyTerm_ReturnsHelpMessage()
    {
        var tools = CreateTools();
        var result = tools.SearchIcons("");

        Assert.Contains("provide a search term", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void SearchIcons_ValidTerm_ReturnsMarkdownTable()
    {
        var tools = CreateTools();
        var result = tools.SearchIcons("Bookmark");

        Assert.Contains("Icon Search Results", result, StringComparison.Ordinal);
        Assert.Contains("Icon Name", result, StringComparison.Ordinal);
        Assert.Contains("Variants", result, StringComparison.Ordinal);
        Assert.Contains("Bookmark", result, StringComparison.Ordinal);
    }

    [Fact]
    public void SearchIcons_SynonymTerm_FindsIcons()
    {
        var tools = CreateTools();
        var result = tools.SearchIcons("trash");

        Assert.Contains("Icon Search Results", result, StringComparison.Ordinal);
        Assert.Contains("Delete", result, StringComparison.Ordinal);
    }

    [Fact]
    public void SearchIcons_NoResults_ReturnsTips()
    {
        var tools = CreateTools();
        var result = tools.SearchIcons("qqrrttzzww");

        Assert.Contains("No icons found", result, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("Tips", result, StringComparison.Ordinal);
    }

    [Fact]
    public void SearchIcons_WithVariantFilter_ShowsFilter()
    {
        var tools = CreateTools();
        var result = tools.SearchIcons("Arrow", variant: "Regular");

        Assert.Contains("variant=Regular", result, StringComparison.Ordinal);
    }

    [Fact]
    public void SearchIcons_WithSizeFilter_ShowsFilter()
    {
        var tools = CreateTools();
        var result = tools.SearchIcons("Arrow", size: 20);

        Assert.Contains("size=20", result, StringComparison.Ordinal);
    }

    [Fact]
    public void SearchIcons_ResultsContainCodeExample()
    {
        var tools = CreateTools();
        var result = tools.SearchIcons("Bookmark");

        Assert.Contains("Icons.", result, StringComparison.Ordinal);
        Assert.Contains("Bookmark()", result, StringComparison.Ordinal);
    }

    [Fact]
    public void SearchIcons_ResultsContainUsageFooter()
    {
        var tools = CreateTools();
        var result = tools.SearchIcons("Bookmark");

        Assert.Contains("**Usage:**", result, StringComparison.Ordinal);
        Assert.Contains("FluentIcon", result, StringComparison.Ordinal);
    }

    #endregion

    #region GetIconDetails Tool

    [Fact]
    public void GetIconDetails_EmptyName_ReturnsHelpMessage()
    {
        var tools = CreateTools();
        var result = tools.GetIconDetails("");

        Assert.Contains("provide an icon name", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetIconDetails_ValidIcon_ReturnsDetails()
    {
        var tools = CreateTools();
        var result = tools.GetIconDetails("Bookmark");

        Assert.Contains("# Bookmark", result, StringComparison.Ordinal);
        Assert.Contains("Recommended default", result, StringComparison.Ordinal);
        Assert.Contains("Available Variants and Sizes", result, StringComparison.Ordinal);
        Assert.Contains("Quick Start", result, StringComparison.Ordinal);
    }

    [Fact]
    public void GetIconDetails_NonExistentIcon_ReturnsSuggestions()
    {
        var tools = CreateTools();
        var result = tools.GetIconDetails("Bookmar"); // Typo - partial match

        // Should suggest similar icons
        Assert.Contains("Bookmark", result, StringComparison.Ordinal);
    }

    [Fact]
    public void GetIconDetails_CompletelyUnknown_ReturnsNotFound()
    {
        var tools = CreateTools();
        var result = tools.GetIconDetails("zzzznonexistentxyz");

        Assert.Contains("not found", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetIconDetails_ValidIcon_ContainsVariantSections()
    {
        var tools = CreateTools();
        var result = tools.GetIconDetails("Bookmark");

        // Should contain at least one variant section
        Assert.Contains("###", result, StringComparison.Ordinal);
    }

    [Fact]
    public void GetIconDetails_ValidIcon_ContainsCodeSnippet()
    {
        var tools = CreateTools();
        var result = tools.GetIconDetails("Bookmark");

        Assert.Contains("```razor", result, StringComparison.Ordinal);
        Assert.Contains("FluentIcon", result, StringComparison.Ordinal);
    }

    #endregion

    #region GetIconUsage Tool

    [Fact]
    public void GetIconUsage_ValidIcon_ReturnsExamples()
    {
        var tools = CreateTools();
        var result = tools.GetIconUsage("Bookmark", "Regular", 20);

        Assert.Contains("Basic Icon", result, StringComparison.Ordinal);
        Assert.Contains("FluentIcon", result, StringComparison.Ordinal);
    }

    [Fact]
    public void GetIconUsage_NonExistentIcon_ReturnsNotFound()
    {
        var tools = CreateTools();
        var result = tools.GetIconUsage("NonExistentXyz");

        Assert.Contains("not found", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetIconUsage_ColorVariant_ReturnsWarning()
    {
        var tools = CreateTools();
        var result = tools.GetIconUsage("Bookmark", "Color", 20);

        Assert.Contains("Color", result, StringComparison.Ordinal);
    }

    [Fact]
    public void GetIconUsage_DefaultParameters_UsesRegularSize20()
    {
        var tools = CreateTools();
        var result = tools.GetIconUsage("Bookmark");

        Assert.Contains("Regular", result, StringComparison.Ordinal);
    }

    #endregion

    #region ListAllIconNames Tool

    [Fact]
    public void ListAllIconNames_NoPrefix_ReturnsAllGroupedByLetter()
    {
        var tools = CreateTools();
        var result = tools.ListAllIconNames();

        Assert.Contains("All Fluent UI Icons", result, StringComparison.Ordinal);
        Assert.Contains("## A", result, StringComparison.Ordinal);
        Assert.Contains("## B", result, StringComparison.Ordinal);
        Assert.Contains("icons)", result, StringComparison.Ordinal);
    }

    [Fact]
    public void ListAllIconNames_NullPrefix_ReturnsAllGroupedByLetter()
    {
        var tools = CreateTools();
        var result = tools.ListAllIconNames(prefix: null);

        Assert.Contains("All Fluent UI Icons", result, StringComparison.Ordinal);
    }

    [Fact]
    public void ListAllIconNames_EmptyPrefix_ReturnsAllGroupedByLetter()
    {
        var tools = CreateTools();
        var result = tools.ListAllIconNames(prefix: "");

        Assert.Contains("All Fluent UI Icons", result, StringComparison.Ordinal);
    }

    [Fact]
    public void ListAllIconNames_SingleLetterPrefix_ReturnsFilteredList()
    {
        var tools = CreateTools();
        var result = tools.ListAllIconNames(prefix: "B");

        Assert.Contains("Icons starting with 'B'", result, StringComparison.Ordinal);
        Assert.Contains("Bookmark", result, StringComparison.Ordinal);
        Assert.DoesNotContain("## A", result, StringComparison.Ordinal);
    }

    [Fact]
    public void ListAllIconNames_MultiCharPrefix_ReturnsFilteredList()
    {
        var tools = CreateTools();
        var result = tools.ListAllIconNames(prefix: "Arrow");

        Assert.Contains("Icons starting with 'Arrow'", result, StringComparison.Ordinal);
        Assert.Contains("ArrowLeft", result, StringComparison.Ordinal);
        Assert.DoesNotContain("Bookmark", result, StringComparison.Ordinal);
    }

    [Fact]
    public void ListAllIconNames_PrefixCaseInsensitive_ReturnsResults()
    {
        var tools = CreateTools();
        var result = tools.ListAllIconNames(prefix: "arrow");

        Assert.Contains("Icons starting with 'arrow'", result, StringComparison.Ordinal);
        Assert.Contains("ArrowLeft", result, StringComparison.Ordinal);
    }

    [Fact]
    public void ListAllIconNames_NonMatchingPrefix_ReturnsNoResults()
    {
        var tools = CreateTools();
        var result = tools.ListAllIconNames(prefix: "Zzzzqqqq");

        Assert.Contains("No icons found starting with 'Zzzzqqqq'", result, StringComparison.Ordinal);
    }

    [Fact]
    public void ListAllIconNames_FilteredResult_ContainsUsageHint()
    {
        var tools = CreateTools();
        var result = tools.ListAllIconNames(prefix: "Book");

        Assert.Contains("GetIconDetails", result, StringComparison.Ordinal);
    }

    [Fact]
    public void ListAllIconNames_FullList_ContainsUsageHint()
    {
        var tools = CreateTools();
        var result = tools.ListAllIconNames();

        Assert.Contains("GetIconDetails", result, StringComparison.Ordinal);
    }

    [Fact]
    public void ListAllIconNames_FullList_ContainsTotalCount()
    {
        var tools = CreateTools();
        var result = tools.ListAllIconNames();

        // The catalog has thousands of icons; check that the count is displayed
        Assert.Matches(@"\(\d+ icons\)", result);
    }

    #endregion
}
