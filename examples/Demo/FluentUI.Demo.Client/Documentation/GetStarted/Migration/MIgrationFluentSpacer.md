### General
The main difference is that the component allows more flexible properties. You can let the spacer grow horizontally and vertically,
including fixed width and heights.

### Added properties
- `Orientation` is added to set the orientation of the spacer. It can be either `Horizontal` or `Vertical`. The default is `Horizontal`.
- `Height` can be used alongside a `Orientation.Vertical` spacer to set the height of the spacer. Otherwise it will default to flex-grow 1.

### Changed properties
- `Width` is now a string and can accept any value, including `px`, `%`, `em`, etc. If no width is set, the spacer behaviour will default to `flex-grow: 1`.


### Keep old behavior
If trying to keep old behavior, simply add the `px` suffix to the previous integer value and change it to a string.
