> **IMPORTANT!!**
> Just as with the standard Blazor Web App template, Blazor will use SSR by default. If you want to have interactive components, make sure you add a rendermode to the app, page or component!

## Installation

Install the templates by running the command:

```cmd
dotnet new install Microsoft.FluentUI.AspNetCore.Templates
```

## Usage

After installing the templates you can create new a project from either the CLI or by creating a new project in Visual studio 2022.

For creating a Fluent UI Blazor Web App project from the CLI

```cmd
dotnet new fluentuiblazor -o {your project name}
``` 

For creating a Fluent UI Blazor WebAssembly Standalone App project from the CLI:

```cmd
dotnet new fluentuiblazorwasm -o {your project name}
``` 

In Visual Studio you can create a new project by selecting either the FluentUI Blazor Server App template or the FluentUI Blazor WebAssembly template in the 'File->New->Project'-dialog.

## Uninstalling the templates

If you want to uninstall the templates, both from the CLI and Visual Studio 2022,  run the following command:

```cmd
dotnet new uninstall Microsoft.FluentUI.AspNetCore.Templates
```

## Support

The Microsoft Fluent UI Blazor library is an open source project and is **not** an official part of ASP.NET Core, which means it’s **not** officially
supported and isn’t committed to ship updates as part of any official .NET updates. It is built and maintained by Microsoft employees (**and** other contributors)
and offers support, like most other open source projects, on a best effort base through the GitHub repository **only**.
