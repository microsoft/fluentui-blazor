
# Code Setup

## Getting Started

### Using our dotnet templates

To easiest way to get started is by using our [Templates](https://www.nuget.org/packages/Microsoft.FluentUI.AspNetCore.Templates) These mimic the regular Blazor templates and come with the design and components pre-configured. You install them with this command:
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

When using Visual Studio, you can also use the **New Project** dialog to create a new project. The templates will be available under the **Blazor** category.

### Manual Install
To start using the **Fluent UI Blazor components** from scratch, you first need to install the main [Nuget package](https://www.nuget.org/packages/Microsoft.FluentUI.AspNetCore.Components/) in the project you want to use the library and its components.
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
We wrap the **Fluent UI Web Components**, which are implemented in a script file, for quite a few of our components. This **file is included in the library** itself and does not have to be downloaded or pulled from a CDN.

> By including the script in the library we can safeguard that you are always using the best matching script version.

In some cases, like when using .NET 8's new **SSR (Static Server Rendering)** rendermode, it might be necessary to include our library script in your `App.razor` manually. You can do so as follows:

```html
<script src="_content/Microsoft.FluentUI.AspNetCore.Components/Microsoft.FluentUI.AspNetCore.Components.lib.module.js" type="module" async></script>
```
If you would later add interactivity, the Blazor script will kick in and try to load the web component script again but JavaScript will handle that gracefully by design.

### Styles

The styles used by FluentUI are included in the package. You normally don't need to do anything to include them in your project.

If needed, you can add the following line to the `<head>`> section of your `index.html`, `_Layout.cshtml` or `App.razor` file in the
project you installed the package in:

```html
<link href="{PROJECT_NAME}.styles.css" rel="stylesheet" />
```

It is possible that the line is already there (but commented out).

>**IMPORTANT:**
>When you change the root namespace/assembly name of your project, you need to update the {PROJECT_NAME} in your code accordingly.
          
You can always add your own styles, using the `class` or `style` attribute on the components.
By default, the classes are organised and checked by the component itself (in particular by checking that the class names are valid).
Some frameworks, such as **Tailwind CSS**, add exceptions to class names (e.g. `min-h-[16px]` or `bg-[#ff0000]`).
In this case, you need to disable class name validation by adding this code to your `Program.cs` file:

```csharp
builder.Services.AddFluentUIComponents(options =>
{
    options.ValidateClassNames = false;
});
```

#### Reboot 
**Reboot** is a collection of element-specific CSS changes in a single file to help kick-start building a site with the **Fluent UI Blazor** components for Blazor. It provides an elegant, consistent, and simple baseline to build upon.
The library automatically includes reboot through the 

If you want to use **Reboot**, you'll need to add to your `app.razor`, `index.html` or `_Layout.cshtml` file a line that includes the stylesheet (`.css` file). This can be done by adding the following line to the `<head>` section:

```html    
<link href="_content/Microsoft.FluentUI.AspNetCore.Components/css/reboot.css" rel="stylesheet" />
```

It is entirely possible to build a site without using **Reboot**. If you do not want to use Reboot and you used the templates as a starting point, just remove the following line from the app.css file (it is the first line in the file):

```
@import '/_content/Microsoft.FluentUI.AspNetCore.Components/css/reboot.css';
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
> **note:** If you get scrollbars in your application, move the providers up in the page elements hierarchy, for example into the `FluentBodyContent` component.

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

### For Right-To-Left languages

One of the most common design tokens is the `Direction` design token. It is required to make the application components visually configured for the right-to-left languages. In order to implement such configuration you need to use the `Direction` design token together with the `FluentDesignTheme` component in the main layout of your Right-to-Left pages:

```csharp
@* MainRtlLayout.razor *@

@using Microsoft.FluentUI.AspNetCore.Components.DesignTokens
@inject Direction DirectionDesignToken
@inherits LayoutComponentBase
...
@Body
...
<FluentDesignTheme Direction="@Direction" />
@code {
    LocalizationDirection Direction { get; set; }
    protected override async Task OnAfterRenderAsync(bool f)
    {
        await base.OnAfterRenderAsync(f);
        if(!f)
            return;
        await DirectionDesignToken.WithDefault("rtl");
        Direction = LocalizationDirection.RightToLeft;
        StateHasChanged();
    }
}
```

## Blazor Hybrid
You can use this library in **Blazor Hybrid** (MAUI/WPF/Windows Forms) projects. Setup is almost the same as described in the "Getting started" section above, but to get everything to work you'll need to take one extra step (for now) as described below:

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
