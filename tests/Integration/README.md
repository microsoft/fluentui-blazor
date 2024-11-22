# Integration tests using Playwright for .NET

## Running the tests

First, make sure you have the Playwright for .NET package installed. If you don't, follow the instructions in the next section.

- Build the **InegrationTests** project to generate the test assembly in the Debug folder: `dotnet build`.
- Next, install the **Playwright browsers**, running the `playwright.ps1` script.

```powershell
dotnet build
./bin/Debug/net9.0/playwright.ps1 install
```

> **Note**: `dotnet build` needs to be run from the `Integration` folder.

More details on https://playwright.dev/dotnet/docs/intro

## Web Server for testing

The tests are running against a local web server.
This server is automatically started and stopped by the tests (see `StartServerFixture.cs`).

The server is started to listen on `http://localhost:5050`.

To start the server automatically, you need to include the test in a specific collection of tests:
```csharp
[Collection(StartServerCollection.Name)]
public class FluentButtonTests : FluentPlaywrightBaseTest
{
    public FluentButtonTests(ITestOutputHelper output, StartServerFixture server)
        : base(output, server)
    {
    }

    [Fact]
    public async Task FluentButton_IncrementCounter()
    {
        // Arrange
        var page = await WaitOpenPageAsync($"/button/default");
        
        // Act
        await page.ClickAsync("fluent-button");
        await Task.Delay(100);  // Wait for page to render
        
        // Assert
        var content = await page.ContentAsync();
        Assert.Contains("Current count: 1", content);
    }
}
```

> ⚠️ **Notes:**
>
> If you interrupt a test abruptly (for example, by pressing the Stop button on the Test Explorer)
> it is possible that the `StartServerFixture.DisposeAsync` procedure will not be called.
> In this case, the **web server remains running**.
> This can block the next compilation of the code.
> ```
> Could not copy "...\apphost.exe" to "bin\Debug\Microsoft.FluentUI.AspNetCore.Components.IntegrationTests.exe".
> Exceeded retry count of 10. Failed.
> The file is locked by: "Microsoft.FluentUI.AspNetCore.Components.IntegrationTests"
> ```
> You can kill it manually using the Task Manager (filtering on ‘IntegrationTests’).
