# Integration tests using Playwright for .NET

## Running the tests

First, make sure you have the Playwright for .NET package installed. If you don't, follow the instructions in the next section.

- Build the **InegrationTests** project to generate the test assembly in the Debug folder: `dotnet build`.
- Next, install the **Playwright browsers**, running the `playwright.ps1` script.

```powershell
dotnet build
pwsh bin/Debug/net<version>/playwright.ps1 install
```

