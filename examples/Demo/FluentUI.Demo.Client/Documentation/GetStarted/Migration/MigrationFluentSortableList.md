### Removed parameters
The following parameters have been removed from `FluentSortableList`:

- ListItemFilteredColor 
- ListBorderWidth 
- ListBorderColor 
- ListPadding 
- ListItemBackgroundColor 
- ListItemHeight 
- ListItemBorderWidth 
- ListItemBorderColor 
- ListItemDropBorderColor 
- ListItemDropColor 
- ListItemPadding 
- ListItemSpacing 

Changing the styling of the list and list items is now done through CSS variables. See below for the list of CSS variables that have
been added and renamed.See the examples on how to apply these variables.

### Styling
The following CSS variables have changed:

`--fluent-sortable-list-background-color` has been renamed to `--fluent-sortable-list-item-background-color`.
`--fluent-sortable-list-filtered` has been renamed to `--fluent-sortable-list-item-filtered-background-color`.

The following CSS variables have been added:

- `--fluent-sortable-list-item-focused-background-color`
- `--fluent-sortable-list-item-focused-border-color`
- `--fluent-sortable-list-item-grabbed-background-color`
- `--fluent-sortable-list-item-grabbed-border-color`
- `--fluent-sortable-list-item-filtered-color:`
