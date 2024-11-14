// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.IntegrationTests.Helpers;
using Microsoft.Playwright;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.FluentUI.AspNetCore.Components.IntegrationTests;

public class UnitTest1
{
    private readonly ITestOutputHelper _output;

    public UnitTest1(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public async Task Test1()
    {
        await Task.CompletedTask;
        var server = new PlaywrightFixture();
        await server.InitializeAsync(@"C:\VSO\Perso\fluentui-blazor-v5\tests\Integration\bin\Debug\net9.0", 5050);

        var page = await server.BrowserContext.NewPageAsync();
        page.Console += (_, msg) => _output.WriteLine(msg.Text);

        await page.GotoAsync("http://localhost:5050/button/default");
        var content = await page.ContentAsync();

        File.WriteAllText("C:\\_temp\\content.html", content);
    }
}
