// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.ColorPicker;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.ColorPicker;

public class ColorHelperTests
{
    [Fact]
    public void FindClosestColor_ExactMatch_ReturnsPaletteColor()
    {
        // Arrange
        var palette = new[] { "#FF0000", "#00FF00", "#0000FF" };

        // Act
        var result = ColorHelper.FindClosestColor("#FF0000", palette);

        // Assert
        Assert.Equal("#FF0000", result);
    }

    [Fact]
    public void FindClosestColor_ExactMatchCaseInsensitive_ReturnsPaletteColor()
    {
        // Arrange
        var palette = new[] { "#FF0000", "#00FF00", "#0000FF" };

        // Act
        var result = ColorHelper.FindClosestColor("#ff0000", palette);

        // Assert
        Assert.Equal("#FF0000", result);
    }

    [Fact]
    public void FindClosestColor_NotInPalette_ReturnsClosest()
    {
        // Arrange
        var palette = new[] { "#FF0000", "#00FF00", "#0000FF" };

        // Act - #FE0101 is very close to red
        var result = ColorHelper.FindClosestColor("#FE0101", palette);

        // Assert
        Assert.Equal("#FF0000", result);
    }

    [Fact]
    public void FindClosestColor_SaddleBrown_FindsClosestInDefaultPalette()
    {
        // Arrange - SaddleBrown (#8B4513) is not in the default palette
        // Act
        var result = ColorHelper.FindClosestColor("#8B4513", DefaultColors.SwatchColors);

        // Assert
        Assert.NotNull(result);
        Assert.Contains(result, DefaultColors.SwatchColors);
    }

    [Fact]
    public void FindClosestColor_NullOrEmpty_ReturnsNull()
    {
        // Arrange
        var palette = new[] { "#FF0000" };

        // Act & Assert
        Assert.Null(ColorHelper.FindClosestColor(null!, palette));
        Assert.Null(ColorHelper.FindClosestColor("", palette));
        Assert.Null(ColorHelper.FindClosestColor("  ", palette));
    }

    [Fact]
    public void FindClosestColor_EmptyPalette_ReturnsNull()
    {
        // Act
        var result = ColorHelper.FindClosestColor("#FF0000", Array.Empty<string>());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void FindClosestColor_InvalidHex_ReturnsNull()
    {
        // Arrange
        var palette = new[] { "#FF0000", "#00FF00" };

        // Act
        var result = ColorHelper.FindClosestColor("invalid", palette);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void FindClosestColor_CloserToGreen_ReturnsGreen()
    {
        // Arrange
        var palette = new[] { "#FF0000", "#00FF00", "#0000FF" };

        // Act - #10EE10 is close to green
        var result = ColorHelper.FindClosestColor("#10EE10", palette);

        // Assert
        Assert.Equal("#00FF00", result);
    }

    [Fact]
    public void FindClosestColor_CloserToBlue_ReturnsBlue()
    {
        // Arrange
        var palette = new[] { "#FF0000", "#00FF00", "#0000FF" };

        // Act - #0000FE is very close to blue
        var result = ColorHelper.FindClosestColor("#0000FE", palette);

        // Assert
        Assert.Equal("#0000FF", result);
    }

    [Fact]
    public void FindClosestColor_ShortHex_ReturnsNull()
    {
        // Arrange
        var palette = new[] { "#FF0000" };

        // Act - 3-char hex is not supported by the parser
        var result = ColorHelper.FindClosestColor("#FFF", palette);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void FindClosestColor_SkipsInvalidPaletteEntries()
    {
        // Arrange - one entry is invalid, one is valid
        var palette = new[] { "not-a-color", "#00FF00" };

        // Act
        var result = ColorHelper.FindClosestColor("#10EE10", palette);

        // Assert - the invalid entry should be skipped and the valid one returned
        Assert.Equal("#00FF00", result);
    }

    [Fact]
    public void FindClosestColor_NoValidColorsInPalette_ReturnsNull()
    {
        // Arrange
        var palette = new[] { "invalid1", "invalid2" };

        // Act
        var result = ColorHelper.FindClosestColor("#FF0000", palette);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ToInvariant_FormatsWithOneDecimal()
    {
        // Act && Assert
        Assert.Equal("1.5", 1.5.ToInvariant());
        Assert.Equal("0.0", 0.0.ToInvariant());
    }
}
