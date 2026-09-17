// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.ColorPicker;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.ColorPicker;

public class WheelColorTests
{
    [Fact]
    public void RowSizes_HasThirteenRows()
    {
        Assert.Equal(13, WheelColor.RowSizes.Length);
        Assert.Equal(13, WheelColor.RowSizes[6]); // middle row
    }

    [Fact]
    public void ViewBox_IsInvariantAndFormatted()
    {
        // Act
        var viewBox = WheelColor.ViewBox;

        // Assert - should not contain comma (invariant) and start with "0 0"
        Assert.StartsWith("0 0 ", viewBox);
        Assert.DoesNotContain(',', viewBox);
        Assert.Contains('.', viewBox);
    }

    [Fact]
    public void ToHexPoints_ReturnsSixCommaSeparatedPoints()
    {
        // Act
        var points = WheelColor.ToHexPoints(50, 50, 20);

        // Assert
        var parts = points.Split(' ');
        Assert.Equal(6, parts.Length);
        Assert.All(parts, p => Assert.Contains(',', p));
    }

    [Fact]
    public void ToHexPoints_UsesInvariantCulture()
    {
        // Act
        var points = WheelColor.ToHexPoints(10.5, 20.5, 15);

        // Assert - F1 formatting should use a '.' as decimal separator
        Assert.Contains('.', points);
    }
}
