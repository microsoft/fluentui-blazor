---
title: Migration FluentMessageBar
route: /Migration/MessageBar
hidden: true
---

### New properties
  `Size`,  `Shape`, `AriaLive` are new properties.

### Renamed properties 🔃
  The `FadeIn` property has been renamed to `Animation`.
  
### Removed properties💥
  The `IconColor` property has been removed. Use `Icon.WithColor()` method instead.

  The `Intent.Custom` property has been removed. Don't use the `Intent` property and set `Icon` and `ChildContent` instead to customize the message bar.
