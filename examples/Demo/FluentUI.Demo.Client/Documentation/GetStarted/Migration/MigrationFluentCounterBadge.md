### General
If `ChildContent` is provided, the counter badge will 'wrap' that content nd be positioned on top.
By default, the counter badge will be displayed at the above-end corner of the content.
Use the `Positioning` parameter to specify a different placement.Use the `OffsetX` and `OffsetY` parameters to specify the offset from the specified position.

### New parameters
- `Size`
- `Shape`
- `OverflowCount`
- `IconStart` 
- `IconEnd` 
- `Positioning`
- `OffsetX`
- `OffsetY`

### Removed parameters💥
  - `HorizontalPosition` and `VerticalPosition`.
  - `ShowOverflow`. 
  - `Max`; Use `OverflowCount` instead. 

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
