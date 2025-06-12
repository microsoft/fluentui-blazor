// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.DataGrid.Infrastructure;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

[CascadingTypeParameter(nameof(TGridItem))]
public partial class FluentDataGridRow<TGridItem> : FluentComponentBase, IHandleEvent, IDisposable
{
    internal string RowId { get; set; } = string.Empty;
    private readonly Dictionary<string, FluentDataGridCell<TGridItem>> cells = [];

    /// <summary>
    /// Gets or sets the reference to the item that holds this row's values.
    /// </summary>
    [Parameter]
    public TGridItem? Item { get; set; }

    /// <summary>
    /// Gets or sets the index of this row.
    /// When FluentDataGrid is virtualized, this value is not used.
    /// </summary>
    [Parameter]
    public int? RowIndex { get; set; }

    /// <summary>
    /// Gets or sets the string that gets applied to the css gridTemplateColumns attribute for the row.
    /// </summary>
    [Parameter]
    public string? GridTemplateColumns { get; set; } = null;

    /// <summary>
    /// Gets or sets the type of row. See <see cref="DataGridRowType"/>.
    /// </summary>
    [Parameter]
    public DataGridRowType? RowType { get; set; } = DataGridRowType.Default;

    [Parameter]
    public VerticalAlignment VerticalAlignment { get; set; } = VerticalAlignment.Center;

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public EventCallback<FluentDataGridCell<TGridItem>> OnCellFocus { get; set; }

    /// <summary>
    /// Gets or sets the owning <see cref="FluentDataGrid{TItem}"/> component
    /// </summary>
    [CascadingParameter]
    internal InternalGridContext<TGridItem> InternalGridContext { get; set; } = default!;

    /// <summary>
    /// Gets a reference to the enclosing <see cref="FluentDataGrid{TGridItem}" />.
    /// </summary>
    protected FluentDataGrid<TGridItem> Grid => InternalGridContext.Grid;

    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("fluent-data-grid-row")
        .AddClass("hover", when: Grid.ShowHover)
        .Build();

    protected string? StyleValue => new StyleBuilder(Style)
       //.AddStyle("height", $"{Grid.ItemSize:0}px", () => Grid.Virtualize && RowType == DataGridRowType.Default)
       //.AddStyle("height", "100%", () => (!Grid.Virtualize || InternalGridContext.Rows.Count == 0) && Grid.Loading && RowType == DataGridRowType.Default)
       .Build();

    protected override void OnInitialized()
    {
        RowId = $"r{InternalGridContext.GetNextRowId()}";
        InternalGridContext.Register(this);
    }

    public void Dispose() => InternalGridContext.Unregister(this);

    internal void Register(FluentDataGridCell<TGridItem> cell)
    {

        cell.CellId = $"c{InternalGridContext.GetNextCellId()}";
        cells.Add(cell.CellId, cell);
    }

    internal void Unregister(FluentDataGridCell<TGridItem> cell)
    {
        cells.Remove(cell.CellId!);
    }

    internal async Task HandleOnRowFocusAsync()
    {
        if (Grid.OnRowFocus.HasDelegate)
        {
            await Grid.OnRowFocus.InvokeAsync(this);
        }
    }

    /// <summary />
    internal async Task HandleOnRowClickAsync(string rowId)
    {
        var row = GetRow(rowId);

        if (row is not null && !string.IsNullOrWhiteSpace(row.Class) && (row.Class.Contains(FluentDataGrid<TGridItem>.EMPTY_CONTENT_ROW_CLASS) || row.Class.Contains(FluentDataGrid<TGridItem>.LOADING_CONTENT_ROW_CLASS)))
        {
            return;
        }

        if (row != null && Grid.OnRowClick.HasDelegate)
        {
            await Grid.OnRowClick.InvokeAsync(row);
        }

        if (row != null && row.RowType == DataGridRowType.Default)
        {
            foreach (var column in Grid._columns)
            {
                await column.OnRowClickAsync(row);
            }
        }
    }

    /// <summary />
    internal async Task HandleOnRowDoubleClickAsync(string rowId)
    {
        var row = GetRow(rowId);
        if (row != null && Grid.OnRowDoubleClick.HasDelegate)
        {
            await Grid.OnRowDoubleClick.InvokeAsync(row);
        }
    }

    /// <summary />
    internal async Task HandleOnRowKeyDownAsync(string rowId, KeyboardEventArgs e)
    {
        if (!SelectColumn<TGridItem>.KEYBOARD_SELECT_KEYS.Contains(e.Code))
        {
            return;
        }

        var row = GetRow(rowId);

        if (row != null && Grid.OnRowClick.HasDelegate)
        {
            await Grid.OnRowClick.InvokeAsync(row);
        }

        if (row != null && row.RowType == DataGridRowType.Default)
        {
            foreach (var column in Grid._columns)
            {
                await column.OnRowKeyDownAsync(row, e);
            }
        }
    }

    private FluentDataGridRow<TGridItem>? GetRow(string rowId, Func<FluentDataGridRow<TGridItem>, bool>? where = null)
    {
        if (!string.IsNullOrEmpty(rowId) && InternalGridContext.Rows.TryGetValue(rowId, out var row))
        {
            return where == null
                 ? row
                 : row is not null && where(row) ? row : null;
        }

        return null;
    }

    /// <summary />
    Task IHandleEvent.HandleEventAsync(EventCallbackWorkItem callback, object? arg)
        => callback.InvokeAsync(arg);
}
