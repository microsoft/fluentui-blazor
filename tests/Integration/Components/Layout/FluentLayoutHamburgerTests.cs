// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.RegularExpressions;
using Microsoft.FluentUI.AspNetCore.Components.IntegrationTests.WebServer;
using Microsoft.Playwright;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.FluentUI.AspNetCore.Components.IntegrationTests.Components.Layout;

// aria-expanded belongs on the hamburger button (role="button"), never on its role-less wrapper div.
// It reflects whether the drawer is open on mobile, and whether the nav area is visible on desktop.
[Collection(StartServerCollection.Name)]
public class FluentLayoutHamburgerTests : FluentPlaywrightBaseTest
{
    private const string MobileOnlyUrl = "/layout/hamburger";
    private const string DesktopMobileUrl = "/layout/hamburger/desktop-mobile";
    private const string DrawerId = "hamburger-drawer";

    private static readonly (int Width, int Height) Mobile = (390, 844);
    private static readonly (int Width, int Height) Desktop = (1280, 900);

    // Long enough for a second, unwanted toggle to have happened.
    private const int SettleDelayMilliseconds = 300;

    public FluentLayoutHamburgerTests(ITestOutputHelper output, StartServerFixture server)
        : base(output, server)
    {
    }

    [Fact]
    public async Task FluentLayoutHamburger_Mobile_ClickAndEscape()
    {
        // Arrange
        var page = await OpenPageAsync(MobileOnlyUrl, Mobile);
        var button = HamburgerButton(page);

        // Assert: closed, and aria-controls points to the drawer.
        await Assertions.Expect(button).ToHaveAttributeAsync("aria-expanded", "false");
        await Assertions.Expect(button).ToHaveAttributeAsync("aria-controls", DrawerId);
        await Assertions.Expect(page.Locator($"#{DrawerId}")).ToHaveCountAsync(1);
        await AssertWrapperHasNoAriaExpandedAsync(page);

        // Act: open
        await button.ClickAsync();

        // Assert
        await Assertions.Expect(button).ToHaveAttributeAsync("aria-expanded", "true");
        Assert.True(await IsDrawerOpenAsync(page));
        await AssertWrapperHasNoAriaExpandedAsync(page);

        // Act: close
        await page.Keyboard.PressAsync("Escape");

        // Assert
        await Assertions.Expect(button).ToHaveAttributeAsync("aria-expanded", "false");
        Assert.False(await IsDrawerOpenAsync(page));
        await AssertWrapperHasNoAriaExpandedAsync(page);
    }

    [Fact]
    public async Task FluentLayoutHamburger_Mobile_ShowAsyncThenOneClickCloses()
    {
        // Arrange
        var page = await OpenPageAsync(MobileOnlyUrl, Mobile);
        var button = HamburgerButton(page);

        // Act: open the drawer from code, before the hamburger was ever clicked.
        await page.ClickAsync("#show-drawer");

        // Assert
        await Assertions.Expect(button).ToHaveAttributeAsync("aria-expanded", "true");
        Assert.True(await IsDrawerOpenAsync(page));

        // Act: one click on the hamburger. The modal drawer covers the page, so the click is dispatched
        // to the button rather than sent with the mouse (it would land on the drawer backdrop).
        await button.DispatchEventAsync("click");

        // Assert
        await Assertions.Expect(button).ToHaveAttributeAsync("aria-expanded", "false");
        Assert.False(await IsDrawerOpenAsync(page));
        await AssertWrapperHasNoAriaExpandedAsync(page);
    }

    [Fact]
    public async Task FluentLayoutHamburger_DesktopMobile_ReflectsNavVisibility()
    {
        // Arrange
        var page = await OpenPageAsync(DesktopMobileUrl, Desktop);
        var button = HamburgerButton(page);
        var nav = NavArea(page);

        // Assert: the nav area is visible, so the hamburger is expanded.
        await Assertions.Expect(nav).Not.ToHaveAttributeAsync("hidden", "");
        await Assertions.Expect(button).ToHaveAttributeAsync("aria-expanded", "true");
        await Assertions.Expect(page.GetByRole(AriaRole.Button, new() { Name = "Menu", Expanded = true })).ToHaveCountAsync(1);

        // Act: hide the nav area
        await button.ClickAsync();

        // Assert
        await Assertions.Expect(nav).ToHaveAttributeAsync("hidden", "");
        await Assertions.Expect(button).ToHaveAttributeAsync("aria-expanded", "false");
        await Assertions.Expect(page.GetByRole(AriaRole.Button, new() { Name = "Menu", Expanded = false })).ToHaveCountAsync(1);

        // Act: show the nav area
        await button.ClickAsync();

        // Assert
        await Assertions.Expect(nav).Not.ToHaveAttributeAsync("hidden", "");
        await Assertions.Expect(button).ToHaveAttributeAsync("aria-expanded", "true");
        await AssertWrapperHasNoAriaExpandedAsync(page);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task FluentLayoutHamburger_DesktopMobile_BreakpointCrossing(bool hideNavFirst)
    {
        // Arrange
        var page = await OpenPageAsync(DesktopMobileUrl, Desktop);
        var button = HamburgerButton(page);
        await Assertions.Expect(button).ToHaveAttributeAsync("aria-expanded", "true");

        if (hideNavFirst)
        {
            await button.ClickAsync();
            await Assertions.Expect(button).ToHaveAttributeAsync("aria-expanded", "false");
        }

        // Act: below the breakpoint, the hamburger controls the (closed) drawer.
        await page.SetViewportSizeAsync(Mobile.Width, Mobile.Height);

        // Assert
        await Assertions.Expect(page.Locator("#layout")).ToHaveAttributeAsync("mobile", "");
        await Assertions.Expect(button).ToHaveAttributeAsync("aria-expanded", "false");
        Assert.False(await IsDrawerOpenAsync(page));

        // Act: back above the breakpoint, the hamburger controls the nav area again.
        await page.SetViewportSizeAsync(Desktop.Width, Desktop.Height);

        // Assert: aria-expanded matches the nav area visibility, whatever it is.
        await Assertions.Expect(page.Locator("#layout")).Not.ToHaveAttributeAsync("mobile", "");
        var navVisible = !await NavArea(page).EvaluateAsync<bool>("nav => nav.hasAttribute('hidden')");
        await Assertions.Expect(button).ToHaveAttributeAsync("aria-expanded", navVisible ? "true" : "false");
        await AssertWrapperHasNoAriaExpandedAsync(page);
    }

    [Theory]
    [InlineData("Enter", true)]
    [InlineData(" ", true)]
    [InlineData("Enter", false)]
    [InlineData(" ", false)]
    public async Task FluentLayoutHamburger_ActivationKey_TogglesOnce(string key, bool mobile)
    {
        // Arrange
        var page = await OpenPageAsync(DesktopMobileUrl, mobile ? Mobile : Desktop);
        var button = HamburgerButton(page);
        await Assertions.Expect(button).ToHaveAttributeAsync("aria-expanded", mobile ? "false" : "true");

        await page.EvaluateAsync("""
            id => {
                window.drawerToggles = [];
                document.getElementById(id).addEventListener('toggle', e => {
                    if (e.target.id === id) {
                        window.drawerToggles.push(e.detail?.newState ?? e.newState);
                    }
                });
            }
            """, DrawerId);
        await button.FocusAsync();

        // Act
        await page.Keyboard.PressAsync(key);

        // Assert: one toggle, not two that cancel each other out.
        await Assertions.Expect(button).ToHaveAttributeAsync("aria-expanded", mobile ? "true" : "false");
        await Task.Delay(SettleDelayMilliseconds);
        await Assertions.Expect(button).ToHaveAttributeAsync("aria-expanded", mobile ? "true" : "false");

        if (mobile)
        {
            Assert.True(await IsDrawerOpenAsync(page));
            Assert.Equal(["open"], await page.EvaluateAsync<string[]>("() => window.drawerToggles"));
        }
        else
        {
            await Assertions.Expect(NavArea(page)).ToHaveAttributeAsync("hidden", "");
        }

        await AssertWrapperHasNoAriaExpandedAsync(page);
    }

    [Fact]
    public async Task FluentLayoutHamburger_DesktopMobile_DeferredNavigation_BreakpointCrossing()
    {
        // Arrange: with NavigationDeferredLoading, the nav area is only rendered on desktop.
        var page = await OpenPageAsync($"{DesktopMobileUrl}?deferred=true", Desktop);
        var button = HamburgerButton(page);
        await Assertions.Expect(button).ToHaveAttributeAsync("aria-expanded", "true");

        // Act: below the breakpoint, .NET removes the nav area.
        await page.SetViewportSizeAsync(Mobile.Width, Mobile.Height);

        // Assert
        await Assertions.Expect(NavArea(page)).ToHaveCountAsync(0);
        await Assertions.Expect(button).ToHaveAttributeAsync("aria-expanded", "false");

        // Act: back above the breakpoint, .NET renders the nav area again, after the breakpoint sync.
        await page.SetViewportSizeAsync(Desktop.Width, Desktop.Height);

        // Assert: the new nav area is visible, so the hamburger is expanded.
        await Assertions.Expect(NavArea(page)).ToHaveCountAsync(1);
        await Assertions.Expect(NavArea(page)).Not.ToHaveAttributeAsync("hidden", "");
        await Assertions.Expect(button).ToHaveAttributeAsync("aria-expanded", "true");
        await Assertions.Expect(button).ToHaveAttributeAsync("aria-controls", "nav");
    }

    [Fact]
    public async Task FluentLayoutHamburger_DesktopMobile_TwoHamburgers_StayInSync()
    {
        // Arrange: two hamburgers of the same layout share the nav area on desktop.
        var page = await OpenPageAsync($"{DesktopMobileUrl}?second=true", Desktop);
        var first = HamburgerButton(page);
        var second = page.Locator("#hamburger2 > fluent-button");
        await Assertions.Expect(first).ToHaveAttributeAsync("aria-expanded", "true");
        await Assertions.Expect(second).ToHaveAttributeAsync("aria-expanded", "true");

        // Act: hide the nav area with the first one
        await first.ClickAsync();

        // Assert: both are collapsed
        await Assertions.Expect(NavArea(page)).ToHaveAttributeAsync("hidden", "");
        await Assertions.Expect(first).ToHaveAttributeAsync("aria-expanded", "false");
        await Assertions.Expect(second).ToHaveAttributeAsync("aria-expanded", "false");

        // Act: show the nav area with the second one. Without the nav area, the content area overlaps
        // the header of this test page, so the click is dispatched to the button rather than sent with the mouse.
        await second.DispatchEventAsync("click");

        // Assert: both are expanded
        await Assertions.Expect(NavArea(page)).Not.ToHaveAttributeAsync("hidden", "");
        await Assertions.Expect(first).ToHaveAttributeAsync("aria-expanded", "true");
        await Assertions.Expect(second).ToHaveAttributeAsync("aria-expanded", "true");
    }

    [Fact]
    public async Task FluentLayoutHamburger_EmptyIconTitle_KeepsButtonRole()
    {
        // Arrange: IconTitle="" means FluentButton gets no Title, so it does not add role="button" itself.
        var page = await OpenPageAsync($"{MobileOnlyUrl}?notitle=true", Mobile);
        var button = HamburgerButton(page);

        // Assert: aria-expanded and aria-controls are on an element with role="button".
        await Assertions.Expect(button).ToHaveAttributeAsync("role", "button");
        await Assertions.Expect(button).ToHaveAttributeAsync("aria-expanded", "false");
        await Assertions.Expect(button).ToHaveAttributeAsync("aria-controls", DrawerId);

        // Act
        await button.ClickAsync();

        // Assert
        await Assertions.Expect(button).ToHaveAttributeAsync("role", "button");
        await Assertions.Expect(button).ToHaveAttributeAsync("aria-expanded", "true");
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task FluentLayoutHamburger_DesktopMobile_AriaControlsFollowsTarget(bool navHasId)
    {
        // Arrange
        var page = await OpenPageAsync(navHasId ? DesktopMobileUrl : $"{DesktopMobileUrl}?nonavid=true", Desktop);
        var button = HamburgerButton(page);
        await Assertions.Expect(button).ToHaveAttributeAsync("aria-expanded", "true");

        // Assert: on desktop the button controls the nav area (only referenced when it has an id).
        await AssertControlsAsync(page, button, navHasId ? "nav" : null);

        // Act: below the breakpoint, the button controls the drawer.
        await page.SetViewportSizeAsync(Mobile.Width, Mobile.Height);

        // Assert
        await Assertions.Expect(page.Locator("#layout")).ToHaveAttributeAsync("mobile", "");
        await AssertControlsAsync(page, button, DrawerId);

        // Act: back above the breakpoint.
        await page.SetViewportSizeAsync(Desktop.Width, Desktop.Height);

        // Assert
        await Assertions.Expect(page.Locator("#layout")).Not.ToHaveAttributeAsync("mobile", "");
        await AssertControlsAsync(page, button, navHasId ? "nav" : null);
    }

    [Fact]
    public async Task FluentLayoutHamburger_NestedLayout_OpensItsOwnDrawer()
    {
        // Arrange: a 400px layout (mobile) nested in a desktop-width layout, statically rendered.
        // Without a circuit, the startup script (also run after each enhanced navigation) wires every hamburger:
        // the nested one must be wired to its own layout, not to the outer one that contains it.
        var page = await WaitOpenPageAsync("/layout/hamburger-nested", openHeadlessBrowser: true, interactive: false);
        await page.SetViewportSizeAsync(Desktop.Width, Desktop.Height);
        await page.ReloadAsync();

        await page.WaitForFunctionAsync("""
            () => ['outer', 'inner'].every(name =>
                document.getElementById(name)?.fluentLayoutInitialized === true
                && document.getElementById(name + '-hamburger')?.fluentHamburgerInitialized === true)
            """);

        var outerButton = page.Locator("#outer-hamburger > fluent-button");
        var innerButton = page.Locator("#inner-hamburger > fluent-button");
        var outerNav = page.Locator("#outer-nav");
        await Assertions.Expect(page.Locator("#outer")).Not.ToHaveAttributeAsync("mobile", "");
        await Assertions.Expect(page.Locator("#inner")).ToHaveAttributeAsync("mobile", "");
        await Assertions.Expect(outerButton).ToHaveAttributeAsync("aria-expanded", "true");
        await Assertions.Expect(innerButton).ToHaveAttributeAsync("aria-expanded", "false");

        // Act
        await innerButton.ClickAsync();

        // Assert: the nested drawer opens, and the outer nav area and hamburger are unchanged.
        await Assertions.Expect(innerButton).ToHaveAttributeAsync("aria-expanded", "true");
        Assert.True(await page.EvaluateAsync<bool>("() => document.getElementById('inner-hamburger-drawer')?.dialog?.open === true"));
        await Assertions.Expect(outerNav).Not.ToHaveAttributeAsync("hidden", "");
        await Assertions.Expect(outerButton).ToHaveAttributeAsync("aria-expanded", "true");
    }

    /// <summary>
    /// Checks aria-controls, and that it references an element of the page.
    /// </summary>
    private static async Task AssertControlsAsync(IPage page, ILocator button, string? id)
    {
        if (id is null)
        {
            await Assertions.Expect(button).Not.ToHaveAttributeAsync("aria-controls", new Regex(".*"));
            return;
        }

        await Assertions.Expect(button).ToHaveAttributeAsync("aria-controls", id);
        await Assertions.Expect(page.Locator($"#{id}")).ToHaveCountAsync(1);
    }

    /// <summary>
    /// Opens the page at the given viewport size, reloading it so the layout starts at that size.
    /// </summary>
    private async Task<IPage> OpenPageAsync(string url, (int Width, int Height) viewport)
    {
        var page = await WaitOpenPageAsync(url, openHeadlessBrowser: true);

        await page.SetViewportSizeAsync(viewport.Width, viewport.Height);
        await page.RunAndWaitForConsoleMessageAsync(
            async () => await page.ReloadAsync(),
            new() { Predicate = msg => msg.Text.Contains("WebSocket connected"), Timeout = 5000 });

        // Wait for the interactive initialization of the layout and of the hamburger.
        await page.WaitForFunctionAsync("""
            mobile => {
                const layout = document.getElementById('layout');
                const hamburger = document.getElementById('hamburger');
                return layout?.fluentLayoutInitialized === true
                    && hamburger?.fluentHamburgerInitialized === true
                    && hamburger._fluentHamburgerDotNetHelper != null
                    && layout.hasAttribute('mobile') === mobile;
            }
            """, viewport.Width <= 768);

        return page;
    }

    private static ILocator HamburgerButton(IPage page) => page.Locator("#hamburger > fluent-button");

    private static ILocator NavArea(IPage page) => page.Locator("#layout .fluent-layout-item[area='nav']");

    private static Task<bool> IsDrawerOpenAsync(IPage page)
        => page.EvaluateAsync<bool>("id => document.getElementById(id)?.dialog?.open === true", DrawerId);

    private static async Task AssertWrapperHasNoAriaExpandedAsync(IPage page)
        => Assert.False(await page.Locator("#hamburger").EvaluateAsync<bool>("wrapper => wrapper.hasAttribute('aria-expanded')"));
}
