---
title: Migration FluentCard
route: /Migration/Card
hidden: true
---

- ### Web component removed 💥

  The `FluentCard` component no longer uses the `<fluent-card>` web component.
  It now renders a plain `<div>` element with CSS classes. This means custom styling that relied
  on the web component's shadow DOM will need to be updated to standard CSS.

- ### Removed properties 💥

  - `AreaRestricted` — no longer applicable since the web component is removed.
  - `MinimalStyle` — use `Appearance` property instead to control visual style.

- ### Migration example

  ```xml
  <!-- V4 -->
  <FluentCard AreaRestricted="false" MinimalStyle="true">
      Card content
  </FluentCard>

  <!-- V5 -->
  <FluentCard Appearance="CardAppearance.Subtle">
      Card content
  </FluentCard>
  ```
