### General
The main difference is that the component allows more flexibility in the `Orientation` and `Size` properties. The size property accepts any value for the `Width` or `Height` of the spacer, depending on the `Orientation`.
The default behavior is to fill the available space in the flex container.

### Added properties
- `Orientation` is added to set the orientation of the spacer. It can be either `Horizontal` or `Vertical`. The default is `Horizontal`.

### Changed properties
- `Width` is renamed to `Size`, to indicate that it can be used for either the `Width` or `Height` of the spacer, depending on the `Orientation`.

### Keep old behavior
If trying to keep old behavior, simply add the `px` suffix to the previous integer value and change it to a string.
