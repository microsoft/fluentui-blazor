# V2.1

A more detailed description of all the changes and everything new can be found in [this blog post](https://baaijte.net/blog/whats-new-in-the-microsoft-fluent-ui-library-for-blazor-version-21/) 

**Important change:**

When not specifying any settings in the project file with regards to usage of icons and/or emoji (see below), **NO** assets will be published to the output folder. This means that the icons and/or emoji will not be available. 
This is a change from how it worked in earlier versions where all (icon) assets would always get published. The properties which can be used in the project file are below. You can use this block  as a starting point in 
your own project. Please see the blog post for more information on how to configure these settings.

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
    <FluentIconSizes>10,12,16,20,24,48</FluentIconSizes>

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
    <FluentEmojiGroups>Activities,Animals_Nature,Flags,Food_Drink,Objects,Smileys_Emotion,Symbols,Travel_Places</FluentEmojiGroups>

    <!-- 
        Specify (at least) one or more styles from the following options (separated by ','): 
        Color,Flat,HighContrast
        Leave out the property to have all styles included.
    -->
    <FluentEmojiStyles>Color</FluentEmojiStyles>
</PropertyGroup>
```

**New component**: 
- `<FluentEmoji>` 

**Other changes:** 
- All `<FluentInputBase>` derived components now need to use `@bind-Value` or `ValueExpression`. This means an input derived component needs to be bound now. This follows the way of working that is in place with the built-in Blazor `<Input...>`components). All examples have been changed to reflect this. The affected components are:
    - `<FluentCheckbox>`
    - `<FluentNumberField>`
    - `<FluentRadioGroup>`
    - `<FluentSearch>`
    - `<FluentSlider>`
    - `<FluentSwitch>`
    - `<FluentTextArea>`
    - `<FluentTextField>`
- Because of the above change, the `<FluentCheckbox>` and `<FluentSwitch>` no longer have the 'Checked' parameter. Initial state canbe set with the `@bind-Value` construct
- `<FluentSwitch>` has two new parameters to get/set the checked and unchecked message text, called `CheckedMessage` and `UncheckedMessage` respectively.
- `<FluentRadioGroup>` component is now generic, so can be bound to other values than just `string`
- Various bug fixes
- Updated Fluent UI System Icons to release 1.1.194

