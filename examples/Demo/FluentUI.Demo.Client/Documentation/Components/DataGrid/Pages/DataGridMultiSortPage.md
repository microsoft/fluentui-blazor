---
title: Multi-column sorting
route: /DataGrid/MultiSort
---

# Multi-column sorting

By default the DataGrid is sorted by one column at a time: sorting another column replaces the current sort. Setting the
`SortMode` parameter to `DataGridSortMode.Multiple` lets the grid be sorted by several columns at once, each with its own
direction. The first sorted column is the primary sort, the next ones break its ties.

Columns are added to the sort with <kbd>Shift</kbd> + click, or from the column header. The sort priority is shown as a
number next to the sort direction icon.

The column header offers these actions, in the header menu when `HeaderCellAsButtonWithMenu` is set and in the column
header popup otherwise:

| Action | Shown when | Result |
| --- | --- | --- |
| Sort ascending / Sort descending | The column is not already sorted in that direction | A column that is already sorted on changes direction and keeps its place in the sort. Any other column becomes the only column the grid is sorted by. |
| Add to sort | The grid is sorted by another column | Adds the column as the next sort level, ascending. |
| Clear sort | The column is sorted on | Stops sorting by this column, keeping the other levels. |
| Clear all sorts | More than one column is sorted on | Removes every sort level, leaving the grid unsorted. |

Set `ShowMultiSortActions` to false to leave the headers exactly as they are in single sort mode. Sorting by several
columns then only happens through <kbd>Shift</kbd> + click, <kbd>Shift</kbd> + <kbd>Enter</kbd> and the grid's sort
methods — see the note under Accessibility before using it.

Declaring `IsDefaultSortColumn` on more than one column gives the grid a multi-column sort to start with, applied in the
order the columns are declared. `ResetSortAsync`, removing the last sort level and <kbd>Shift</kbd> + <kbd>s</kbd> all
return to it; **Clear all sorts** and `ClearSortAsync` leave the grid unsorted instead.

## Programmatic sorting

| Method | Description |
| --- | --- |
| `SortByColumnAsync` | Sorts by one column, replacing every sort level. |
| `AddSortByColumnAsync` | Adds a column as an extra sort level, or changes the direction of a column that is already sorted on. |
| `RemoveSortByColumnAsync` | Removes one column's sort level, keeping the others. |
| `SetSortAsync` | Replaces the whole sort, for example to restore a sort your application stored. |
| `ClearSortAsync` | Removes every sort level, leaving the grid unsorted. |
| `ResetSortAsync` | Returns to the sort the columns declare through `IsDefaultSortColumn`. |

The columns the grid is sorted by are available from the grid's `SortColumns` property, from `DataGridSortEventArgs`
in the `OnSortChanged` event, and from `GridItemsProviderRequest.SortColumns` when the data is fetched. All three list
every sort level in priority order, so read the first entry where you used to read a single sorted column:

```csharp
var primary = args.SortColumns.FirstOrDefault();
```


For remote data, `GridItemsProviderRequest.GetSortByProperties()` returns the property name and direction of every sort
level, in priority order. A `ColumnKeyGridSort` that sorts the data itself through a sort function also needs a
then-sort function to be used as a secondary sort level, since its `OrderBy` would otherwise replace the levels before
it.

## Accessibility

Sorting by several columns can be done entirely from the keyboard, with a screen reader and on touch:

- <kbd>Shift</kbd> + click, or <kbd>Shift</kbd> + <kbd>Enter</kbd> on a focused column header, adds the column to the
  sort. If the column is already sorted on, its direction is reversed and it keeps its level.
- Every one of those actions is also in the column header, which is what makes them reachable without a physical
  <kbd>Shift</kbd> key: the header menu with `HeaderCellAsButtonWithMenu`, the column header popup without it. Both are
  built from the same menu items, so they offer the same actions in the same order.
- Because a sortable column then has several actions, activating its header opens the menu rather than sorting right
  away.
- Turning `ShowMultiSortActions` off removes that route. There is then no way to build a multi-column sort by touch, or
  with a screen reader that does not pass <kbd>Shift</kbd> + <kbd>Enter</kbd> through, so only do it when your
  application offers its own UI for sorting.
- Right-clicking a header removes that column from the sort, and <kbd>Shift</kbd> + <kbd>s</kbd> returns to the sort
  the columns declare, as it does in single sort mode.

The sort state is exposed as follows:

- Only the primary sort column carries `aria-sort`, because WAI-ARIA states that authors should apply it to one header
  at a time.
- Every sorted column's header describes its own level ("Sorted descending, sort level 2 of 3"), which a screen reader
  reads out after the column name. The number shown next to the icon is hidden from assistive technology so it is not
  announced twice.
- Changing the sort updates neither the focused element nor its name, so the new order is announced through a status
  message ("Sorted by Department ascending, then by Location descending").

## Example

{{ DataGridMultiSort }}
