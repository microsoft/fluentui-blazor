// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components.ColorPicker;

/// <summary>
/// Contains layout constants and helpers for the hexagonal color wheel.
/// </summary>
internal static class WheelColor
{
    /// <summary>
    /// Number of hexagons per row in the wheel (diamond shape).
    /// </summary>
    public static readonly int[] RowSizes = [7, 8, 9, 10, 11, 12, 13, 12, 11, 10, 9, 8, 7];

    /// <summary>
    /// Radius (in pixels) of each hexagon cell.
    /// </summary>
    public const double HexSize = 20;

    /// <summary>
    /// Vertical spacing between hexagon rows.
    /// </summary>
    public const double HexYSpacing = 1.5 * HexSize;

    /// <summary>
    /// Width of a single hexagon cell.
    /// </summary>
    public static readonly double HexWidth = Math.Sqrt(3) * HexSize;

    /// <summary>
    /// Padding around the wheel SVG content.
    /// </summary>
    public const double Padding = 5;

    /// <summary>
    /// Horizontal center of the wheel SVG.
    /// </summary>
    public static readonly double CenterX = 7 * HexWidth;

    /// <summary>
    /// SVG viewBox attribute value for the wheel.
    /// </summary>
    public static readonly string ViewBox = FormattableString.Invariant($"0 0 {14 * HexWidth:F1} {(RowSizes.Length - 1) * HexYSpacing + 2 * HexSize + 2 * Padding:F1}");

    /// <summary>
    /// Returns the SVG polygon points attribute for a regular hexagon centered at (<paramref name="cx"/>, <paramref name="cy"/>).
    /// </summary>
    public static string ToHexPoints(double cx, double cy, double hexSize)
    {
        var points = new string[6];
        for (var i = 0; i < 6; i++)
        {
            var angle = Math.PI / 180.0 * (60.0 * i - 90.0);
            points[i] = $"{(cx + hexSize * Math.Cos(angle)).ToInvariant()},{(cy + hexSize * Math.Sin(angle)).ToInvariant()}";
        }

        return string.Join(' ', points);
    }
}
