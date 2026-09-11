// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json;
using Microsoft.FluentUI.AspNetCore.Components.Charts;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Charts.Models;

public class LineChartLineOptionsTests
{
    [Fact]
    public void LineChartLineOptions_Defaults_AreExpected()
    {
        var options = new LineChartLineOptions();

        Assert.Null(options.StrokeWidth);
        Assert.Null(options.StrokeDasharray);
        Assert.Null(options.StrokeDashoffset);
        Assert.Null(options.StrokeLinecap);
        Assert.Null(options.SerializedStrokeLinecap);
        Assert.Null(options.LineBorderWidth);
        Assert.Null(options.LineBorderColor);
    }

    [Fact]
    public void LineChartLineOptions_SerializedStrokeLinecap_UsesEnumAttributeValue()
    {
        var options = new LineChartLineOptions
        {
            StrokeLinecap = ChartStrokeLinecap.Round,
        };

        Assert.Equal("round", options.SerializedStrokeLinecap);
    }

    [Fact]
    public void LineChartLineOptions_Serialize_UsesExpectedPropertyNames()
    {
        var options = new LineChartLineOptions
        {
            StrokeWidth = 2.5,
            StrokeDasharray = "5 2",
            StrokeDashoffset = "1",
            StrokeLinecap = ChartStrokeLinecap.Square,
            LineBorderWidth = 3,
            LineBorderColor = "#ABCDEF",
        };

        var json = JsonSerializer.Serialize(options);

        Assert.Contains("\"strokeWidth\":2.5", json);
        Assert.Contains("\"strokeDasharray\":\"5 2\"", json);
        Assert.Contains("\"strokeDashoffset\":\"1\"", json);
        Assert.Contains("\"strokeLinecap\":\"square\"", json);
        Assert.Contains("\"lineBorderWidth\":3", json);
        Assert.Contains("\"lineBorderColor\":\"#ABCDEF\"", json);
        Assert.DoesNotContain("\"StrokeLinecap\"", json);
    }
}
