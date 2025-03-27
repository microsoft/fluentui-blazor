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

In the example below, a menu is attached to the button. When a menu item is clicked, a message will be shown in the console.

Another menu is attached to the icon. There is no code attached to the menu items, so nothing will happen when they are clicked.

{{ MenuDefault }}

## Menu open on hover
A menu can be configured to open when the mouse hovers over the trigger element.
{{ MenuOpenOnHover }}

## Menu open on context
A menu can be configured to open when the right mouse button is clicked on the trigger element.
{{ MenuOpenOnContext }}

## Menu persists on item click
A menu can be configured to stay open when an item is clicked.
{{ MenuPersistOnItemClick }}

<!-- This doesn't seem to work yet
## Menu close on scroll
A menu can be configured to close when the window is scrolled.
{{ MenuCloseOnScroll }}
-->

## Menu with max height
{{ MenuMaxHeight }}

## Menu with interactive items
{{ MenuInteractive }}

## Open/close menu programmatically
A menu can be opened and closed programmatically by using the `OpenMenuAsync` and `CloseMenuAsync` methods.

{{ MenuProgrammatic }}


## API FluentMenu

{{ API Type=FluentMenu }}

## API FluentMenuList
{{ API Type=FluentMenuList }}

## API FluentMenuItem

{{ API Type=FluentMenuItem }}Add more API docs

## Migrating from v4

{{ INCLUDE File=MigrationFluentMenu }}
