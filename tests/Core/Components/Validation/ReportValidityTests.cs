// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using AngleSharp.Html.Parser;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.Validation;

public class ReportValidityTests
{
    [Fact]
    public void Default_NoOptIn_ReportValidityNotCalled()
    {
        using var ctx = new Bunit.BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        ctx.Services.AddSingleton<IHtmlParser>(new HtmlParser());
        ctx.Services.AddFluentUIComponents();

        // Arrange
        var cut = ctx.Render<FluentTextArea>(parameters => parameters.Add(p => p.Id, "myId").Add(p => p.Value, "init"));

        // Act
        cut.Find("fluent-textarea").Change("new value");

        // Assert
        var reportValidityInvocations = ctx.JSInterop.Invocations
            .Where(invocation => invocation.Identifier == "Microsoft.FluentUI.Blazor.Utilities.Attributes.reportValidity")
            .ToList();

        Assert.Empty(reportValidityInvocations);
    }

    [Fact]
    public void OptIn_ReportsValidity()
    {
        using var ctx = new Bunit.BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        ctx.Services.AddSingleton<IHtmlParser>(new HtmlParser());
        ctx.Services.AddFluentUIComponents(config => config.UseNativeConstraintValidationUI = true);

        // Arrange
        var cut = ctx.Render<FluentTextArea>(parameters => parameters.Add(p => p.Id, "myId").Add(p => p.Value, "init"));

        // Act
        cut.Find("fluent-textarea").Change("new value");

        // Assert
        var reportValidityInvocations = ctx.JSInterop.Invocations
            .Where(invocation => invocation.Identifier == "Microsoft.FluentUI.Blazor.Utilities.Attributes.reportValidity")
            .ToList();

        Assert.Single(reportValidityInvocations);
    }
}
