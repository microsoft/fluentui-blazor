## V2.2
For version 2.2 we started working on adding .NET 8 support. One important new feature in Blazor with .NET 8 is the addition of the QuickGrid component. 
QuickGrid is a high performance grid component for displaying data in tabular form. It is built to be a simple and convenient way to display your data, while 
still providing powerful features like sorting, filtering, paging, and virtualization. 

QuickGrid was originally introduced as an experimental package based on .NET 7 and we copied it's code over to the Fluent UI library to re-use it's 
features (and some more) but render it with the Fluent UI Web Components instead of it's orignal rendering based on HTML table, tr and td elements. As part 
of bringing QuickGrid into .NET 8 the ASP.NET Core team made some changes and improvements to the API. We brougth these changes over to the `<FluentDataGrid>` as well. To update an app that uses `<FluentDataGrid>`, 
you may need to make the following adjustments:

**------BREAKING CHANGES------**
- Rename the `Value` attribute on the `Paginator` component to `State`
- Rename the `IsDefaultSort` attribute on columns to `InitialSortDirection` and add `IsDefaultSortColumn=true` to indicate the column should still be sorted by default.

**------BREAKING CHANGES------**

*To use the `<FluentDataGrid>` component, you do not need to add a reference to the `Microsoft.AspNetCore.Components.QuickGrid` package to your project. In fact, if you do so it will lead to compilation errors.*

All the examples in the [demo site](https://aka.ms/fluentui-blazor) have been updated to reflect these changes.

### New features
- Updated Fluent UI System Icons to version 1.1.198

### .NET 8 package

Because of .NET 8 not yet being available on GitHub Actions yet, we cannot supply a NuGet package targetting that version yet. If you want to create your own package, you need to augment the `<TargetFramworks>` property in the solutions `.csproj` files so it reads the following:

```xml
<TargetFrameworks>net6.0;net7.0;net8.0</TargetFrameworks>
```

After that you need to build the solution yourself and run `dotnet pack` on the `Microsoft.Fast.Components.FluentUI` project


## V2.1

A more detailed description of all the changes and everything new can be found in [this blog post](https://baaijte.net/blog/whats-new-in-the-microsoft-fluent-ui-library-for-blazor-version-21/) 

**Important change:**

**If you are currently *not using* icons and are *not planning* on using icons and/or moji in your application moving forward, 
you do *not* have to make any changes to your project. If you *are* currently using icons, please read on.**

With earlier versions of the library, all (then only icon) assets would always get published. Starting with this version, when not specifying settings
in the project file with regards to usage of icons and/or emoji (see below) **NO** assets will be published to the output folder. 
This means that no icons and/or emoji will be available for rendering (with exception of the icons that are used by the library itself). 

For icons and emoji to work properly with 2.1.1 and later, two changes need to be made:
1) Add properties to the `.csproj` file
2) Add/change code in `Program.cs'

### Changes to `.csproj`
The (annotated) `PropertyGroup` below can be used as a starting point in your own project. Copying this as-is will result in all icon and emoji assets being published.
See the blog post for more information.


```xml
<PropertyGroup>
    <!-- 
        The icon component is part of the library. By default, NO icons (static assets) will be included when publishing the project. 
 
        Setting the property 'PublishFluentIconAssets' to false (default), or leaving the property out completely, will disable publishing of the 
        icon static assets (with exception of the icons that are being used by the library itself). 

        Setting the property 'PublishFluentIconAssets' to 'true' will enable publishing of the icon static assets. You can limit what icon assets get 
        published by specifying a set of icon sizes and a set of variants in the '<FluentIconSizes>' and '<FluentIconVariants>' properties respectively.

        To determine what icons will be published, the specified options for sizes and variants are combined. Specifying sizes '10' and '16' and 
        variants 'Filled' and 'Regular' means all '10/Filled', all '10/Regular', all '16/Filled' and all '16/Regular' icons assets will be published. 
        It is not possible to specify multiple individual combinations like '10/Filled' and '16/Regular' in the same set. 

        When no icon size set is specified in the '<FluentIconSizes>' property, ALL sizes will be included*  
        When no icon variant set is specified in the '<FluentIconVariants>' property, ALL variants will be included*  
        * when publishing of icon assets is enabled 
    -->
    <PublishFluentIconAssets>true</PublishFluentIconAssets>

    <!-- 
        Specify (at least) one or more sizes from the following options (separated by ','):
        10,12,16,20,24,28,32,48 
        Leave out the property to have all sizes included.
    -->
    <FluentIconSizes>10,12,16,20,24,28,32,48</FluentIconSizes>

    <!-- 
        Specify (at least) one or more variants from the following options (separated by ','):
        Filled,Regular 
        Leave out the property to have all variants included.
    -->
    <FluentIconVariants>Filled,Regular</FluentIconVariants>

    <!-- 
        The emoji component is part of the library. By default, NO emojis (static assets) will be included when publishing the project. 
 
        Setting the property 'PublishFluentEmoji' to false (default), or leaving the property out completely, will disable publishing of the emoji static assets. 

        Setting the property 'PublishFluentEmojiAssets' to 'true' will enable publishing of the emoji static assets. You can limit what emoji assets get 
        published by specifying a set of emoji groups and a set of emoji styles in the '<FluentEmojiGroups>' and '<FluentEmojiStyles>' properties respectively.

        To determine what emojis will be published, the specified options for sizes and variants are combined. Specifying emoji groups 'Activities' and 'Flags' 
        and emoji styles 'Color' and 'Flat' means all 'Activities/Color', all 'Activities/Flat', all 'Flags/Color' and all 'Flags/Flat' emoji assets will be published.

        It is not possible to specify multiple individual combinations like 'Activities/Color' and 'Flags/Flat' in the same published set

        When no emoji group set is specified in the '<FluentEmojiGroups>' property, ALL groups will be included*  
        When no emoji variant set is specified in the '<FluentEmojiStyles>' property, ALL styles will be included*  
        * when publishing of emoji assets is enabled 
    -->
    <PublishFluentEmojiAssets>true</PublishFluentEmojiAssets>

    <!-- 
        Specify (at least) one or more groups from the following options (separated by ','):
        Activities,Animals_Nature,Flags,Food_Drink,Objects,People_Body,Smileys_Emotion,Symbols,Travel_Places 
        Leave out the property to have all groups included.
    -->
    <FluentEmojiGroups>Activities,Animals_Nature,Flags,Food_Drink,Objects,People_Body,Smileys_Emotion,Symbols,Travel_Places</FluentEmojiGroups>

    <!-- 
        Specify (at least) one or more styles from the following options (separated by ','): 
        Color,Flat,HighContrast
        Leave out the property to have all styles included.
    -->
    <FluentEmojiStyles>Color,Flat,HighContrast</FluentEmojiStyles>
</PropertyGroup>
```
### Changes to `Program.cs`
The AddFluentUIComponents() service collection extension needs to be changed. This enables the system to check if a requested icon or emoji is available
Services and configuration classed have been added to the library for this. You do not need to specify the configuration in code yourself. A source generator has been added that reads the settings from the project file and adds the necessary code at compile time. That way the settings made in the project file and the source code are always kept in sync.

The two lines that need to be added to the Program.cs file are:
```
LibraryConfiguration config = new(ConfigurationGenerator.GetIconConfiguration(), ConfigurationGenerator.GetEmojiConfiguration());
builder.Services.AddFluentUIComponents(config);
```

## Other changes
**New component**: 
- `<FluentEmoji>` 

**Other changes:** 
- All `<FluentInputBase>` derived components now need to use `@bind-Value` or `ValueExpression`. This means an input derived component needs to be bound now.
  This is in-line with how it works with the built-in Blazor `<Input...>` components. All examples in the demo site have been updated to reflect this. The affected components are:
    - `<FluentCheckbox>`
    - `<FluentNumberField>`
    - `<FluentRadioGroup>`
    - `<FluentSearch>`
    - `<FluentSlider>`
    - `<FluentSwitch>`
    - `<FluentTextArea>`
    - `<FluentTextField>`
- Because of the above change, the `<FluentCheckbox>` and `<FluentSwitch>` no longer have the 'Checked' parameter. Initial state can be set by using `@bind-Value`
- `<FluentSwitch>` has two new parameters to get/set the checked and unchecked message text, called `CheckedMessage` and `UncheckedMessage` respectively.
- `<FluentRadioGroup>` component is now generic, so can be bound to other values than just `string`
- Various bug fixes
- Updated Fluent UI System Icons to release 1.1.194

