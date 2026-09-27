// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.IntegrationTests.WebServer;
using Microsoft.Playwright;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.FluentUI.AspNetCore.Components.IntegrationTests.Components.Tabs;

[Collection(StartServerCollection.Name)]
public class FluentTabsTests : FluentPlaywrightBaseTest
{
    public FluentTabsTests(ITestOutputHelper output, StartServerFixture server)
        : base(output, server)
    {
    }

    [Theory]
    [InlineData(false, false, false, "block", "block")]
    [InlineData(false, true, false, "block", "flex")]
    [InlineData(true, false, false, "flex", "block")]
    [InlineData(true, true, false, "flex", "flex")]
    [InlineData(false, false, true, "block", "block")]
    [InlineData(false, true, true, "block", "flex")]
    [InlineData(true, false, true, "flex", "block")]
    [InlineData(true, true, true, "flex", "flex")]
    public async Task FluentTabs_Nested_UsesOwnOrientation(
        bool outerVertical, bool innerVertical, bool overflow, string outerDisplay, string innerDisplay)
    {
        var url = $"/tabs/nested?outerVertical={outerVertical}&innerVertical={innerVertical}&overflow={overflow}";
        var page = await WaitOpenPageAsync(url, openHeadlessBrowser: true);

        await Assertions.Expect(page.Locator("#outer-tabs")).ToHaveCSSAsync("display", outerDisplay);
        await Assertions.Expect(page.Locator("#inner-tabs")).ToHaveCSSAsync("display", innerDisplay);
    }
}
