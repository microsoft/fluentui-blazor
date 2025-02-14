---
title: Migration FluentProgressBar
route: /Migration/ProgressBar
hidden: true
---

### New properties
  `State`, `Thickness` are new properties.

### Renamed properties 🔃

  `Stroke` property has been renamed to `Thickness`.
  The `Stroke` property has been flagged as obsolete and will be removed in the next major version.

### Removed properties💥
  The `ChildContent` property has been removed. Use `FluentField` instead, to display
  a message below the ProgressBar.

  The `Paused` property has been removed. Set a `Value` to display the
  ProgressBar in "paused" state. Set a `null` value to display the ProgressBar in
  "indeterminate" state.
