// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.IntegrationTests.WebServer;
using Microsoft.Playwright;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.FluentUI.AspNetCore.Components.IntegrationTests.Components.Button;

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
        var page = await WaitOpenPageAsync($"/button/default", openDevTools: false);

        // Act
        await page.ClickAsync("fluent-button");
        await Task.Delay(100);  // Wait for page to render

        // Assert
        var content = await page.ContentAsync();
        await page.ScreenshotAsync(new()
        {
            Path = $"{Server.ScreenshotsFolder}FluentButton_IncrementCounter.png"
        });

        Trace.WriteLine(content);

        Assert.Contains("Current count: 1", content);
    }
}

