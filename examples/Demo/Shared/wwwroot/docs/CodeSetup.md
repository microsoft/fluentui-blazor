
# Code Setup

## Getting Started

### Using our dotnet templates

To get started quickly, you can use our  [FluentUI.Blazor Templates](https://www.nuget.org/packages/Microsoft.FluentUI.AspNetCore.Templates)  they are pre-configured with FluentUI.Blazor. Open a terminal and install them with this command.
```shall
dotnet new install Microsoft.FluentUI.AspNetCore.Templates
```

Navigate to a folder where you want your project and run the following command to create a new project.
```shell
dotnet new fluentblazor --name MyApplication
```

### Manual Install
To get started using the **Fluent UI Blazor components** for Blazor, you will first need to install the official [Nuget package for Fluent UI Blazor](https://www.nuget.org/packages/Microsoft.FluentUI.AspNetCore.Components/) in the project you would like to use the library and components.

You can also add the library of [icons](https://www.nuget.org/packages/Microsoft.FluentUI.AspNetCore.Components.Icons) or [emojis](https://www.nuget.org/packages/Microsoft.FluentUI.AspNetCore.Components.Emoji).

```shell
dotnet add package Microsoft.FluentUI.AspNetCore.Components
dotnet add package Microsoft.FluentUI.AspNetCore.Components.Icons
```

### Script
The heart of this library is formed by the **Fluent UI Web Components** and implemented in a script file. This **file is included in the library** itself and does not have to be downloaded or pulled from a CDN.

> By including the script in the library we can safeguard that you are always using the best matching script version.

When using **SSR (Server Side Rendering)**, you will need to include the web components script in your `App.razor`. As there is no Blazor script being loaded/used, our script will also not get loaded.

```html
<script src="_content/Microsoft.FluentUI.AspNetCore.Components/js/web-components-v2.5.16.min.js" type="module" async></script>
```
If you would later add interactivity, the Blazor script will kick in and try to load the web component script again but JavaScript will handle that gracefully by design.

### Reboot (optional)
**Reboot** is a collection of element-specific CSS changes in a single file to help kick-start building a site with the **Fluent UI Blazor** components for Blazor. It provides an elegant, consistent, and simple baseline to build upon.

If you want to use **Reboot**, you'll need to add to your `app.razor`, `index.html` or `_Layout.cshtml` file a line that includes the stylesheet (`.css` file). This can be done by adding the following line to the `<head>` section:

```html    
<link href="_content/Microsoft.FluentUI.AspNetCore.Components/css/reboot.css" rel="stylesheet" />
```

### Register Services
Add the following in `Program.cs`

```csharp
builder.Services.AddFluentUIComponents();
```

If you're running your application on **Blazor Server**, make sure a default `HttpClient` is registered before the `AddFluentUIComponents` method.

```csharp
builder.Services.AddHttpClient();
```

> **Note**
> An options argument can be added to `AddFluentUIComponents`. The `options.HostingModel` parameter is used to determine the type of project you're building. Choose the appropriate value from the `BlazorHostingModel` enumeration.
> 
> See the section **Blazor Hybrid** below.

### Add Component Providers
Add the following components at the end of your `MainLayout.razor` file.
These providers are used by associated services to display Toasts, Dialog boxes, Tooltips or Message Bars correctly.

```xml
<FluentToastProvider />
<FluentDialogProvider />
<FluentTooltipProvider />
<FluentMessageBarProvider />
```
> **note:** You can remove providers that are not used in your application.

## Working with Icons and Emoji
We have additional packages available that include the complete **Fluent UI System icons** and **Fluent UI Emoji** collections. 
Please refer to the [Icons and Emoji](https://www.fluentui-blazor.net/IconsAndEmoji) page for more information.

## Usage
With the package installed, you can begin using the **Fluent UI Blazor components** in the same way as any other Blazor component. 

### Add Imports

After the package is added, you need to add the following in your  `_Imports.razor`

```razor
@using Microsoft.FluentUI.AspNetCore.Components
```

### Quick Start
This is literally all you need in your views to use Fluent UI Blazor components.

```xml
<FluentCard>
  <h2>Hello World!</h2>
  <FluentButton Appearance="@Appearance.Accent">Click Me</FluentButton>
</FluentCard>
```

## Configuring the Design System
The **Fluent UI Blazor** components are built on FAST's (Adaptive UI) technology, which enables design customization and personalization, while automatically
maintaining accessibility. This is accomplished through setting various "design tokens". The library exposes all design tokens, which you can use both from code as in a declarative way in your `.razor` pages. The different ways of working with design tokens are described in the [design tokens](https://www.fluentui-blazor.net/DesignTokens) page.

## Blazor Hybrid
You can use this library in **Blazor Hybrid** (MAUI/WPF/Windows Forms) projects. Setup is almost the same as described in the "Getting started" section above, but to get everything to work you'll need to take some extra steps (for now):

1. You need to make some changes in your `{Type}Program.cs` file.
Make sure the following is added before the `return builder.Build()` line:  
```csharp
builder.Services.AddFluentUIComponents(options =>
{
		options.HostingModel = BlazorHostingModel.Hybrid;
});
```

### Temporary workaround for MAUI/WPF/Windows Forms issues
Currently when using the WebView to run Blazor (so all Hybrid variants) the web-components script is not imported automatically (see [#404](https://github.com/microsoft/fluentui-blazor/issues/404)). 
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
