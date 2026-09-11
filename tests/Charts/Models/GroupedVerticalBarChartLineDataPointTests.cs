// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json;
using Microsoft.FluentUI.AspNetCore.Components.Charts;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Charts.Models;

public class GroupedVerticalBarChartLineDataPointTests
{
    [Fact]
    public void GroupedVerticalBarChartLineDataPoint_Defaults_AreExpected()
    {
        var point = new GroupedVerticalBarChartLineDataPoint();

        Assert.Equal(0, point.Y);
        Assert.Equal(string.Empty, point.Legend);
        Assert.Null(point.Color);
        Assert.Null(point.YAxisCalloutData);
        Assert.Null(point.UseSecondaryYScale);
    }

    [Fact]
    public void GroupedVerticalBarChartLineDataPoint_Serialize_UsesExpectedPropertyNames()
    {
        var point = new GroupedVerticalBarChartLineDataPoint
        {
            Y = 42.5,
            Legend = "Target",
            Color = "#0099BC",
            YAxisCalloutData = "42.5 units",
            UseSecondaryYScale = true,
        };

        var json = JsonSerializer.Serialize(point);

        Assert.Contains("\"y\":42.5", json);
        Assert.Contains("\"legend\":\"Target\"", json);
        Assert.Contains("\"color\":\"#0099BC\"", json);
        Assert.Contains("\"yAxisCalloutData\":\"42.5 units\"", json);
        Assert.Contains("\"useSecondaryYScale\":true", json);
    }

    [Fact]
    public void GroupedVerticalBarChartLineDataPoint_Serialize_OmitsNullableProperties_WhenNull()
    {
        var point = new GroupedVerticalBarChartLineDataPoint
        {
            Y = 10,
            Legend = "Series A",
        };

        var json = JsonSerializer.Serialize(point);

        Assert.DoesNotContain("\"color\"", json);
        Assert.DoesNotContain("\"yAxisCalloutData\"", json);
        Assert.DoesNotContain("\"useSecondaryYScale\"", json);
    }
}
