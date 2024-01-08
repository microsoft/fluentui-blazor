using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

public interface IDataGrid<TGridItem>
{
    public bool Virtualize { get; }
    float ItemSize { get; }
    public bool? SortByAscending { get; }
    EventCallback<FluentDataGridCell<TGridItem>> OnCellFocus { get; }
    void AddColumn(ColumnBase<TGridItem> column, SortDirection? initialSortDirection, bool isDefaultSortColumn);
    Task ShowColumnOptionsAsync(ColumnBase<TGridItem> column);
    Task SortByColumnAsync(ColumnBase<TGridItem> column, SortDirection direction = SortDirection.Auto);
}