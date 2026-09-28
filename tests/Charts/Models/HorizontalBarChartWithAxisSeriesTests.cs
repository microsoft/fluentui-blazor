// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json;
using Microsoft.FluentUI.AspNetCore.Components.Charts;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Charts.Models;

public class HorizontalBarChartWithAxisSeriesTests
{
    [Fact]
    public void HorizontalBarChartWithAxisSeries_Defaults_AreExpected()
    {
        var series = new HorizontalBarChartWithAxisSeries();

        Assert.Null(series.ChartSeriesTitle);
        Assert.Empty(series.ChartData);
        Assert.Null(series.BenchmarkData);
        Assert.Null(series.ChartDataText);
    }

    [Fact]
    public void HorizontalBarChartWithAxisSeries_Serialize_UsesExpectedPropertyNames()
    {
        var series = new HorizontalBarChartWithAxisSeries
        {
            ChartSeriesTitle = "FY24",
            ChartData =
            [
                new HorizontalBarChartWithAxisDataPoint
                {
                    X = 15,
                    Y = "A",
                    Legend = "L1",
                },
            ],
            BenchmarkData = 20,
            ChartDataText = "Summary",
        };

        var json = JsonSerializer.Serialize(series);

        Assert.Contains("\"chartSeriesTitle\":\"FY24\"", json);
        Assert.Contains("\"chartData\"", json);
        Assert.Contains("\"benchmarkData\":20", json);
        Assert.Contains("\"chartDataText\":\"Summary\"", json);
    }

    [Fact]
    public void HorizontalBarChartWithAxisSeries_Deserialize_ReadsExpectedProperties()
    {
        const string json = "{" +
                            "\"chartSeriesTitle\":\"FY25\"," +
                            "\"chartData\":[{\"x\":7,\"y\":\"B\",\"legend\":\"L2\"}]," +
                            "\"benchmarkData\":11," +
                            "\"chartDataText\":\"Details\"}";

        var series = JsonSerializer.Deserialize<HorizontalBarChartWithAxisSeries>(json);

        Assert.NotNull(series);
        Assert.Equal("FY25", series.ChartSeriesTitle);
        Assert.Single(series.ChartData);
        Assert.Equal(11, series.BenchmarkData);
        Assert.Equal("Details", series.ChartDataText);
    }
}
