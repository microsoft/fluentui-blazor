---
title: Migration FluentMenu
route: /Migration/Menu
hidden: true
---

### General
We do not (currently) use the Menu service and Menu provider anymore. These are not needed as the underlying web component uses a different way of
displaying the menu (using `popover` when available).



### Removed parameters💥
  - `UseMenuService`
  - `Anchor`
  - `Open` and `OpenChanged`
  - `Trigger` as type `MouseButton`
  - `HorizontalPosition` and `VerticalPosition`
  - `HorizontalInset` and `VerticalInset`
  - `HorizontalThreshold` and `VerticalThreshold`
  - `Anchored` 
  - `Width`
  - `HorizontalViewportLock`
  - `HorizontalScaling`


    
### Migrating to v5
- You need to change the type of the `Trigger` parameter from `MouseButton` to `string`. The value should be the `id` of the element the menu is attached to. If you want to attach opening the menu to the context menu action, you can use the `OpenOnContext` parameter. Opening the menu omn oter mouse button actions is no longer supported.
- You need to change the type argument of the `OnCheckedChanged` event from `FluentMenuItem` to `MenuItemEventArgs`. the item concerned is available in the `Item` property of the event args.
- You need to check the `MenuItemRole` values usage when migrating to v5. See below for the mapping of the values. The changed values have been mapped to the corresponding new values but they are marked obsolete and will be removed in a future release. 

|v3 & v4|v5|
|-----|-----|
|`MenuItemRole.MenuItem`    |`MenuItemRole.MenuItem`|
|`MenuItemRole.MenuItemCheckbox`     |`MenuItemRole.Checkbox`|
|`MenuItemRole.MenuItemRadio` |`MenuItemRole.Radio`|

### FluentMenu new properties
- `OpenOnHover` (`bool?`) — opens the menu on hover.
- `OpenOnContext` (`bool?`) — opens the menu on right-click.
- `CloseOnScroll` (`bool?`) — closes the menu when the page is scrolled.
- `PersistOnItemClick` (`bool?`) — keeps the menu open after an item is clicked.
- `Height` (`string?`)
- `OnClick` (`EventCallback<MenuItemEventArgs>`)
- `Tooltip` (`string?`)

### FluentMenuItem changes

#### Removed properties 💥
- `Expanded` (`bool`)
- `KeepOpen` (`bool`)

#### Changed properties
| V4 Property | V5 Property | Change |
|-------------|-------------|--------|
| `Disabled` (`bool`) | `Disabled` (`bool?`) | Now nullable |
| `Checked` (`bool`) | `Checked` (`bool?`) | Now nullable |
| `OnClick` (`EventCallback<MouseEventArgs>`) | `OnClick` (`EventCallback<MenuItemEventArgs>`) | Event args changed |
| `CheckedChanged` (`EventCallback<bool>`) | `CheckedChanged` (`EventCallback<bool?>`) | Now nullable |

#### New properties
- `IconStart` (`Icon?`)
- `IconEnd` (`Icon?`)
- `IconSubmenu` (`Icon?`)
- `IconIndicator` (`Icon?`)

