---
title: Icon
route: /Icon
---

# Icon

The [Fluent UI System Icons](https://github.com/microsoft/fluentui-system-icons) are a (still growing) collection of familiar,
friendly and modern icons from Microsoft. At the moment there are more than 2200 distinct icons available in both filled
and outlined versions and in various sizes. In total the collections consists of well over 11k icons in SVG format.

This [FluentUI Icons NuGet package](https://www.nuget.org/packages/Microsoft.FluentUI.AspNetCore.Components.Icons) contains all these icons,
which you can access directly in your projects. To use them, simply reference this package in your project.

During the **DotNet Publication process**, the unused icons are automatically removed from the final library.
You can configure this behavior by setting the `PublishTrimmed` property in your project file.

More details on [this page](https://learn.microsoft.com/aspnet/core/blazor/host-and-deploy/configure-trimmer).

> ⚠️ We recommend always using the `Value` property to specify the icon to be rendered (and not the `Icon` property).
> This ensures that the icon is referenced by your project and will not deleted from the final library.
> 
> `<FluentIcon Value="@(new Icons.Regular.Size24.Bookmark())" />`

## FluentIcon

You can use any of these icons by levaraging the `<FluentIcon>` component. See below for the parameters and examples. 

There is also a search capability available on this page wich allows you to browse to all the different icons. At the moment the icons 
displayed in this explorer below are always using the default blue accent color. When using the icons in your application, they will render 
in the color set with the `Color` parameter.

## Explore Icons

{ IconExplorer SourceCode=false }

## Overview

A lot of the web components have named **slots** that declare locations in which content can be rendered.
`FluentIcon` leverages this capability through its `Slot` parameter. With this you can for
example render the icon **in front** (`Slot="start"`) or **after** (`Slot="end"`) a label in a `FluentButton` component.

{ IconDefault }

## Color options

Icons can be drawn and filled with a color through the `Color` parameter which takes a `Color` enumeration value.

```xml
<FluentIcon Value="@(new Icons.Filled.Size48.Alert())" Color="<···>" />
<FluentIcon Value="@(new Icons.Filled.Size48.Alert().WithColor(<···>)" />
```

With the code above you can use the following options at the `<···>`:

{ IconColorTable SourceCode=false }

With `Color.Custom`, supply your own color value through the `CustomColor` parameter. <br/>
Needs to be formatted as an HTML hex color string (`#rrggbb` or `#rgb`) or a CSS variable (`var(--...)`).

## Customization

You can create your own icons by adding a class like this one:

<pre>
Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello 
</pre>
