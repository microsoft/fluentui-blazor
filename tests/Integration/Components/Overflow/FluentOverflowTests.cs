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

    [Theory]
    [InlineData(1280, 800)]
    [InlineData(390, 844)]
    public async Task FluentOverflow_Resize_UpdatesHiddenItemsAndPayload(int width, int height)
    {
        var page = await WaitOpenPageAsync("/overflow/scheduling", openDevTools: false);
        await page.SetViewportSizeAsync(width, height);

        await page.GetByTestId("render-overflows").ClickAsync();

        var overflows = page.Locator("fluent-overflow.test-overflow");
        var firstOverflow = page.Locator("#test-overflow-0");
        await Assertions.Expect(overflows).ToHaveCountAsync(220);
        await Assertions.Expect(firstOverflow).ToHaveCSSAsync("visibility", "visible");
        await Assertions.Expect(firstOverflow).ToHaveAttributeAsync("data-event-count", new Regex("^[1-9]"));

        Assert.True(await firstOverflow.EvaluateAsync<bool>("element => !!element.shadowRoot"));
        await Assertions.Expect(firstOverflow).ToHaveAttributeAsync("visible-on-load", "false");
        await Assertions.Expect(firstOverflow.Locator("[slot='trigger']")).ToBeHiddenAsync();
        var normalOverflowCount = await GetIntAttributeAsync(firstOverflow, "data-overflow-count");
        var normalPayloadCount = await GetIntAttributeAsync(firstOverflow, "data-payload-count");
        Assert.Equal(normalOverflowCount, normalPayloadCount);
        await Assertions.Expect(firstOverflow.Locator(".overflow-indicator"))
            .ToHaveTextAsync($"+{normalOverflowCount}");

        await page.GetByTestId("narrow-first").ClickAsync();
        await Assertions.Expect(firstOverflow).ToHaveClassAsync(new Regex("narrow"));
        await Assertions.Expect(firstOverflow)
            .Not.ToHaveAttributeAsync("data-overflow-count", normalOverflowCount.ToString(CultureInfo.InvariantCulture));

        var narrowOverflowCount = await GetIntAttributeAsync(firstOverflow, "data-overflow-count");
        var narrowPayloadCount = await GetIntAttributeAsync(firstOverflow, "data-payload-count");
        Assert.True(narrowOverflowCount > normalOverflowCount);
        Assert.True(narrowOverflowCount > 3);
        Assert.Equal(narrowOverflowCount, narrowPayloadCount);
        await Assertions.Expect(firstOverflow.Locator(".managed-item[hidden]"))
            .ToHaveCountAsync(narrowOverflowCount);
        await Assertions.Expect(firstOverflow.Locator("[slot='trigger']")).ToBeVisibleAsync();
        await Assertions.Expect(firstOverflow.Locator(".overflow-indicator"))
            .ToHaveTextAsync($"+{narrowOverflowCount}");

        await page.GetByTestId("restore-first").ClickAsync();
        await Assertions.Expect(firstOverflow)
            .ToHaveAttributeAsync("data-overflow-count", normalOverflowCount.ToString(CultureInfo.InvariantCulture));
        await Assertions.Expect(firstOverflow.Locator(".managed-item[hidden]")).ToHaveCountAsync(0);
        await Assertions.Expect(firstOverflow.Locator("[slot='trigger']")).ToBeHiddenAsync();

        await Assertions.Expect(page.Locator(".managed-item"))
            .ToHaveCountAsync(220 * 6);
    }

    private static async Task<int> GetIntAttributeAsync(ILocator locator, string name)
    {
        var value = await locator.GetAttributeAsync(name);
        return int.Parse(value!, CultureInfo.InvariantCulture);
    }

    [Fact]
    public async Task FluentOverflow_Tabs_PreservesKeyboardSelectionAndDisposal()
    {
        var page = await WaitOpenPageAsync("/overflow/consumers", openDevTools: false);
        var tabList = page.Locator("#overflow-tabs-tablist");
        var more = page.Locator("#overflow-tabs-more");
        await Assertions.Expect(more).ToBeVisibleAsync();
        await page.WaitForFunctionAsync("() => document.querySelectorAll('#overflow-tabs-tablist fluent-tab[hidden]').length > 0");
        var lastVisibleTab = tabList.Locator("fluent-tab:not([hidden])").Last;
        await lastVisibleTab.FocusAsync();
        await lastVisibleTab.PressAsync("ArrowRight");
        await Assertions.Expect(more).ToBeFocusedAsync();
        await more.PressAsync("ArrowLeft");
        await Assertions.Expect(lastVisibleTab).ToBeFocusedAsync();

        await more.ClickAsync();
        await page.Locator("#overflow-tabs-overflow-menu fluent-menu-item").Last.ClickAsync();
        await Assertions.Expect(tabList).ToHaveAttributeAsync("activeid", "tab-7");
        await Assertions.Expect(page.Locator("#tab-7")).ToBeVisibleAsync();

        await page.GetByTestId("toggle-tabs").ClickAsync();
        await Assertions.Expect(tabList.Locator("fluent-tab[hidden]")).ToHaveCountAsync(0);
        await Assertions.Expect(page.Locator("#overflow-tabs-more")).ToHaveCountAsync(0);
        await page.GetByTestId("toggle-tabs").ClickAsync();
        await Assertions.Expect(more).ToBeVisibleAsync();
        await Assertions.Expect(tabList.Locator("fluent-tab[aria-selected='true']")).ToBeVisibleAsync();
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task FluentOverflow_AppBar_UpdatesCompleteMenuAndRestoresItems(bool vertical)
    {
        var page = await WaitOpenPageAsync("/overflow/consumers", openDevTools: false);
        if (vertical)
        {
            await page.GetByTestId("rotate-appbar").ClickAsync();
        }

        var host = page.Locator("#overflow-appbar-overflow");
        await page.WaitForFunctionAsync("() => document.querySelectorAll('#overflow-appbar-overflow > .fluent-appbar-item[hidden]').length > 25");
        var hiddenCount = await host.Locator(":scope > .fluent-appbar-item[hidden]").CountAsync();
        await Assertions.Expect(host.Locator("[slot='trigger']")).ToBeVisibleAsync();
        await host.Locator("[slot='trigger']").ClickAsync();
        await Assertions.Expect(page.Locator("#overflow-appbar fluent-popover-b .fluent-appbar-item"))
            .ToHaveCountAsync(hiddenCount);

        await page.GetByTestId("expand-appbar").ClickAsync();
        await Assertions.Expect(host.Locator(":scope > .fluent-appbar-item[hidden]")).ToHaveCountAsync(0);
        await Assertions.Expect(host.Locator("[slot='trigger']")).ToBeHiddenAsync();
        await Assertions.Expect(page.Locator("#overflow-appbar fluent-popover-b")).ToHaveCountAsync(0);
    }

    [Fact]
    public async Task FluentOverflow_SelectorAndBehaviors_PreserveFixedItemsAndCustomTrigger()
    {
        var page = await WaitOpenPageAsync("/overflow/consumers", openDevTools: false);
        var host = page.Locator("#behavior-overflow");
        await page.WaitForFunctionAsync("() => document.querySelectorAll('#behavior-overflow > .managed[hidden]').length > 0");
        var hiddenCount = await host.Locator(".managed[hidden]").CountAsync();
        await Assertions.Expect(host.Locator("#fixed-item")).ToBeVisibleAsync();
        await Assertions.Expect(host.Locator("#ellipsis-item")).Not.ToHaveAttributeAsync("hidden", "");
        await Assertions.Expect(host.Locator("#unmanaged-item")).ToBeVisibleAsync();
        await Assertions.Expect(host.Locator("#custom-count")).ToHaveTextAsync($"+{hiddenCount}");
        await host.Locator("[slot='trigger']").HoverAsync();
        await Assertions.Expect(page.Locator("fluent-tooltip[anchor='behavior-overflow-more']")).ToContainTextAsync("Item 5");
    }

    [Fact]
    public async Task FluentOverflow_EllipsisBehavior_OverflowsRegularItemsBeforeShrinking()
    {
        var page = await WaitOpenPageAsync("/overflow/consumers", openDevTools: false);
        var host = page.Locator("#priority-overflow");
        var ellipsisItems = host.Locator(".priority-ellipsis");

        await Assertions.Expect(host.Locator(".priority-badge[hidden]")).ToHaveCountAsync(2);
        var initialWidths = await ellipsisItems.EvaluateAllAsync<double[]>(
            "items => items.map(item => item.getBoundingClientRect().width)");
        Assert.All(initialWidths, width => Assert.InRange(width, 79, 81));

        await host.Locator("[slot='trigger']").HoverAsync();
        var tooltip = page.Locator("fluent-tooltip[anchor='priority-overflow-more']");
        await Assertions.Expect(tooltip).ToContainTextAsync("Badge 2");
        await Assertions.Expect(tooltip).ToContainTextAsync("Badge 3");

        await host.EvaluateAsync("element => element.style.width = '170px'");
        await Assertions.Expect(host.Locator(".priority-badge[hidden]")).ToHaveCountAsync(3);
        var narrowedWidths = await ellipsisItems.EvaluateAllAsync<double[]>(
            "items => items.map(item => item.getBoundingClientRect().width)");
        Assert.All(narrowedWidths, width => Assert.True(width < 79));
    }

    [Theory]
    [InlineData(1280, 800)]
    [InlineData(390, 844)]
    public async Task FluentOverflow_RenderLimit_ReservesTriggerAndExposesOmittedItems(int width, int height)
    {
        var page = await WaitOpenPageAsync("/overflow/consumers", openDevTools: false);
        await page.SetViewportSizeAsync(width, height);
        var host = page.Locator("#bounded-overflow");
        await Assertions.Expect(host.Locator(".bounded-item")).ToHaveCountAsync(10);
        await Assertions.Expect(page.Locator(".bounded-popup-item")).ToHaveCountAsync(0);
        await Assertions.Expect(host.Locator("[slot='trigger']")).ToBeVisibleAsync();
        await Assertions.Expect(host.Locator(".bounded-item[hidden]")).ToHaveCountAsync(1);
        await Assertions.Expect(page.Locator("#bounded-count")).ToHaveTextAsync("91");

        await host.EvaluateAsync("element => element.style.width = '700px'");
        await Assertions.Expect(host.Locator(".bounded-item[hidden]")).ToHaveCountAsync(0);
        await Assertions.Expect(page.Locator("#bounded-count")).ToHaveTextAsync("90");
        await Assertions.Expect(host.Locator("[slot='trigger']")).ToBeVisibleAsync();

        await page.GetByTestId("resize-bounded").ClickAsync();
        await Assertions.Expect(host.Locator(".bounded-item[hidden]")).ToHaveCountAsync(7);
        await Assertions.Expect(page.Locator("#bounded-count")).ToHaveTextAsync("97");
        var trigger = host.Locator("[slot='trigger']");
        var popup = page.Locator("fluent-popover-b[anchor-id='bounded-overflow-more'] [part='dialog']");
        await trigger.ClickAsync();
        await Assertions.Expect(popup).ToBeVisibleAsync();
        await Assertions.Expect(page.Locator(".bounded-popup-item")).ToHaveCountAsync(97);
        await Assertions.Expect(page.Locator(".bounded-popup-item").Last).ToHaveTextAsync("99");
        await Assertions.Expect(host.Locator(".bounded-item")).ToHaveCountAsync(10);

        await page.Keyboard.PressAsync("Escape");
        await Assertions.Expect(popup).ToBeHiddenAsync();
        await Assertions.Expect(page.Locator(".bounded-popup-item")).ToHaveCountAsync(0);
        await trigger.FocusAsync();
        await trigger.PressAsync("Enter");
        await Assertions.Expect(popup).ToBeVisibleAsync();
        await trigger.ClickAsync();
        await Assertions.Expect(popup).ToBeHiddenAsync();
    }
}