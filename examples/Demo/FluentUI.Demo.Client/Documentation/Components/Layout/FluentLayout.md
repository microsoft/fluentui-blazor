---
title: Layout
route: /Layout
---

# Layout

`FluentLayout` is a component that defines a layout for a page, using a grid composed of a **Header**, a **Footer**
and 3 columns: **Menu**, **Content** and **Aside** panes.

{{ LayoutSchema SourceCode=false }}

For mobile devices (< 768px), the layout is a single column with the **Menu**, the **Content** and the **Footer** panes stacked vertically.

The layout adapts automatically if you decide not to use or hide any of the panels.

## Sticky Panels

  All panels (except `Content`) can be fixed using the `Sticky` property.
  In this case, the panel remains fixed when the page is scrolled.

## Hamburger Menu

  **On mobile device only** (<768 px) the **Menu** pane can be collapsed into a hamburger menu.
  The hamburger menu is displayed when the screen width is less than 768px.

  By default, on mobile, the menu is hidden and a hamburger button is displayed to make it appear or disappear.
  or make it disappear. Once displayed, this menu takes up all available screen space (except for the header and footer).

  To use this Hamburger icon, you need to add the `FluentLayoutHamburger` component to the **Header**.
  
  ```razor
  <FluentLayoutItem Area="@LayoutArea.Header" Style="display: flex; ">
    <FluentLayoutHamburger />
    My company
  </FluentLayoutItem>
  ```

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
