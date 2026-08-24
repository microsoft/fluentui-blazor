// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.FluentUI.AspNetCore.Components.IntegrationTests.WebServer;
using Microsoft.Playwright;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.FluentUI.AspNetCore.Components.IntegrationTests.Components.Overflow;

[Collection(StartServerCollection.Name)]
public class FluentOverflowTests : FluentPlaywrightBaseTest
{
    public FluentOverflowTests(ITestOutputHelper output, StartServerFixture server)
        : base(output, server)
    {
    }

    [Fact(Skip = "Playwright is optional for the moment")]
    public async Task FluentOverflow_AutomaticRefreshes_AreCoalesced()
    {
        var page = await WaitOpenPageAsync("/overflow/scheduling", openDevTools: false);
        await InstallMeasurementCounterAsync(page);

        await page.GetByTestId("render-overflows").ClickAsync();

        var overflows = page.Locator("fluent-overflow.test-overflow");
        var firstOverflow = page.Locator("#test-overflow-0");
        await Assertions.Expect(overflows).ToHaveCountAsync(220);
        await page.WaitForFunctionAsync("count => window.overflowMeasurements >= count", 220);
        await Assertions.Expect(firstOverflow).ToHaveCSSAsync("visibility", "visible");
        await Assertions.Expect(firstOverflow).ToHaveAttributeAsync("data-event-count", new Regex("^[1-9]"));

        var initialMeasurements = await GetMeasurementCountAsync(page);
        Assert.Equal(220, initialMeasurements);
        Assert.Equal(220, await GetHiddenMeasurementCountAsync(page));
        var normalOverflowCount = await GetIntAttributeAsync(firstOverflow, "data-overflow-count");
        var normalPayloadCount = await GetIntAttributeAsync(firstOverflow, "data-payload-count");
        Assert.Equal(Math.Min(normalOverflowCount, 3), normalPayloadCount);
        await Assertions.Expect(firstOverflow.Locator(".overflow-indicator"))
            .ToHaveTextAsync($"+{normalOverflowCount}");

        await page.GetByTestId("narrow-first").ClickAsync();
        await Assertions.Expect(firstOverflow).ToHaveClassAsync(new Regex("narrow"));
        await Assertions.Expect(firstOverflow)
            .Not.ToHaveAttributeAsync("data-overflow-count", normalOverflowCount.ToString(CultureInfo.InvariantCulture));

        var narrowOverflowCount = await GetIntAttributeAsync(firstOverflow, "data-overflow-count");
        var narrowPayloadCount = await GetIntAttributeAsync(firstOverflow, "data-payload-count");
        Assert.True(narrowOverflowCount > normalOverflowCount);
        Assert.Equal(Math.Min(narrowOverflowCount, 3), narrowPayloadCount);
        await Assertions.Expect(firstOverflow.Locator(".overflow-indicator"))
            .ToHaveTextAsync($"+{narrowOverflowCount}");

        await page.GetByTestId("restore-first").ClickAsync();
        await Assertions.Expect(firstOverflow)
            .ToHaveAttributeAsync("data-overflow-count", normalOverflowCount.ToString(CultureInfo.InvariantCulture));

        await Assertions.Expect(page.Locator(".managed-item"))
            .ToHaveCountAsync(220 * 6);
    }

    private static async Task InstallMeasurementCounterAsync(IPage page)
    {
        await page.EvaluateAsync("""
            () => {
                const overflowPrototype = customElements.get("fluent-overflow").prototype;
                const originalRefreshNow = overflowPrototype.refreshNow;
                window.overflowMeasurements = 0;
                window.hiddenOverflowMeasurements = 0;
                overflowPrototype.refreshNow = function (...args) {
                    window.overflowMeasurements++;
                    if (this.style.visibility === "hidden") {
                        window.hiddenOverflowMeasurements++;
                    }
                    return originalRefreshNow.apply(this, args);
                };
            }
            """);
    }

    private static Task<int> GetMeasurementCountAsync(IPage page) =>
        page.EvaluateAsync<int>("() => window.overflowMeasurements");

    private static Task<int> GetHiddenMeasurementCountAsync(IPage page) =>
        page.EvaluateAsync<int>("() => window.hiddenOverflowMeasurements");

    private static async Task<int> GetIntAttributeAsync(ILocator locator, string name)
    {
        var value = await locator.GetAttributeAsync(name);
        return int.Parse(value!, CultureInfo.InvariantCulture);
    }
}