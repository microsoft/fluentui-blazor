# Microsoft Fluent UI Blazor components

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET C#](https://img.shields.io/badge/.NET-C%23-blue)](https://docs.microsoft.com/en-us/dotnet/csharp/)
![Nuget](https://img.shields.io/nuget/v/Microsoft.FluentUI.AspNetCore.Components?label=NuGet%20Component%20Library)
![Nuget](https://img.shields.io/nuget/v/Microsoft.Fast.Templates.FluentUI?label=NuGet%20Templates)

[![Validate Security](https://github.com/microsoft/fluentui-blazor/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/microsoft/fluentui-blazor/actions/workflows/codeql-analysis.yml)
[![Gitter](https://img.shields.io/badge/chat%20on-gitter-7289da.svg)](https://app.gitter.im/#/room/#fluentui-blazor:gitter.im)
[![Discord](https://img.shields.io/badge/chat%20on-discord-7289da.svg)](https://discord.gg/FcSNfg4)

:star:  We appreciate your star, it helps!

**This package is for use in .NET 8 Blazor projects. If you are using .NET 6 or 7, please use the v3 version f the package which is named `Microsoft.Fast.Components.FluentUI`**


## V4.0 progress
- v4.0.0-rc.1 is out! Please test and report issues you find.
- v4.0.0-preview.2 is functionally equal to v3.2.2

## Introduction

The `Microsoft.FluentUI.AspNetCore.Components` package provides a set of [Blazor](https://blazor.net) components which are used to build applications that have a Fluent design (i.e. have the look and feel or modern Microsoft applications).

Some of the components in the library are wrappers around Microsoft's official Fluent UI Web Components. Others are components that leverage the Fluent design system or make it easier to work with Fluent UI. To get up and running with the library, see the 'Getting Started' section below.

The source for the library is hosted in the [fluentui-blazor](https://github.com/microsoft/fluentui-blazor) repository at GitHub. Documentation on the components is available at the [demo site](https://www.fluentui-blazor.net). 

## Upgrading from an earlier version 

If you are upgrading from an earlier version of the library, please see the [what's new](https://www.fluentui-blazor.net/whatsnew) for information on (breaking) changes.

## Getting Started

To get started using the Fluent UI Blazor components for Blazor, you will first need to install the official [Nuget package for Fluent UI Blazor](https://www.nuget.org/packages/Microsoft.FluentUI.AspNetCore.Components/) in the project you would like to use the library and components. You can use the following command:

```shell
dotnet add package Microsoft.FluentUI.AspNetCore.Components
```

### Script
The heart of this library is formed by the Fluent UI Web Components and implemented in a script file. This file 
is included in the library itself and does not have to be downloaded or pulled from a CDN.

It is dependant on what type of project you are creating, if the script needs to be added to your
`index.html` or `App.razor` file. When using SSR, you will need to include the web components script. As there is no Blazor script being loaded/used, our script will also not get loaded.

Include the following in your App.razor:
```
<script src="_content/Microsoft.FluentUI.AspNetCore.Components/js/web-components-v2.5.16.min.js" type="module" async></script>
```
If you would later add interactivity, the Blazor script will kick in and try to load the web component script again but JavaScript will handle that gracefully by design.

If you use the templates to create your project, inserting the script will be taken care of based on the choices you make when creating an app from the template.

By including the script in the library we can safeguard that you are always using the best matching script version.



### Styles
In order for this library to work as expected, you will need to add the composed scoped CSS file for the components. This can be done by 
adding the following line to the <head> section of your `index.html` or `_Layout.cshtml` file in the project you installed the package:


```html
<link href="{PROJECT_NAME}.styles.css" rel="stylesheet" /> 
```

It is possible that the line is already in place (but commented out).

#### Reboot
Reboot is a collection of element-specific CSS changes in a single file to help kick-start building a site with the Fluent UI Blazor components for Blazor. It provides an elegant, consistent, and simple baseline to build upon.

If you want to use Reboot, you'll need to add to your `index.html` or `_Layout.cshtml` file a line that includes the stylesheet (`.css` file). This can be done by adding the following line to the `<head>` section:

```html    
<link href="_content/Microsoft.FluentUI.AspNetCore.Components/css/reboot.css" rel="stylesheet" />
```    

It is entirely possible to build a site without using Reboot. If you choose not to use it, please do add the `variables.css` file (which is otherwise imported through the `reboot.css` file) 
to your `index.html` or `_Layout.cshtml` file in the `<head>` section like this:

```html
<link href="_content/Microsoft.FluentUI.AspNetCore.Components/css/variables.css" rel="stylesheet" />
```

The file contains a number of CSS variables that are required to be defined for the components to work correctly. 

### Code
Please refer to the [code setup](https://www.fluentui-blazor.net/CodeSetup) document to learn what needs to be included in your `Program.cs` file 
so that all necessary services are available and setup in the correct way.

## Working with Icons and Emoji
We have additional packages available that include the complete Fluent UI System icons and Fluent UI Emoji collections. 
Please refer to the [Icons and Emoji](https://www.fluentui-blazor.net/IconsAndEmoji) page for more information.

## Getting started by using project templates
To make it easier to start a project that uses the Fluent UI Blazor components out of the box, we have created the
[Microsoft.FluentUI.AspNetCore.Templates](https://www.nuget.org/packages/Microsoft.FluentUI.AspNetCore.Templates) template package.

The package contains templates for creating Blazor Server and/or Blazor WebAssembly apps that mimic the regular Blazor 
templates. The library is already set up (and all the Bootstrap styling removed). All components from the regular template have been 
replaced with Fluent UI Blazor counterparts (and a few extra have been added). Please see the [documentation page](https://www.fluentui-blazor.net/Templates)
for more information.

If you want to use icons and/or emoji with applications based on the templates, you still need to make the changes to the project file 
and `Program.cs` as described in the [project setup](https://www.fluentui-blazor.net/ProjectSetup) and [code setup](https://www.fluentui-blazor.net/CodeSetup) documents.


## Using the FluentUI Web Components
With the package installed and the script configured, you can begin using the Fluent UI Blazor components in the same way 
as any other Blazor component. Just be sure to add the following using statement to your views:

```razor
@using Microsoft.FluentUI.AspNetCore.Components
```

Here's a small example of a `FluentCard` with a `FluentButton` that uses the Fluent "Accent" appearance:

```razor
@using Microsoft.FluentUI.AspNetCore.Components

<FluentCard>
  <h2>Hello World!</h2>
  <FluentButton Appearance="@Appearance.Accent">Click Me</FluentButton>
</FluentCard>
```
> **Tip**
> 
> You can add `@using Microsoft.FluentUI.AspNetCore.Components` to the namespace collection in `_Imports.razor`, so you don't have to add it to every razor page 
that uses one of the components.


## Configuring the Design System
The Fluent UI Blazor components are built on FAST's (Adaptive UI) technology, which enables design customization and personalization, while automatically
maintaining accessibility. This is accomplished through setting various "design tokens". The library exposes all design tokens, which you can use both from code as in a declarative way in your `.razor` pages. The different ways of working with design tokens are described in the [design tokens](https://www.fluentui-blazor.net/DesignTokens) page.

## Blazor Hybrid
You can use this library in Blazor Hybrid (MAUI/WPF/Windows Forms) projects. Setup is almost the same as described in the "Getting started" section above, but to get everything to work you'll need to take some extra steps (for now):

1. You need to make some changes in your `{Type}Program.cs` file.
Make sure the following is added before the `return builder.Build()` line:  
```csharp
builder.Services.AddFluentUIComponents(options =>
{
		options.HostingModel = BlazorHostingModel.Hybrid;
});
```

### Temporary workaround for MAUI/WPF/Windows Forms issues
Currently when using the WebView to run Blazor (so all Hybrid variants) the web-components script is not imported automatically (see [#404](https://github.com/microsoft/fluentui-blazor/issues/404). 
There is also an issue with loading the custom event handlers that are being configured by the web-components script. Until these are fixed on the WebView side, there is a workaround available, namely to intercept `'_framework/blazor.modules.json'` and provide proper JS initializers file (created by build). The needed	`initializersLoader.webview.js` has been added to the library and needs to be included with a script tag **before** the `_framework/blazor.webview.js` script tag:

```xml
<script app-name="{NAME OF YOUR APP}" src="./_content/Microsoft.FluentUI.AspNetCore.Components/js/initializersLoader.webview.js"></script>
<script src="_framework/blazor.webview.js"></script>
```

The `app-name` attribute needs to match your app's assembly name - initializersLoader uses 'app-name' to resolve name of the file with initializers.
initializersLoader replaces standard `fetch` function with one which provides the correct file in place of the empty `blazor.modules.json`. `fetch` is restored to its original state once `_framework/blazor.modules.json` request is intercepted.

For more information regarding the bug, see issue [15234](https://github.com/dotnet/maui/issues/15234) in the MAUI repo.
	
## Use the DataGrid component with EF Core
If you want to use the `<FluentDataGrid>` with data provided through EF Core, you need to install an additional package so the grid knows how to resolve queries asynchronously for efficiency.  .

### Installation
Install the package by running the command:
```
dotnet add package Microsoft.FluentUI.AspNetCore.Components.DataGrid.EntityFrameworkAdapter
```

### Usage
In your `Program.cs` file, you need to add the following after the `builder.Services.AddFluentUIComponents(...);` lines:
```csharp
builder.Services.AddDataGridEntityFrameworkAdapter();
```


## Additional resources
* The Microsoft Fluent UI Blazor components [documentation and demo site](https://www.fluentui-blazor.net)


## Contributing to the project

We offer some guidelines on how you can get started [contributing to the project](https://github.com/microsoft/fluentui-blazor/blob/main/CONTRIBUTING.md). 
We alo have a document that explains and shows how to [write and develop unit tests](https://github.com/microsoft/fluentui-blazor/blob/main/unit-tests.md)

## Joining the Community

Looking to get answers to questions or engage with us in real-time? Our community is  active on [Gitter](https://app.gitter.im/#/room/#fluentui-blazor:gitter.im) and [Discord](https://discord.gg/FcSNfg4). Submit requests 
and issues on [GitHub](https://github.com/microsoft/blazor-fluentui/issues/new/choose), or join us by contributing on [some good first issues via GitHub](https://github.com/microsoft/fluentui-blazor/labels/community:good-first-issue).

We look forward to building an amazing open source community with you!

## Contact

* Join the community and chat with us in real-time on [Gitter](https://app.gitter.im/#/room/#fluentui-blazor:gitter.im) or [Discord](https://discord.gg/FcSNfg4).
* Submit requests and issues on [GitHub](https://github.com/microsoft/fluentui-blazor/issues/new/choose).
* Contribute by helping out on some of our recommended first issues on [GitHub](https://github.com/microsoft/fluentui-blazor/labels/community:good-first-issue).
