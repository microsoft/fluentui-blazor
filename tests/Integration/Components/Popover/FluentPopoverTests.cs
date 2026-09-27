// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.IntegrationTests.WebServer;
using Microsoft.Playwright;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.FluentUI.AspNetCore.Components.IntegrationTests.Components.Popover;

[Collection(StartServerCollection.Name)]
public class FluentPopoverTests : FluentPlaywrightBaseTest
{
    public FluentPopoverTests(ITestOutputHelper output, StartServerFixture server)
        : base(output, server)
    {
    }

    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public async Task FluentPopover_ContentResizes_RemainsAnchored(bool rtl, bool above)
    {
        var page = await WaitOpenPageAsync("/popover/resize", openHeadlessBrowser: true);
        await page.SetViewportSizeAsync(800, 600);
        await page.Locator("#anchor").EvaluateAsync("(element, rtl) => element.dir = rtl ? 'rtl' : 'ltr'", rtl);
        await page.Locator("#anchor").EvaluateAsync("(element, above) => element.style.top = above ? '570px' : '50px'", above);
        var popover = page.Locator("#resizing-popover");
        await popover.EvaluateAsync("element => element.opened = true");
        await ExpectAnchoredAsync(page, above);

        // Let anchor polling settle before changing only the content size.
        await page.WaitForTimeoutAsync(300);
        await page.Locator("#popover-content").EvaluateAsync("element => { element.style.height = '50px'; element.style.width = '250px'; }");
        await ExpectAnchoredAsync(page, above);

        await page.Locator("#popover-content").EvaluateAsync("element => { element.style.height = '250px'; element.style.width = '100px'; }");
        await ExpectAnchoredAsync(page, above);

        await popover.EvaluateAsync("element => element.opened = false");
        await popover.EvaluateAsync("element => element.opened = true");
        await ExpectAnchoredAsync(page, above);
        await page.WaitForTimeoutAsync(300);
        await page.Locator("#popover-content").EvaluateAsync("element => element.style.height = '75px'");
        await ExpectAnchoredAsync(page, above);

        await popover.EvaluateAsync("element => element.shadowRoot.querySelector('[part=dialog]').hidePopover()");
        await Assertions.Expect(popover).ToHaveAttributeAsync("opened", "false");
        await popover.EvaluateAsync("element => element.opened = true");
        await ExpectAnchoredAsync(page, above);
        await page.WaitForTimeoutAsync(300);
        await page.Locator("#popover-content").EvaluateAsync("element => element.style.height = '125px'");
        await ExpectAnchoredAsync(page, above);
    }

    private static async Task ExpectAnchoredAsync(IPage page, bool above)
    {
        await page.WaitForFunctionAsync("""
            above => {
                const anchor = document.querySelector('#anchor');
                const dialog = document.querySelector('#resizing-popover').shadowRoot.querySelector('[part="dialog"]');
                const anchorRect = anchor.getBoundingClientRect();
                const dialogRect = dialog.getBoundingClientRect();
                const rtl = getComputedStyle(anchor).direction === 'rtl';
                return Math.abs(above ? dialogRect.bottom - anchorRect.top : dialogRect.top - anchorRect.bottom) < 1 &&
                    Math.abs((rtl ? dialogRect.right - anchorRect.right : dialogRect.left - anchorRect.left)) < 1;
            }
            """, above, options: new PageWaitForFunctionOptions { Timeout = 5000 });
    }
}
