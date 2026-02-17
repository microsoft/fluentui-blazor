# Setup Guide — Fluent UI Blazor v5

## Prerequisites

- **.NET 9 SDK** or later (v5 does **not** support net8.0)
- Visual Studio 2022 17.12+ or VS Code with C# Dev Kit

## Install the NuGet Package

```bash
dotnet add package Microsoft.FluentUI.AspNetCore.Components --prerelease
```

> During the preview period, use `--prerelease`. Once stable, drop the flag.

## Register Services — Program.cs

```csharp
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);

// Add Fluent UI services with default configuration
builder.Services.AddFluentUIComponents();
```

### Custom configuration

```csharp
builder.Services.AddFluentUIComponents(new LibraryConfiguration
{
    // Change service lifetime (default: Scoped, also supports Singleton)
    ServiceLifetime = ServiceLifetime.Scoped,

    // Customize tooltip behavior
    Tooltip = new LibraryTooltipOptions
    {
        UseServiceProvider = true,  // Required for ITooltipService
        Delay = 300,                // Delay in ms before showing
    },

    // Customize field/form defaults
    DefaultStyles = new DefaultStyles
    {
        FluentFieldClass = "my-field-class",
    },

    // Set component-level defaults
    DefaultValues = new DefaultValues
    {
        // Per-component property defaults
        // See LibraryConfiguration documentation for available options
    },
});
```

### Action-based configuration

```csharp
builder.Services.AddFluentUIComponents(config =>
{
    config.Tooltip.Delay = 500;
});
```

## Add Providers — Layout

In your `MainLayout.razor` or `App.razor`, add the providers component:

```razor
@using Microsoft.FluentUI.AspNetCore.Components

<FluentProviders />

<FluentLayout>
    @* Your layout content *@
    @Body
</FluentLayout>
```

`<FluentProviders />` registers the required overlay containers for:
- **Dialogs** (FluentDialogProvider)
- **Tooltips** (FluentTooltipProvider)
- **Keyboard shortcuts** (FluentKeyCodeProvider)

> **Important**: Without `<FluentProviders />`, dialogs and tooltips will
> silently fail to appear.

## Add Stylesheet — index.html / App.razor

```html
<link href="_content/Microsoft.FluentUI.AspNetCore.Components/css/default-fuib.css"
      rel="stylesheet" />
```

This stylesheet applies Fluent design tokens (font family, foreground color,
etc.) to the document body.

## Add Imports — _Imports.razor

```razor
@using Microsoft.FluentUI.AspNetCore.Components
```

## Blazor Server vs WebAssembly vs Auto

The setup is identical for all hosting models. The only difference is **where**
you call `AddFluentUIComponents()`:

| Hosting Model | Register services in |
|---|---|
| Blazor Server | Server `Program.cs` |
| Blazor WebAssembly | Client `Program.cs` |
| Blazor Web App (Auto) | **Both** Server and Client `Program.cs` |

### Blazor Web App (Auto) — dual registration

```csharp
// Server/Program.cs
builder.Services.AddFluentUIComponents();

// Client/Program.cs
builder.Services.AddFluentUIComponents();
```

## Verify Installation

Create a test page to verify everything works:

```razor
@page "/fluent-test"

<FluentButton Appearance="ButtonAppearance.Primary"
              OnClick="@(() => count++)">
    Clicked @count times
</FluentButton>

@code {
    private int count = 0;
}
```

If the button renders with Fluent styling and responds to clicks, setup is
complete.

## Troubleshooting

| Symptom | Cause | Fix |
|---|---|---|
| Components render as plain HTML | Missing CSS/JS assets | Add the stylesheet link |
| Dialog doesn't appear | Missing providers | Add `<FluentProviders />` to layout |
| `InvalidOperationException` on component render | Missing service registration | Call `AddFluentUIComponents()` in Program.cs |
| Build error: `net8.0 not supported` | Wrong TFM | Change to `net9.0` or `net10.0` |
| `Appearance.Accent` doesn't compile | v4 enum removed | Use `ButtonAppearance.Primary` |
