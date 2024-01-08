using Microsoft.FluentUI.AspNetCore.Components.Infrastructure;

namespace Microsoft.FluentUI.AspNetCore.Components.DataGrid.Infrastructure;

// The grid cascades this so that descendant columns can talk back to it. It's an internal type
// so that it doesn't show up by mistake in unrelated components.
internal sealed class InternalGridContext<TGridItem>
{
    private int _index = 0;
    public Dictionary<string, FluentDataGridRow<TGridItem>> Rows { get; set; } = [];

    public IDataGrid<TGridItem> Grid { get; }
    public EventCallbackSubscribable<object?> ColumnsFirstCollected { get; } = new();

    public InternalGridContext(IDataGrid<TGridItem> grid)
    {
        Grid = grid;
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
