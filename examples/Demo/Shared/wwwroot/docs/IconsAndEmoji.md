Starting with v3, the assets for the icons and emoji are removed from the library package and are provided through additional (separate) packages for 
both the icon and emoji resources. The components, and icons that are used by the library itself, are still part of the package. 
Adding the [Microsoft.Fast.Components.FluentUI.Icons package](https://www.nuget.org/packages/Microsoft.Fast.Components.FluentUI.Icons) and/or [Microsoft.Fast.Components.FluentUI.Emojis package](https://www.nuget.org/packages/Microsoft.Fast.Components.FluentUI.Emojis) 
is enough to make the resources available to your code.
 
We use the [.NET trimming capabilities](https://learn.microsoft.com/aspnet/core/blazor/host-and-deploy/configure-trimmer) to publish only those assests that are actually being used in your program. Usually this results in some very small DLL's that only contain the resources that are actually being used in your application.
 
We still have support for both the **complete** [Fluent UI System Icons](https://github.com/microsoft/fluentui-system-icons) and the [Fluent Emoji](https://github.com/microsoft/fluentui-emoji) libraries.
 
## Getting Started
 
To use the **Fluent UI System Icons** in your application, you will need to install the [Microsoft.Fast.Components.FluentUI.Icons](https://www.nuget.org/packages/Microsoft.Fast.Components.FluentUI.Icons/) NuGet package in the project are using the main library. 


To use the **Fluent UI Emoji** in your application, you need to install the [Microsoft.Fast.Components.FluentUI.Emojis](https://www.nuget.org/packages/Microsoft.Fast.Components.FluentUI.Emojis/) NuGet package in the project are using the main library.
 
#### `FluentIcon` component
 
To use the icons, you add a `FluentIcon` component in your code like this:

```razor
<FluentIcon Icon="Icons.Regular.Size24.Save" />
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
<FluentIcon Icon="MyIcons.SettingsEmail" />
```
 
#### `FluentEmoji` component
 
To use the emoji, you add a `FleuntEmoji` component in your code like this:

```razor
<FluentSystemEmoji Emoji="Emojis.PeopleBody.Color.Default.Artist" />
```

> **Note:** Names are structured like this: `Emojis.[EmojiGroup].[EmojiStyle].[EmojiSkintone].[EmojiName]`


 