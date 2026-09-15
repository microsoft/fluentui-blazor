// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json;
using Microsoft.FluentUI.AspNetCore.Components.Charts;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Charts.Models;

public class PolarAxisOptionsTests
{
    [Fact]
    public void PolarAxisOptions_Defaults_AreExpected()
    {
        var options = new PolarAxisOptions();

        Assert.Null(options.TickCount);
        Assert.Null(options.TickValues);
        Assert.Null(options.TickText);
        Assert.Null(options.TickFormat);
        Assert.Null(options.TickStep);
        Assert.Null(options.Tick0);
        Assert.Null(options.CategoryOrder);
        Assert.Null(options.SerializedCategoryOrder);
        Assert.Null(options.ScaleType);
        Assert.Null(options.SerializedScaleType);
        Assert.Null(options.RangeStart);
        Assert.Null(options.RangeEnd);
        Assert.Null(options.Unit);
        Assert.Null(options.SerializedUnit);
    }

    [Fact]
    public void PolarAxisOptions_SerializedEnumProperties_UseAttributeValues()
    {
        var options = new PolarAxisOptions
        {
            CategoryOrder = ChartCategoryOrder.CategoryAscending,
            ScaleType = ChartAxisScaleType.Log,
            Unit = PolarAxisUnit.Degrees,
        };

        Assert.Equal("category ascending", options.SerializedCategoryOrder);
        Assert.Equal("log", options.SerializedScaleType);
        Assert.Equal("degrees", options.SerializedUnit);
    }

    [Fact]
    public void PolarAxisOptions_Serialize_UsesExpectedPropertyNames()
    {
        var options = new PolarAxisOptions
        {
            TickCount = 5,
            TickValues = [(ChartAxisValue)1, (ChartAxisValue)2],
            TickText = ["One", "Two"],
            TickFormat = ".2f",
            TickStep = "2",
            Tick0 = (ChartAxisValue)0,
            CategoryOrder = ChartCategoryOrder.Default,
            ScaleType = ChartAxisScaleType.Default,
            RangeStart = (ChartAxisValue)(-1),
            RangeEnd = (ChartAxisValue)10,
            Unit = PolarAxisUnit.Radians,
        };

        var json = JsonSerializer.Serialize(options);

        Assert.Contains("\"tickCount\":5", json);
        Assert.Contains("\"tickValues\":[1,2]", json);
        Assert.Contains("\"tickText\":[\"One\",\"Two\"]", json);
        Assert.Contains("\"tickFormat\":\".2f\"", json);
        Assert.Contains("\"tickStep\":\"2\"", json);
        Assert.Contains("\"tick0\":0", json);
        Assert.Contains("\"categoryOrder\":\"default\"", json);
        Assert.Contains("\"scaleType\":\"default\"", json);
        Assert.Contains("\"rangeStart\":-1", json);
        Assert.Contains("\"rangeEnd\":10", json);
        Assert.Contains("\"unit\":\"radians\"", json);
    }
}
