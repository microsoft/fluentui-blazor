---
title: Layout
route: /Layout
---

# Layout

`FluentLayout` is a component that defines a layout for a page, using a grid composed of a **Header**, a **Footer**
and 3 columns: **Menu**, **Content** and **Aside** panes.

<table class="layout-schema">
  <tr>
    <td colspan="3">Header</td>
  </tr>
  <tr>
    <td>Menu</td>
    <td style="width: 100%; height: 60px;">Content</td>
    <td>Aside</td>
  <tr>
    <td colspan="3">Footer</td>
  </tr>
</table>

For mobile devices (< 768px), the layout is a single column with the **Menu**, the **Content** and the **Footer** panes stacked vertically.

<table class="layout-schema">
  <tr>
    <td>Header</td>
  </tr>
  <tr>
    <td style="width: 100%; height: 60px;">Content</td>
  <tr>
    <td>Footer</td>
  </tr>
</table>

The layout adapts automatically if you decide not to use or hide any of the panels.

## Sticky Panels

  All panels (except `Content`) can be fixed using the `Sticky` parameter.  
  In this case, the panel remains fixed when the page is scrolled.

## Hamburger Menu

  **On mobile device only** (<768 px) the **Menu** pane can be collapsed into a hamburger menu.
  The hamburger menu is displayed when the screen width is less than 768px.

  By default, on mobile, the menu is hidden and a hamburger button is displayed to make it appear or disappear.
  or make it disappear. Once displayed, this menu takes up all available screen space (except for the header and footer).

  To use this Hamburger icon, you need to add the `FluentLayoutHamburger` component to the **Header**.

  > &#9432; You can set only one `FluentLayoutHamburger` component per `FluentLayout`.

  ```razor
  <FluentLayoutItem Area="@LayoutArea.Header" Style="display: flex; ">
    <FluentLayoutHamburger />
    My company
  </FluentLayoutItem>
  ```

  The menu is displayed inside the **Content** and **Aside** panels of the `FluentLayout` in which it is placed.
  If you've placed the `FluentLayoutHamburger` component in a different location, you need to specify the
  the `FluentLayout` to be used, using the `Layout` parameter.

  ```razor
  <FluentLayoutItem Area="@LayoutArea.Header">
    <FluentLayoutHamburger />
    My company
  </FluentLayoutItem>

  <FluentLayout @ref="@Layout" Style="height: 330px;">
    ...
  </FluentLayout>

  @code
  {
      FluentLayout? Layout;
  }
  ```

## Example

Using the `GlobalScrollbar="true"` parameter, you can set the scrollbar to be global for the entire page.  
Using the `Sticky` paremeter to fix the header and footer.

{{ LayoutDefault }}

You can set the **Header** and **Footer** using the `Sticky` parameter,
but you can also "move" these elements outside the `FluentLayout` to keep a scrollbar for content only.

## API FluentLayout

{{ API Type=FluentLayout }}

## API FluentLayoutItem

{{ API Type=FluentLayoutItem }}

<style>
  .layout-schema {
    margin-left: 50px;
    max-width: 300px;
  }

  .layout-schema td {
    text-align: center;
    vertical-align: middle;
    border: 1px solid var(--colorNeutralStroke1);
    min-width: 65px;
  }
  .layout-schema tr:first-child {
    background-color: var(--colorBrandBackgroundHover);
    color: var(--colorNeutralForegroundOnBrand);
  }

  .layout-schema tr:last-child {
    background-color: var(--colorNeutralBackgroundDisabled);
    color: var(--colorNeutralForeground1);
  }

</style>

## Migrating to v5

{{ INCLUDE File=MigrationFluentLayout }}
