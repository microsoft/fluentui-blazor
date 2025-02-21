---
title: Migration FluentProgressRing
route: /Migration/ProgressRing
hidden: true
---

The component itself has been renamed from `FluentProgressRing` to `FluentSpinner`
to be coherent with the Web Components and React naming conventions.

> **Note:** The new component `FluentSpinner` cannot be paused. It must always be spinning.
> So, the `Paused`, `Min`, `Max`, `Value` properties has been removed.

### New properties
  `Size`, `AppearanceInverted` are new properties.

### Renamed properties 🔃

  `Stroke` property has been renamed to `Size`.
  The `Stroke` property has been flagged as obsolete and will be removed in the next major version.

### Removed properties💥
  The `ChildContent` property has been removed. Use `FluentField` instead, to display
  a message below the ProgressBar.

  The `Paused` property has been removed. A spinner must alway be spinning.

  The `Color`, `Min`, `Max`, `Value` properties have been removed.

  The `Width` property has been removed. Use `Size` instead.
