### General
We do not (currently) use the Menu service and Menu provider anymore. These are not needed as the underlying web component uses a different way of
displaying the menu (using `popover` when available).



### New parameters
- `Trigger` (string?)
- `OpenOnHover` (bool)
- `OpenOnContext` (bool)
- `PersistOnItemClick` (bool)
- `CloseOnScroll` (bool)*
- `Height` (string?)

\* not working yet

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
- You need to change the type of the `Trigger` parameter from `MouseButton` to `string`. The value should be the `id` of the element the menu is attached to.
- You need to change the type argument of the `OnCheckedChanged` event from `FluentMenuItem` to `MenuItemEventArgs`. the item concerned is available in the `Item` property of the event args.
- You need to check the `MenuItemRole` values usage when migrating to v5. See below for the mapping of the values.

|v3 & v4|v5|
|-----|-----|
|`MenuItemRole.MenuItem`    |`MenuItemRole.menuItem`|
|`MenuItemRole.MenuItemCheckbox`     |`MenuItemRole.Checkbox`|
|`MenuItemRole.MenuItemCheckbox`|`MenuItemRole.Radio`|

