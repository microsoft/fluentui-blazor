---
title: Menu
route: /Menu
---

# Menu

A menu displays a list of actions and can be attached to a button or any other element. The element the menu is attached to must have an `id` attribute
set. This `id` needs to be passed to the `FluentMenu` component in the `Trigger` parameter. The menu is displayed when the element is clicked.

### Best Practices

#### Don't
- Don't render focusable or clickable items inside menu items.
- Don't use more than 2 levels of nested menus.
- Don't use verbose secondary content for menuitems.

## Default Menu

In the example below, a menu is attached to the button. Another menu is attached to the `Label`. It also shows how a menu can be opened and closed programmatically.
See the Code tab on how to detect a menu item is clicked.

{{ MenuDefault }}

## Menu open on hover
{{ MenuOpenOnHover }}

## Menu open on context
{{ MenuOpenOnContext }}

## Menu with max height
{{ MenuMaxHeight }}

## Menu with interactive items

{{ MenuInteractive }}
