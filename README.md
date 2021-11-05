# Microsoft.Fast

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET C#](https://img.shields.io/badge/.NET-C%23-blue)](https://img.shields.io/badge/.NET-C%23-blue)
[![NuGet version](https://badge.fury.io/nu/Microsoft.Fast.Components.FluentUI.svg)](https://badge.fury.io/nu/Microsoft.Fast.Components.FluentUI)

[![Discord](https://img.shields.io/badge/chat%20on-discord-7289da.svg)](https://discord.gg/FcSNfg4)
[![Twitter](https://img.shields.io/twitter/follow/fast_ui.svg?style=social&label=Follow)](https://twitter.com/intent/follow?screen_name=fast_ui)

:star: We appreciate your star, it helps!

## Introduction

The `Microsoft.Fast.Components.FluentUI` package provides a lightweight set of [Blazor](https://blazor.net) component wrappers around Microsoft's official FluentUI Web Components. The FluentUI Web Components are built on [FAST](https://www.fast.design/) and work in every major browser. To get up and running with `Microsoft.Fast.Components.FluentUI` see the Getting Started section below.

The source for `@fluentui/web-components` is hosted in [the Fluent UI monorepo](https://github.com/microsoft/fluentui/tree/master/packages/web-components). An implementation of all the current Fluent Web Components can be found at https://aka.ms/fluentwebcomponents.

## Getting Started

To get started using `Microsoft.Fast.Components.FluentUI`, you will need both the Nuget package and the accompanying Web Component implementations. First, install [the Nuget package](https://www.nuget.org/packages/Microsoft.Fast.Components.FluentUI/). If using the .NET CLI, you can run the following command to accomplish that.

```shell
dotnet add package Microsoft.Fast.Components.FluentUI
```

Next, add a script tag to your project.

```html
<script type="module" src="https://unpkg.com/@fluentui/web-components"></script>
```

Depending on what type of Blazor project you are building, it will go into one of two places:

* For a Blazor Server project, add the script tag to your `_Host.cshtml` file.
* For a Blazor WASM project, add the script tag to your index or main layout.

> **Note:** If the script reference is added to a `.razor` or `.cshtml` file, you will need to escape the `@` with a second `@` like so `https://unpkg.com/@@fluentui/web-components`.

Once these steps are completed, you can then begin using the components throughout your Blazor application by adding the following `using` statement to your views:

```cs
@using Microsoft.Fast.Components.FluentUI
```

Take a look in the `examples` folder of this repository to see how to use the various components.

## Joining the Community

Looking to get answers to questions or engage with us in realtime? Our community is most active [on Discord](https://discord.gg/FcSNfg4). Submit requests and issues on [GitHub](https://github.com/dotnet/blazor-fluentui/issues/new/choose), or join us by contributing on [some good first issues via GitHub](https://github.com/dotnet/blazor-fluentui/labels/community:good-first-issue).

We look forward to building an amazing open source community with you!

## Contact

* Join the community and chat with us in real-time on [Discord](https://discord.gg/FcSNfg4).
* Submit requests and issues on [GitHub](https://github.com/dotnet/blazor-fluentui/issues/new/choose).
* Contribute by helping out on some of our recommended first issues on [GitHub](https://github.com/dotnet/blazor-fluentui/labels/community:good-first-issue).

Test