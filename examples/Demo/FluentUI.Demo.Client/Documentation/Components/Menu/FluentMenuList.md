---
title: MenuList
route: /MenuList
---

# MenuList

{{ MenuListDefault }}

## Item variants
{{ MenuListItemVariants } }}

## Submenu
{{ MenuListSubmenu }}

## Item groups
Items can be placed in groups by using dividers.
{{ MenuListDivider }}

## Custom slots
A menu item can has slots that can be used for specific content. The slots are `indicator`, `start`, `end` and `submenu-glyph`. In the `FluentMenu`
component, we have made parameters to fill these slots with an icon. The available parameters are `IconIndicator`, `IconStart`, `IconEnd` and `IconSubmenu`.
We dthink this covers the vast majority of use cases.

If you need more customization, you can use the slots directly. The example below shows how to do that.
{{ MenuListCustomSlots }}
