---
title: Migration FluentCheckbox
route: /Migration/Checkbox
hidden: true
---

- ### FluentField wrapping 💥

  In V5, `FluentCheckbox` is now wrapped inside a `FluentField` component in its template.
  The label, validation message, and required indicator are handled by the `FluentField` wrapper
  instead of being rendered inline in the checkbox template.

- ### Removed properties 💥

  - `ChildContent` — label content can no longer be passed as child content.
    Use the `Label` property or `LabelTemplate` from the `IFluentField` interface instead.

    ```xml
    <!-- V4 -->
    <FluentCheckbox @bind-Value="isChecked">Accept terms</FluentCheckbox>

    <!-- V5 -->
    <FluentCheckbox @bind-Value="isChecked" Label="Accept terms" />
    ```

- ### Other changes

  - Event handling uses `@onchange` instead of `@oncheckedchange` — update any custom event bindings.
