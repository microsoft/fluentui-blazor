---
title: Migration FluentRadio and FluentRadioGroup
route: /Migration/Radio
hidden: true
---

### FluentRadio specific changes
- The `FluentRadio` component now inherits from `FluentInputBase` (instead of `FluentComponentBase` before). This means it supports and uses all parameters from `FluentInputBase`, such as `Disabled`, `Required`, `Value`, `Label`, etc. 
- Using the `ChildContent` parameter to specify the contents/label of a Radio item is no longer supported. Use the `Label` or `LabelTemplate` parameters instead
- The `ReadOnly` parameter is not supported. Use the `Disabled` parameter instead.

### New properties


### Renamed properties 🔃


### Removed properties💥
