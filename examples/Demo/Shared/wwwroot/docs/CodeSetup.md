## Configuring the `AddFluentUIComponents()` service collection extension

The following service collection extension needs to be added to the `Program.cs` file:

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
@using Microsoft.Fast.Components.FluentUI
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
You can use this library in **Blazor Hybrid** (MAUI/WPF/Windows Forms) projects. Setup is almost the same as described in the "Getting started" section above, but to get everything to work you'll need to take one extra step (for now) as described below:

### Temporary workaround for MAUI/WPF/Windows Forms issues
Currently when using the WebView to run Blazor (so all Hybrid variants) the web-components script is not imported automatically (see [#404](https://github.com/microsoft/fluentui-blazor/issues/404)). 
There is also an issue with loading the custom event handlers that are being configured by the web-components script. Until these are fixed on the WebView side, there is a workaround available, namely to intercept `'_framework/blazor.modules.json'` and provide proper JS initializers file (created by build). The needed	`initializersLoader.webview.js` has been added to the library and needs to be included with a script tag **before** the `_framework/blazor.webview.js` script tag:

```xml
<script app-name="{NAME OF YOUR APP}" src="./_content/Microsoft.Fast.Components.FluentUI/js/initializersLoader.webview.js"></script>
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
