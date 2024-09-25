---
title: Migration Color
route: /Migration/Color
hidden: true
---

- ### Renamed values 🔃
  `Default` is equivalent of previous `Neutral` and `Primary` is equivalent of previous `Accent` values.

- ### Removed values💥
  `Neutral`, `Accent`, `Fill`, `FillInverse` values have been flagged as `Obsolete` and will be removed in the next version.

  |v3 & v4|v5|
  |---|---|
  |`Color.Neutral`      (--neutral-foreground-rest)     | `Color.Default`       (--colorNeutralForeground1)        |
  |`Color.Accent`       (--accent-fill-rest)            | `Color.Primary`       (--colorBrandForeground1)          |
  |`Color.Fill`         (--neutral-fill-rest)           | `Color.Default`       (--colorNeutralForeground1)        |
  |`Color.FillInverse`  (--neutral-fill-inverse-rest)   | `Color.Lightweight`   (--colorNeutralForegroundInverted) |
