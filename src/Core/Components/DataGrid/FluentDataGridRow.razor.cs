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
    internal InternalGridContext<TGridItem> Owner { get; set; } = default!;

    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("hover", when: Owner.Grid.ShowHover)
        .Build();

    protected string? StyleValue => new StyleBuilder(Style)
       .AddStyle("height", $"{Owner.Grid.ItemSize:0}px", () => Owner.Grid.Virtualize && RowType == DataGridRowType.Default)
       .AddStyle("height", "100%", () => (!Owner.Grid.Virtualize || Owner.Rows.Count == 0) && Owner.Grid.Loading && RowType == DataGridRowType.Default)
       .AddStyle("align-items", "center", () => Owner.Grid.Virtualize && RowType == DataGridRowType.Default && string.IsNullOrEmpty(Style))
       .Build();

    protected override void OnInitialized()
    {
        RowId = $"r{Owner.GetNextRowId()}";
        Owner.Register(this);
    }

    public void Dispose() => Owner.Unregister(this);

    internal void Register(FluentDataGridCell<TGridItem> cell)
    {

        cell.CellId = $"c{Owner.GetNextCellId()}";
        cells.Add(cell.CellId, cell);
    }

    internal void Unregister(FluentDataGridCell<TGridItem> cell)
    {
        cells.Remove(cell.CellId!);
    }

    private async Task HandleOnCellFocusAsync(DataGridCellFocusEventArgs args)
    {
        var cellId = args.CellId;
        if (cells.TryGetValue(cellId!, out var cell))
        {
            if (cell != null && cell.CellType == DataGridCellType.Default)
            {
                await Owner.Grid.OnCellFocus.InvokeAsync(cell);
            }
        }
    }

    /// <summary />
    internal async Task HandleOnRowClickAsync(string rowId)
    {
        var row = GetRow(rowId);

        if (row != null && Owner.Grid.OnRowClick.HasDelegate)
        {
            await Owner.Grid.OnRowClick.InvokeAsync(row);
        }

        if (row != null && row.RowType == DataGridRowType.Default)
        {
            foreach (var column in Owner.Grid._columns)
            {
                await column.OnRowClickAsync(row);
            }
        }
    }

    /// <summary />
    internal async Task HandleOnRowDoubleClickAsync(string rowId)
    {
        var row = GetRow(rowId);
        if (row != null && Owner.Grid.OnRowDoubleClick.HasDelegate)
        {
            await Owner.Grid.OnRowDoubleClick.InvokeAsync(row);
        }
    }

    /// <summary />
    internal async Task HandleOnRowKeyDownAsync(string rowId, KeyboardEventArgs e)
    {
        if (!SelectColumn<TGridItem>.KEYBOARD_SELECT_KEYS.Contains(e.Code))
        {
            return;
        }

        var row = GetRow(rowId, r => r.RowType == DataGridRowType.Default);
        if (row != null)
        {
            foreach (var column in Owner.Grid._columns)
            {
                await column.OnRowKeyDownAsync(row, e);
            }
        }
    }

    private FluentDataGridRow<TGridItem>? GetRow(string rowId, Func<FluentDataGridRow<TGridItem>, bool>? where = null)
    {
        if (!string.IsNullOrEmpty(rowId) && Owner.Rows.TryGetValue(rowId, out var row))
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
