// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.IntegrationTests.WebServer;
using Microsoft.Playwright;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.FluentUI.AspNetCore.Components.IntegrationTests.Components.List;

[Collection(StartServerCollection.Name)]
public class FluentListboxTests : FluentPlaywrightBaseTest
{
    public FluentListboxTests(ITestOutputHelper output, StartServerFixture server)
        : base(output, server)
    {
    }

    [Theory]
    [InlineData("selected")]
    [InlineData("current-selected")]
    public async Task FluentListbox_RenderedSelection_DoesNotRaiseChange(string attribute)
    {
        var page = await WaitOpenPageAsync("/list/listbox/selection", openHeadlessBrowser: true);
        await page.WaitForFunctionAsync("() => document.querySelector('#colors fluent-option').tabIndex === 0");
        await page.Locator("#colors fluent-option").Nth(1).ClickAsync();

        var changes = await page.EvaluateAsync<string[]>("""
            async attribute => {
                const changes = [];
                document.getElementById('colors').addEventListener('listboxchange', event => {
                    changes.push(event.detail.selectedOptions);
                });
                const options = document.querySelectorAll('#colors fluent-option');
                options[1].removeAttribute(attribute);
                options[0].setAttribute(attribute, '');
                await new Promise(resolve => requestAnimationFrame(() => requestAnimationFrame(resolve)));
                return changes;
            }
            """, attribute);

        Assert.Empty(changes);
        await Assertions.Expect(page.GetByTestId("change-count")).ToHaveTextAsync("0");
        Assert.True(await page.Locator("#colors fluent-option").First.EvaluateAsync<bool>("option => option.selected"));
    }

    [Fact]
    public async Task FluentListbox_MultipleSelection_PreservesClickAndKeyboardChanges()
    {
        var page = await WaitOpenPageAsync("/list/listbox/selection", openHeadlessBrowser: true);
        var options = page.Locator("#multiple-colors fluent-option");

        await options.First.ClickAsync();
        await options.Nth(2).ClickAsync();

        await Assertions.Expect(page.GetByTestId("selected-colors")).ToHaveTextAsync("Red, Blue");
        await Assertions.Expect(page.GetByTestId("multiple-change-count")).ToHaveTextAsync("2");

        await options.First.FocusAsync();
        await options.First.PressAsync("Space");

        await Assertions.Expect(page.GetByTestId("selected-colors")).ToHaveTextAsync("Blue");
        await Assertions.Expect(page.GetByTestId("multiple-change-count")).ToHaveTextAsync("3");
    }

    [Theory]
    [InlineData("Space")]
    [InlineData("Enter")]
    public async Task FluentListbox_KeyboardSelection_RaisesOneChange(string key)
    {
        var page = await WaitOpenPageAsync("/list/listbox/selection", openHeadlessBrowser: true);
        var option = page.Locator("#colors fluent-option").First;
        await option.FocusAsync();

        await option.PressAsync(key);

        await Assertions.Expect(page.GetByTestId("current-value")).ToHaveTextAsync("Red");
        await Assertions.Expect(page.GetByTestId("change-count")).ToHaveTextAsync("1");
    }

    [Fact]
    public async Task FluentListbox_RapidClicks_SettlesOnLastSelection()
    {
        var page = await WaitOpenPageAsync("/list/listbox/selection", openHeadlessBrowser: true);
        await page.WaitForFunctionAsync("() => document.querySelector('#colors fluent-option').tabIndex === 0");

        await page.EvaluateAsync("""
            () => {
                const options = document.querySelectorAll('#colors fluent-option');
                for (let index = 0; index < 30; index++) {
                    options[index % 2].click();
                }
            }
            """);

        await Assertions.Expect(page.GetByTestId("change-count")).ToHaveTextAsync("30");
        await Assertions.Expect(page.GetByTestId("current-value")).ToHaveTextAsync("Green");
        await page.WaitForTimeoutAsync(20000);
        await Assertions.Expect(page.GetByTestId("change-count")).ToHaveTextAsync("30");
        await Assertions.Expect(page.GetByTestId("current-value")).ToHaveTextAsync("Green");
        Assert.Equal("Green", await page.Locator("#colors fluent-listbox").EvaluateAsync<string>(
            "listbox => listbox.selectedOptions.map(option => option.textContent.trim()).join(';')"));
    }
}
