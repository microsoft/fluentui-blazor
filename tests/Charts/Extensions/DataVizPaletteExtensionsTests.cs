// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.Charts;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Charts.Extensions;

public class DataVizPaletteExtensionsTests
{
    // ── TryGetDataVizPaletteFromToken ─────────────────────────────────────────

    [Fact]
    public void TryGetDataVizPaletteFromToken_Null_ReturnsNull()
    {
        Assert.Null(DataVizPaletteExtensions.TryGetDataVizPaletteFromToken(null));
    }

    [Fact]
    public void TryGetDataVizPaletteFromToken_EmptyString_ReturnsNull()
    {
        Assert.Null(DataVizPaletteExtensions.TryGetDataVizPaletteFromToken(string.Empty));
    }

    [Fact]
    public void TryGetDataVizPaletteFromToken_CustomHexString_ReturnsNull()
    {
        Assert.Null(DataVizPaletteExtensions.TryGetDataVizPaletteFromToken("#FF5733"));
    }

    [Fact]
    public void TryGetDataVizPaletteFromToken_CssVariable_ReturnsNull()
    {
        Assert.Null(DataVizPaletteExtensions.TryGetDataVizPaletteFromToken("var(--brand)"));
    }

    [Fact]
    public void TryGetDataVizPaletteFromToken_CustomToken_ReturnsNull()
    {
        // "custom" is deliberately excluded from the TokenMap
        Assert.Null(DataVizPaletteExtensions.TryGetDataVizPaletteFromToken("custom"));
    }

    [Theory]
    [InlineData("color1",   DataVizPalette.Color1)]
    [InlineData("color5",   DataVizPalette.Color5)]
    [InlineData("color10",  DataVizPalette.Color10)]
    [InlineData("color20",  DataVizPalette.Color20)]
    [InlineData("color40",  DataVizPalette.Color40)]
    [InlineData("info",     DataVizPalette.Info)]
    [InlineData("disabled", DataVizPalette.Disabled)]
    [InlineData("error",    DataVizPalette.Error)]
    [InlineData("warning",  DataVizPalette.Warning)]
    [InlineData("success",  DataVizPalette.Success)]
    public void TryGetDataVizPaletteFromToken_RecognizedToken_ReturnsEnumValue(string token, DataVizPalette expected)
    {
        Assert.Equal(expected, DataVizPaletteExtensions.TryGetDataVizPaletteFromToken(token));
    }

    [Theory]
    [InlineData("COLOR1")]
    [InlineData("Color1")]
    [InlineData("INFO")]
    [InlineData("Error")]
    public void TryGetDataVizPaletteFromToken_IsCaseInsensitive(string token)
    {
        Assert.NotNull(DataVizPaletteExtensions.TryGetDataVizPaletteFromToken(token));
    }

    // ── ToDataVizPaletteHex (DataVizPalette, bool) ────────────────────────────

    [Fact]
    public void ToDataVizPaletteHex_Enum_Custom_ReturnsEmptyString()
    {
        // DataVizPalette.Custom is not in the Palette dictionary
        Assert.Equal(string.Empty, DataVizPalette.Custom.ToDataVizPaletteHex());
    }

    [Theory]
    [InlineData(DataVizPalette.Color1,  "#637cef")]
    [InlineData(DataVizPalette.Color2,  "#e3008c")]
    [InlineData(DataVizPalette.Color3,  "#2aa0a4")]
    [InlineData(DataVizPalette.Color5,  "#13a10e")]
    [InlineData(DataVizPalette.Color10, "#ae8c00")]
    [InlineData(DataVizPalette.Info,    "#015cda")]
    [InlineData(DataVizPalette.Error,   "#c50f1f")]
    [InlineData(DataVizPalette.Warning, "#f7630c")]
    [InlineData(DataVizPalette.Success, "#107c10")]
    public void ToDataVizPaletteHex_Enum_LightTheme_ReturnsExpectedHex(DataVizPalette palette, string expectedHex)
    {
        Assert.Equal(expectedHex, palette.ToDataVizPaletteHex(isDarkTheme: false));
    }

    [Theory]
    [InlineData(DataVizPalette.Color11, false, "#3c51b4")]
    [InlineData(DataVizPalette.Color11, true,  "#93a4f4")]
    [InlineData(DataVizPalette.Color12, false, "#ad006a")]
    [InlineData(DataVizPalette.Color12, true,  "#ee5fb7")]
    [InlineData(DataVizPalette.Disabled, false, "#dbdbdb")]
    [InlineData(DataVizPalette.Disabled, true,  "#4d4d4d")]
    [InlineData(DataVizPalette.Error,    false, "#c50f1f")]
    [InlineData(DataVizPalette.Error,    true,  "#dc626d")]
    [InlineData(DataVizPalette.Success,  false, "#107c10")]
    [InlineData(DataVizPalette.Success,  true,  "#54b054")]
    public void ToDataVizPaletteHex_Enum_DarkThemeVariant_ReturnsCorrectColor(
        DataVizPalette palette, bool isDark, string expectedHex)
    {
        Assert.Equal(expectedHex, palette.ToDataVizPaletteHex(isDarkTheme: isDark));
    }

    [Theory]
    [InlineData(DataVizPalette.Color1)]
    [InlineData(DataVizPalette.Color5)]
    [InlineData(DataVizPalette.Info)]
    public void ToDataVizPaletteHex_Enum_SingleEntryPalette_DarkTheme_ReturnsSameLightHex(DataVizPalette palette)
    {
        // Colors with only one entry in the palette fall back to index 0 for dark theme too
        var light = palette.ToDataVizPaletteHex(isDarkTheme: false);
        var dark  = palette.ToDataVizPaletteHex(isDarkTheme: true);

        Assert.Equal(light, dark);
    }

    [Fact]
    public void ToDataVizPaletteHex_Enum_ReturnsHexStartingWithHash()
    {
        var hex = DataVizPalette.Color3.ToDataVizPaletteHex();

        Assert.StartsWith("#", hex);
    }

    // ── ToDataVizPaletteHex (DataVizPalette?, bool) — nullable, no fallback ───

    [Fact]
    public void ToDataVizPaletteHex_NullablePalette_Null_ReturnsEmptyString()
    {
        DataVizPalette? palette = null;

        Assert.Equal(string.Empty, palette.ToDataVizPaletteHex());
    }

    [Fact]
    public void ToDataVizPaletteHex_NullablePalette_WithValue_ReturnsPaletteHex()
    {
        DataVizPalette? palette = DataVizPalette.Color4;

        Assert.Equal("#9373c0", palette.ToDataVizPaletteHex());
    }

    [Fact]
    public void ToDataVizPaletteHex_NullablePalette_DarkTheme_DelegatesToNonNullable()
    {
        DataVizPalette? palette = DataVizPalette.Color11;

        Assert.Equal(DataVizPalette.Color11.ToDataVizPaletteHex(isDarkTheme: true),
                     palette.ToDataVizPaletteHex(isDarkTheme: true));
    }

    // ── ToDataVizPaletteHex (DataVizPalette?, string?, bool) — with fallback ──

    [Fact]
    public void ToDataVizPaletteHex_WithFallback_Null_ReturnsEmptyString()
    {
        DataVizPalette? palette = null;

        Assert.Equal(string.Empty, palette.ToDataVizPaletteHex(fallback: null));
    }

    [Fact]
    public void ToDataVizPaletteHex_WithFallback_Null_ReturnsCustomColorFallback()
    {
        DataVizPalette? palette = null;

        Assert.Equal("#A23B72", palette.ToDataVizPaletteHex(fallback: "#A23B72"));
    }

    [Fact]
    public void ToDataVizPaletteHex_WithFallback_NullPaletteAndCssVariable_ReturnsCssVariable()
    {
        DataVizPalette? palette = null;

        Assert.Equal("var(--brand)", palette.ToDataVizPaletteHex(fallback: "var(--brand)"));
    }

    [Fact]
    public void ToDataVizPaletteHex_WithFallback_PaletteSet_IgnoresFallback()
    {
        DataVizPalette? palette = DataVizPalette.Color6;

        var result = palette.ToDataVizPaletteHex(fallback: "#FF0000");

        Assert.Equal("#3a96dd", result);
        Assert.DoesNotContain("#FF0000", result);
    }

    [Theory]
    [InlineData(DataVizPalette.Error,   "#c50f1f", false)]
    [InlineData(DataVizPalette.Error,   "#dc626d", true)]
    [InlineData(DataVizPalette.Success, "#107c10", false)]
    [InlineData(DataVizPalette.Success, "#54b054", true)]
    public void ToDataVizPaletteHex_WithFallback_PaletteSet_RespectsIsDarkTheme(
        DataVizPalette value, string expectedHex, bool isDark)
    {
        DataVizPalette? palette = value;

        Assert.Equal(expectedHex, palette.ToDataVizPaletteHex(fallback: "#ignored", isDarkTheme: isDark));
    }

    // ── ToDataVizPaletteHex (this string?, bool) ──────────────────────────────

    [Fact]
    public void ToDataVizPaletteHex_String_Null_ReturnsEmptyString()
    {
        string? token = null;

        Assert.Equal(string.Empty, token.ToDataVizPaletteHex());
    }

    [Fact]
    public void ToDataVizPaletteHex_String_EmptyString_ReturnsEmptyString()
    {
        Assert.Equal(string.Empty, string.Empty.ToDataVizPaletteHex());
    }

    [Theory]
    [InlineData("color1",   "#637cef")]
    [InlineData("color2",   "#e3008c")]
    [InlineData("color5",   "#13a10e")]
    [InlineData("color10",  "#ae8c00")]
    [InlineData("info",     "#015cda")]
    [InlineData("error",    "#c50f1f")]
    [InlineData("warning",  "#f7630c")]
    [InlineData("success",  "#107c10")]
    public void ToDataVizPaletteHex_String_RecognizedToken_ReturnsHex(string token, string expectedHex)
    {
        Assert.Equal(expectedHex, token.ToDataVizPaletteHex());
    }

    [Fact]
    public void ToDataVizPaletteHex_String_CustomHex_ReturnedUnchanged()
    {
        Assert.Equal("#FF5733", "#FF5733".ToDataVizPaletteHex());
    }

    [Fact]
    public void ToDataVizPaletteHex_String_CssVariable_ReturnedUnchanged()
    {
        Assert.Equal("var(--custom)", "var(--custom)".ToDataVizPaletteHex());
    }

    [Fact]
    public void ToDataVizPaletteHex_String_DarkTheme_ForTokenWithDarkVariant()
    {
        // "error" has a dark variant (#dc626d)
        Assert.Equal("#dc626d", "error".ToDataVizPaletteHex(isDarkTheme: true));
    }

    [Fact]
    public void ToDataVizPaletteHex_String_DarkTheme_ForTokenWithoutDarkVariant_ReturnsSameAsLight()
    {
        // "color1" has only one palette entry
        var light = "color1".ToDataVizPaletteHex(isDarkTheme: false);
        var dark  = "color1".ToDataVizPaletteHex(isDarkTheme: true);

        Assert.Equal(light, dark);
    }

    [Fact]
    public void ToDataVizPaletteHex_String_UnknownToken_ReturnedUnchanged()
    {
        Assert.Equal("not-a-token", "not-a-token".ToDataVizPaletteHex());
    }
}
