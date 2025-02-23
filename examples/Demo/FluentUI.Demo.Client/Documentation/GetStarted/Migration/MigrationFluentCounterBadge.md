### General
A CounterBadge is now just a badge. If child content is provided, the counter badge
will be displayed on top of that content. By default, the counter badge will be displayed
at the above-end corner of the content. Use the Positioning parameter to specify a different placement.

### New parameters
  `Size`, `Shape`, `OverflowCount`, `IconStart` and `IconEnd` are new parameters.

### Removed parameters💥
  The `ChildContent` parameter has been removed. Only the `Count` can be displayed.
  The `HorizontalPosition` and `VerticalPosition` parameters have been removed.
  The `ShowOverflow` parameter has been removed. 
  The `Max` parameter has been removed. Use `OverflowCount` instead. 

### Appearance 💥
  The `Appearance` parameter has been updated to use the `BadgeAppearance` enum
    instead of `Appearance` enum.

    `BadgeAppearance` enum has the following values :
    - `Filled`
    - `Ghost`
    - `Outline`*
    - `Tint`*

    * Not supported in CounterBadge

### Color 💥
    The `Color` parameter has been updated to use the `BadgeColor` enum
      instead of being of type `String?`.

    `BadgeColor` enum has the following values:
    - `Brand`
    - `Danger`
    - `Important`
    - `Informative`
    - `Severe`
    - `Subtle`
    - `Success`
    - `Warning`
    
### Migrating to v5

You can use the `ToBadgeAppearance()` method to convert the `Appearance` parameter to the `ButtonAppearance` enum.
```csharp	
@using Microsoft.FluentUI.AspNetCore.Components.Migration

<FluentButton Appearance="Appearance.Accent.ToBadgeAppearance()">Click</FluentButton>
//                                          ^^^^^^^^^^^^^^^^^^^^
```


|v3 & v4|v5|
|-----|-----|
|`Appearance.Neutral`    |`BadgeAppearance.Filled`|
|`Appearance.Accent`     |`BadgeAppearance.Filled`|
|`Appearance.Lightweight`|`BadgeAppearance.Ghost`|

The other values of the `Appearance` enum were not supported in earlier versions
and will be converted to `BadgeAppearance.Filled`.
