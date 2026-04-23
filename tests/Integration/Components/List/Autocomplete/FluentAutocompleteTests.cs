// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.IntegrationTests.WebServer;
using Microsoft.Playwright;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.FluentUI.AspNetCore.Components.IntegrationTests.Components.List;

[Collection(StartServerCollection.Name)]
public class FluentAutocompleteTests : FluentPlaywrightBaseTest
{
    public FluentAutocompleteTests(ITestOutputHelper output, StartServerFixture server)
        : base(output, server)
    {
    }

    [Fact(Skip = "Playwright is optional for the moment. This test will fail.")]
    public async Task FluentAutocomplete_OnChangeAfter()
    {
        // Arrange
        var page = await WaitOpenPageAsync($"/list/autocomplete/onchange-after", openDevTools: false);

        // Act
        await page.ClickAsync("fluent-text-input");
        await page.ClickAsync("fluent-option");
        await Task.Delay(100); // Wait for the onChange event to propagate

        // Assert
        await page.ScreenshotAsync(new()
        {
            Path = $"{Server.ScreenshotsFolder}FluentAutocomplete_OnChangeAfter.png"
        });

        await Assertions.Expect(page.GetByTestId("current-value"))
                        .ToContainTextAsync("1");
    }
}

