// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.IntegrationTests.WebServer;
using Microsoft.Playwright;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.IntegrationTests.Components.Dialog;

[Collection(StartServerCollection.Name)]
public class FluentDialogTests
{
    private readonly StartServerFixture _server;

    public FluentDialogTests(StartServerFixture server)
    {
        _server = server;
    }

    [Theory]
    [InlineData("chromium", "options-height")]
    [InlineData("chromium", "options-style")]
    [InlineData("chromium", "declarative-style")]
    [InlineData("webkit", "options-height")]
    [InlineData("webkit", "options-style")]
    [InlineData("webkit", "declarative-style")]
    public async Task FluentDialog_ExplicitHeight_FooterStaysAtBottom(string browserName, string source)
    {
        using var playwright = await Playwright.Playwright.CreateAsync();
        var browserType = browserName == "webkit" ? playwright.Webkit : playwright.Chromium;
        await using var browser = await browserType.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });
        var page = await browser.NewPageAsync(new BrowserNewPageOptions { ReducedMotion = ReducedMotion.Reduce });
        await page.GotoAsync($"{_server.BaseUrl}/dialog/height/{source}");
        await page.Locator("#open-dialog").ClickAsync();

        await page.WaitForFunctionAsync("""
            () => {
                const host = [...document.querySelectorAll('fluent-dialog')]
                    .find(element => element.shadowRoot.querySelector('dialog[open]'));
                if (!host) {
                    return false;
                }

                const dialog = host.shadowRoot.querySelector('dialog').getBoundingClientRect();
                const body = host.querySelector('fluent-dialog-body');
                const footer = body.shadowRoot.querySelector('[part="actions"]').getBoundingClientRect();
                return Math.abs(dialog.height - 400) < 1 &&
                    Math.abs(body.getBoundingClientRect().height - dialog.height) < 1 &&
                    Math.abs(footer.bottom - dialog.bottom) < 1;
            }
            """, options: new PageWaitForFunctionOptions { Timeout = 5000 });
    }
}
