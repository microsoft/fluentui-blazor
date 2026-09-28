// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json;
using Microsoft.FluentUI.AspNetCore.Components.Charts;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Charts.Models;

public class VerticalStackedBarChartLineDataPointTests
{
    [Fact]
    public void VerticalStackedBarChartLineDataPoint_Defaults_AreExpected()
    {
        var point = new VerticalStackedBarChartLineDataPoint();

        Assert.Equal(0, point.Y);
        Assert.Null(point.YAxisCalloutData);
        Assert.Equal(string.Empty, point.Legend);
        Assert.Null(point.Color);
        Assert.Null(point.UseSecondaryYScale);
    }

    [Fact]
    public void VerticalStackedBarChartLineDataPoint_Serialize_UsesExpectedPropertyNames()
    {
        var point = new VerticalStackedBarChartLineDataPoint
        {
            Y = 75,
            YAxisCalloutData = "75",
            Legend = "Projected",
            Color = "var(--accent)",
            UseSecondaryYScale = false,
        };

        var json = JsonSerializer.Serialize(point);

        Assert.Contains("\"y\":75", json);
        Assert.Contains("\"yAxisCalloutData\":\"75\"", json);
        Assert.Contains("\"legend\":\"Projected\"", json);
        Assert.Contains("\"color\":\"var(--accent)\"", json);
        Assert.Contains("\"useSecondaryYScale\":false", json);
    }

    [Fact]
    public void VerticalStackedBarChartLineDataPoint_Serialize_OmitsNullableProperties_WhenNull()
    {
        var point = new VerticalStackedBarChartLineDataPoint
        {
            Y = 15,
            Legend = "Actual",
        };

        var json = JsonSerializer.Serialize(point);

        Assert.DoesNotContain("\"yAxisCalloutData\"", json);
        Assert.DoesNotContain("\"color\"", json);
        Assert.DoesNotContain("\"useSecondaryYScale\"", json);
    }
}
