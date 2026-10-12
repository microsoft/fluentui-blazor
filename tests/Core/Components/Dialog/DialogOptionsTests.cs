// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.Dialog;

public class DialogOptionsTests
{
    [Fact]
    public void DialogOptions_FactoryCtor_InvokesFactory()
    {
        var options = new DialogOptions(o =>
        {
            o.Id = "dialog";
            o.Class = "custom";
            o.Margin = "10px";
            o.Padding = "20px";
        });

        Assert.Equal("dialog", options.Id);
        Assert.Equal("custom", options.Class);
        Assert.Contains("custom", options.ClassValue);
        Assert.Contains("margin: 10px", options.StyleValue);
        Assert.Contains("padding: 20px", options.StyleValue);
    }
}
