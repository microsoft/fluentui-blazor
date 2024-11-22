// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.IntegrationTests.WebServer;
using Microsoft.Playwright;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.FluentUI.AspNetCore.Components.IntegrationTests.Components.Button;

#pragma warning disable CS0612 // Type or member is obsolete

[Collection(StartServerCollection.Name)]
public class FluentButtonTests : FluentPlaywrightBaseTest
{
    public FluentButtonTests(ITestOutputHelper output, StartServerFixture server)
        : base(output, server)
    {
    }

    [Fact]
    public async Task FluentButton_IncrementCounter()
    {
        // Arrange
        var page = await WaitOpenPageAsync($"/button/default");

        // Act
        await page.ClickAsync("fluent-button");
        await Task.Delay(100);  // Wait for page to render

        // Assert
        var content = await page.ContentAsync();

        Trace.WriteLine(content);

        Assert.Contains("Current count: 1", content);
    }
}

#pragma warning restore CS0612 // Type or member is obsolete
