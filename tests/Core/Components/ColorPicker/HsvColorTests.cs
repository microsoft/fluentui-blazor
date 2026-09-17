// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.ColorPicker;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.ColorPicker;

public class HsvColorTests
{
    [Fact]
    public void Default_ReturnsRed()
    {
        // Act
        var hsv = HsvColor.Default;

        // Assert
        Assert.Equal(0, hsv.Hue);
        Assert.Equal(1, hsv.Saturation);
        Assert.Equal(1, hsv.Value);
    }

    [Fact]
    public void FromHex_RedWithHash_ReturnsRedHsv()
    {
        // Act
        var hsv = HsvColor.FromHex("#FF0000");

        // Assert
        Assert.Equal(0, hsv.Hue);
        Assert.Equal(1, hsv.Saturation);
        Assert.Equal(1, hsv.Value);
    }

    [Fact]
    public void FromHex_RedWithoutHash_ReturnsRedHsv()
    {
        // Act
        var hsv = HsvColor.FromHex("FF0000");

        // Assert
        Assert.Equal(0, hsv.Hue);
        Assert.Equal(1, hsv.Saturation);
        Assert.Equal(1, hsv.Value);
    }

    [Fact]
    public void FromHex_Green_ReturnsHueAt120()
    {
        // Act
        var hsv = HsvColor.FromHex("#00FF00");

        // Assert
        Assert.Equal(120, hsv.Hue);
        Assert.Equal(1, hsv.Saturation);
        Assert.Equal(1, hsv.Value);
    }

    [Fact]
    public void FromHex_Blue_ReturnsHueAt240()
    {
        // Act
        var hsv = HsvColor.FromHex("#0000FF");

        // Assert
        Assert.Equal(240, hsv.Hue);
        Assert.Equal(1, hsv.Saturation);
        Assert.Equal(1, hsv.Value);
    }

    [Fact]
    public void FromHex_Black_ReturnsZeroSaturationAndValue()
    {
        // Act
        var hsv = HsvColor.FromHex("#000000");

        // Assert
        Assert.Equal(0, hsv.Hue);
        Assert.Equal(0, hsv.Saturation);
        Assert.Equal(0, hsv.Value);
    }

    [Fact]
    public void FromHex_White_ReturnsZeroSaturationFullValue()
    {
        // Act
        var hsv = HsvColor.FromHex("#FFFFFF");

        // Assert
        Assert.Equal(0, hsv.Hue);
        Assert.Equal(0, hsv.Saturation);
        Assert.Equal(1, hsv.Value);
    }

    [Fact]
    public void FromHex_PinkRedDominant_ReturnsHueWrappedTo330()
    {
        // Arrange - r=255, g=0, b=128 -> max=r, (g-b)/delta = -0.5 => hue = -30 -> +360 = 330
        // Act
        var hsv = HsvColor.FromHex("#FF0080");

        // Assert
        Assert.InRange(hsv.Hue, 329, 331);
    }

    [Fact]
    public void FromHex_InvalidShortString_ReturnsDefault()
    {
        // Act
        var hsv = HsvColor.FromHex("#FFF");

        // Assert
        Assert.Equal(HsvColor.Default, hsv);
    }

    [Fact]
    public void SquareStyle_UsesInvariantCulture()
    {
        // Arrange
        var hsv = new HsvColor(120.5, 0.5, 0.5);

        // Act
        var style = hsv.SquareStyle;

        // Assert
        Assert.Equal("background-color: hsl(120.5, 100%, 50%);", style);
    }

    [Fact]
    public void IndicatorLeft_FormatsSaturationAsPercent()
    {
        // Arrange
        var hsv = new HsvColor(0, 0.5, 1);

        // Act && Assert
        Assert.Equal("50.0%", hsv.IndicatorLeft);
    }

    [Fact]
    public void IndicatorTop_FormatsInvertedValueAsPercent()
    {
        // Arrange
        var hsv = new HsvColor(0, 1, 0.25);

        // Act && Assert
        Assert.Equal("75.0%", hsv.IndicatorTop);
    }

    [Fact]
    public void HueIndicatorTop_FormatsHueRatioAsPercent()
    {
        // Arrange
        var hsv = new HsvColor(180, 1, 1);

        // Act && Assert
        Assert.Equal("50.0%", hsv.HueIndicatorTop);
    }
}
