---
title: Migration — Removed Components (Minor)
route: /Migration/RemovedComponents
hidden: true
---

## Overview

  The following V4 components have been **removed** in V5 with no direct replacement.
  This page covers the smaller components that don't warrant their own migration page.

---

## FluentFlipper

  `FluentFlipper` (left/right navigation arrows) has been removed.

  | V4 Parameter | Type |
  |-------------|------|
  | `Direction` | `FlipperDirection?` |
  | `Disabled` | `bool?` |
  | `HiddenScroll` | `bool` |

  **Migration**: Use `FluentButton` with arrow icons:

  ```xml
  <FluentButton OnClick="@Previous" Appearance="ButtonAppearance.Subtle">
      <FluentIcon Value="@(new Icons.Regular.Size20.ChevronLeft())" />
  </FluentButton>
  ```

---

## FluentHorizontalScroll

  `FluentHorizontalScroll` (horizontal scrolling container) has been removed.

  | V4 Parameter | Type | Default |
  |-------------|------|---------|
  | `Speed` | `int` | `600` |
  | `Duration` | `string?` | — |
  | `Easing` | `ScrollEasing?` | — |
  | `FlippersHiddenScroll` | `bool` | — |
  | `View` | `HorizontalScrollView?` | — |

  **Migration**: Use CSS `overflow-x: auto` on a container `<div>`.

---

## FluentCollapsibleRegion

  `FluentCollapsibleRegion` has been removed.

  | V4 Parameter | Type | Default |
  |-------------|------|---------|
  | `Expanded` / `ExpandedChanged` | `bool` | `false` |
  | `MaxHeight` | `string?` | — |

  **Migration**: Use Blazor's conditional rendering:

  ```xml
  @if (expanded)
  {
      <div style="max-height: 300px; overflow: auto;">
          @ChildContent
      </div>
  }
  ```

---

## FluentAccessibility

  `FluentAccessibility` (keyboard shortcut registration with notifications) has been removed.

  **Migration**: Use standard Blazor `@onkeydown` events or JavaScript interop.

---

## FluentEditForm

  `FluentEditForm` (wrapper around Blazor's `EditForm`) has been removed.
  V5 extends standard Blazor `EditForm` directly with Fluent styling.

  **Migration**: Use Blazor's `EditForm` directly. V5's `FluentField` components provide
  validation display.

---

## Components reimplemented in V5

`FluentMenuButton`, `FluentPresenceBadge`, and `FluentProfileMenu` are not
removed. They are available in V5 with significant implementation and API
changes:

- See [FluentMenuButton](/Migration/MenuButton) for its replacement API.
- See [FluentPresenceBadge](/Migration/PresenceBadge) for its replacement API.
- See [FluentProfileMenu](/Migration/ProfileMenu) to migrate the V4 component
  to the V5 `FluentProfileMenu` and `FluentProfilePopover` combination.

In particular, do not replace `FluentProfileMenu` with a generic button or
popover. Keep `FluentProfileMenu` as the avatar trigger and move the V4 header,
profile details, and footer parameters into a nested `FluentProfilePopover`.
