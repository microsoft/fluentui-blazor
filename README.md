# Microsoft Fluent UI Blazor components

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![NuGet component library](https://img.shields.io/nuget/vpre/Microsoft.FluentUI.AspNetCore.Components?label=NuGet%20Component%20Library)](https://www.nuget.org/packages/Microsoft.FluentUI.AspNetCore.Components)
[![NuGet templates](https://img.shields.io/nuget/vpre/Microsoft.FluentUI.AspNetCore.Templates?label=NuGet%20Templates)](https://www.nuget.org/packages/Microsoft.FluentUI.AspNetCore.Templates)
[![Validate Security](https://github.com/microsoft/fluentui-blazor/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/microsoft/fluentui-blazor/actions/workflows/codeql-analysis.yml)

:star:  We appreciate your star, it helps!

**Fluent UI Blazor** is a component library for building Blazor applications with Microsoft's Fluent design language. It includes components based on the official Fluent UI Web Components as well as Blazor-specific components and services.

> [!IMPORTANT]
> This branch contains Fluent UI Blazor v5. See the [v5 documentation and demos](https://v5.fluentui-blazor.net) and the [v4 to v5 migration guide](https://v5.fluentui-blazor.net/MigrationV5).

If you'd like to view the code for version 4, navigate to the [archive-v4](https://github.com/microsoft/fluentui-blazor/tree/archive-v4) branch.

**This package is for use in Blazor projects.**

## Introduction

The `Microsoft.FluentUI.AspNetCore.Components` package provides a set of [Blazor](https://blazor.net) components
which are used to build applications that have a [Fluent design](https://developer.microsoft.com/en-us/fluentui#/)
(i.e. have the look and feel of modern Microsoft applications).

Some of the components in the library are wrappers around Microsoft's official **Fluent UI Web Components**.
Others are components that leverage the Fluent Design System or make it easier to work with Fluent UI.
To get up and running with the library, see the **Getting Started** section below.

The source for the library is hosted in the [fluentui-blazor](https://github.com/microsoft/fluentui-blazor) repository
at GitHub. Documentation on the components is available at the [demo site](https://www.fluentui-blazor.net).

## Upgrading from an earlier version

If you are upgrading from an earlier version of the library, please see the [what's new](https://www.fluentui-blazor.net/whatsnew) for information on (breaking) changes.

## Create a project from a template

The template package provides ready-to-run Fluent UI versions of the standard Blazor templates:

- Fluent Blazor Web App
- Fluent Blazor WebAssembly Standalone App
- Fluent Aspire Starter App
- Fluent .NET MAUI Blazor Hybrid and Web App

Install the current v5 prerelease of the template package. Replace `<VERSION>` with the version shown on [NuGet](https://www.nuget.org/packages/Microsoft.FluentUI.AspNetCore.Templates):

```bash
dotnet new install Microsoft.FluentUI.AspNetCore.Templates::<VERSION>
```

Create a Blazor Web App or a standalone WebAssembly app:

```bash
dotnet new fluentblazor -o MyApplication
dotnet new fluentblazorwasm -o MyApplication
```

The templates configure the packages, styles, services, providers, and icons for you. Run `dotnet new list fluent` to see all installed Fluent templates and their options.

## Create a project manually

### 1. Install the packages

```bash
dotnet add package Microsoft.FluentUI.AspNetCore.Components --prerelease
```

Install the optional icons package when your application uses Fluent icons:

```bash
dotnet add package Microsoft.FluentUI.AspNetCore.Components.Icons
```

Daily builds are also available. See [Using the latest daily build](docs/using-latest-daily.md) before opting into them because they may contain breaking changes.

### 2. Add the imports

Add these namespaces to `_Imports.razor`:

```razor
@using Microsoft.FluentUI.AspNetCore.Components
@using Icons = Microsoft.FluentUI.AspNetCore.Components.Icons
```

The icons alias is only required when the icons package is installed.

### 3. Add the component styles

Add the library stylesheet to the `<head>` of `App.razor`, `index.html`, `_Layout.cshtml`, or `_Host.cshtml`, depending on the application's hosting model:

```html
<link href="_content/Microsoft.FluentUI.AspNetCore.Components/Microsoft.FluentUI.AspNetCore.Components.bundle.scp.css" rel="stylesheet" />
```

Existing Bootstrap or other framework styles can be removed when they are no longer used.

### 4. Register the services

In `Program.cs`:

```csharp
builder.Services.AddFluentUIComponents();
```

For a Blazor Web App using WebAssembly or Auto interactivity, register the services in both the server and client projects.

### 5. Add the providers

Add the provider component at the end of `MainLayout.razor` (or another top-level layout):

```razor
<FluentProviders />
```

It hosts the providers required by services such as dialogs and tooltips.

### 6. Enable interactivity

Interactive Fluent components require an interactive Blazor render mode. Configure interactivity globally or apply a render mode to the page or component that needs it:

```razor
@rendermode InteractiveServer
```

See the [ASP.NET Core Blazor render modes documentation](https://learn.microsoft.com/aspnet/core/blazor/components/render-modes) for Server, WebAssembly, and Auto configuration.

## Verify the installation

Add this to an interactive Razor page:

```razor
<FluentStack Orientation="Orientation.Vertical">
  <FluentButton Appearance="ButtonAppearance.Primary" OnClick="@IncrementCount">
    Click me
  </FluentButton>
  <FluentLabel>Current count: @currentCount</FluentLabel>
</FluentStack>

@code {
  private int currentCount;

  private void IncrementCount()
  {
    currentCount++;
  }
}
```

## Working with Icons and Emoji

We have additional packages available that include the complete **Fluent UI System icons** and **Fluent UI Emoji** collections.
Please refer to the [Icons](https://v5.fluentui-blazor.net/icon) and [Emojis](https://v5.fluentui-blazor.net/emoji) 
page for more information.

## Additional resources

* The Microsoft Fluent UI Blazor components [documentation and demo site](https://v5.fluentui-blazor.net)

## Additional packages

- [Fluent UI System Icons](https://www.nuget.org/packages/Microsoft.FluentUI.AspNetCore.Components.Icons)
- [Fluent UI Emoji](https://www.nuget.org/packages/Microsoft.FluentUI.AspNetCore.Components.Emoji)
- [Entity Framework adapter for FluentDataGrid](https://www.nuget.org/packages/Microsoft.FluentUI.AspNetCore.Components.DataGrid.EntityFrameworkAdapter)

## Documentation and support

- [v5 documentation and component demos](https://v5.fluentui-blazor.net)
- [GitHub issues](https://github.com/microsoft/fluentui-blazor/issues/new/choose)
- [Discord community](https://discord.gg/M5cBTfp6J2)
- [Contributing guide](docs/contributing.md)

The Microsoft Fluent UI Blazor library is an open source project and is **not** an official part of ASP.NET Core, which means it’s **not** officially
supported and isn’t committed to ship updates as part of any official .NET updates. Like with most other open source projects, support is offered on a best effort basis
through the GitHub repository **only**.

## Contributing to the project

We offer some guidelines on how you can get started [contributing to the project](https://github.com/microsoft/fluentui-blazor/blob/dev/docs/contributing.md).
We also have a document that explains and shows how to [write and develop unit tests](https://github.com/microsoft/fluentui-blazor/blob/dev/docs/unit-tests.md)

## Joining the Community

Looking to get answers to questions or engage with us in real-time? Our community is  active on [Gitter](https://app.gitter.im/#/room/#fluentui-blazor:gitter.im) and [Discord](https://discord.gg/FcSNfg4). Submit requests
and issues on [GitHub](https://github.com/microsoft/fluentui-blazor/issues/new/choose), or join us by contributing on [some good first issues via GitHub](https://github.com/microsoft/fluentui-blazor/labels/community:good-first-issue).

We look forward to building an amazing open source community with you!

## Contact

* Join the DotNetEvolution server and chat with us in real-time on [Discord](https://discord.gg/M5cBTfp6J2). You can also find us on [Gitter](https://app.gitter.im/#/room/#fluentui-blazor:gitter.im).
* Submit requests and issues (only) on [GitHub](https://github.com/microsoft/fluentui-blazor/issues/new/choose).
* Contribute by helping out on some of our recommended first issues on [GitHub](https://github.com/microsoft/fluentui-blazor/labels/community:good-first-issue).