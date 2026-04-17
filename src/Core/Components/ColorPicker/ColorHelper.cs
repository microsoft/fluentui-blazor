// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Globalization;

namespace Microsoft.FluentUI.AspNetCore.Components.ColorPicker;

/// <summary>
/// Provides color matching utilities for finding the closest color in a palette.
/// </summary>
internal static class ColorHelper
{
    /// <summary>
    /// Formats a <see cref="double"/> value as an invariant string with one decimal place.
    /// </summary>
    internal static string ToInvariant(this double value)
    {
        return value.ToString("F1", CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Finds the closest color in the given palette to the specified hex color.
    /// Uses a weighted Euclidean distance in RGB space for perceptual accuracy.
    /// </summary>
    /// <param name="hexColor">The target hex color (e.g. "#FF0000").</param>
    /// <param name="palette">The palette of hex colors to search.</param>
    /// <returns>The closest matching hex color from the palette, or <c>null</c> if no match is found.</returns>
    internal static string? FindClosestColor(string hexColor, IReadOnlyList<string> palette)
    {
        if (string.IsNullOrWhiteSpace(hexColor) || palette.Count == 0)
        {
            return null;
        }

        // If color already exists in palette, return it directly
        foreach (var c in palette)
        {
            if (string.Equals(c, hexColor, StringComparison.OrdinalIgnoreCase))
            {
                return c;
            }
        }

        if (!TryParseHexColor(hexColor, out var target))
        {
            return null;
        }

        string? closest = null;
        var minDistance = double.MaxValue;

        foreach (var hex in palette)
        {
            if (!TryParseHexColor(hex, out var color))
            {
                continue;
            }

            var distance = ColorDistance(target, color);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = hex;
            }
        }

        return closest;
    }

    /// <summary>
    /// Computes a weighted Euclidean distance between two RGB colors.
    /// The weighting accounts for human color perception where green
    /// differences are more noticeable than red or blue.
    /// </summary>
    private static double ColorDistance((int R, int G, int B) c1, (int R, int G, int B) c2)
    {
        var rMean = (c1.R + c2.R) / 2.0;
        var dR = c1.R - c2.R;
        var dG = c1.G - c2.G;
        var dB = c1.B - c2.B;

        return Math.Sqrt(
            (2 + rMean / 256) * dR * dR +
            4 * dG * dG +
            (2 + (255 - rMean) / 256) * dB * dB);
    }

    /// <summary>
    /// Tries to parse a hex color string into its RGB components.
    /// </summary>
    private static bool TryParseHexColor(string hex, out (int R, int G, int B) color)
    {
        color = default;

        if (string.IsNullOrWhiteSpace(hex))
        {
            return false;
        }

        var span = hex.AsSpan().TrimStart('#');
        if (span.Length != 6)
        {
            return false;
        }

        if (int.TryParse(span[..2], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var r) &&
            int.TryParse(span[2..4], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var g) &&
            int.TryParse(span[4..6], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var b))
        {
            color = (r, g, b);
            return true;
        }

        return false;
    }
}
