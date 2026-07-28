---
title: Overflow
route: /Overflow
icon: StackAdd
---

# Overflow

The `FluentOverflow` component is used to manage and display a collection of items that may exceed the available space. It automatically handles the overflow by providing a way to access hidden items.

## Simple Usage

{{ OverflowDefault }}

## Overflow not visible on load

With below example the `VisibleOnLoad` parameter is set to false.Make sure the screen dimension is small enough to show an overflow badge with count.
Then refresh the page to see the difference between this example and the one above

{{ OverflowVisibleOnLoad }}

## Custom templates and dynamic items

This example shows a fully customized More button and tooltip content, and includes add/remove actions to demonstrate dynamic overflow recalculation.
{{ OverflowCustomExample }}

## Selector-based overflow

This example uses `Selector=".overflow-item"` so only matching children are overflow-managed; the non-matching `behavior="fixed"` badges stay visible.
{{ OverflowSelectorExample }}

## MaxRenderedItems payload cap

This example sets `MaxRenderedItems="2"` and shows the difference between `OverflowCount` (total) and `ItemsOverflow` (rendered subset).
{{ OverflowMaxRenderedItemsExample }}

## Item overflow behavior modes

The `behavior` attribute on direct children supports these modes:

| Value | Behavior |
| --- | --- |
| `behavior="fixed"` | Item always remains visible at full size and does not move to overflow. |
| `behavior="ellipsis"` | Item always remains visible but can shrink with text ellipsis when space is limited. |

Notes:

- `behavior` is an HTML attribute on child elements (not a `FluentOverflow` parameter).
- Use `Selector` to control which children are overflow-managed; non-selected children can still be marked with `behavior`.

## Multiple items with ellipsis behavior

This example demonstrates multiple items with ellipsis behavior (`behavior="ellipsis"`) combined with normal overflowed items.
{{ OverflowMultipleFixedItemsExample }}

## API FluentOverflow

{{ API Type=FluentOverflow }}

## Migrating to v5

{{ INCLUDE File=MigrationFluentOverflow }}
