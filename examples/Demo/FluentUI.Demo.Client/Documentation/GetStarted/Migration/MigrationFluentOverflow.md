---
title: Migration FluentOverflow
route: /Migration/Overflow
hidden: true
---

## Web component migration

`FluentOverflow` now wraps a `<fluent-overflow>` web component. Overflow calculation
and resize are handled there, with Blazor receiving overflow state updates.

## Content model changes

- The old dedicated overflow item component is removed.
- Overflow now works with direct children of `FluentOverflow`.
- Per-item behavior is expressed with HTML attributes on the child element (for example `behavior="fixed"` or `behavior="ellipsis"`).
- Hidden items use the standard `hidden` attribute instead of `overflow`.
- The More trigger uses the web component's `trigger` slot. Existing `MoreTemplate`, `OverflowTemplate`, and tooltip service customization remain available.

## New/updated parameters

- `Selector` — CSS selector for direct children to include in overflow handling.
- `MaxOverflowItems` (`int`, default `0`) replaces `MaxRenderedItems` and limits the number of overflow items returned in the payload (`<= 0` means unlimited). Set it to `25` to preserve the previous default limit.
- `VisibleOnLoad="false"` hides the component until its first layout completes without changing the parameter value.

## Removed APIs

- `FluentOverflowItem` component — removed; use direct child content instead.
- `OverflowItemFixed` enum — removed; use the child `behavior` attribute values instead.
- `StoreOverflowInMemory` and `store-overflow-in-memory` are removed.
- `GetOverflowState` and the internal `OverflowState` snapshot are removed. `RefreshAsync()` only requests recalculation; changed state arrives through events.
- The plural `selectors` alias is removed; use `Selector` in Blazor or `selector` in HTML.

## Event payload changes

Every item in `ItemsOverflow` and `OnOverflowRaised` now represents a hidden item and contains only `Id`, `Text`, and `Index`. The `Overflow` and `Behavior` fields are removed from `OverflowItem` and `OverflowChangedItem`. The `OverflowChangedEventArgs` event retains `Id`, `Items`, and `OverflowCount`; `FirstOverflowIndex` and `OrderedItemIds` are removed.

`OverflowCount` is the total hidden count, even when `MaxOverflowItems` limits the number of entries. An empty event clears the previous state. `OverflowRaisedAsync` accepts only the hidden items, not a mixed list of visible and hidden items.

## Migration example

```xml
<!-- V4 -->
<FluentOverflow>
    <FluentOverflowItem Text="Pinned" Fixed="OverflowItemFixed.Fixed" />
    <FluentOverflowItem Text="Blazor" />
    <FluentOverflowItem Text="Microsoft" />
</FluentOverflow>

<!-- V5 -->
<FluentOverflow>
    <div behavior="fixed">Pinned</div>
    <div>Blazor</div>
    <div>Microsoft</div>
</FluentOverflow>
```
