---
title: Migration FluentDivider
route: /Migration/Divider
hidden: true
---

- ### Removed properties 💥

  - `Role` (`DividerRole?`) — the role attribute is no longer configurable.
  - `Orientation` (`Orientation?`) — use `Vertical="true"` instead.

    ```xml
    <!-- V4 -->
    <FluentDivider Role="DividerRole.Separator"
                   Orientation="Orientation.Vertical" />

    <!-- V5 -->
    <FluentDivider Vertical="true" />
    ```
