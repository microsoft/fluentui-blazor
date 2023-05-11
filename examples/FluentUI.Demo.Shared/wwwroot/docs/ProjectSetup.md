## Setup your project to use Fluent UI Icons and/or Emoji assets
>**If you are currently *not using* icons and are *not planning* on using icons and/or emoji in your application going forward, 
you do *not* have to make any changes to your project.** 

>**If you want to use icons and/or emoji in your new project or you are upgrading to this version and *are* currently using icons, please read on.**

### How to configure which icons get published
 
The [Fluent UI System Icons collection](https://github.com/microsoft/fluentui-system-icons) is set up in such a way that each icon is made available in a number of different sizes where each size usually comes in 2 variants ('Filled', 'Regular'). The sizes available are '10', '12', '16', '20', '24', '28', '32', and '48'. Not every icon is available in all sizes and/or all variants.
 
You can limit what icon assets get published by specifying a set of icon sizes and a set of icon variants in the project file of the application you are using the library in. To determine what icons are published, these sets for sizes and variants are combined. This means specifying icon sizes '10' and '16' and icon variants 'Filled' and 'Regular', all '10/Filled', all '10/Regular', all '16/Filled' and all '16/Regular' icons assets will be published.
 
It is not possible to specify multiple individual combinations like '10/Filled' and '16/Regular' in the same set.
 
When you have configured to publish icons but you have not specified a set of icon sizes, **ALL** sizes will be included. Equally so, when no set of icon variants is specified, **ALL** variants will be included.
 
### How to configure which emoji get published
 
The [Fluent Emoji collection](https://github.com/microsoft/fluentui-emoji) is divided into the following 9 groups: 'Activities', 'Animals & Nature', 'Flags', "Food & Drink', 'Objects', 'People & Body', 'Smileys & Emotion', 'Symbols' and 'Travel & Places'. Each emoji is made available in 3 styles ('Color', 'Flat', 'High Contrast'). For a lot of emoji depicting a person (mostly in the 'People & Body' category) there are also 6 different skin tone variants available per style. Not every emoji is made available in every style/skintone.
 
You can limit what emoji assets get published by specifying a set of emoji groups and a set of emoji styles in the project file of the application you are using the library in. To determine what emojis will be published, these sets for groups and styles are combined. This means specifying emoji groups 'Activities' and 'Flags' and emoji styles 'Color' and 'Flat' means all 'Activities/Color', all 'Activities/Flat', all 'Flags/Color' and all 'Flags/Flat' emoji assets will be published.
 
It is not possible to filter on specific skin tones. It is also not possible to specify multiple individual combinations like 'Activities/Color' and 'Flags/Flat' in the same published set.
 
When you have configured to publish emoji but you have not specified a set of emoji groups, **ALL** groups will be included. Equally so, When no set of emoji variants is specified, **ALL** styles will be included.
 
### Configuring the project
 
You will need to add settings to the `.csproj` file of the project you are using the library in. The easiest way to do this is by creating a new `<PropertyGroup>` and gather all the relevant and needed properties in there.
 
The first two properties (with a value of 'true' or 'false') that need to be added are:

- `<PublishFluentIconAssets>`
- `<PublishFluentEmojiAssets>`

As you can probably guess from their names, these enable completely turning off or on publishing icons and/or emoji assets.

> **The default value for BOTH properties is `false`. This means that if you do not add the property (with a value of 'true') in your project file,
NO ICON/EMOJI ASSETS WILL GET PUBLISHED** *(with exception of the icons the library is using itself)* **If you are upgrading from an earlier version *AND* 
you are using icons, you'll need to (at least) add the `<PublishFluentIconAssets>` property to your project file and set it to 'true' to get the same 
behavior as before.** 

The properties that you use to specify the sets of icons to be published are:

- `<FluentIconSizes>`
- `<FluentIconVariants>`

For emoji you use the properties:

- `<FluentEmojiGroups>`
- `<FluentEmojiStyles>`

In each property you specify (at least) one or more values separated by commas (','). Any additional leading/trailing spaces will be trimmed when processing the values.
 
The possible values per property are:

| Property | Options |
| --- | --- |
| `<FluentIconSizes>` | 10,12,16,20,24,28,32,48 |
| `<FluentIconVariants>` | Filled, Regular |
| `<FluentEmojiGroups>` | Activities, Animals\_Nature, Flags, Food\_Drink, Objects, People\_Body, Smileys\_Emotion, Symbols, Travel\_Places |
| `<FluentEmojiStyles>` | Color, Flat, HighContrast |

**If you don't specify a property for a set in the project file, by default *ALL* possible options for that set will be included in the publish action**
 


### Changes to `.csproj` file

#### Annotated example
The (annotated) `PropertyGroup` below can be used as a starting point in your own project. Copying this as-is will result in all icon and emoji assets being published.
A `PropertyGroup` without additional comments is supplied below. 

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

`PropertyGroup` without comments

#### `PropertyGroup` without comments
```xml
<PropertyGroup>
    <PublishFluentIconAssets>true</PublishFluentIconAssets>
    <FluentIconSizes>10,12,16,20,24,28,32,48</FluentIconSizes>
    <FluentIconVariants>Filled,Regular</FluentIconVariants>
    <PublishFluentEmojiAssets>true</PublishFluentEmojiAssets>
    <FluentEmojiGroups>Activities,Animals_Nature,Flags,Food_Drink,Objects,People_Body,Smileys_Emotion,Symbols,Travel_Places</FluentEmojiGroups>
    <FluentEmojiStyles>Color,Flat,HighContrast</FluentEmojiStyles>
</PropertyGroup>
```

## Changes needed in code 
Please see the [Code Setup](https://www.fluentui-blazor.net/CodeSetup) page to lean more about the neccesary changes to your `Program.cs` file.