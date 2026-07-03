// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.Charts;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Charts.Models;

public class TooltipContextTests
{
    // ── Color ────────────────────────────────────────────────────────────────

    [Fact]
    public void TooltipContext_Color_Null_ByDefault()
    {
        var ctx = new TooltipContext();

        Assert.Null(ctx.Color);
    }

    [Fact]
    public void TooltipContext_Color_RetainsPaletteValue()
    {
        var ctx = new TooltipContext { Color = DataVizPalette.Color5 };

        Assert.Equal(DataVizPalette.Color5, ctx.Color);
    }

    [Theory]
    [InlineData(DataVizPalette.Color1)]
    [InlineData(DataVizPalette.Color2)]
    [InlineData(DataVizPalette.Color10)]
    [InlineData(DataVizPalette.Info)]
    [InlineData(DataVizPalette.Success)]
    [InlineData(DataVizPalette.Warning)]
    [InlineData(DataVizPalette.Error)]
    public void TooltipContext_Color_AcceptsAllPaletteValues(DataVizPalette value)
    {
        var ctx = new TooltipContext { Color = value };

        Assert.Equal(value, ctx.Color);
    }

    [Fact]
    public void TooltipContext_Color_NullWhenCustomColorIsUsed()
    {
        // When a data point uses DataVizPalette.Custom, the JS bridge sets Color=null
        // and CustomColor to the raw hex string instead.
        var ctx = new TooltipContext { Color = null, CustomColor = "#FF5733" };

        Assert.Null(ctx.Color);
        Assert.Equal("#FF5733", ctx.CustomColor);
    }

    // ── CustomColor ──────────────────────────────────────────────────────────

    [Fact]
    public void TooltipContext_CustomColor_Null_ByDefault()
    {
        var ctx = new TooltipContext();

        Assert.Null(ctx.CustomColor);
    }

    [Fact]
    public void TooltipContext_CustomColor_RetainsHexString()
    {
        var ctx = new TooltipContext { CustomColor = "#C19A6B" };

        Assert.Equal("#C19A6B", ctx.CustomColor);
    }

    [Fact]
    public void TooltipContext_CustomColor_RetainsCssVariableString()
    {
        var ctx = new TooltipContext { CustomColor = "var(--custom-brand)" };

        Assert.Equal("var(--custom-brand)", ctx.CustomColor);
    }

    [Fact]
    public void TooltipContext_ToDataVizPaletteHex_ReturnsCustomColorWhenColorIsNull()
    {
        // The recommended pattern: Color?.ToDataVizPaletteHex(CustomColor) falls back to CustomColor
        var ctx = new TooltipContext { Color = null, CustomColor = "#A23B72" };

        var resolved = ctx.Color.ToDataVizPaletteHex(ctx.CustomColor);

        Assert.Equal("#A23B72", resolved);
    }

    [Fact]
    public void TooltipContext_ToDataVizPaletteHex_ReturnsPaletteHexWhenColorIsSet()
    {
        var ctx = new TooltipContext { Color = DataVizPalette.Color1, CustomColor = null };

        var resolved = ctx.Color.ToDataVizPaletteHex(ctx.CustomColor);

        Assert.NotEmpty(resolved);
        Assert.StartsWith("#", resolved);
    }

    // ── RawJson ──────────────────────────────────────────────────────────────

    [Fact]
    public void TooltipContext_RawJson_Null_ByDefault()
    {
        var ctx = new TooltipContext();

        Assert.Null(ctx.RawJson);
    }

    [Fact]
    public void TooltipContext_RawJson_RetainsJsonString()
    {
        const string json = """{"legend":"Task A","x":{"start":1704067200000,"end":1705276800000}}""";

        var ctx = new TooltipContext { RawJson = json };

        Assert.Equal(json, ctx.RawJson);
    }

    [Fact]
    public void TooltipContext_RawJson_CanBeDeserialized()
    {
        const string json = """{"legend":"Category A","data":42}""";
        var ctx = new TooltipContext { RawJson = json };

        // Verify the raw JSON is valid and accessible for custom parsing
        using var doc = System.Text.Json.JsonDocument.Parse(ctx.RawJson!);
        Assert.Equal("Category A", doc.RootElement.GetProperty("legend").GetString());
        Assert.Equal(42, doc.RootElement.GetProperty("data").GetInt32());
    }

    // ── Combined: all typed members together ─────────────────────────────────

    [Fact]
    public void TooltipContext_AllMembers_InitializedTogether()
    {
        const string raw = """{"legend":"Item","data":99,"color":"color3"}""";

        var ctx = new TooltipContext
        {
            Legend    = "Item",
            YValue    = "99",
            XValue    = null,
            Color     = DataVizPalette.Color3,
            CustomColor = null,
            RawJson   = raw,
        };

        Assert.Equal("Item",                  ctx.Legend);
        Assert.Equal("99",                    ctx.YValue);
        Assert.Null(ctx.XValue);
        Assert.Equal(DataVizPalette.Color3,   ctx.Color);
        Assert.Null(ctx.CustomColor);
        Assert.Equal(raw,                     ctx.RawJson);
    }
}
