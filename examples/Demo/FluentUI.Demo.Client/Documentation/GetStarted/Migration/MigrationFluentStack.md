---
title: Migration FluentStack
route: /Migration/Stack
hidden: true
---

### Updated properties
  `HorizontalGap` and `VerticalGap` has been update to a `string?` type.
  In v4, the type was `int?` (in pixels).
  This allows the use of other units proposed by the CSS standard.
  If this value is an integer, the unit `px` is automatically added.

  This should not generate any breaking changes.

### New properties
  `Height` is a new property.
