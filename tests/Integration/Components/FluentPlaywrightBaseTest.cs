// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.IntegrationTests.WebServer;
using Microsoft.Playwright;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.FluentUI.AspNetCore.Components.IntegrationTests.Components;

#pragma warning disable CS0612 // Type or member is obsolete

public abstract class FluentPlaywrightBaseTest : IAsyncDisposable, IDisposable
{
    private IPlaywright? _playwright;
    private IBrowser? _browser;

    /// <summary>
    /// Constructor for the FluentPlaywrightBaseTest
    /// </summary>
    /// <param name="output"></param>
    /// <param name="server"></param>
    protected FluentPlaywrightBaseTest(ITestOutputHelper output, StartServerFixture server)
    {
        Trace = output;
        Server = server;
    }

    /// <summary>
    /// Output helper for logging
    /// </summary>
    public virtual ITestOutputHelper Trace { get; set; }

    /// <summary>
    /// Server fixture for starting the server
    /// </summary>
    protected virtual StartServerFixture Server { get; set; }

    public async Task<IPage> WaitOpenPageAsync(string url, bool? openDevTools = null)
    {
        _playwright = await Playwright.Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions()
        {
            Devtools = openDevTools
        });

        var page = await _browser.NewPageAsync();
        page.Console += (_, msg) => Trace.WriteLine(msg.Text);

        await page.GotoAsync($"{Server.BaseUrl}{url}");
        await page.WaitForConsoleMessageAsync(new PageWaitForConsoleMessageOptions()
        {
            Predicate = msg => msg.Text.Contains("WebSocket connected"),
            Timeout = 2000
        });
        await Task.Delay(100);  // Wait for page to render

        return page;
    }

    public void Dispose()
    {
        Task.Run(async () => await DisposeAsync());
    }

    public async ValueTask DisposeAsync()
    {
        if (_browser != null)
        {
            await _browser.CloseAsync();
        }

        _playwright?.Dispose();
    }
}

#pragma warning restore CS0612 // Type or member is obsolete
