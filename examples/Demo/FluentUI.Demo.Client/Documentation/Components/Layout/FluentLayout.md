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

The mobile breakpoint is defined in the `FluentLayout` component using the `MobileBreakdownWidth` parameter.
By default, it is set to `768px`.

This breakpoint is based on the FluentLayout component width (not the screen width)
using the [@container](https://developer.mozilla.org/en-US/docs/Web/CSS/@container) CSS rule.

Each time the breakpoint is reached, the layout will be updated to reflect the new layout, and the event `OnBreakpointEnter` will be triggered.

**💡 Note**: The `FluentLayout` component can be used with a Blazor static web app or a Blazor interactive app.
The hamburger menu is available in all modes, but the event `OnBreakpointEnter` and the `MenuDeferredLoading` parameter are only available in "interactive mode".

## Sticky Panels

  All panels (except `Content`) can be fixed using the `Sticky` parameter.  
  In this case, the panel remains fixed when the page is scrolled.

## Hamburger Menu

  **On mobile device only** (<768 px) the **Menu** pane will be collapsed into a hamburger menu.
  The hamburger button is displayed when the screen width is less than 768px.

  💡 You can "force" the visibility of the hamburger button using
  the `FluentLayoutHamburger.Visible="true"` parameter.

  By default, on mobile, the menu is hidden and a hamburger button is displayed to make it appear or disappear.
  or make it disappear. Once displayed, this menu takes a large part of the screen width.
  This is configurable using the `FluentLayoutHamburger.PanelSize` parameter.

  To use this Hamburger icon, you need to add the `FluentLayoutHamburger` component to the **Header**.

  ```razor
  <FluentLayoutItem Area="@LayoutArea.Header">
    <FluentLayoutHamburger />
    My company
  </FluentLayoutItem>
  ```

## Customized Hamburger Menu

By default, the hamburger menu contains the **Menu** FluentLayoutItem.
This hamburger menu can be customized using some parameters like the `ChildContent` for the panel content,
the `PanelHeader` for the header/title content, the `PanelSize` for the panel width and the `PanelPosition` for the panel position (left or right).

If `ChildContent` is not defined, the menu content will be used.
It is then generated **twice** in the HTML code, once for the menu and once for the hamburger panel.
If your menu is very large, it is best to set the `FluentLayout.MenuDeferredLoading` parameter to `true`.
In this case, Blazor will generate the content in the menu area in Desktop mode and then remove it from the DOM to place it in the hamburger panel in mobile mode.
This parameter `MenuDeferredLoading` is only available in Blazor "interactive mode".

## Example

Using the `GlobalScrollbar="true"` parameter, you can set the scrollbar to be global for the entire page.  
Using the `Sticky` paremeter to fix the header and footer.

> TODO: When `GlobalScrollbar=“true”`, a problem persists with the fixed footer.

{{ LayoutDefault }}

You can set the **Header** and **Footer** using the `Sticky` parameter,

## CSS Variables

To make it easier to customize your sub-components, you can use these CSS variables,
whose default values are as follows.

You can adapt them using the `Height` parameter of the `FluentLayout`,
`FluentLayoutItem` with `Area="LayoutArea.Header"`, `Area="LayoutArea.Content"` and `Area="LayoutArea.Footer"`.

```css
--layout-height: 100dvh;
--layout-header-height: 44px;
--layout-footer-height: 36px;
--layout-body-height: calc(var(--layout-height) - var(--layout-header-height) - var(--layout-footer-height));
```

## API FluentLayout

{{ API Type=FluentLayout }}

## API FluentLayoutItem

{{ API Type=FluentLayoutItem }}

## API FluentLayoutHamburger

{{ API Type=FluentLayoutHamburger }}

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
