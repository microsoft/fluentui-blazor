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

namespace Microsoft.FluentUI.AspNetCore.Components.IntegrationTests.Helpers;

public class PlaywrightFixture : IAsyncLifetime
{
    private readonly KestrelLocalServer _localServer = new KestrelLocalServer();

    /// <summary>
    /// Gets or sets the Web Server directory listening.
    /// </summary>
    public string ServerPath { get; set; } = "./";

    /// <summary>
    /// Gets or sets the Web Server port listening.
    /// </summary>
    public int ServerPort { get; set; } = 5050;

    /// <summary>
    /// Initializes a new instance of the <see cref="PlaywrightFixture"/> class.
    /// </summary>
    /// <returns></returns>
    public virtual Task InitializeAsync()
    {
        return InitializeAsync(ServerPath, ServerPort);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PlaywrightFixture"/> class,
    /// with the specified port.
    /// </summary>
    /// <param name="serverPath"></param>
    /// <param name="port"></param>
    /// <returns></returns>
    public virtual async Task InitializeAsync(string serverPath, int port)
    {
        ServerPort = port;

        // Start the local HTTP server
        await _localServer.StartAsync(serverPath, port);

        PlaywrightInstance = await Microsoft.Playwright.Playwright.CreateAsync();
        var browser = await PlaywrightInstance.Chromium.LaunchAsync(new BrowserTypeLaunchOptions()
        {
            // Devtools = System.Diagnostics.Debugger.IsAttached,
        });
        BrowserContext = await browser.NewContextAsync();
    }

    /// <summary>
    /// Gets the browser context.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("nullable", "NX0002:Find usages of the NullForgiving operator on null or default expression", Justification = "This property is assigned from InitializeAsync")]
    internal IBrowserContext BrowserContext { get; private set; } = null!;

    /// <summary>
    /// Gets the Playwright instance.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("nullable", "NX0002:Find usages of the NullForgiving operator on null or default expression", Justification = "This property is assigned from InitializeAsync")]
    internal IPlaywright PlaywrightInstance { get; private set; } = null!;

    /// <summary>
    /// Dispose the Playwright instance and stop the Local Web Server.
    /// </summary>
    /// <returns></returns>
    public async Task DisposeAsync()
    {
        await BrowserContext.DisposeAsync();
        PlaywrightInstance.Dispose();

        // Stop the local HTTP server
        await _localServer.StopAsync();
    }
}
