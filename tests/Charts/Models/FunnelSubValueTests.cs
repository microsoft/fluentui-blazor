// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json;
using Microsoft.FluentUI.AspNetCore.Components.Charts;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Charts.Models;

public class FunnelSubValueTests
{
    // ── Defaults ─────────────────────────────────────────────────────────────

    [Fact]
    public void FunnelSubValue_Category_EmptyString_ByDefault()
    {
        var sub = new FunnelSubValue();

        Assert.Equal(string.Empty, sub.Category);
    }

    [Fact]
    public void FunnelSubValue_Value_Zero_ByDefault()
    {
        var sub = new FunnelSubValue();

        Assert.Equal(0, sub.Value);
    }

    [Fact]
    public void FunnelSubValue_Color_Null_ByDefault()
    {
        var sub = new FunnelSubValue();

        Assert.Null(sub.Color);
    }

    [Fact]
    public void FunnelSubValue_CustomColor_Null_ByDefault()
    {
        var sub = new FunnelSubValue();

        Assert.Null(sub.CustomColor);
    }

    [Fact]
    public void FunnelSubValue_SerializedColor_Null_WhenNeitherColorNorCustomColorSet()
    {
        var sub = new FunnelSubValue();

        Assert.Null(sub.SerializedColor);
    }

    // ── Category and Value ───────────────────────────────────────────────────

    [Fact]
    public void FunnelSubValue_Category_RetainsValue()
    {
        var sub = new FunnelSubValue { Category = "Segment A" };

        Assert.Equal("Segment A", sub.Category);
    }

    [Fact]
    public void FunnelSubValue_Value_RetainsNumericValue()
    {
        var sub = new FunnelSubValue { Value = 123.45 };

        Assert.Equal(123.45, sub.Value);
    }

    // ── Color / SerializedColor ───────────────────────────────────────────────

    [Fact]
    public void FunnelSubValue_SerializedColor_ReturnsPaletteToken_WhenColorIsSet()
    {
        var sub = new FunnelSubValue { Color = DataVizPalette.Color3 };

        Assert.Equal("color3", sub.SerializedColor);
    }

    [Theory]
    [InlineData(DataVizPalette.Color1,  "color1")]
    [InlineData(DataVizPalette.Color2,  "color2")]
    [InlineData(DataVizPalette.Color10, "color10")]
    [InlineData(DataVizPalette.Info,    "info")]
    [InlineData(DataVizPalette.Success, "success")]
    [InlineData(DataVizPalette.Warning, "warning")]
    [InlineData(DataVizPalette.Error,   "error")]
    public void FunnelSubValue_SerializedColor_ReturnsPaletteToken_ForAllPaletteValues(DataVizPalette value, string expectedToken)
    {
        var sub = new FunnelSubValue { Color = value };

        Assert.Equal(expectedToken, sub.SerializedColor);
    }

    // ── CustomColor / SerializedColor ─────────────────────────────────────────

    [Fact]
    public void FunnelSubValue_SerializedColor_ReturnsCustomColor_WhenColorIsCustom()
    {
        var sub = new FunnelSubValue { Color = DataVizPalette.Custom, CustomColor = "#FF5733" };

        Assert.Equal("#FF5733", sub.SerializedColor);
    }

    [Fact]
    public void FunnelSubValue_SerializedColor_ReturnsCssVariable_WhenColorIsCustom()
    {
        var sub = new FunnelSubValue { Color = DataVizPalette.Custom, CustomColor = "var(--brand-color)" };

        Assert.Equal("var(--brand-color)", sub.SerializedColor);
    }

    [Fact]
    public void FunnelSubValue_SerializedColor_Null_WhenColorIsCustom_AndCustomColorIsNull()
    {
        // Custom with no CustomColor string → SerializedColor falls through to null
        var sub = new FunnelSubValue { Color = DataVizPalette.Custom, CustomColor = null };

        Assert.Null(sub.SerializedColor);
    }

    // ── JSON serialization ────────────────────────────────────────────────────

    [Fact]
    public void FunnelSubValue_Json_ContainsCategoryAndValue()
    {
        var data = new FunnelDataPoint
        {
            Stage = "Top",
            Value = 500,
            SubValues = [new FunnelSubValue { Category = "Alpha", Value = 300 }],
        };

        var json = ChartJson.Serialize([data]);

        Assert.Contains("\"category\":\"Alpha\"", json);
        Assert.Contains("\"value\":300", json);
    }

    [Fact]
    public void FunnelSubValue_Json_ContainsPaletteColorToken()
    {
        var data = new FunnelDataPoint
        {
            Stage = "Middle",
            Value = 200,
            SubValues = [new FunnelSubValue { Category = "Beta", Value = 100, Color = DataVizPalette.Color5 }],
        };

        var json = ChartJson.Serialize([data]);

        Assert.Contains("\"color\":\"color5\"", json);
    }

    [Fact]
    public void FunnelSubValue_Json_ContainsCustomColorHex()
    {
        var data = new FunnelDataPoint
        {
            Stage = "Bottom",
            Value = 100,
            SubValues = [new FunnelSubValue { Category = "Gamma", Value = 50, Color = DataVizPalette.Custom, CustomColor = "#0099BC" }],
        };

        var json = ChartJson.Serialize([data]);

        Assert.Contains("\"color\":\"#0099BC\"", json);
    }

    [Fact]
    public void FunnelSubValue_Json_OmitsColorProperty_WhenNeitherColorNorCustomColorSet()
    {
        var data = new FunnelDataPoint
        {
            Stage = "Top",
            Value = 400,
            SubValues = [new FunnelSubValue { Category = "Delta", Value = 200 }],
        };

        var json = ChartJson.Serialize([data]);

        // JsonIgnore(WhenWritingNull) means the "color" key should not appear in sub value
        using var doc = JsonDocument.Parse(json);
        var subValue = doc.RootElement[0].GetProperty("subValues")[0];
        Assert.False(subValue.TryGetProperty("color", out _));
    }

    // ── Record equality ───────────────────────────────────────────────────────

    [Fact]
    public void FunnelSubValue_RecordEquality_SameValues_AreEqual()
    {
        var a = new FunnelSubValue { Category = "X", Value = 10, Color = DataVizPalette.Color1 };
        var b = new FunnelSubValue { Category = "X", Value = 10, Color = DataVizPalette.Color1 };

        Assert.Equal(a, b);
    }

    [Fact]
    public void FunnelSubValue_RecordEquality_DifferentValues_AreNotEqual()
    {
        var a = new FunnelSubValue { Category = "X", Value = 10 };
        var b = new FunnelSubValue { Category = "X", Value = 20 };

        Assert.NotEqual(a, b);
    }
}
