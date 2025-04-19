# Microsoft Fluent UI Blazor library

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET C#](https://img.shields.io/badge/.NET-C%23-blue)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![NuGet](https://img.shields.io/nuget/v/Microsoft.FluentUI.AspNetCore.Components?label=NuGet%20Component%20Library)](https://www.nuget.org/packages/Microsoft.FluentUI.AspNetCore.Components)
[![NuGet](https://img.shields.io/nuget/v/Microsoft.FluentUI.AspNetCore.Templates?label=NuGet%20Templates)](https://www.nuget.org/packages/Microsoft.FluentUI.AspNetCore.Templates)

[![Validate Security](https://github.com/microsoft/fluentui-blazor/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/microsoft/fluentui-blazor/actions/workflows/codeql-analysis.yml)
[![Discord](https://img.shields.io/badge/chat%20on-discord-7289da.svg)](https://discord.gg/M5cBTfp6J2)

:star:  We appreciate your star, it helps!

This package is for use in .NET 8 and 9 Blazor projects. If you are using **now unsupported** .NET 6 or 7, please use the v3 version of the packages (names starting with `Microsoft.Fast.Components.FluentUI`)

## Introduction

The `Microsoft.FluentUI.AspNetCore` family of packages provides a set of Razor components for [Blazor](https://blazor.net) applications, tools and utilities which are used to build applications that have a Fluent design (i.e. have the look and feel of modern Microsoft applications). 

Some of the components in the library are wrappers around Microsoft's official Fluent UI Web Components. Others are components that leverage the Fluent Design System or make it easier to work with Fluent. To get up and running with the library, see the **Getting Started** section below.

The source for the library is hosted in the [fluentui-blazor](https://github.com/microsoft/fluentui-blazor) repository at GitHub. Documentation on the components is available at the [demo site](https://www.fluentui-blazor.net). 

## Upgrading from an earlier version 

If you are upgrading from an earlier version of the library, please see the [what's new](https://www.fluentui-blazor.net/whatsnew) for information on (breaking) changes.

## Getting Started

### Using our dotnet templates

The easiest way to get started is by using our [Templates](https://www.nuget.org/packages/Microsoft.FluentUI.AspNetCore.Templates). These mimic the regular Blazor templates and come with the design and components pre-configured. You install them with this command:
```shell
dotnet new install Microsoft.FluentUI.AspNetCore.Templates
```

Navigate to a folder where you want to create your new project and run the following command to create a new project.
```shell
dotnet new fluentblazor --name MyApplication
```

If you want to create a new standalone WebAssembly project, you can use the following command:
```shell
dotnet new fluentblazorwasm --name MyApplication
```

Other available templates are:
- Fluent .NET Aspire Starter App (fluentaspire-starter)
- Fluent .NET MAUI Blazor Hybrid and Web App (fluentmaui-blazor-web)

When using Visual Studio, you can also use the **New Project** dialog to create a new project. The templates can be found by typine **Fluent** in the search field.

### Manual Install
To start using the Fluent UI Blazor library from scratch, you first need to install the main [NuGet package](https://www.nuget.org/packages/Microsoft.FluentUI.AspNetCore.Components/) in the project you want to use the library and its components.
You can use the NuGet package manager in your IDE or use the following command when using a CLI:

```shell
dotnet add package Microsoft.FluentUI.AspNetCore.Components
```

If you want to extend the functionality of the library with [icons](https://www.nuget.org/packages/Microsoft.FluentUI.AspNetCore.Components.Icons) or [emoji](https://www.nuget.org/packages/Microsoft.FluentUI.AspNetCore.Components.Emoji), you can install additional packages for that:

```shell
dotnet add package Microsoft.FluentUI.AspNetCore.Components.Icons
dotnet add package Microsoft.FluentUI.AspNetCore.Components.Emoji
```

### Script
As mentioned, we wrap the **Fluent UI Web Components** which are implemented in a script file. This **file is included in the library** itself and does not have to be downloaded or pulled from a CDN.

> By including the script in the library we can safeguard that you are always using the best matching script version.

Even when using **SSR (Static Server Rendering)**, the script will be included and loaded automatically. If you want the script to be loaded before Blazor starts, add it to your `App.razor` file like this:

```html
<script src="_content/Microsoft.FluentUI.AspNetCore.Components/Microsoft.FluentUI.AspNetCore.Components.lib.module.js" type="module" async></script>
```
If you add interactivity later, the Blazor script will kick in and try to load the web component script again but JavaScript will handle that gracefully by design.

### Reboot (optional)
**Reboot** is a collection of element-specific CSS changes in a single file to help kick-start building a site with the **Fluent UI Blazor** components. It provides an elegant, consistent, and simple baseline to build upon.

If you want to use **Reboot**, you'll need to add to your `app.razor`, `index.html` or `_Layout.cshtml` file a line that includes the stylesheet (`.css` file). This can be done by adding the following line to the `<head>` section:

```html    
<link href="/_content/Microsoft.FluentUI.AspNetCore.Components/css/reboot.css" rel="stylesheet" />
```

When using the templates to create your application, **Reboot** is already set-up for you.

_When creating a site that is hosted in a different base path,it might be necessary to remove the leading '/' from the stylesheet link._

### Register Services
Add the following in `Program.cs`

```csharp
builder.Services.AddFluentUIComponents();
```

If you're running your application on **Blazor Server**, make sure a default `HttpClient` is registered before the `AddFluentUIComponents` method.

```csharp
builder.Services.AddHttpClient();
```

### Add Component Providers
Add the following components at the end of your `MainLayout.razor` file.  
These providers are used by associated services to display Toasts, Dialog boxes, Tooltips or Message Bars correctly.

```xml
<FluentToastProvider />
<FluentDialogProvider />
<FluentTooltipProvider />
<FluentMessageBarProvider />
<FluentMenuProvider />
```
> **note:** You can remove providers which you are using in your application.

## Working with Icons and Emoji
We have additional packages available that include the complete Fluent UI System icons and Fluent UI Emoji collections. 
Please refer to the [Icons and Emoji](https://www.fluentui-blazor.net/IconsAndEmoji) page for more information.

## Usage
With the package installed, you can begin using the Fluent UI Blazor library components in the same way as any other Razor component. 

### Add Imports

After the package is added, you need to add the following in your  `_Imports.razor`

```razor
@using Microsoft.FluentUI.AspNetCore.Components
```

### Quick Start
This is literally all you need in your views to use Fluent UI Blazor library in your application.

```xml
<FluentCard>
  <h2>Hello World!</h2>
  <FluentButton Appearance="@Appearance.Accent">Click Me</FluentButton>
</FluentCard>
```

## Configuring the Design System
The Fluent UI Razor components are built on FAST's (Adaptive UI) technology, which enables design customization and personalization, while automatically
maintaining accessibility. This is accomplished through setting various "design tokens". The library exposes all design tokens, which you can use both from code as in a declarative way in your `.razor` pages. The different ways of working with design tokens are described in the [design tokens](https://www.fluentui-blazor.net/DesignTokens) page.

## Blazor Hybrid
You can use this library in **Blazor Hybrid** (MAUI/WPF/Windows Forms) projects. Setup is almost the same as described in the "Getting started" section above, but to get everything to work you'll need to take one extra steps (for now) described below.

### Temporary workaround for MAUI/WPF/Windows Forms issues

> [!NOTE]
> The workaround below only applies to .NET 8. As of .NET 9 this workaround is no longer needed. If you have this workaround in place for .NET 9 your Blazor Hybrid project **will not load**.

Currently when using the WebView to run Blazor (so all Hybrid variants) the web-components script is not imported automatically (see [#404](https://github.com/microsoft/fluentui-blazor/issues/404)). 
There is also an issue with loading the custom event handlers that are being configured by the web-components script. Until these are fixed on the WebView side, there is a workaround available, namely to intercept `'_framework/blazor.modules.json'` and provide proper JS initializers file (created by build). The needed	`initializersLoader.webview.js` has been added to the library and needs to be included with a script tag **before** the `_framework/blazor.webview.js` script tag:

```xml
<script app-name="{NAME OF YOUR APP}" src="./_content/Microsoft.FluentUI.AspNetCore.Components/js/initializersLoader.webview.js"></script>
<script src="_framework/blazor.webview.js"></script>
```

The `app-name` attribute needs to match your app's assembly name - initializersLoader uses 'app-name' to resolve name of the file with initializers.
initializersLoader replaces standard `fetch` function with one which provides the correct file in place of the empty `blazor.modules.json`. `fetch` is restored to its original state once `_framework/blazor.modules.json` request is intercepted.

For more information regarding the bug, see issue [15234](https://github.com/dotnet/maui/issues/15234) in the MAUI repo.
	
## Use the DataGrid component with EF Core or OData Client
If you want to use the `<FluentDataGrid>` with data provided through EF Core or an OData Client, you need to install an additional package so the grid knows how to resolve queries asynchronously for efficiency.
Please see the [DataGrid](https://www.fluentui-blazor.net/DataGrid) page for more information.


## Additional resources
* The Microsoft Fluent UI Blazor library [documentation and demo site](https://www.fluentui-blazor.net)

## Support
The Microsoft Fluent UI Blazor library is an open source project and is **not** an official part of ASP.NET Core, which means it’s **not** officially
supported and isn’t committed to ship updates as part of any official .NET updates. It is built and maintained by Microsoft employees (**and** other contributors)
and offers support, like most other open source projects, on a best effort base through the GitHub repository **only**.

## Contributing to the project

We offer some guidelines on how you can get started [contributing to the project](https://github.com/microsoft/fluentui-blazor/blob/main/docs/contributing.md). 
We also have a document that explains and shows how to [write and develop unit tests](https://github.com/microsoft/fluentui-blazor/blob/main/docs/unit-tests.md)

### 🏆 Contributors

<a href="https://github.com/microsoft/fluentui-blazor/graphs/contributors">
  <img src="https://contrib.rocks/image?repo=microsoft/fluentui-blazor" />
</a>

Made with [contrib.rocks](https://contrib.rocks).

## Joining the Community

Looking to get answers to questions or engage with us in real-time? Our community is active on [Discord](https://discord.gg/FcSNfg4). Submit requests 
and issues on [GitHub](https://github.com/microsoft/fluentui-blazor/issues/new/choose), or join us by contributing on [some good first issues via GitHub](https://github.com/microsoft/fluentui-blazor/labels/community:good-first-issue).

We look forward to building an amazing open source community with you!

## Contact

* Join the DotNetEvolution server and chat with us in real-time on [Discord](https://discord.gg/M5cBTfp6J2). 
* Submit requests and issues (only) on [GitHub](https://github.com/microsoft/fluentui-blazor/issues/new/choose).
* Contribute by helping out on some of our recommended first issues on [GitHub](https://github.com/microsoft/fluentui-blazor/labels/community:good-first-issue).
