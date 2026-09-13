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
- `FluentOverflow<TItem>` supports the existing direct `ChildContent`; add `TItem="string"` without changing its markup.
- Optionally supply `Items` and an `ItemTemplate` to render typed source objects. Each item template must produce exactly one root HTML element.
- Per-item behavior is expressed with HTML attributes on the child element (for example `behavior="fixed"` or `behavior="ellipsis"`).
- Hidden items use the standard `hidden` attribute instead of `overflow`.
- The More trigger uses the web component's `trigger` slot. Existing `MoreTemplate`, `OverflowTemplate`, and tooltip service customization remain available.

## New/updated parameters

- `TItem` specifies the source item type and can usually be inferred from `Items`. For direct child content, specify it explicitly, for example `TItem="string"`.
- `MaxRenderedItems` (`int`, default `0`) limits the number of source items rendered for measurement when `Items` is supplied. All omitted source items remain available in the typed overflow context. Values less than or equal to zero render all source items. With direct child content, use `Take` or another LINQ operator in `OverflowTemplate` to limit the displayed overflow records.
- `Selector` continues to select managed direct children when `Items` is not supplied.
- `ItemText` selects the text shown in the default tooltip for complex source objects.
- `MoreTemplate` and `OverflowTemplate` receive `OverflowContext<TItem>`, retaining `ItemsOverflow`, `OverflowCount`, and `IdMoreButton` and adding typed `Items`.
- `VisibleOnLoad="false"` hides the component until its first layout completes without changing the parameter value.

## Removed APIs

- `FluentOverflowItem` component — removed; use direct child content instead.
- `OverflowItemFixed` enum — removed; use the child `behavior` attribute values instead.
- `StoreOverflowInMemory` and `store-overflow-in-memory` are removed.
- `GetOverflowState` and the internal `OverflowState` snapshot are removed. `RefreshAsync()` only requests recalculation; changed state arrives through events.
- The plural `selectors` alias is removed; use `Selector` in Blazor or `selector` in HTML.

## Event payload changes

`ItemsOverflow` and `OnOverflowRaised` continue to expose rendered `OverflowItem` records (`Id`, `Text`, `Index`). Existing callbacks and templates can remain unchanged. In data-bound mode, use `context.Items` to access all overflowed source objects, including entries omitted from the DOM, in source order.

With direct content, `OverflowCount` is the total measured overflow count even when the record payload is capped. With `Items`, it includes pre-overflowed items. An empty browser event clears measured overflow; source items excluded by the rendering ceiling remain in overflow.

Custom overflow content is not automatically deferred. Keep `FluentPopover` mounted in `OverflowTemplate` and conditionally render its item content when opened. Use `OnMoreClick` to activate a custom popup from the default badge. The default tooltip displays all overflowed items as text.

## Migration example

```xml
<!-- V4 -->
<FluentOverflow>
    <FluentOverflowItem Text="Pinned" Fixed="OverflowItemFixed.Fixed" />
    <FluentOverflowItem Text="Blazor" />
    <FluentOverflowItem Text="Microsoft" />
</FluentOverflow>

<!-- V5 -->
<FluentOverflow TItem="string">
    <div behavior="fixed">Pinned</div>
    <div>Blazor</div>
    <div>Microsoft</div>
</FluentOverflow>
```

Existing component references also need a type argument: `FluentOverflow<string>?`. See the data-bound `Items` example on the Overflow page to opt into bounded rendering.
