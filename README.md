# Microsoft Fluent UI Blazor components

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET C#](https://img.shields.io/badge/.NET-C%23-blue)](https://docs.microsoft.com/en-us/dotnet/csharp/)
![Nuget](https://img.shields.io/nuget/v/Microsoft.Fast.Components.FluentUI?label=NuGet%20Component%20Library)
![Nuget](https://img.shields.io/nuget/v/Microsoft.Fast.Templates.FluentUI?label=NuGet%20Templates)

[![Validate Security](https://github.com/microsoft/fluentui-blazor/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/microsoft/fluentui-blazor/actions/workflows/codeql-analysis.yml)
[![Gitter](https://img.shields.io/badge/chat%20on-gitter-7289da.svg)](https://app.gitter.im/#/room/#fluentui-blazor:gitter.im)
[![Discord](https://img.shields.io/badge/chat%20on-discord-7289da.svg)](https://discord.gg/FcSNfg4)

:star:  We appreciate your star, it helps!

## Introduction

The `Microsoft.Fast.Components.FluentUI` package provides a set of [Blazor](https://blazor.net) components which you can use to build applications that have 
the look and feel or modern Microsoft applications. Some of the componets are wrappers around Microsoft's official FluentUI Web Components. Others are components 
that leverage the Fluent UI design system or make it easier to work with Fluent UI. To get up and running with the library, see the 'Getting Started' section below.

The source for the library is hosted in the [fluentui-blazor](https://github.com/microsoft/fluentui-blazor) repository at GitHub. Documentation on the components is available at the [demo site](https://www.fluentui-blazor.net) and at [docs.microsoft.com](https://docs.microsoft.com/en-us/fluent-ui/web-components/). 

The source for `@fluentui/web-components` is hosted in the [fluentui](https://github.com/microsoft/fluentui/tree/master/packages/web-components) mono-repository. Documentation for those components is available on [docs.microsoft.com](https://docs.microsoft.com/en-us/fluent-ui/web-components/).
The FluentUI Web Components are built on [FAST](https://www.fast.design/) and work in every major browser. 

## Upgrading from an earlier version 

If you are upgrading from an earlier version of the library, please see the [what's new](https://www.fluentui-blazor.net/whatsnew) for information on (breaking) changes.

## Getting Started

To get started using the Fluent UI Blazor components for Blazor, you will first need to install the official [Nuget package for Fluent UI Blazor](https://www.nuget.org/packages/Microsoft.Fast.Components.FluentUI/) in the project you would like to use the library and components. You can use the following command:

```shell
dotnet add package Microsoft.Fast.Components.FluentUI
```

### Script
As of version 2.3 it is no longer needed to include the `web-components` script in your `index.html` or `_Layout.cshtml` file. The script is now included in 
the library. This way we can safeguard that the you are always getting the version of the script that best matches the Blazor components.

> **If you are upgrading from an earlier version please remove the script from your `index.html` or `_Layout.cshtml` file.**


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
<link href="_content/Microsoft.Fast.Components.FluentUI/css/reboot.css" rel="stylesheet" />
```    

It is entirely possible to build a site without using Reboot. If you choose not to use it, please do add the `variables.css` file (which is otherwise imported through the `reboot.css` file) 
to your `index.html` or `_Layout.cshtml` file in the `<head>` section like this:

```html
<link href="_content/Microsoft.Fast.Components.FluentUI/css/variables.css" rel="stylesheet" />
```

The file contains a number of CSS variables that are required to be defined for the components to work correctly. 

### Project file
if you want to use icons and/or emoji, starting with version 2.1 you need add a `<PropertyGroup>` to your project file. Within this group you can specify which icons and emoji are made available for usage and publication. Please refer to the [project setup](https://www.fluentui-blazor.net/ProjectSetup) document for more information.


### Code
Please refer to the [code setup](https://www.fluentui-blazor.net/CodeSetup) document to learn what needs to be included in your `Program.cs` file 
so that all necessary services are available and setup in the correct way.


## Getting started by using project templates
To make it easier to start a project that uses the Fluent UI Blazor components out of the box, we have created the
[Microsoft.Fast.Templates.FluentUI](https://www.nuget.org/packages/Microsoft.Fast.Templates.FluentUI/) template package.

The package contains templates for creating Blazor Server and/or Blazor WebAssembly apps that mimic the regular Blazor 
templates. The library is already set up (and all the Bootstrap styling removed). All components fromthe regular template have been 
replaced with Fluent UI Blazor counterparts (and a few extra have been added). Please see the [documentation page](https://www.fluentui-blazor.net/Templates)
for more information.

If you want to use icons and/or emoji with applications based on the templates, you still need to make the changes to the project file 
and `Program.cs` as described in the previous sections.


## Using the FluentUI Web Components
With the package installed and the script configured, you can begin using the Fluent UI Blazor components in the same way 
as any other Blazor component. Just be sure to add the following using statement to your views:

```razor
@using Microsoft.Fast.Components.FluentUI
```

Here's a small example of a `FluentCard` with a `FluentButton` that uses the Fluent "Accent" appearance:

```razor
@using Microsoft.Fast.Components.FluentUI

<FluentCard>
  <h2>Hello World!</h2>
  <FluentButton Appearance="@Appearance.Accent">Click Me</FluentButton>
</FluentCard>
```
> :bulb: **Tip**
> 
> You can add `@using Microsoft.Fast.Components.FluentUI` to the namespace collection in `_Imports.razor`, so you don't have to add it to every razor page that uses one of the components.


## Configuring the Design System
The Fluent UI Blazor components are built on FAST's (Adaptive UI) technology, which enables design customization and personalization, while automatically
maintaining accessibility. This is accomplished through setting various "design tokens". The library exposes all design tokens, which you can use both from code as in a declarative way in your `.razor` pages. The three different ways of working with design tokens are described in the [design tokens](https://www.fluentui-blazor.net/DesignTokens) page.

## Blazor Hybrid
Starting with the 2.0 release, you can also use this library in your Blazor Hybrid projects. Setup is almost the same as described in the "Getting started" section above, but to get everything to work you'll need to take two extra steps:
1. You need to add a MAUI specific IStaticAssetService implementation.  
 Due to some issues, this file can't be part of the library (yet) so this needs to be added manually to your MAUI Blazor project.  
Create a new class in you project called `FileBasedStaticAssetService.cs` Replace it's contents with the following:  

```csharp
using System.Net;
using Microsoft.Fast.Components.FluentUI.Infrastructure;

namespace Microsoft.Fast.Components.FluentUI;

public class FileBasedStaticAssetService : IStaticAssetService
{
	public async Task<string> GetAsync(string assetUrl, bool useCache = false)
	{
		string result = null;
		HttpRequestMessage message = CreateMessage(assetUrl);
		if (string.IsNullOrEmpty(result))
		{
			result = await ReadData(assetUrl);
		}
		return result;
	}
	private static HttpRequestMessage CreateMessage(string url) => new(HttpMethod.Get, url);
 
	private static async Task<string> ReadData(string file)
	{
		using var stream = await FileSystem.OpenAppPackageFileAsync($"wwwroot/{file}");
		using var reader = new StreamReader(stream);
		return await reader.ReadToEndAsync();
	}
}
```

2. You need to make some changes in your `MauiProgram.cs` file  
Make sure the following is added before the `return builder.Build()` line:  
```csharp
builder.Services.AddFluentUIComponents(options =>
{
		options.HostingModel = BlazorHostingModel.Hybrid;
});
builder.Services.AddScoped<IStaticAssetService, FileBasedStaticAssetService>();
```
### Tempory workaround for MAUI issues
Currently in MAUI the web-components script is not imported automatically (see [#404](https://github.com/microsoft/fluentui-blazor/issues/404). There is also an isue with loading the custom event handelers that are being raised by the web-components script. Until these are fixed on the MAUI side, there is a workaround available, namely to intercept '_framework/blazor.modules.json' and provide proper JS initializers file (created by build). You can drop this [initializersLoader.windows.js](https://github.com/andreisaperski/fluentui-blazor/blob/hybrid-examples/examples/FluentUI.Demo.Hybrid.Shared/wwwroot/js/initializersLoader.windows.js) into wwwroot folder of your app and add a script tag for it to your `index.html` right before the `_framework/blazor.webview.js` tag:

```xml
<script app-name="FluentUI.Demo.Hybrid.MAUI" src="./_content/FluentUI.Demo.Hybrid.Shared/js/initializersLoader.windows.js"></script>
<script src="_framework/blazor.webview.js"></script>
```

The `app-name` attribute needs to match your app's assembly name - initializersLoader uses 'app-name' to resolve name of the file with initializers.
initializersLoader replaces standard `fetch` function with one which provides the correct file in place of the empty `blazor.modules.json`. `fetch` is restored to its original state once `_framework/blazor.modules.json` request is intercepted.

For more information regarding the MAUI bug see issues [15234](https://github.com/dotnet/maui/issues/15234) there.
	
## Use the DataGrid component with EF Core
If you want to use the `<FluentDataGrid>` with data provided through EF Core, you need to install 
an additional package so the grid knows how to resolve queries asynchronously for efficiency.  .

### Installation
Install the package by running the command:
```
dotnet add package Microsoft.Fast.Components.FluentUI.DataGrid.EntityFrameworkAdapter
```

### Usage
In your Program.cs file you need to add the following after the `builder.Services.AddFluentUIComponents(...);` lines:
```csharp
builder.Services.AddDataGridEntityFrameworkAdapter();
```


## Additional resources
* The Microsoft Fluent UI Blazor components [documentation and demo site](https://www.fluentui-blazor.net)


### Joining the Community

Looking to get answers to questions or engage with us in real-time? Our community is  active on [Gitter](https://app.gitter.im/#/room/#fluentui-blazor:gitter.im) and [Discord](https://discord.gg/FcSNfg4). Submit requests 
and issues on [GitHub](https://github.com/microsoft/blazor-fluentui/issues/new/choose), or join us by contributing on [some good first issues via GitHub](https://github.com/microsoft/fluentui-blazor/labels/community:good-first-issue).

We look forward to building an amazing open source community with you!

### Contact

* Join the community and chat with us in real-time on [Gitter](https://app.gitter.im/#/room/#fluentui-blazor:gitter.im) or [Discord](https://discord.gg/FcSNfg4).
* Submit requests and issues on [GitHub](https://github.com/microsoft/fluentui-blazor/issues/new/choose).
* Contribute by helping out on some of our recommended first issues on [GitHub](https://github.com/microsoft/fluentui-blazor/labels/community:good-first-issue).
