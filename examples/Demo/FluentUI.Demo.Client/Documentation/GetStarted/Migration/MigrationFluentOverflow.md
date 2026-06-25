---
title: Migration FluentOverflow
route: /Migration/Overflow
hidden: true
---

### Web component migration 💥

`FluentOverflow` now wraps a `<fluent-overflow>` web component. Overflow calculation
and resize are handled there, with Blazor receiving overflow state updates.

### Content model changes 💥

- The old dedicated overflow item component is removed.
- Overflow now works with direct children of `FluentOverflow`.
- Per-item behavior is expressed with HTML attributes on the child element (for example `fixed="fixed"` or `fixed="ellipsis"`).

### New/updated parameters

- `Selector` — CSS selector for direct children to include in overflow handling.
- `MaxRenderedItems` (`int`, default `25`) — limits the number of overflow items returned in the payload (`<= 0` means unlimited).
- `StoreOverflowInMemory` (`bool`) — keeps overflow payload items in JavaScript memory.

### Removed APIs 💥

- `FluentOverflowItem` component — removed; use direct child content instead.
- `OverflowItemFixed` enum — removed; use the child `fixed` attribute values instead.

### Migration example

```xml
<!-- V4 -->
<FluentOverflow>
    <FluentOverflowItem Text="Pinned" Fixed="OverflowItemFixed.Fixed" />
    <FluentOverflowItem Text="Blazor" />
    <FluentOverflowItem Text="Microsoft" />
</FluentOverflow>

<!-- V5 -->
<FluentOverflow>
    <div fixed="fixed">Pinned</div>
    <div>Blazor</div>
    <div>Microsoft</div>
</FluentOverflow>
```
