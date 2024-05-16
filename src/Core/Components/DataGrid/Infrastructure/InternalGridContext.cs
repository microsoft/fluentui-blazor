using Microsoft.FluentUI.AspNetCore.Components.Infrastructure;

namespace Microsoft.FluentUI.AspNetCore.Components.DataGrid.Infrastructure;

// The grid cascades this so that descendant columns can talk back to it. It's an internal type
// so that it doesn't show up by mistake in unrelated components.
internal sealed class InternalGridContext<TGridItem>
{
    private int _index = 0;
    private int _rowId = 0;
    private int _cellId = 0;

    public (ColumnBase<TGridItem>? Column, SortDirection? Direction) DefaultSortColumn { get; set; }
    //public SortDirection? DefaultSortDirection { get; set; }

    public Dictionary<string, FluentDataGridRow<TGridItem>> Rows { get; set; } = [];

    public ICollection<TGridItem> Items { get; set; } = [];
    public int TotalItemCount { get; set; }

    public FluentDataGrid<TGridItem> Grid { get; }
    public EventCallbackSubscribable<object?> ColumnsFirstCollected { get; } = new();

    public InternalGridContext(FluentDataGrid<TGridItem> grid)
    {
        Grid = grid;
    }

    public int GetNextRowId()
    {
        Interlocked.Increment(ref _rowId);
        return _rowId;
    }

    public int GetNextCellId()
    {
        Interlocked.Increment(ref _cellId);
        return _cellId;
    }

    internal void ResetRowIndexes(int start)
    {
        _index = start;
    }

    internal void Register(FluentDataGridRow<TGridItem> row)
    {
        Rows.Add(row.RowId, row);
        if (!Grid.Virtualize)
        {
            row.RowIndex = _index++;
        }
    }

    internal void Unregister(FluentDataGridRow<TGridItem> row)
    {
        Rows.Remove(row.RowId);
    }
}
