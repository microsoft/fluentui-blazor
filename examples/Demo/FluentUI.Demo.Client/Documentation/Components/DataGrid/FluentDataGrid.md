---
title: DataGrid
route: /DataGrid
---

# DataGrid

The DataGrid actually comprises several components and used to display tabular data. The `<FluentDataGrid>` component is the most important
component and is the one normally put onto a page. Internally it uses the`<FluentDataGridRow>` and `<FluentDataGridCell>` components to build up
the grid. Technically it is possible to use these components manually, but that is not the recommended way of working with the DataGrid.

## Rendering
The DataGrid uses standard HTML table elements for rendering the grid. It supports 2 different display modes through the `DisplayMode`
parameter: `DataGridDisplayMode.Grid` (default) and `DataGridDisplayMode.Table`.
- With the `Grid` mode, the `GridTableColumns` parameter can be
used to specify column widths in fractions. It basically provides an HTML table element with a `display: grid;` style.
- With the `Table` mode, it uses standard HTML table elements and rendering. Column widths are best specified through the `Width` parameter on the columns.

> [!NOTE]
Specifically when using `Virtualize`, it is **highly recommended** to use the `Table` display mode as the `Grid` mode can exhibit odd scrolling behavior.


## Accessibility

You can use the <kbd>Arrow</kbd> keys to navigate through a DataGrid. When a header cell is focused and the column is sortable, you can use the
<kbd>Tab</kbd> key to select the sort button. Pressing the <kbd>Enter</kbd> key will toggle the sorting direction. Pressing <kbd>Shift</kbd> + <kbd>s</kbd>
removes the column sorting and restores the default/start situation with regards to sorting. *You cannot remove the default grid sorting with
this key combination*.

When a header cell is focused and the column allows setting options, you can use the <kbd>Tab</kbd> key to select the options button. Pressing
the <kbd>Enter</kbd> key then will toggle the options popover. Pressing <kbd>Esc</kbd> closes the popover .

When a grid allows resizing of the columns, you can use the <kbd>+</kbd> and <kbd>-</kbd> keys to resize the column the focused header belongs
to. Incrementing/decrementing width is done in steps of 10 pixels at a time. You can reset to the original initial column widths by pressing
<kbd>Shift</kbd> + <kbd>r</kbd>.

When a row cell is focused, the grid contains a `SelectColumn` (with `SelectFromEntireRow` parameter set to `true` (default)), you can use the <kbd>Enter</kbd> key to select or unselect the current
row.

## Sorting

The DataGrid supports sorting by clicking on the column headers (when the `HeaderCellAsButtonWithMenu` parameter is `false` (default)). The default sort direction is ascending. Clicking on the same column header
again will toggle the sort direction. When `HeaderCellAsButtonWithMenu` is true, a menu will be used to get trigger the sorting action.

A sort can be removed by right clicking (or by pressing <kbd>Shift</kbd> + <kbd>r</kbd>) on the header column (with exception of
the default sort).

## Row size

The DataGrid offers a `RowSize` parameter which allows you to use different preset row heights. The value uses the `DataGridRowSize`
enumeration for its type. When using `Virtualize`, the `ItemSize` value **must** still be used to indicate the row height.

## Change strings used in the UI

The DataGrid has a number of strings that are used in the UI. These can be changed by leveraging the built-in [localization](/localization) functionality.
The following values can be localized:

- - DataGrid_OptionsMenu
- DataGrid_ResizeDiscrete
- DataGrid_ResizeExact
- DataGrid_ResizeGrow
- DataGrid_ResizeMenu
- DataGrid_ResizeReset
- DataGrid_ResizeShrink
- DataGrid_ResizeSubmit
- DataGrid_SortMenu
- DataGrid_SortMenuAscending
- DataGrid_SortMenuDescending

## Using the DataGrid component with EF Core

If you want to use the `FluentDataGrid` with data provided through EF Core, you need to install an additional package so the grid knows how to resolve queries asynchronously for efficiency.

### Installation
Install the package by running the command:

```cshtml
dotnet add package Microsoft.FluentUI.AspNetCore.Components.DataGrid.EntityFrameworkAdapterCopy
```

### Usage
In your `Program.cs` file you need to add the following after the `builder.Services.AddFluentUIComponents();` line:

```csharp
builder.Services.AddDataGridEntityFrameworkAdapter();Copy
```

## Using the DataGrid component with OData

*Added in 4.11.0*

If you want to use the `FluentDataGrid` with data provided through OData, you need to install an additional package so the grid knows how to resolve queries asynchronously for efficiency.

### Installation
Install the package by running the command:

```cshtml
dotnet add package Microsoft.FluentUI.AspNetCore.Components.DataGrid.ODataAdapterCopy
```

### Usage
In your `Program.cs` file you need to add the following after the `builder.Services.AddFluentUIComponents();` line:

```csharp
builder.Services.AddDataGridODataAdapter();
```


## Typical usage
Here is an example of a data grid that uses in-memory data and enables features including pagination, sorting, filtering, column options, row highlighting and column resizing.

All columns, except 'Bronze', have a `Tooltip` parameter value of `true`.

- When using this for a `TemplateColumn` (like 'Rank' here), you need to also supply a value for the `TooltipText` parameter. **No value given equals no tooltip shown**.
- When using this for a `PropertyColumn`, a value for the `TooltipText` is **not** required. By default, the value given for `Property`
will be re-used for the tooltip. If you do supply a value for `TooltipText` its outcome will be used instead.

`TooltipText` is a lambda function that takes the current item as input and returns the text to show in the tooltip (and `aria-label`).
Look at the Razor tab to see how this is done and how it can be customized.

The Country filter option can be used to quickly filter the list of countries shown. Pressing the ESC key just closes the option popup without changing the filtering currently being used.
Pressing enter finishes the filter action by the current input to filter on and closes the option popup.

The resize options UI is using a customized string for the label ('Width (+/- 10px)' instead of the normal 'Column width'). This is done through
the custom localizer which is registered in the Server project's `Program.cs` file.



{{ DataGridTypical }}

{{ DataGridMultiSelect }}
