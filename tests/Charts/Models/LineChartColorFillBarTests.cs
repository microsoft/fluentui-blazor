// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json;
using Microsoft.FluentUI.AspNetCore.Components.Charts;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Charts.Models;

public class LineChartColorFillBarTests
{
    [Fact]
    public void LineChartColorFillBar_Defaults_AreExpected()
    {
        var bar = new LineChartColorFillBar();

        Assert.Equal(string.Empty, bar.Legend);
        Assert.Equal(string.Empty, bar.Color);
        Assert.Empty(bar.Data);
        Assert.Null(bar.ApplyPattern);
    }

    [Fact]
    public void LineChartColorFillBar_Serialize_UsesExpectedPropertyNames()
    {
        var bar = new LineChartColorFillBar
        {
            Legend = "Expected range",
            Color = "#112233",
            Data =
            [
                new LineChartColorFillBarData
                {
                    StartX = (ChartAxisValue)1,
                    EndX = (ChartAxisValue)5,
                },
            ],
            ApplyPattern = true,
        };

        var json = JsonSerializer.Serialize(bar);

        Assert.Contains("\"legend\":\"Expected range\"", json);
        Assert.Contains("\"color\":\"#112233\"", json);
        Assert.Contains("\"data\"", json);
        Assert.Contains("\"startX\":1", json);
        Assert.Contains("\"endX\":5", json);
        Assert.Contains("\"applyPattern\":true", json);
    }

    [Fact]
    public void LineChartColorFillBar_Serialize_OmitsApplyPattern_WhenNull()
    {
        var bar = new LineChartColorFillBar
        {
            Legend = "No pattern",
            Color = "#334455",
        };

        var json = JsonSerializer.Serialize(bar);

        Assert.DoesNotContain("\"applyPattern\"", json);
    }
}
