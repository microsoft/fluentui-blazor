// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.McpServer.Models;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tests.Models;

/// <summary>
/// Tests for the <see cref="IconModel"/> record.
/// </summary>
public class IconModelTests
{
    private static IconModel CreateIcon(string name, IDictionary<string, IList<int>>? variants = null)
    {
        variants ??= new Dictionary<string, IList<int>>(StringComparer.OrdinalIgnoreCase)
        {
            ["Regular"] = new List<int> { 16, 20, 24 },
            ["Filled"] = new List<int> { 20, 24 },
        };

        return new IconModel(name, variants);
    }

    #region Constructor and Basic Properties

    [Fact]
    public void Constructor_SetsNameCorrectly()
    {
        // Arrange & Act
        var icon = CreateIcon("Bookmark");

        // Assert
        Assert.Equal("Bookmark", icon.Name);
    }

    [Fact]
    public void Constructor_SetsVariantsCorrectly()
    {
        // Arrange & Act
        var icon = CreateIcon("Bookmark");

        // Assert
        Assert.Equal(2, icon.Variants.Count);
        Assert.True(icon.Variants.ContainsKey("Regular"));
        Assert.True(icon.Variants.ContainsKey("Filled"));
    }

    #endregion

    #region VariantNames

    [Fact]
    public void VariantNames_ReturnsAllVariantKeys()
    {
        // Arrange
        var icon = CreateIcon("Bookmark");

        // Act
        var names = icon.VariantNames.ToList();

        // Assert
        Assert.Contains(names, static n => string.Equals(n, "Regular", StringComparison.Ordinal));
        Assert.Contains(names, static n => string.Equals(n, "Filled", StringComparison.Ordinal));
        Assert.Equal(2, names.Count);
    }

    [Fact]
    public void VariantNames_EmptyVariants_ReturnsEmpty()
    {
        // Arrange
        var icon = CreateIcon("Empty", new Dictionary<string, IList<int>>(StringComparer.OrdinalIgnoreCase));

        // Act
        var names = icon.VariantNames.ToList();

        // Assert
        Assert.Empty(names);
    }

    #endregion

    #region AllSizes

    [Fact]
    public void AllSizes_ReturnsDistinctSortedSizes()
    {
        // Arrange
        var icon = CreateIcon("Bookmark");

        // Act
        var sizes = icon.AllSizes.ToList();

        // Assert
        Assert.Equal([16, 20, 24], sizes);
    }

    [Fact]
    public void AllSizes_DeduplicatesSizesAcrossVariants()
    {
        // Arrange
        var variants = new Dictionary<string, IList<int>>(StringComparer.OrdinalIgnoreCase)
        {
            ["Regular"] = new List<int> { 20, 24 },
            ["Filled"] = new List<int> { 20, 24 },
            ["Light"] = new List<int> { 32 },
        };
        var icon = CreateIcon("Star", variants);

        // Act
        var sizes = icon.AllSizes.ToList();

        // Assert
        Assert.Equal([20, 24, 32], sizes);
    }

    #endregion

    #region HasVariantAndSize

    [Fact]
    public void HasVariantAndSize_ExistingCombination_ReturnsTrue()
    {
        var icon = CreateIcon("Bookmark");
        Assert.True(icon.HasVariantAndSize("Regular", 20));
    }

    [Fact]
    public void HasVariantAndSize_NonExistingVariant_ReturnsFalse()
    {
        var icon = CreateIcon("Bookmark");
        Assert.False(icon.HasVariantAndSize("Light", 20));
    }

    [Fact]
    public void HasVariantAndSize_NonExistingSize_ReturnsFalse()
    {
        var icon = CreateIcon("Bookmark");
        Assert.False(icon.HasVariantAndSize("Regular", 48));
    }

    [Fact]
    public void HasVariantAndSize_CaseInsensitiveVariant_ReturnsTrueWithOrdinalDictionary()
    {
        var variants = new Dictionary<string, IList<int>>(StringComparer.OrdinalIgnoreCase)
        {
            ["Regular"] = new List<int> { 20 },
        };
        var icon = CreateIcon("Test", variants);

        Assert.True(icon.HasVariantAndSize("regular", 20));
    }

    #endregion

    #region GetSizesForVariant

    [Fact]
    public void GetSizesForVariant_ExistingVariant_ReturnsSizes()
    {
        var icon = CreateIcon("Bookmark");
        var sizes = icon.GetSizesForVariant("Regular");
        Assert.Equal([16, 20, 24], sizes);
    }

    [Fact]
    public void GetSizesForVariant_NonExistingVariant_ReturnsEmpty()
    {
        var icon = CreateIcon("Bookmark");
        var sizes = icon.GetSizesForVariant("Light");
        Assert.Empty(sizes);
    }

    #endregion
}
