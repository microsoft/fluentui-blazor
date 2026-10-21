// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.IntegrationTests.WebServer;
using Microsoft.Playwright;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.FluentUI.AspNetCore.Components.IntegrationTests.Components.Button;

// Each interaction is tried with the trigger disabled and enabled, so that a menu staying closed is down to the
// trigger being disabled rather than to the interaction not opening the menu at all.
[Collection(StartServerCollection.Name)]
public class FluentSplitButtonTests : FluentPlaywrightBaseTest
{
    private const string Url = "/splitbutton/disabled";

    // Long enough for a menu that is going to open to have opened.
    private const int OpenDelayMilliseconds = 300;

    public FluentSplitButtonTests(ITestOutputHelper output, StartServerFixture server)
        : base(output, server)
    {
    }

    [Fact]
    public async Task FluentSplitButton_DisabledFocusable_IsDisabledForAssistiveTechnology()
    {
        // Arrange
        var page = await WaitOpenPageAsync(Url, openHeadlessBrowser: true);

        // Assert: both halves stay in the tab order and state that they are disabled.
        foreach (var half in new[] { "#split > fluent-button", "#split > fluent-menu-button" })
        {
            await Assertions.Expect(page.Locator(half)).ToHaveAttributeAsync("tabindex", "0");
            await Assertions.Expect(page.Locator(half)).ToHaveAttributeAsync("aria-disabled", "true");
        }

        // The browser's own accessibility tree, which is what assistive technology reads, has both as disabled buttons.
        var disabledButtons = await GetDisabledButtonNamesAsync(page);
        Assert.Contains("Save", disabledButtons);
        Assert.Contains("More actions", disabledButtons);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task FluentSplitButton_Click(bool disabled)
    {
        // Arrange
        var page = await OpenPageAsync(disabled);

        // Act: forced, as Playwright otherwise waits for a disabled button to be enabled.
        await page.ClickAsync("#split > fluent-button", new() { Force = true });
        await page.ClickAsync("#split > fluent-menu-button", new() { Force = true });

        // Assert
        Assert.Equal(!disabled, await IsMenuOpenAsync(page, "split"));
        await Assertions.Expect(page.GetByTestId("primary-clicks")).ToHaveTextAsync(disabled ? "0" : "1");
    }

    [Theory]
    [InlineData("Enter", true)]
    [InlineData(" ", true)]
    [InlineData("Enter", false)]
    [InlineData(" ", false)]
    public async Task FluentSplitButton_ActivationKey_IsConsumed(string key, bool disabled)
    {
        // Arrange
        var page = await OpenPageAsync(disabled);
        await page.FocusAsync("#split > fluent-menu-button");

        // Act
        await page.Keyboard.PressAsync(key);

        // Assert: the key does not scroll the page either way.
        Assert.Equal(!disabled, await IsMenuOpenAsync(page, "split"));
        Assert.Equal(0, await page.EvaluateAsync<double>("window.scrollY"));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task FluentMenu_Hover(bool disabled)
    {
        // Arrange
        var page = await OpenPageAsync(disabled);

        // Act
        await page.HoverAsync("#hover-menu > fluent-menu-button");

        // Assert
        Assert.Equal(!disabled, await IsMenuOpenAsync(page, "hover-menu"));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task FluentMenu_ContextMenu(bool disabled)
    {
        // Arrange
        var page = await OpenPageAsync(disabled);

        // Act
        await page.ClickAsync("#context-menu > fluent-menu-button", new() { Button = Microsoft.Playwright.MouseButton.Right, Force = true });

        // Assert
        Assert.Equal(!disabled, await IsMenuOpenAsync(page, "context-menu"));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task FluentMenu_ExternalTrigger_Click(bool disabled)
    {
        // Arrange
        var page = await OpenPageAsync(disabled);

        // Act
        await page.ClickAsync("#external-trigger", new() { Force = true });

        // Assert
        Assert.Equal(!disabled, await IsMenuOpenAsync(page, "external-menu"));
    }

    [Fact]
    public async Task FluentSplitButton_DisabledWhileTheMenuIsOpen_DoesNotRunItsItems()
    {
        // Arrange: the split button gets disabled once its menu is open.
        var page = await OpenPageAsync(disabled: false);
        await page.ClickAsync("#disable-soon");
        await page.ClickAsync("#split > fluent-menu-button");
        Assert.True(await IsMenuOpenAsync(page, "split"));
        await Assertions.Expect(page.Locator("#split-item")).ToHaveAttributeAsync("disabled", "");

        // Act
        await page.ClickAsync("#split-item", new() { Force = true });

        // Assert
        await Task.Delay(OpenDelayMilliseconds);
        await Assertions.Expect(page.GetByTestId("item-clicks")).ToHaveTextAsync("0");
    }

    [Fact]
    public async Task FluentMenu_ConnectedBeforeTheOverride_DoesNotOpenFromADisabledTrigger()
    {
        // Arrange: a menu in the page before the web components are defined, as prerendered markup is.
        var page = await WaitOpenPageAsync(Url, openHeadlessBrowser: true);
        await page.AddInitScriptAsync("""
            document.addEventListener('DOMContentLoaded', () => {
                window.staticMenuBeforeDefinition = !customElements.get('fluent-menu');
                document.body.insertAdjacentHTML('beforeend',
                    '<fluent-menu id="static-menu" open-on-hover>' +
                    '<fluent-menu-button id="static-trigger" slot="trigger" disabled-focusable>Static menu</fluent-menu-button>' +
                    '<fluent-menu-list><fluent-menu-item>Static item</fluent-menu-item></fluent-menu-list>' +
                    '</fluent-menu>');
            });
            """);
        await page.ReloadAsync();
        await Assertions.Expect(page.Locator("#static-menu")).ToHaveCountAsync(1);
        Assert.True(await page.EvaluateAsync<bool>("window.staticMenuBeforeDefinition"));

        // Act: hovering, as a click is already stopped by the disabled-focusable button itself.
        await page.HoverAsync("#static-trigger");
        var openedWhileDisabled = await IsMenuOpenAsync(page, "static-menu");

        await page.Mouse.MoveAsync(0, 0);
        await page.EvaluateAsync("document.getElementById('static-trigger').removeAttribute('disabled-focusable')");
        await page.HoverAsync("#static-trigger");
        var openedOnceEnabled = await IsMenuOpenAsync(page, "static-menu");

        // Assert
        Assert.False(openedWhileDisabled);
        Assert.True(openedOnceEnabled);
    }

    /// <summary>Opens the test page with its split button and menu triggers disabled-focusable, or enabled.</summary>
    private async Task<IPage> OpenPageAsync(bool disabled)
    {
        var page = await WaitOpenPageAsync(Url, openHeadlessBrowser: true);
        var trigger = page.Locator("#split > fluent-menu-button");

        // The page starts disabled, and its interactive render replaces the prerendered markup.
        await Assertions.Expect(trigger).ToHaveAttributeAsync("disabled-focusable", "");
        if (!disabled)
        {
            await page.ClickAsync("#toggle-disabled");
            await Assertions.Expect(trigger).Not.ToHaveAttributeAsync("disabled-focusable", "");
        }

        return page;
    }

    /// <summary>
    /// Gets the names of the disabled buttons in Chromium's accessibility tree. Unlike Playwright's role queries, it
    /// includes the role and state that the web components set through ElementInternals.
    /// </summary>
    private static async Task<List<string>> GetDisabledButtonNamesAsync(IPage page)
    {
        var cdp = await page.Context.NewCDPSessionAsync(page);
        var tree = await cdp.SendAsync("Accessibility.getFullAXTree");

        return tree!.Value.GetProperty("nodes").EnumerateArray()
            .Where(node => node.TryGetProperty("role", out var role) && role.GetProperty("value").GetString() == "button")
            .Where(node => node.TryGetProperty("properties", out var properties) && properties.EnumerateArray().Any(property =>
                property.GetProperty("name").GetString() == "disabled" && property.GetProperty("value").GetProperty("value").GetBoolean()))
            .Select(node => node.TryGetProperty("name", out var name) ? name.GetProperty("value").GetString() ?? "" : "")
            .ToList();
    }

    /// <summary>Gets whether the menu list of the menu with the specified id is shown.</summary>
    private static async Task<bool> IsMenuOpenAsync(IPage page, string menuId)
    {
        await Task.Delay(OpenDelayMilliseconds);
        return await page.EvaluateAsync<bool>(
            "id => document.querySelector(`#${id} > fluent-menu-list`).matches(':popover-open')",
            menuId);
    }
}
