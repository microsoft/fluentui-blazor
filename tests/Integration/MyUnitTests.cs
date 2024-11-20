// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.FluentUI.AspNetCore.Components.IntegrationTests.WebServer;
using Microsoft.Playwright;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.FluentUI.AspNetCore.Components.IntegrationTests;

#pragma warning disable CS0612 // Type or member is obsolete

[Collection("Web Server Collection")]
public class MyUnitTests
{
    private readonly ITestOutputHelper _output;
    private readonly StartServerFixture _server;

    public MyUnitTests(ITestOutputHelper output, StartServerFixture server)
    {
        _output = output;
        _server = server;
    }

    [Fact]
    public async Task Navigate_to_counter_ensure_current_counter_increases_on_click()
    {
        //Arrange
        using var playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions()
        {
            //Devtools = true
        });
        var page = await browser.NewPageAsync();
        page.Console += (_, msg) => _output.WriteLine(msg.Text);

        //Act
        await page.GotoAsync($"http://localhost:5000/button/default");
        await Task.Delay(300);

        await page.ClickAsync("button");
        await Task.Delay(300);

        //Assert
        var content = await page.ContentAsync();

        _output.WriteLine(content);

        Assert.Contains("Current count: 1", content);
    }
}

#pragma warning restore CS0612 // Type or member is obsolete
