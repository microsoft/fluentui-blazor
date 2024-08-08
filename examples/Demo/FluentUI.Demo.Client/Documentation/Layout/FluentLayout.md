---
title: Layout
route: /Layout
---

# Layout

`FluentLayout` is a component that defines a layout for a page, using a grid composed of a **Header**, a **Footer**
and 3 columns: **Menu**, **Content** and **Aside** panes.

{{ LayoutSchema SourceCode=false }}

For mobile devices (< 768px), the layout is a single column with the **Menu**, the **Content** and the **Footer** panes stacked vertically.

The layout adapts automatically if you decide not to display one of the panels.

All panels (except `Content`) can be fixed using the `Sticky` property.
In this case, the panel remains fixed when the page is scrolled.

## Example

{{ LayoutDefault }}

## Fixed Header and Footer

You can set the **Header** and **Footer** using the `Sticky` property,
property, but you can also "move" these elements outside the `FluentLayout` to keep a scrollbar for content only.

{{ LayoutFixed }}

## API FluentLayout

{{ API Type=FluentLayout }}

## API FluentLayoutItem

{{ API Type=FluentLayoutItem }}
