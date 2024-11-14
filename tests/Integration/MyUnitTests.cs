// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.FluentUI.AspNetCore.Components.IntegrationTests.WebServer;
using Microsoft.Playwright;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.FluentUI.AspNetCore.Components.IntegrationTests;

#pragma warning disable CS0612 // Type or member is obsolete

public class MyUnitTests : IClassFixture<LocalWebApplicationFactory>
{
    private readonly string _serverAddress;
    private readonly ITestOutputHelper _output;

    public MyUnitTests(LocalWebApplicationFactory fixture, ITestOutputHelper output)
    {
        _serverAddress = fixture.ServerAddress;
        _output = output;
    }

    [Fact]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2007:Consider calling ConfigureAwait on the awaited task", Justification = "<Pending>")]
    public async Task Navigate_to_counter_ensure_current_counter_increases_on_click()
    {
        //Arrange
        using var playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions()
        {
            Devtools = true
        });
        var page = await browser.NewPageAsync();
        page.Console += (_, msg) => _output.WriteLine(msg.Text);

        //Act
        await page.GotoAsync($"{_serverAddress}/button/default");
        await page.ClickAsync("button");

        //Assert
        var content = await page.ContentAsync();

        _output.WriteLine(content);
    }
}

#pragma warning restore CS0612 // Type or member is obsolete
