---
title: Templates
order: 0015
category: 10|Get Started
route: /templates
icon: Classification
---

To make it easier to start a project that uses the Fluent UI Web Components for Blazor out of the box, we have created the [Microsoft.FluentUI.AspNetCore.Templates](https://www.nuget.org/packages/Microsoft.FluentUI.AspNetCore.Templates/) template package. The package contains 4 Blazor templates for creating the following type of applications:

- Fluent Blazor Web App
- Fluent Blazor WebAssembly Standalone App
- .NET MAUI Fluent Blazor Hybrid and Web App
- Aspire Starter app (ASP.NET Core/Blazor/Fluent UI)

All of these templates mimic their standard Blazor template counterpart but have the Fluent UI Blazor library already fully set up. If you choose to add sample pages when creating a project, all components have been replaced with Fluent UI components (and a few extra have been added). All Bootstrap styling is removed of course as well.

If you want to use the Icon component with applications based on the templates, we have already included and set up the Icon package for you. The full collection of approximately 12 thousand icons in different variants and sizes can be browsed and searched from the [Icon](Icon) page.

If you want to use the Emoji component with applications based on the templates, you still need to make some changes to the project. See the [Emoji](/Emoji) page for more information.

The pages created from these templates will appear like the following based on the type of project and options selected during creation.

![Home page for site created Fluent UI templates](/images/template-home.jpeg)
![Counter page for site created Fluent UI templates](/images/template-counter.jpeg)
![Fetch data page for site created Fluent UI templates](/images/template-weather.jpeg)

> **IMPORTANT!!**
> Just as with the standard Blazor Web App template, Blazor will use SSR by default. If you want to have interactive components, make sure you add a rendermode to the app, page or component!

## Installation

You can install the templates by running the following command:

```cshtml
dotnet new install Microsoft.FluentUI.AspNetCore.Templates@5.0.0-rc.5.26219.1 (*)
```

The current version can be found on the [NuGet site](https://www.nuget.org/packages/Microsoft.FLuentUI.AspNetCore.Templates/).

> **IMPORTANT!!**
> (*) As long as v5 is not the main version, you'll need to add the exact version number to your install command

## Usage

After installing the templates, you can create a new project from either the CLI or by using the 'Creating a new project'-dialog in Visual Studio 2026.

For creating a new Fluent Blazor Web App project from the CLI:

```cshtml
dotnet new fluentblazor -o {your project name}
```

For creating a Fluent Blazor WebAssembly Standalone App project from the CLI:

```cshtml
dotnet new fluentblazorwasm -o {your project name}
```

In Visual Studio you can create a new project by selecting on of the templates in the 'File-&gt;New-&gt;Project'-dialog. It looks like this (when you select 'Fluent'
in the 'All project types' drop down:

![new project dialog](/images/template-new-project.png)

### Blazor Web App details

The rendermode and interactivity choices made when creating an application with the template determine the behavior of the NavMenu and whether we include the web components script in `App.razor`. Starting with v4.2.1 of the templates, the result of that choices is described in the table below:

| Rendermode / Interactivity | NavMenu @rendermode | NavMenu Collapsible |
| --- | --- | --- |
| SSR / not applicable | ❌ | ✅^\*^ |
| Server / Global | ❌ | ✅ |
| Server / Per Page | ✅ | ✅ |
| WebAssembly / Global^\*\*^ | ❌ | ✅ |
| WebAssembly / Per Page^\*\*^ | ✅ | ✅ |
| Auto / Global^\*\*^ | ❌ | ✅ |
| Auto / Per Page^\*\*^ | ✅ | ✅ |

*\* For expanding/collapsing items, this uses a JavaScript file that is only active when running in SSR mode.*
*\*\* The NavMenu component is located in the Client project.*

## Uninstalling the templates

If you want to uninstall the templates, both from the CLI and Visual Studio 2026, run the following command:

```cshtml
dotnet new uninstall Microsoft.FluentUI.AspNetCore.Templates
```
