### New parameters
  `Size`, `Shape`, `IconStart` and `IconEnd` are new parameters.

### Removed parameters💥
  The `Fill` parameter has been removed.
  The `Circular` parameter has been removed. Use `Shape` instead.
  The `Width` and `Height` parameters have been removed. Use `Size` instead.
  The `OnClick` parameter has been removed. This to make clearer a Badge should not be used as a Button.
  The `DismissIcon`, `DismissTitle` and `OnClickDismiss` parameters have been removed. There might be a more general dismissable component in the future.


### Appearance 💥
  The `Appearance` parameter has been updated to use the `BadgeAppearance` enum
    instead of `Appearance` enum.

    `BadgeAppearance` enum has the following values:
    - `Filled`
    - `Ghost`
    - `Outline`
    - `Tint`

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
