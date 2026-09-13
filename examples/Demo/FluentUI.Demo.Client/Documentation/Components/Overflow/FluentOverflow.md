---
title: Overflow
route: /Overflow
icon: StackAdd
---

# Overflow

The `FluentOverflow<TItem>` component manages items that may exceed the available space. Existing direct `ChildContent` remains supported: specify `TItem="string"` without supplying `Items`. Alternatively, supply a typed `Items` collection and an `ItemTemplate` as shown in the data-bound example below.

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

This example uses `Selector=".overflow-item"` so only matching children are overflow-managed.
{{ OverflowSelectorExample }}

## MaxOverflowItems payload cap

With direct child content, `MaxOverflowItems="2"` limits the number of DOM records returned through `ItemsOverflow` and `OnOverflowRaised`. It does not limit the rendered child content. `OverflowCount` still reports the total measured overflow count. The default is `0` (unlimited); negative values are also unlimited.

{{ OverflowMaxOverflowItemsExample }}

## Data-bound Items

Supply `Items` to render source objects through `ItemTemplate`, whose context is `TItem`. Each invocation must produce exactly one root HTML element so browser measurement indices correspond to source items. When `Items` is supplied, `ChildContent` and `Selector` are not used. An empty collection renders no items; omitting `Items` uses direct child content.

In this mode, `MaxOverflowItems="20"` renders only the first 20 source items for browser measurement. All remaining source items are pre-overflowed, even when those first 20 fit. The limit is a hard rendering ceiling, not a batch size. Values less than or equal to zero render all source items.

`MoreTemplate` and `OverflowTemplate` receive an `OverflowContext<TItem>`. Its `Items` property contains all overflowed source objects in source order, including those never rendered. `OverflowCount` includes these omitted items, and `IdMoreButton` anchors a custom popup. `ItemsOverflow` and `OnOverflowRaised` continue to expose measured DOM records (`Id`, `Text`, `Index`); use `overflow.Items` for the complete typed collection.

Without an `OverflowTemplate`, the default tooltip lists all overflowed items as text. Use `ItemText` to select a display property for complex objects. Use `OnMoreClick` to open a custom popup from the default badge without supplying a `MoreTemplate`. Keep the `FluentPopover` mounted and conditionally render its item content when opened; this lets the popover connect to the DOM before opening while avoiding early creation of its item components. Pagination or `Virtualize` can also be used inside it for large collections.

{{ OverflowItemsExample }}

The component requests all measured indices in data-bound mode. Do not override its `max-overflow-items` or `selector` HTML attributes through additional attributes.

## Item overflow behavior modes

The `behavior` attribute on direct children supports these modes:

| Value | Behavior |
| --- | --- |
| `behavior="fixed"` | Item always remains visible at full size and does not move to overflow. |
| `behavior="ellipsis"` | Item always remains visible but can shrink with text ellipsis when space is limited. |

Notes:

- `behavior` is an HTML attribute on child elements (not a `FluentOverflow` parameter).
- Use `Selector` with direct child content to choose the managed elements. With `Items`, all rendered source items participate in measurement.
- With `Items`, entries beyond `MaxOverflowItems` are always pre-overflowed; their templates and behavior attributes are not evaluated.

## Multiple items with ellipsis behavior

This example demonstrates multiple items with ellipsis behavior (`behavior="ellipsis"`) combined with normal overflowed items.
{{ OverflowMultipleFixedItemsExample }}

## API FluentOverflow

{{ API Type=FluentOverflow }}

## Migrating to v5

{{ INCLUDE File=MigrationFluentOverflow }}
