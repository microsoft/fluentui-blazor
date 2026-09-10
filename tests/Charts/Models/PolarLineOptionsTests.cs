// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json;
using Microsoft.FluentUI.AspNetCore.Components.Charts;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Charts.Models;

public class PolarLineOptionsTests
{
    [Fact]
    public void PolarLineOptions_Defaults_AreExpected()
    {
        var options = new PolarLineOptions();

        Assert.Null(options.StrokeWidth);
        Assert.Null(options.StrokeDasharray);
        Assert.Null(options.StrokeDashoffset);
        Assert.Null(options.StrokeLinecap);
        Assert.Null(options.SerializedStrokeLinecap);
        Assert.Null(options.Curve);
        Assert.Null(options.SerializedCurve);
    }

    [Fact]
    public void PolarLineOptions_SerializedProperties_UseAttributeValues()
    {
        var options = new PolarLineOptions
        {
            StrokeLinecap = ChartStrokeLinecap.Inherit,
            Curve = PolarLineCurve.StepAfter,
        };

        Assert.Equal("inherit", options.SerializedStrokeLinecap);
        Assert.Equal("stepAfter", options.SerializedCurve);
    }

    [Fact]
    public void PolarLineOptions_Serialize_UsesExpectedPropertyNames()
    {
        var options = new PolarLineOptions
        {
            StrokeWidth = 1.25,
            StrokeDasharray = "3 1",
            StrokeDashoffset = "2",
            StrokeLinecap = ChartStrokeLinecap.Butt,
            Curve = PolarLineCurve.Natural,
        };

        var json = JsonSerializer.Serialize(options);

        Assert.Contains("\"strokeWidth\":1.25", json);
        Assert.Contains("\"strokeDasharray\":\"3 1\"", json);
        Assert.Contains("\"strokeDashoffset\":\"2\"", json);
        Assert.Contains("\"strokeLinecap\":\"butt\"", json);
        Assert.Contains("\"curve\":\"natural\"", json);
        Assert.DoesNotContain("\"StrokeLinecap\"", json);
        Assert.DoesNotContain("\"Curve\"", json);
    }
}
