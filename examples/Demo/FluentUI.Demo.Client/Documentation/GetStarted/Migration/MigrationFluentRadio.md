---
title: Migration FluentRadio and FluentRadioGroup
route: /Migration/Radio
hidden: true
---

### FluentRadio specific changes
- Using the `ChildContent` parameter to specify the contents/label of a Radio item is no longer supported. Use the `Label` or `LabelTemplate` parameters instead.
- The `ReadOnly` parameter is not supported. Use the `Disabled` parameter instead.



### Removed properties💥
- `ChildContent`, use `Label` or `LabelTemplate`
