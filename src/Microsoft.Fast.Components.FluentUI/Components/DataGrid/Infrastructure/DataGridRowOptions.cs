using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;

namespace Microsoft.Fast.Components.FluentUI.DataGrid.Infrastructure;

public class DataGridRowOptions<TGridItem> where TGridItem : class
{

    public DataGridRowOptions(int rowIndex, TGridItem item, string? gridTemplateColumns, IEnumerable<ColumnBase<TGridItem>>? columns)
    {
        RowIndex = rowIndex;
        Item = item;
        GridTemplateColumns = gridTemplateColumns;
        Columns = columns;
    }

    public int? RowIndex { get; set; }

    public TGridItem? Item { get; set; }

    public string? GridTemplateColumns { get; set; } = null;

    public IEnumerable<ColumnBase<TGridItem>>? Columns { get; set; }

}
