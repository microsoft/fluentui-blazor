// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.IntegrationTests.WebServer;
using Microsoft.Playwright;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.FluentUI.AspNetCore.Components.IntegrationTests.Components.KeyCode;

[Collection(StartServerCollection.Name)]
public class FluentKeyCodeTests : FluentPlaywrightBaseTest
{
    public FluentKeyCodeTests(ITestOutputHelper output, StartServerFixture server)
        : base(output, server)
    {
    }

    [Fact]
    public async Task FluentKeyCode_HandlesKeyboardEventWithMissingProperties()
    {
        // Arrange
        var page = await WaitOpenPageAsync("/keycode/malformed-keyboard-event", openDevTools: false);
        await page.WaitForFunctionAsync(
            "() => Object.keys(document._fluentKeyCodeEvents ?? {}).length > 0",
            null,
            new() { Timeout = 5000 });

        // Act
        await page.GetByTestId("key-target").EvaluateAsync<bool>(
            "element => element.dispatchEvent(new Event('keydown', { bubbles: true }))");

        // Assert
        await Assertions.Expect(page.GetByTestId("key-code")).ToHaveTextAsync("0");
        await Assertions.Expect(page.GetByTestId("key-location")).ToHaveTextAsync("0");
    }
}
