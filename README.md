# Microsoft Fluent UI Blazor components

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET C#](https://img.shields.io/badge/.NET-C%23-blue)](https://docs.microsoft.com/en-us/dotnet/csharp/)
![Nuget](https://img.shields.io/nuget/v/Microsoft.Fast.Components.FluentUI?label=NuGet%20Component%20Library)
![Nuget](https://img.shields.io/nuget/v/Microsoft.Fast.Templates.FluentUI?label=NuGet%20Templates)

[![Validate PRs](https://github.com/microsoft/fluentui-blazor/actions/workflows/ci-validate.yml/badge.svg)](https://github.com/microsoft/fluentui-blazor/actions/workflows/ci-validate.yml)
[![Validate Security](https://github.com/microsoft/fluentui-blazor/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/microsoft/fluentui-blazor/actions/workflows/codeql-analysis.yml)
[![Discord](https://img.shields.io/badge/chat%20on-discord-7289da.svg)](https://discord.gg/FcSNfg4)


:star:  We appreciate your star, it helps!

## Introduction

The `Microsoft.Fast.Components.FluentUI` package provides a set of [Blazor](https://blazor.net) components which you can use to build applications that have 
the look and feel or modern Microsoft applications. Some of the componets are wrappers around Microsoft's official FluentUI Web Components. Others are components 
that leverage the Fluent UI design system or make it easier to work with Fluent UI. To get up and running with the library, see the 'Getting Started' section below.

The source for the library is hosted in the [Fast Blazor](https://github.com/microsoft/fluentui-blazor) repository at GitHub. Documentation on the components is available at the [demo site](https://www.fluentui-blazor.net) and at [docs.microsoft.com](https://docs.microsoft.com/en-us/fluent-ui/web-components/). 

The source for `@fluentui/web-components` is hosted in the [Fluent UI](https://github.com/microsoft/fluentui/tree/master/packages/web-components) mono-repository. Documentation on the components is available on [docs.microsoft.com](https://docs.microsoft.com/en-us/fluent-ui/web-components/).
The FluentUI Web Components are built on [FAST](https://www.fast.design/) and work in every major browser. 

## When upgrading from an earlier version 

If you are upgrading from an earlier version of the library, please see the [what's new](https://www.fluentui-blazor.net/whatsnew) for information on (breaking) changes.

## Getting Started

To get started using the Fluent UI Blazor components for Blazor, you will first need to install [the official Nuget package for Fluent UI](https://www.nuget.org/packages/Microsoft.Fast.Components.FluentUI/) in the project you would like to use the library and components. You can use the following command:

```shell
dotnet add package Microsoft.Fast.Components.FluentUI
```

### Scripts

Next, you need to add the web components script. You can either add the script from CDN directly, or you can install it with NPM, whichever you prefer.

To add the script from CDN use the following markup:

```html
<script type="module" src="https://cdn.jsdelivr.net/npm/@fluentui/web-components/dist/web-components.min.js"></script>
```
> :notebook: **Note**
>
> If you prefer to use another CDN, that is entirely possible. Just make sure it is offering the Fluent UI package and you are getting the right `web-components.min.js` file)

The markup above always references the latest release of the components. When deploying to production, you will want to ship with a specific version. Here's an example of the markup for that:

```html
<script type="module" src="https://cdn.jsdelivr.net/npm/@fluentui/web-components@2.0.2/dist/web-components.min.js"></script>
```

The script tag is normally placed  in your `index.html` (`_Layout.cshtml` for Blazor server project) file in the script section at the bottom of the `<body>`.

If you wish to leverage NPM instead, run the following command:

```shell
npm install --save @fluentui/web-components
```

You can locate the single file script build in the following location:

```shell
node_modules/@fluentui/web-components/dist/web-components.min.js
```

Copy this to your `wwwroot/script` folder and reference it with a script tag as described above.

> :notebook: **Note**
>
> If you are setting up Fluent UI Blazor components on a Blazor Server project, you will need to escape the `@` character by repeating it in the source link. For more information check out the [Razor Pages syntax documentation](https://docs.microsoft.com/aspnet/core/mvc/views/razor).

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
if you want to use icons and/or emoji, starting with the 2.1 version of the library you need add a `<PropertyGroup>` to your project file. With this you can specify which icons and emoji are made available for usage and publication. Please refer to the v2.1 section in the [what's new](https://aka.ms/fluentui-blazor/whatsnew) document for more information.


### Code
In your `Program.cs` file you need to add the following:
```csharp
builder.Services.AddFluentUIComponents();
```

This addition makes sure all the necessary services the library uses are setup in a correct way.

**If you want to use icons and/or emoji, starting with the 2.1 version of the library you need to make some changes here. Please refer to the v2.1 section in the [what's new](https://aka.ms/fluentui-blazor/whatsnew) document for more information.**

*If you're running your application on Blazor Server, make sure a default HttpClient is available by adding the following:*

```csharp
builder.Services.AddHttpClient();
```

## Getting started by using project templates
To make it easier to start a project that uses the Fluent UI Web Components for Blazor out of the box, we have created the
[Microsoft.Fast.Templates.FluentUI](https://www.nuget.org/packages/Microsoft.Fast.Templates.FluentUI/) template package.

The package contains templates for creating Blazor Server and/or Blazor WebAssembly apps that mimic the regular Blazor 
templates with the Fluent UI components already set up (and all the Bootstrap styling removed). All components have been 
replaced with Fluent UI counterparts (and a few extra have been added). Please see the [documentation page](https://brave-cliff-0c0c93310-365.centralus.azurestaticapps.net/Templates)
for more information.

If you want to use icons and/or emoji with applications based on the templates, you still need to make the changes to the project file 
and `Program.cs` as described in the previous sections.


### Using the FluentUI Web Components
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


### Configuring the Design System
The Fluent UI Blazor components are built on FAST's Adaptive UI technology, which enables design customization and personalization, while automatically maintaining accessibility. This is accomplished through setting various "Design Tokens". The library exposes all of the (over 160) Design Tokens, which you can use both from code as in a declarative way in your `.razor` pages. See <a href="https://docs.microsoft.com/en-us/fluent-ui/web-components/design-system/design-tokens" target="_blank">https://docs.microsoft.com/en-us/fluent-ui/web-components/design-system/design-tokens</a> for more information on how Design Tokens work

#### Option 1: Using Design Tokens from C# code

Given the following `.razor` page fragment:
```html
<FluentButton @ref="ref1" Appearance="Appearance.Filled">A button</FluentButton>
<FluentButton @ref="ref2" Appearance="Appearance.Filled">Another button</FluentButton>
<FluentButton @ref="ref3" Appearance="Appearance.Filled">And one more</FluentButton>
<FluentButton @ref="ref4" Appearance="Appearance.Filled" @onclick=OnClick>Last button</FluentButton>

```
You can use Design Tokens to manipulate the styles from C# code as follows:

```csharp
[Inject]
private BaseLayerLuminance BaseLayerLuminance { get; set; } = default!;

[Inject]
private AccentBaseColor AccentBaseColor { get; set; } = default!;

[Inject]
private BodyFont BodyFont { get; set; } = default!;

[Inject]
private StrokeWidth StrokeWidth { get; set; } = default!;

[Inject]
private ControlCornerRadius ControlCornerRadius { get; set; } = default!;

private FluentButton? ref1;
private FluentButton? ref2;
private FluentButton? ref3;
private FluentButton? ref4;

protected override async Task OnAfterRenderAsync(bool firstRender)
{
	if (firstRender)
	{
		//Set to dark mode
		await BaseLayerLuminance.SetValueFor(ref1!.Element, (float)0.15);

		//Set to Excel color
		await AccentBaseColor.SetValueFor(ref2!.Element, "#185ABD".ToSwatch());

		//Set the font
		await BodyFont.SetValueFor(ref3!.Element, "Comic Sans MS");

		//Set 'border' width for ref4
		await StrokeWidth.SetValueFor(ref4!.Element, 7);
		//And change conrner radius as well
		await ControlCornerRadius.SetValueFor(ref4!.Element, 15);

		StateHasChanged();
	}
}

public async Task OnClick()
{
	//Remove the wide border
	await StrokeWidth.DeleteValueFor(ref4!.Element);
}
```
As can be seen in the code above (with the `ref4.Element`), it is possible to apply multiple tokens to the same component. 

For Design Tokens that work with a color value, you must call the `ToSwatch()` extension method on a string value or use one of the `Swatch` constructors. This makes sure the color is using a format that Design Tokens can handle. A `Swatch` has a lot of commonality with the `System.Drawing.Color` struct. Instead of the values of the components being between 0 and 255, in a `Swatch` they are expressed as a value between 0 and 1.

> :notebook: **Note**
> 
> The Design Tokens are manipulated through JavaScript interop working with an `ElementReference`. There is no JavaScript element until after the component is rendered. This means you can only work with the Design Tokens from code after the component has been rendered in `OnAfterRenderAsync` and not in any earlier lifecycle methods. 

#### Option 2: Using Design Tokens as components
The Design Tokens can also be used as components in a `.razor` page directely. It looks like this:

```html
<BaseLayerLuminance Value="(float?)0.15">
	<FluentCard BackReference="@context">
		<div class="contents">
			Dark
			<FluentButton Appearance="Appearance.Accent">Accent</FluentButton>
			<FluentButton Appearance="Appearance.Stealth">Stealth</FluentButton>
			<FluentButton Appearance="Appearance.Outline">Outline</FluentButton>
			<FluentButton Appearance="Appearance.Lightweight">Lightweight</FluentButton>
		</div>
	</FluentCard>
</BaseLayerLuminance>
```

To make this work, a link needs to be created between the Design Token component and its child components. This is done with the `BackReference="@context"` construct. 

> :notebook: **Note**
> 
> Only one Design Token component at a time can be used this way. If you need to set more tokens, use the code approach as described in Option 1 above.


#### Option 3: Using the `<FluentDesignSystemProvider>`
The third way to customize the design in Blazor is to wrap the entire block you want to manipulate in a `<FluentDesignSystemProvider>`. This special element has a number of properties you can set to configure a subset of the tokens. **Not all tokens are available/supported** and we recommend this to only be used as a fall-back mechanism. The preferred method of working with the design tokens is to manipulate them from code as described in option 1. 

Here's an example of changing the "accent base color" and switching the system into dark mode (in the file `app.razor`):

```html
<FluentDesignSystemProvider AccentBaseColor="#464EB8" BaseLayerLuminance="0">
	<Router AppAssembly="@typeof(App).Assembly">
		<Found Context="routeData">
			<RouteView RouteData="@routeData" DefaultLayout="@typeof(MainLayout)" />
		</Found>
		<NotFound>
			<PageTitle>Not found</PageTitle>
			<LayoutView Layout="@typeof(MainLayout)">
				<p role="alert">Sorry, there's nothing at this address.</p>
			</LayoutView>
		</NotFound>
	</Router>
</FluentDesignSystemProvider>
```

> :notebook: **Note**
> 
> FluentDesignSystemProvider token attributes can be changed on-the-fly like any other Blazor component attribute.

#### Colors for integration with specific Microsoft products
If you are configuring the components for integration into a specific Microsoft product, the following table provides `AccentBaseColor` values you can use. 
*The library offers an `OfficeColor` enumeration which contains the specific accent colors for 17 different Office applications.*

Product | AccentBaseColor
------- | ---------------
| Office | #D83B01 |
| Word | #185ABD |
| Excel | #107C41 |
| PowerPoint | #C43E1C |
| Teams | #6264A7 |
| OneNote | #7719AA |
| SharePoint | #03787C |
| Stream | #BC1948 |


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
	private readonly CacheStorageAccessor _cacheStorageAccessor;

	public FileBasedStaticAssetService(CacheStorageAccessor cacheStorageAccessor)
	{
		_cacheStorageAccessor = cacheStorageAccessor;
	}

	public async Task<string> GetAsync(string assetUrl, bool useCache = false)
	{
		string result = null;

		HttpRequestMessage message = CreateMessage(assetUrl);


		if (useCache)
		{
			// Get the result from the cache
			result = await _cacheStorageAccessor.GetAsync(message);
		}

		if (string.IsNullOrEmpty(result))
		{
			//It not in the cache (or cache not used), read the asset from disk
			result = await ReadData(assetUrl);

			if (!string.IsNullOrEmpty(result))
			{
				if (useCache)
				{
					// If successful, create the response and store in the cache (when used)
					HttpResponseMessage response = new()
					{
						StatusCode = HttpStatusCode.OK,
						Content = new StringContent(result)
					};

					await _cacheStorageAccessor.PutAsync(message, response);
				}
			}
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

## Use the DataGrid component with EF Core
If you want to use the `FluentDataGrid` with data provided through EF Core, you need to install 
an additional package so the grid knows how to resolve queries asynchronously for efficiency.  .

### Installation
Install the package by running the command:
```
dotnet add package Microsoft.Fast.Components.FluentUI.DataGrid.EntityFrameworkAdapter
```

### Usage
In your Program.cs file you need to add the following after the `builder.Services.AddFluentUIComponents();` line:
```csharp
builder.Services.AddDataGridEntityFrameworkAdapter();
```


## Joining the Community

Looking to get answers to questions or engage with us in real-time? Our community is most active [on Discord](https://discord.gg/FcSNfg4). Submit requests and issues on [GitHub](https://github.com/dotnet/blazor-fluentui/issues/new/choose), or join us by contributing on [some good first issues via GitHub](https://github.com/dotnet/blazor-fluentui/labels/community:good-first-issue).

If you don't find a component you're looking for, it's best to create the issue in our FAST repo [here](https://github.com/microsoft/fast) and limit issues on this repo to bugs in the Blazor component wrappers or Blazor-specific features.

We look forward to building an amazing open source community with you!

## Contact

* Join the community and chat with us in real-time on [Discord](https://discord.gg/FcSNfg4).
* Submit requests and issues on [GitHub](https://github.com/microsoft/fluentui-blazor/issues/new/choose).
* Contribute by helping out on some of our recommended first issues on [GitHub](https://github.com/microsoft/fluentui-blazor/labels/community:good-first-issue).

## Additional resources
* The Microsoft [Fluent UI Blazor components demo](https://aka.ms/fluentui-blazor) site
* The Microsoft [Fluent UI Blazor components documentation](https://learn.microsoft.com/en-us/fluent-ui/web-components/)
* The [Fluent UI Web components demo](https://fluent-components.azurewebsites.net/?path=/docs/getting-started-overview--page) site
* The [FAST](https://www.fast.design/) site
