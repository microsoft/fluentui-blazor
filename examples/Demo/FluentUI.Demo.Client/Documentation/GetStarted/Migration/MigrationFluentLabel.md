---
title: Migration FluentLabel
route: /Migration/Label
hidden: true
---

### Changed properties 
- `Weight`, now used to determine if the label text is shown regular or semibold

### Removed properties

- `Alignment`
- `Color`
- `CustomColor`
- `MarginBlock`
- `Typo`

### New properties
- `Required` (`bool`) — displays a required indicator.
- `Size` (`LabelSize?`) — controls the label size.
- `Tooltip` (`string?`)

### Migrating from v4 to v5
Label is now exclusively being used for labeling input fields.
If you want to use a more v4 compatible component to show text using Fluent's opinions on typography, you can use the new `Text` component instead.
