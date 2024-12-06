# Microsoft Fluent UI Icons for Blazor

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET C#](https://img.shields.io/badge/.NET-C%23-blue)](https://docs.microsoft.com/en-us/dotnet/csharp/)

:star:  We appreciate your star, it helps!

## Introduction

The [Microsoft.FluentUI.AspNetCore](https://github.com/microsoft/fluentui-blazor) family of packages provides
a set of [Blazor](https://blazor.net) components and utilites which you can use to build applications
that have the look and feel or modern Microsoft applications.

This package contains libraries of icons wrapping Microsoft’s official [Fluent UI System Icons library](https://github.com/microsoft/fluentui-system-icons).

## Getting Started

To get started using the Fluent UI Icons in your Blazor applications, you will first need 
to install the official [Nuget package](https://www.nuget.org/packages/Microsoft.FluentUI.AspNetCore.Components.Icons/)
in the project you would like to use the library and components. You can use the following command:

```shell
dotnet add package Microsoft.FluentUI.AspNetCore.Components.Icons
```

## Usage

To use the icons, you will need to add the following using statement to your `_Imports.razor` file:

```razor
@using Icons = Microsoft.FluentUI.AspNetCore.Components.Icons
```

Then you can use the icons in your Blazor components like this:

```razor
<FluentIcon Icon="@(Icons.Regular.Size24.Save)" />
```

> **Note:** Names are structured as follows: `Icons.[IconVariant].[IconSize].[IconName]`.

You can use your custom images by setting the `Value` property calling the `Icon.FromImageUrl` method:

```razor
<FluentIcon Value="@(Icon.FromImageUrl("/Blazor.png"))" Width="32px" />
```

## List of icons and additional resources

Use the Microsoft Fluent UI Blazor components [documentation and demo site](https://www.fluentui-blazor.net) to search through the complete set of icons and see how they look.


## Joining the Community

Looking to get answers to questions or engage with us in real-time? Our community is  active on [Discord](https://discord.gg/FcSNfg4). Submit requests 
and issues on [GitHub](https://github.com/microsoft/blazor-fluentui/issues/new/choose), or join us by contributing on [some good first issues via GitHub](https://github.com/microsoft/fluentui-blazor/labels/community:good-first-issue).

We look forward to building an amazing open source community with you!

### Contact

* Join the community and chat with us in real-time on [Discord](https://discord.gg/FcSNfg4).
* Submit requests and issues on [GitHub](https://github.com/microsoft/fluentui-blazor/issues/new/choose).
* Contribute by helping out on some of our recommended first issues on [GitHub](https://github.com/microsoft/fluentui-blazor/labels/community:good-first-issue).
