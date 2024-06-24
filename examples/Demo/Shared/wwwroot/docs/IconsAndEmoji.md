>**If you are upgrading from a previous version of the library and you were alreasy using icons and/or emoji, please see the [Upgrade Guide](https://www.fluentui-blazor.net/UpgradeGuide) for additional information.**

Icons and Emoji are provided through additional packages (except for the icons that are used by the library itself). Adding the 
[Microsoft.FluentUI.AspNetCore.Components.Icons](https://www.nuget.org/packages/Microsoft.FluentUI.AspNetCore.Components.Icons) and/or 
[Microsoft.FluentUI.AspNetCore.Components.Emoji](https://www.nuget.org/packages/Microsoft.FluentUI.AspNetCore.Components.Emoji) packages 
is enough to make the resources available to your code.
 
We use the [.NET trimming capabilities](https://learn.microsoft.com/aspnet/core/blazor/host-and-deploy/configure-trimmer) to publish only those 
assests that are actually being used in your program. Usually this results in some very small DLL's that only contain the resources that are 
actually being used in your application.
 
We support the **complete** [Fluent UI System Icons](https://github.com/microsoft/fluentui-system-icons) and 
[Fluent Emoji](https://github.com/microsoft/fluentui-emoji) collections.

To browse the collections, use the following links:
- [Icon explorer](/Icon#explorer)
- [Emoji explorer](/Emoji#explore-emojis)
 
## Getting Started
 
To use the **Fluent UI System Icons** in your application, you will need to install the [Microsoft.FluentUI.AspNetCore.Components.Icons](https://www.nuget.org/packages/Microsoft.FluentUI.AspNetCore.Components.Icons/) NuGet package in the project which is using the main library. 

```shell
dotnet add package Microsoft.FluentUI.AspNetCore.Components.Icons
```

To use the **Fluent UI Emoji** in your application, you need to install the [Microsoft.FluentUI.AspNetCore.Components.Emojis](https://www.nuget.org/packages/Microsoft.FluentUI.AspNetCore.Components.Emoji/) NuGet package in the project which is using the main library.

```shell
dotnet add package Microsoft.FluentUI.AspNetCore.Components.Emojis
```

 
#### Using the `FluentIcon` component

> **Note:** Use the [Icon explorer](/Icon#explorer) to search through the entire collection of icons. You can easily copy a complete icon component 
instance from the explorer by clicking the clipboard in the results list.
 
To use the icons, you add a `FluentIcon` component in your code like this:

```razor
<FluentIcon Value="@(new Icons.Regular.Size24.Save())" />
```

> **Note:** Icon names are structured like this: `Icons.[IconVariant].[IconSize].[IconName]`


Following this structure in combination with Visual Studio's IntelliSense makes it easier to find the icon you need.
 
There are several other ways of using the `FluentIcon` component with the `Icon` class:
 
- Using an icon instance.

    ```razor
    <FluentIcon Value="@(new Icons.Regular.Size24.Save())" />
    ```
- Using the `Icon.FromImageUrl` method to use your ow images as icons.

    ```razor
    <FluentIcon Value="@(Icon.FromImageUrl("/Blazor.png"))" />
    ```
- Using the `ToMarkup` method. This generates the SVG code without using the `FluentIcon` features.

#### Custom Icons
You can even use your own set of custom SVG files as icons. Start by creating a class with you Icons like this:

```csharp
public static class MyIcons
{
    public class SettingsEmail : Icon { public SettingsEmail() : base("SettingsEmail", IconVariant.Regular, IconSize.Size20, "<svg width=\"20\" height=\"19\" viewBox=\"0 0 20 19\" fill=\"none\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M15.6251 2.5H4.37508L4.2214 2.50428C2.79712 2.58396 1.66675 3.76414 1.66675 5.20833V13.125L1.67103 13.2787C1.75071 14.7029 2.93089 15.8333 4.37508 15.8333H9.76425C9.91725 15.4818 10.1354 15.1606 10.4087 14.8873L10.7126 14.5833H4.37508L4.25547 14.5785C3.50601 14.5177 2.91675 13.8902 2.91675 13.125V6.97833L9.709 10.5531L9.78908 10.5883C9.95267 10.647 10.135 10.6353 10.2912 10.5531L17.0834 6.9775V9.17258C17.5072 9.14483 17.9362 9.21517 18.3334 9.38358V5.20833L18.3292 5.05465C18.2494 3.63038 17.0693 2.5 15.6251 2.5ZM4.37508 3.75H15.6251L15.7447 3.75483C16.4942 3.81568 17.0834 4.44319 17.0834 5.20833V5.565L10.0001 9.29375L2.91675 5.56583V5.20833L2.92158 5.08873C2.98242 4.33926 3.60994 3.75 4.37508 3.75ZM15.9167 10.5579L10.9979 15.4766C10.7112 15.7633 10.5077 16.1227 10.4093 16.5162L10.0279 18.0418C9.86208 18.7052 10.4631 19.3062 11.1265 19.1403L12.6521 18.7588C13.0455 18.6605 13.4048 18.4571 13.6917 18.1703L18.6103 13.2516C19.3542 12.5078 19.3542 11.3018 18.6103 10.5579C17.8665 9.814 16.6605 9.814 15.9167 10.5579Z\" fill=\"#212121\"/></svg>") { } }
}
```

The `Icon` class and enumerations are in the Fluent UI library itself (so, you can be independant of the Fluent UI Icons library)

After adding the class, you can start using this custom icon like a "normal" Fluent UI Icon:

```razor
<FluentIcon Value="@(new MyIcons.SettingsEmail())" />
```
 
#### Using the `FluentEmoji` component
> **Note:** Use the [Emoji explorer](/Emoji#explore-emojis) to search through the entire collection of emoji. You can easily copy a complete emoji component 
instance from the explorer by clicking the clipboard in the results list.
 
To use the emoji, you add a `FleuntEmoji` component in your code like this:

```razor
<FluentEmoji Value="@(new Emojis.PeopleBody.Color.Default.Artist())" />
```

> **Note:** Names are structured like this: `Emojis.[EmojiGroup].[EmojiStyle].[EmojiSkintone].[EmojiName]`


### Recommendation

During the DotNet Publication process, the unused icons are automatically removed from the final library.
You can configure this behavior by setting the [PublishTrimmed](https://learn.microsoft.com/aspnet/core/blazor/host-and-deploy/configure-trimmer) property in your project file.

We recommend always using the **Value** property to specify the Icon/Emoji to be rendered (and not the Icon/Emoji property).
This ensures that the icon is not deleted from the final library.

```csharp
<FluentIcon Value="@(new Icons.Regular.Size24.Bookmark())" />
<FluentEmoji Value="@(new Emojis.PeopleBody.Color.Default.Artist())" />
```

If you have already written your code using the **Icon** property or the **Emoji** property,
you can use the **Visual Studio Find and Replace** functionality, checking the **Use regular expressions** item.

> Before to use Find and Replace in Files, you have to backup your project. The following steps are provided as an example and may not work in your case. You have to adapt them to your project.

For Icons:
- To search: `<FluentIcon Icon="(?<name>[^"]+)"?`
- To replace by: `<FluentIcon Value="@(new ${name}())"`

For Emojis:
- To search: `<FluentEmoji Emoji="(?<name>[^"]+)"?`
- To replace by: `<FluentEmoji Value="@(new ${name}())"`
