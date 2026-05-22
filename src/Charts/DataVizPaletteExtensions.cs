// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.Extensions;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Extension methods for <see cref="DataVizPalette"/>.
/// </summary>
public static class DataVizPaletteExtensions
{
    // Static palette data
    // Index 0 = light theme, index 1 = dark theme (falls back to index 0 when absent).
    private static readonly Dictionary<DataVizPalette, string[]> s_palette = new()
    {
        // Qualitative
        [DataVizPalette.Color1] = ["#637cef"],
        [DataVizPalette.Color2] = ["#e3008c"],
        [DataVizPalette.Color3] = ["#2aa0a4"],
        [DataVizPalette.Color4] = ["#9373c0"],
        [DataVizPalette.Color5] = ["#13a10e"],
        [DataVizPalette.Color6] = ["#3a96dd"],
        [DataVizPalette.Color7] = ["#ca5010"],
        [DataVizPalette.Color8] = ["#57811b"],
        [DataVizPalette.Color9] = ["#b146c2"],
        [DataVizPalette.Color10] = ["#ae8c00"],
        [DataVizPalette.Color11] = ["#3c51b4", "#93a4f4"],
        [DataVizPalette.Color12] = ["#ad006a", "#ee5fb7"],
        [DataVizPalette.Color13] = ["#026467", "#4cb4b7"],
        [DataVizPalette.Color14] = ["#674c8c", "#a083c9"],
        [DataVizPalette.Color15] = ["#0e7a0b", "#27ac22"],
        [DataVizPalette.Color16] = ["#2c72a8", "#4fa1e1"],
        [DataVizPalette.Color17] = ["#9a3d0c", "#d77440"],
        [DataVizPalette.Color18] = ["#405f14", "#73aa24"],
        [DataVizPalette.Color19] = ["#863593", "#c36bd1"],
        [DataVizPalette.Color20] = ["#6d5700", "#d0b232"],
        [DataVizPalette.Color21] = ["#4f6bed"],
        [DataVizPalette.Color22] = ["#ea38a6"],
        [DataVizPalette.Color23] = ["#038387"],
        [DataVizPalette.Color24] = ["#8764b8"],
        [DataVizPalette.Color25] = ["#11910d"],
        [DataVizPalette.Color26] = ["#3487c7"],
        [DataVizPalette.Color27] = ["#d06228"],
        [DataVizPalette.Color28] = ["#689920"],
        [DataVizPalette.Color29] = ["#ba58c9"],
        [DataVizPalette.Color30] = ["#937700", "#c19c00"],
        [DataVizPalette.Color31] = ["#2c3c85", "#c8d1fa"],
        [DataVizPalette.Color32] = ["#7f004e", "#f7adda"],
        [DataVizPalette.Color33] = ["#02494c", "#9bd9db"],
        [DataVizPalette.Color34] = ["#4c3867", "#b29ad4"],
        [DataVizPalette.Color35] = ["#0b5a08", "#a7e3a5"],
        [DataVizPalette.Color36] = ["#20547c", "#83bdeb"],
        [DataVizPalette.Color37] = ["#712d09", "#df8e64"],
        [DataVizPalette.Color38] = ["#23330b", "#a4cc6c"],
        [DataVizPalette.Color39] = ["#63276d", "#cf87da"],
        [DataVizPalette.Color40] = ["#3a2f00", "#dac157"],
        // Semantic
        [DataVizPalette.Info] = ["#015cda"],
        [DataVizPalette.Disabled] = ["#dbdbdb", "#4d4d4d"],
        [DataVizPalette.HighError] = ["#6e0811", "#cc2635"],
        [DataVizPalette.Error] = ["#c50f1f", "#dc626d"],
        [DataVizPalette.Warning] = ["#f7630c", "#f87528"],
        [DataVizPalette.Success] = ["#107c10", "#54b054"],
        [DataVizPalette.HighSuccess] = ["#094509", "#218c21"],
    };

    // Reverse lookup: token string (e.g. "color5") → DataVizPalette enum value.
    private static readonly Dictionary<string, DataVizPalette> s_tokenMap =
        Enum.GetValues<DataVizPalette>()
            .Where(v => v != DataVizPalette.Custom)
            .ToDictionary(
                v => v.ToAttributeValue()!,
                v => v,
                StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Resolves a color token string (e.g. <c>"color5"</c>, <c>"info"</c>) to its hex color,
    /// using the static palette table. Non-token strings (e.g. custom hex values) are returned
    /// unchanged; <see langword="null"/> returns an empty string.
    /// </summary>
    /// <param name="token">
    /// A <see cref="DataVizPalette"/> token string, a custom hex/CSS color, or <see langword="null"/>.
    /// </param>
    /// <param name="isDarkTheme">
    /// <see langword="true"/> to return the dark-theme variant; <see langword="false"/> (default)
    /// for the light-theme variant.
    /// </param>
    /// <returns>A hex color string, or the original value if it is not a recognized token.</returns>
    public static string ToHex(this string? token, bool isDarkTheme = false)
    {
        if (token is null)
        {
            return string.Empty;
        }

        var palette = FromToken(token);
        return palette.HasValue ? palette.Value.ToHex(isDarkTheme) : token;
    }

    /// <summary>
    /// Parses a token string (e.g. <c>"color5"</c>, <c>"info"</c>) back to its
    /// <see cref="DataVizPalette"/> enum value, or <see langword="null"/> if the
    /// string is not a recognized token (e.g. it is a custom hex color).
    /// </summary>
    public static DataVizPalette? FromToken(string? token)
        => token is not null && s_tokenMap.TryGetValue(token, out var value) ? value : null;

    /// <summary>
    /// Returns the hex color string for this palette value using the static palette table.
    /// </summary>
    /// <param name="palette">The palette value to resolve.</param>
    /// <param name="isDarkTheme">
    /// <see langword="true"/> to return the dark-theme variant; <see langword="false"/> (default)
    /// for the light-theme variant. When no dark variant exists, the light color is returned.
    /// </param>
    /// <returns>A hex color string such as <c>#637cef</c>, or an empty string for <see cref="DataVizPalette.Custom"/>.</returns>
    public static string ToHex(this DataVizPalette palette, bool isDarkTheme = false)
    {
        if (!s_palette.TryGetValue(palette, out var colors))
        {
            return string.Empty;
        }

        return isDarkTheme && colors.Length > 1 ? colors[1] : colors[0];
    }

    /// <summary>
    /// Returns the hex color string for this nullable palette value, or an empty string when <see langword="null"/>.
    /// </summary>
    /// <param name="palette">The nullable palette value to resolve.</param>
    /// <param name="isDarkTheme">
    /// <see langword="true"/> to return the dark-theme variant; <see langword="false"/> (default)
    /// for the light-theme variant.
    /// </param>
    public static string ToHex(this DataVizPalette? palette, bool isDarkTheme = false)
        => palette.HasValue ? palette.Value.ToHex(isDarkTheme) : string.Empty;

    /// <summary>
    /// Returns the hex color string for this nullable palette value, falling back to
    /// <paramref name="fallback"/> when the value is <see langword="null"/> (i.e. a custom color).
    /// </summary>
    /// <param name="palette">The nullable palette value to resolve.</param>
    /// <param name="fallback">The raw CSS color string to return when <paramref name="palette"/> is <see langword="null"/>.</param>
    /// <param name="isDarkTheme">
    /// <see langword="true"/> to return the dark-theme variant; <see langword="false"/> (default)
    /// for the light-theme variant.
    /// </param>
    public static string ToHex(this DataVizPalette? palette, string? fallback, bool isDarkTheme = false)
        => palette.HasValue ? palette.Value.ToHex(isDarkTheme) : (fallback ?? string.Empty);

}
