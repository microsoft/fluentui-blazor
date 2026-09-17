// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json;
using Microsoft.FluentUI.AspNetCore.Components.Charts;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Charts.Models;

public class CalloutAccessibilityDataTests
{
    [Fact]
    public void AriaLabel_Default_IsNull()
    {
        var data = new CalloutAccessibilityData();

        Assert.Null(data.AriaLabel);
    }

    [Fact]
    public void AriaLabel_Init_AssignsValue()
    {
        var data = new CalloutAccessibilityData
        {
            AriaLabel = "Sales for Q1"
        };

        Assert.Equal("Sales for Q1", data.AriaLabel);
    }

    [Fact]
    public void Serialize_UsesAriaLabelJsonPropertyName()
    {
        var data = new CalloutAccessibilityData
        {
            AriaLabel = "Task callout"
        };

        var json = JsonSerializer.Serialize(data);

        Assert.Contains("\"ariaLabel\":\"Task callout\"", json);
        Assert.DoesNotContain("\"AriaLabel\"", json);
    }

    [Fact]
    public void Deserialize_ReadsAriaLabelJsonPropertyName()
    {
        const string json = "{\"ariaLabel\":\"Revenue callout\"}";

        var data = JsonSerializer.Deserialize<CalloutAccessibilityData>(json);

        Assert.NotNull(data);
        Assert.Equal("Revenue callout", data.AriaLabel);
    }

    [Fact]
    public void RecordEquality_SameAriaLabel_AreEqual()
    {
        var left = new CalloutAccessibilityData { AriaLabel = "Label" };
        var right = new CalloutAccessibilityData { AriaLabel = "Label" };

        Assert.Equal(left, right);
    }

    [Fact]
    public void RecordEquality_DifferentAriaLabel_AreNotEqual()
    {
        var left = new CalloutAccessibilityData { AriaLabel = "Label A" };
        var right = new CalloutAccessibilityData { AriaLabel = "Label B" };

        Assert.NotEqual(left, right);
    }
}
