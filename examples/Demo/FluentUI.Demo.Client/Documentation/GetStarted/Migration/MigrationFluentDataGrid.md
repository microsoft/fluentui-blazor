---
title: Migration FluentDataGrid
route: /Migration/DataGrid
hidden: true
---

### Renamed parameters
- `ColumnOptionsLabels` has been renamed to `ColumnOptionsUISettings`
- `ColumnResizeLabels` has been renamed to `ColumnResizeUISettings`
- `ColumnSortLabels` has been renamed to `ColumnSortUISettings`

These `...UISettings` parameters are now only used to set a custom icon and icon position. All labels that could be set in earlier versions
have now been replaced with our standard Localization capabilities. You can use a custom localizer to set custom labels for these UI settings.
An example of this can be found in the `Server` project of the demo application, where a custom localizer is registered in the `Program.cs` file.

### Removed properties 
- `NoTabbing` (`bool`) — removed.
- `SortByAscending` (`bool?`) - removed. The grid can be sorted by more than one column, so its sort is exposed as
  the `SortColumns` list instead. Read `SortColumns` for every sorted column, or `SortColumns.FirstOrDefault()` for
  what used to be the single sorted column.


### Sorting by more than one column

Setting `SortMode` to `DataGridSortMode.Multiple` lets the grid be sorted by several columns at once, each with its own
direction (see [Multi-column sorting](/DataGrid/MultiSort)). Because the sort is no longer one column and one
direction, the types that reported it now carry a list of `DataGridSortColumn<TGridItem>` in priority order, and the
properties that reported only the first of them are gone.

`GridItemsProviderRequest<TGridItem>`:

- `SortByColumn` (`ColumnBase<TGridItem>?`) - removed.
- `SortByAscending` (`bool`) - removed.

```csharp
// Before
if (request.SortByColumn is not null)
{
    Sort(request.SortByColumn, request.SortByAscending);
}

// After
foreach (var level in request.SortColumns)
{
    Sort(level.Column, level.Ascending);
}

// Or, for a data source that sorts by one column only (null when the grid is not sorted)
var primary = request.SortColumns.FirstOrDefault();
```

`ApplySorting()` and `GetSortByProperties()` keep their signatures and now cover every sort level, so code that used
either of them needs no change. `GetSortByProperties()` returns the property name and direction of each level, in
priority order.

`DataGridSortEventArgs<TGridItem>` (the `OnSortChanged` argument):

- `Column` (`ColumnBase<TGridItem>?`) - removed.
- `SortByAscending` (`bool`) - removed.

```csharp
// Before
void HandleSortChanged(DataGridSortEventArgs<Person> args)
    => Log(args.Column?.Title, args.SortByAscending);

// After
void HandleSortChanged(DataGridSortEventArgs<Person> args)
{
    var primary = args.SortColumns.FirstOrDefault();
    Log(primary?.Column.Title, primary?.Ascending ?? false);
}
```

An empty `SortColumns` now means the grid is not sorted, where `Column` used to be `null`. `FirstOrDefault()` then
returns `null`, so check the entry itself rather than its `Column`, which is never `null`.

`IGridSort<TGridItem>` gained `CanApplyThen` and `ApplyThen`, which append a sort to another column's ordering with
`ThenBy`. Both have default implementations, so existing implementations keep compiling: their columns can be the
first column sorted on, but cannot be added as a further sort level until they implement the two members.
`GridSort<TGridItem>` supports it as it is. `ColumnKeyGridSort<TGridItem>` gained a constructor overload that takes a
`thenSortFunction` next to the `sortFunction`; supply it whenever the column has a `sortFunction` and should be able to
take part in a multi-column sort. The existing `(columnKey, sortFunction)` constructor is unchanged.

### Type changes
- `GenerateHeader`: `GenerateHeaderOption?` → `DataGridGeneratedHeaderType?`
- `ErrorContent`: `RenderFragment<Exception>?` → `RenderFragment<Exception?>?`

### Enum changes
- `Align` has been renamed to `DataGridCellAlignment`
- `GenerateHeaderOption` has been renamed to `DataGridGeneratedHeaderType`
- `SortDirection` has been renamed to `DataGridSortDirection`

### New properties
- `OnExpandAll` (`EventCallback`)
- `OnCollapseAll` (`EventCallback`)
