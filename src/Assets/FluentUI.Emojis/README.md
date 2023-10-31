# Microsoft Fluent UI Emoji's for Blazor

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET C#](https://img.shields.io/badge/.NET-C%23-blue)](https://docs.microsoft.com/en-us/dotnet/csharp/)

:star:  We appreciate your star, it helps!

## Introduction

The [Microsoft.FluentUI.AspNetCore.Components](https://github.com/microsoft/fluentui-blazor) package provides
a set of [Blazor](https://blazor.net) components which you can use to build applications
that have the look and feel or modern Microsoft applications.

This library wraps the official Microsoft [Fluent UI Emoji library](https://github.com/microsoft/fluentui-emoji)
and offers a way to make the emoji usable in projects that use the Microsoft Fluent UI Blazor library.

## Getting Started

To get started using the **Fluent UI Emoji's** for Blazor, you will first need 
to install the official [Nuget package](https://www.nuget.org/packages/Microsoft.FluentUI.AspNetCore.Components.Emoji/)
in the project you would like to use the library and components. You can use the following command:

```shell
dotnet add package Microsoft.FluentUI.AspNetCore.Components.Emoji
```

## Usage

To use the emoji's, you will need to add the following using statement to your `_Imports.razor` file:

```razor
@using Microsoft.FluentUI.AspNetCore.Components.Emoji
```

Then you can use the emoji's in your Blazor components like this:

> **Note:** Names are structured as follows: `Emojis.[EmojiGroup].[EmojiStyle].[EmojiSkintone].[EmojiName]`.

```razor
<FluentSystemEmoji Emoji="@(Emojis.PeopleBody.Color.Default.Artist)" />
```

## List of emoji's and additional resources

The Microsoft Fluent UI Blazor components [documentation and demo site](https://www.fluentui-blazor.net)


### Joining the Community

Looking to get answers to questions or engage with us in real-time? Our community is  active on [Gitter](https://app.gitter.im/#/room/#fluentui-blazor:gitter.im) and [Discord](https://discord.gg/FcSNfg4). Submit requests 
and issues on [GitHub](https://github.com/microsoft/blazor-fluentui/issues/new/choose), or join us by contributing on [some good first issues via GitHub](https://github.com/microsoft/fluentui-blazor/labels/community:good-first-issue).

We look forward to building an amazing open source community with you!

### Contact

* Join the community and chat with us in real-time on [Gitter](https://app.gitter.im/#/room/#fluentui-blazor:gitter.im) or [Discord](https://discord.gg/FcSNfg4).
* Submit requests and issues on [GitHub](https://github.com/microsoft/fluentui-blazor/issues/new/choose).
* Contribute by helping out on some of our recommended first issues on [GitHub](https://github.com/microsoft/fluentui-blazor/labels/community:good-first-issue).
