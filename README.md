# Microsoft.Fast

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET C#](https://img.shields.io/badge/.NET-C%23-blue)](https://docs.microsoft.com/en-us/dotnet/csharp/)
![Nuget](https://img.shields.io/nuget/v/Microsoft.Fast.Components.FluentUI?label=NuGet%20Component%20Library)
![Nuget](https://img.shields.io/nuget/v/Microsoft.Fast.Templates.FluentUI?label=NuGet%20Templates)

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

```html
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

In your Program.cs file you need to add the following:
```csharp
builder.Services.AddFluentUIComponents();
```

if you are using Blazor Server, you need to make sure the `HttpClient` service is added:

```csharp
builder.Services.AddHttpClient();
```

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

The Fluent UI Web Components are built on FAST's Adaptive UI technology, which enables design customization and personalization, while automatically maintaining accessibility. This is accomplished through setting various "design tokens". The easiest way to accomplish this in Blazor is to wrap the entire UI in a `FluentDesignSystemProvider`. This special element has a number of properties you can set to configure the tokens to your desired settings. Here's an example of changing the "accent base color" and switching the system into dark mode (in the file `app.razor`):

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
> Provider token attributes can be changed on-the-fly like any other Blazor component attribute.

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

As of version 1.4 you can also use all of the (160) individual Design Tokens, both from code as in a declarative way in your `.razor` pages. See https://docs.microsoft.com/en-us/fluent-ui/web-components/design-system/design-tokens for more information on how Design Tokens work

### Working with Design Tokens from code
Given the following `.razor` page:
```html
@page "/"

<PageTitle>Design Token Test Page</PageTitle>

<FluentButton @ref="ref1" Appearance="Appearance.Filled">A button</FluentButton>
<FluentButton @ref="ref2" Appearance="Appearance.Filled">Another button</FluentButton>
<FluentButton @ref="ref3" Appearance="Appearance.Filled">And one more</FluentButton>
<FluentButton @ref="ref4" Appearance="Appearance.Filled" @onclick=OnClick>Last button</FluentButton>
```
You can use Design Tokens to manipulate the styles from code as follows:

```csharp
public partial class Index
{
    [Inject]
    private BaseLayerLuminance BaseLayerLuminance { get; set; } = default!;

    [Inject]
    private BaseHeightMultiplier BaseHeightMultiplier { get; set; } = default!;

    [Inject]
    private ControlCornerRadius ControlCornerRadius { get; set; } = default!;

    private FluentAnchor ref1 = default!;
    private FluentAnchor ref2 = default!;
    private FluentAnchor ref3 = default!;
    private FluentButton ref4 = default!;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await BaseLayerLuminance.SetValueFor(ref1.Element, 0);

            //Enabling this line below will generate an error because no default is set
            //await BaseHeightMultiplier.SetValueFor(ref2.Element);
            
            await BaseHeightMultiplier.WithDefault(25).SetValueFor(ref3.Element);

            await BaseHeightMultiplier.SetValueFor(ref4.Element, 52);
            await ControlCornerRadius.SetValueFor(ref4.Element, 15);

            StateHasChanged();
        }
    }

    public async Task OnClick()
    {
        await BaseHeightMultiplier.DeleteValueFor(ref4.Element);
    }
}
```

### Working with Design Tokens components
All the Design Tokens can also be used as a component in a razor page directely. It looks like this:

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

To make this work, you need to create a link between the Design Token component and its child FluentUI components. You do this with the `BackReference="@context"` construct. The restriction is that you can use only one Design Token this way. If you need to set more tokens, keep usin the `<FluentDesignSystemProvider>`


##  Web components / Blazor components mapping, implementation status and remarks
Web component | Blazor component | Status | Remarks
----------------- | -------------- | ------ | -------
|[`<fluent-accordion>`](https://docs.microsoft.com/en-us/fluent-ui/web-components/components/accordion)|`<FluentAccordion>`|✔️|-|
|`<fluent-accordion-item>`|`<FluentAccordionItem>`|✔️|-|
|[`<fluent-anchor>`](https://docs.microsoft.com/en-us/fluent-ui/web-components/components/anchor)|`<FluentAnchor>`|✔️|-|
|[`<fluent-anchored-region>`](https://docs.microsoft.com/en-us/fluent-ui/web-components/components/anchored-region)|`<FluentAnchoredRegion>`|✔️|-|
|[`<fluent-badge>`](https://docs.microsoft.com/en-us/fluent-ui/web-components/components/badge)|`<FluentBadge>`|✔️|-|
|[`<fluent-breadcrumb>`](https://docs.microsoft.com/en-us/fluent-ui/web-components/components/breadcrumb)|`<FluentBreadcrumb>`|✔️|-|
|`<fluent-breadcrumb-item>`|`<FluentBreadcrumbItem>`|✔️|-|
|[`<fluent-button>`](https://docs.microsoft.com/en-us/fluent-ui/web-components/components/button)|`<FluentButton>`|✔️|-|
|[`<fluent-card>`](https://docs.microsoft.com/en-us/fluent-ui/web-components/components/card)|`<FluentCard>`|✔️|-|
|[`<fluent-checkbox>`](https://docs.microsoft.com/en-us/fluent-ui/web-components/components/checkbox)|`<FluentCheckbox>`|✔️|-|
|[`<fluent-combobox>`](https://docs.microsoft.com/en-us/fluent-ui/web-components/components/combobox)|`<FluentCombobox>`|✔️|-|
|[`<fluent-data-grid>`](https://docs.microsoft.com/en-us/fluent-ui/web-components/components/grid)|`<FluentDataGrid>`|✔️|-|
|`<fluent-data-grid-cell>`|`<FluentDataGridCell>`|✔️|-|
|`<fluent-data-grid-row>`|`<FluentDataGridRow>`|✔️|-|
|[`<fluent-design-system-provider>`](https://docs.microsoft.com/en-us/fluent-ui/web-components/components/provider)|`<FluentDesignSystemProvider>`|✔️|-|
|[`<fluent-dialog>`](https://docs.microsoft.com/en-us/fluent-ui/web-components/components/dialog)|`<FluentDialog>`|✔️|-|
|[`<fluent-divider>`](https://docs.microsoft.com/en-us/fluent-ui/web-components/components/divider)|`<FluentDivider>`|✔️|-|
|[`<fluent-flipper>`](https://docs.microsoft.com/en-us/fluent-ui/web-components/components/flipper)|`<FluentFlipper>`|✔️|-|
|[`<fluent-horizontal-scroll>`](https://docs.microsoft.com/en-us/fluent-ui/web-components/components/horizontal-scroll)|`<FluentHorizontalScroll>`|✔️|-|
|No web component|`<FluentIcon>`|✔️|-|
|[`<fluent-listbox>`](https://docs.microsoft.com/en-us/fluent-ui/web-components/components/listbox)|`<FluentListbox>`|✔️|-|
|[`<fluent-menu>`](https://docs.microsoft.com/en-us/fluent-ui/web-components/components/menu)|`<FluentMenu>`|✔️|-|
|`<fluent-menu-item>`|`<FluentMenuItem>`|✔️|-|
|[`<fluent-number-field>`](https://docs.microsoft.com/en-us/fluent-ui/web-components/components/number-field)|`<FluentNumberField>`|✔️|-|
|`<fluent-option>`|`<FluentOption>`|✔️|-|
|[`<fluent-progress>`](https://docs.microsoft.com/en-us/fluent-ui/web-components/components/progress)|`<FluentProgress>`|✔️|-|
|[`<fluent-progress-ring>`](https://docs.microsoft.com/en-us/fluent-ui/web-components/components/progress-ring)|`<FluentProgressRing>`|✔️|-|
|[`<fluent-radio>`](https://docs.microsoft.com/en-us/fluent-ui/web-components/components/radio-group)|`<FluentRadio>`|✔️|-|
|[`<fluent-radio-group>`](https://docs.microsoft.com/en-us/fluent-ui/web-components/components/radio-group)|`<FluentRadioGroup>`|✔️|-|
|[`<fluent-select>`](https://docs.microsoft.com/en-us/fluent-ui/web-components/components/select)|`<FluentSelect>`|✔️|-|
|[`<fluent-skeleton>`](https://docs.microsoft.com/en-us/fluent-ui/web-components/components/skeleton)|`<FluentSkeleton>`|✔️|-|
|[`<fluent-slider>`](https://docs.microsoft.com/en-us/fluent-ui/web-components/components/slider)|`<FluentSlider>`|✔️|-|
|`<fluent-slider-label>`|`<FluentSliderLabel>`|✔️|-|
|[`<fluent-switch>`](https://docs.microsoft.com/en-us/fluent-ui/web-components/components/switch)|`<FluentSwitch>`|✔️|-|
|[`<fluent-tabs>`](https://docs.microsoft.com/en-us/fluent-ui/web-components/components/tabs)|`<FluentTabs>`|✔️|-|
|`<fluent-tab>`|`<FluentTab>`|✔️|-|
|`<fluent-tab-panel>`|`<FluentTabPanel>`|✔️|-|
|[`<fluent-text-area>`](https://docs.microsoft.com/en-us/fluent-ui/web-components/components/text-area)|`<FluentTextArea>`|✔️|-|
|[`<fluent-text-field>`](https://docs.microsoft.com/en-us/fluent-ui/web-components/components/text-field)|`<FluentTextField>`|✔️|-|
|[`<fluent-toolbar>`](https://docs.microsoft.com/en-us/fluent-ui/web-components/components/toolbar)|`<FluentToolbar>`|✔️|-|
|[`<fluent-tooltip>`](https://docs.microsoft.com/en-us/fluent-ui/web-components/components/tooltip)|`<FluentTooltip>`|✔️|-|
|[`<fluent-tree-view>`](https://docs.microsoft.com/en-us/fluent-ui/web-components/components/tree-view)|`<FluentTreeView>`|✔️|-|
|`<fluent-tree-item>`|`<FluentTreeItem>`|✔️|-|

## Joining the Community

Looking to get answers to questions or engage with us in realtime? Our community is most active [on Discord](https://discord.gg/FcSNfg4). Submit requests and issues on [GitHub](https://github.com/dotnet/blazor-fluentui/issues/new/choose), or join us by contributing on [some good first issues via GitHub](https://github.com/dotnet/blazor-fluentui/labels/community:good-first-issue).

If you don't find a component you're looking for, it's best to create the issue in our FAST repo [here](https://github.com/microsoft/fast) and limit issues on this repo to bugs in the Blazor component wrappers or Blazor-specific features.

We look forward to building an amazing open source community with you!

## Contact

* Join the community and chat with us in real-time on [Discord](https://discord.gg/FcSNfg4).
* Submit requests and issues on [GitHub](https://github.com/microsoft/fast-blazor/issues/new/choose).
* Contribute by helping out on some of our recommended first issues on [GitHub](https://github.com/microsoft/fast-blazor/labels/community:good-first-issue).
