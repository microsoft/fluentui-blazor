---
title: Migration FluentCheckbox
route: /Migration/Checkbox
hidden: true
---

### ChildContent removed 💥

In V5, `FluentCheckbox` no longer accepts `ChildContent` for the label.
Use the `Label` property or `LabelTemplate` from the `FluentField` wrapper instead.

```xml
<!-- V4 -->
<FluentCheckbox @bind-Value="isChecked">Accept terms</FluentCheckbox>

<!-- V5 -->
<FluentCheckbox @bind-Value="isChecked" Label="Accept terms" />
```

### New parameters

- `Element` (`ElementReference`) — reference to the underlying HTML element.
- `Shape` (`CheckboxShape?`) — controls the checkbox shape (square, circular).
- `Size` (`CheckboxSize?`) — controls the checkbox size.
- `Tooltip` (`string?`) — tooltip text shown on hover.
