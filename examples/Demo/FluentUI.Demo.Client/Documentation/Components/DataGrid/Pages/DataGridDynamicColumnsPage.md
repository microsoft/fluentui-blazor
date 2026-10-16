---
title: Dynamic columns  
route: /DataGrid/DynamicColumns
---

# Dynamic columns

You can make columns appear conditionally using normal Razor logic such as `@if`. Example:

Also, in this example the column's Width parameter is being set instead of specifying all widths for all columns in the `GridTemplateColumn` parameter.

{{ DataGridDynamicColumns }}

## Hide columns

A column left out with `@if` is removed from the grid, and comes back as a new column: its header no longer shows the
sort the grid was using, and it is placed after all the other columns.

To let users choose which columns they see, set the column's `Visible` parameter instead. A hidden column stays part of
the grid: the data stays sorted by it, and it comes back at the place it had in the column order (`ColumnOrder`,
`GetColumnOrder()`), even when the other columns have been moved in the meantime.

In this example, sort the grid by a column, or move one with its header menu, then hide it and show it again.

{{ DataGridColumnVisibility }}
