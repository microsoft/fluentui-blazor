---
title: Emoji
route: /Emoji
icon: EmojiSmileSlight
---

# Emoji

## Overview

The [Fluent Emoji](https://github.com/microsoft/fluentui-emoji) are a (still growing) collection of familiar,
friendly, and modern emoji from Microsoft. At the moment there are over 1500 distinct emoji
available in color, flat and high contrast styles and 6 different skintones (where applicable)
divided in 9 groups. In total the
collections consists of well over 13k emoji in SVG format.

This [FluentUI Emoji NuGet package](https://www.nuget.org/packages/Microsoft.FluentUI.AspNetCore.Components.Emoji)
contains all these emoji, which you can access directly in your projects.
To use them, simply reference this package in your project.

During the **DotNet Publication process**, the unused emojis are automatically removed from the final library.
You can configure this behavior by setting the `PublishTrimmed` property in your project file.

More details on [this page](https://learn.microsoft.com/aspnet/core/blazor/host-and-deploy/configure-trimmer).

> [!WARNING] We recommend always using the `Value` property to specify the emoji to be rendered (and not the `Emoji` property).
> This ensures that the emoji is referenced by your project and will not deleted from the final library.
> 
> `<FluentEmoji Value="@(new Emojis.PeopleBody.Color.Default.Artist())" />`

To simplify your code, in your `_Imports.razor` file, include this line. 

```razor
@using Emojis = Microsoft.FluentUI.AspNetCore.Components.Emojis
```

## FluentEmoji

You can use any of these emojis by levaraging the `<FluentEmoji` component. See below for the parameters and examples. 

There is also a search capability available on this page wich allows you to browse to all the different emojis.

{{ EmojiDefault }}

## Explore Emojis

`{ EmojiExplorer SourceCode=false }  ⚠️ TO ACTIVATE LATER`
