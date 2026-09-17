// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Globalization;
using System.Text.Json;
using Microsoft.FluentUI.AspNetCore.Components.Charts;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Charts.Models;

public class ChartAxisValueJsonConverterTests
{
    [Fact]
    public void Deserialize_Number_ReadsAsNumericChartAxisValue()
    {
        // Arrange
        const string json = "123.5";

        // Act
        var result = JsonSerializer.Deserialize<ChartAxisValue>(json);

        // Assert
        Assert.Equal((ChartAxisValue)123.5, result);
    }

    [Fact]
    public void Deserialize_IsoDate_ReadsAsDateChartAxisValue()
    {
        // Arrange
        const string json = "\"2024-01-31T12:34:56.0000000+00:00\"";

        // Act
        var result = JsonSerializer.Deserialize<ChartAxisValue>(json);

        // Assert
        var expectedDate = DateTimeOffset.Parse("2024-01-31T12:34:56.0000000+00:00", CultureInfo.InvariantCulture);
        Assert.Equal((ChartAxisValue)expectedDate, result);
    }

    [Fact]
    public void Deserialize_InvalidDateString_ThrowsFormatException()
    {
        // Arrange
        const string json = "\"not-a-date\"";

        // Act + Assert
        Assert.Throws<FormatException>(() => JsonSerializer.Deserialize<ChartAxisValue>(json));
    }

    [Fact]
    public void Serialize_NumericChartAxisValue_WritesJsonNumber()
    {
        // Arrange
        ChartAxisValue value = 42.75;

        // Act
        var json = JsonSerializer.Serialize(value);

        // Assert
        Assert.Equal("42.75", json);
    }

    [Fact]
    public void Serialize_DateChartAxisValue_WritesIso8601String()
    {
        // Arrange
        var date = new DateTimeOffset(2024, 2, 29, 7, 8, 9, TimeSpan.FromHours(2));
        ChartAxisValue value = date;

        // Act
        var json = JsonSerializer.Serialize(value);

        // Assert
        using var doc = JsonDocument.Parse(json);
        var parsed = doc.RootElement.GetString();
        Assert.Equal(date.ToString("O", CultureInfo.InvariantCulture), parsed);
    }

    [Fact]
    public void Serialize_DateTimeUnspecified_UsesUtcKindInOutput()
    {
        // Arrange
        var dateTime = new DateTime(2024, 3, 1, 0, 0, 0, DateTimeKind.Unspecified);
        ChartAxisValue value = dateTime;

        // Act
        var json = JsonSerializer.Serialize(value);

        // Assert
        using var doc = JsonDocument.Parse(json);
        var parsed = doc.RootElement.GetString();
        Assert.EndsWith("+00:00", parsed);
    }

    [Fact]
    public void RoundTrip_Number_PreservesValue()
    {
        // Arrange
        ChartAxisValue original = -999.125;

        // Act
        var json = JsonSerializer.Serialize(original);
        var result = JsonSerializer.Deserialize<ChartAxisValue>(json);

        // Assert
        Assert.Equal(original, result);
    }

    [Fact]
    public void RoundTrip_Date_PreservesValue()
    {
        // Arrange
        ChartAxisValue original = new DateTimeOffset(2025, 5, 6, 11, 12, 13, TimeSpan.Zero);

        // Act
        var json = JsonSerializer.Serialize(original);
        var result = JsonSerializer.Deserialize<ChartAxisValue>(json);

        // Assert
        Assert.Equal(original, result);
    }
}
