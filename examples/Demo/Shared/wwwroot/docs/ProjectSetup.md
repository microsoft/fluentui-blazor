# Setup your project to use Fluent UI Icons and/or Emoji assets

The Fluent UI Icons and Emoji's are available as a separate Nuget package. This is done to keep the size of the main library as small as possible.
If you do not want to use icons and/or emoji in your project, you do not need to install these packages.

>**If you are upgrading from a previous version of the library, please see the [Upgrade Guide](https://www.fluentui-blazor.net/UpgradeGuide) for more information.**

## Getting Started with icons

To start using the **Fluent UI System Icons**, you will need 
to install the official [Nuget package](https://www.nuget.org/packages/Microsoft.Fast.Components.FluentUI.Icons/)
in the project you would like to use the library and components. You can use the following command:

```shell
dotnet add package Microsoft.Fast.Components.FluentUI.Icons
```

### Usage

To use the icons, you will need to add the following using statement to your `_Imports.razor` file:

```razor
@using Microsoft.Fast.Components.FluentUI
```

Then you can use the icons in your Blazor components like this:

> **Note:** Names are structured as follows: `Icons.[IconVariant].[IconSize].[IconName]`.

```razor
<FluentIcon Value="@(new @(Icons.Regular.Size24.Save)())" />
```

You can use your custom images by setting the `Value` property calling the `Icon.FromImageUrl` method:

```razor
<FluentIcon Value="@(Icon.FromImageUrl("/Blazor.png"))" Width="32px" />
```

## Getting Started with emoji

To start using the **Fluent UI Emoji's** for Blazor, you will first need 
to install the official [Nuget package](https://www.nuget.org/packages/Microsoft.Fast.Components.FluentUI.Emojis/)
in the project you would like to use the library and components. You can use the following command:

```shell
dotnet add package Microsoft.Fast.Components.FluentUI.Emojis
```

## Usage

To use the emoji's, you will need to add the following using statement to your `_Imports.razor` file:

```razor
@using Microsoft.Fast.Components.FluentUI
```

Then you can use the emoji's in your Blazor components like this:

> **Note:** Names are structured as follows: `Emojis.[EmojiGroup].[EmojiStyle].[EmojiSkintone].[EmojiName]`.

```razor
<FluentEmoji Value="@(new Emojis.PeopleBody.Color.Default.Artist())" />
```


## Changes needed in code 

Please see the [Code Setup](https://www.fluentui-blazor.net/CodeSetup) page to lean more about the neccesary changes to your `Program.cs` file.

## Can I include the library in a Razor Class Library (RCL) project?
Yes, it should work without any changes to your project.