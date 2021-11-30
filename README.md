# Microsoft.Fast

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET C#](https://img.shields.io/badge/.NET-C%23-blue)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![Nuget](https://img.shields.io/nuget/v/Microsoft.Fast.Components.FluentUI)](https://www.nuget.org/packages/Microsoft.Fast.Components.FluentUI/)

[![Validate PRs](https://github.com/microsoft/fast-blazor/actions/workflows/ci-validate.yml/badge.svg)](https://github.com/microsoft/fast-blazor/actions/workflows/ci-validate.yml)
[![Validate Security](https://github.com/microsoft/fast-blazor/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/microsoft/fast-blazor/actions/workflows/codeql-analysis.yml)

[![Discord](https://img.shields.io/badge/chat%20on-discord-7289da.svg)](https://discord.gg/FcSNfg4)
[![Twitter](https://img.shields.io/twitter/follow/fast_ui.svg?style=social&label=Follow)](https://twitter.com/intent/follow?screen_name=fast_ui)

:star:  We appreciate your star, it helps!

## Introduction

The `Microsoft.Fast.Components.FluentUI` package provides a lightweight set of [Blazor](https://blazor.net) component wrappers around Microsoft's official FluentUI Web Components. The FluentUI Web Components are built on [FAST](https://www.fast.design/) and work in every major browser. To get up and running with `Microsoft.Fast.Components.FluentUI` see the Getting Started section below.

The source for `@fluentui/web-components` is hosted in the [Fluent UI](https://github.com/microsoft/fluentui/tree/master/packages/web-components) mono-repository. Documentation on the components is available on [docs.microsoft.com](https://docs.microsoft.com/en-us/fluent-ui/web-components/).

## Getting Started

To get started using the Fluent UI Web Components for Blazor, you will first need to install [the official Nuget package for Fluent UI](https://www.nuget.org/packages/Microsoft.Fast.Components.FluentUI/). You can use the following command:

```shell
dotnet add package Microsoft.Fast.Components.FluentUI
```

Next, you need to add the web components script. You can either add the script from CDN directly, or you can install it with NPM, whichever you prefer.

To add the script from CDN use the following markup:

```html
<script type="module" src="https://cdn.jsdelivr.net/npm/@fluentui/web-components/dist/web-components.min.js"></script>
```

The markup above always references the latest release of the components. When deploying to production, you will want to ship with a specific version. Here's an example of the markup for that:

```
<script type="module" src="https://cdn.jsdelivr.net/npm/@fluentui/web-components@2.0.2/dist/web-components.min.js"></script>
```

The best place to put the script tag is typically in your `index.html` (`_Layout.cshtml` for blazor server project) file in the script section at the bottom of the `<body>`.

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
> If you are setting up Fluent UI Web Components on a Blazor Server project, you will need to escape the `@` character by repeating it in the source link. For more information check out the [Razor Pages syntax documentation](/aspnet/core/mvc/views/razor).

### Using the FluentUI Web Components

With the package installed and the script configured, you can begin using the Fluent UI Web Components in the same way as any other Blazor component. Just be sure to add the following using statement to your views:

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
> You can add `@using Microsoft.Fast.Components.FluentUI` to namespace collection in `_Imports.razor`, so that you can avoid repeating it in every single razor page.


### Configuring the Design System

The Fluent UI Web Components are built on FAST's Adaptive UI technology, which enables design customization and personalization, while automatically maintaining accessibility. This is accomplished through setting various "design tokens". The easiest way to accomplish this in Blazor is to wrap the entire UI in a `FluentDesignSystemProvider`. This special element has a number of properties you can set to configure the tokens to your desired settings. Here's an example of changing the "accent base color" and switching the system into dark mode:

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
> Provider token attributes can be changed on-th-fly like any other Blazor component attribute.

If you are attempting to configure the components for integration into a specific Microsoft product, the following table provides `AccentBaseColor` values you can use:

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

For a list of all available token attributes, [see here](https://github.com/microsoft/fast-blazor/blob/main/src/Microsoft.Fast.Components.FluentUI/Components/FluentDesignSystemProvider.razor#L69). More examples for other components can be found in the `examples` folder [of this repository](https://github.com/microsoft/fast-blazor).

## Joining the Community

Looking to get answers to questions or engage with us in realtime? Our community is most active [on Discord](https://discord.gg/FcSNfg4). Submit requests and issues on [GitHub](https://github.com/dotnet/blazor-fluentui/issues/new/choose), or join us by contributing on [some good first issues via GitHub](https://github.com/dotnet/blazor-fluentui/labels/community:good-first-issue).

If you don't find a component you're looking for, it's best to create the issue in our FAST repo [here](https://github.com/microsoft/fast) and limit issues on this repo to bugs in the Blazor component wrappers or Blazor-specific features.

We look forward to building an amazing open source community with you!

## Contact

* Join the community and chat with us in real-time on [Discord](https://discord.gg/FcSNfg4).
* Submit requests and issues on [GitHub](https://github.com/microsoft/fast-blazor/issues/new/choose).
* Contribute by helping out on some of our recommended first issues on [GitHub](https://github.com/microsoft/fast-blazor/labels/community:good-first-issue).
