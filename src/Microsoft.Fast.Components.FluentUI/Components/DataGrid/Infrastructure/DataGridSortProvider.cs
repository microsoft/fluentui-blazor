using Microsoft.Fast.Components.FluentUI;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Linq;

namespace Microsoft.Fast.Components.FluentUI.DataGrid.Infrastructure;

internal class DataGridSortProvider
{

    public IQueryable<TGridItem> ApplySort<TGridItem, TValue>(ISortableColumn<TGridItem,TValue> Column, IQueryable<TGridItem> Source,bool IsFirst)
    {
        if (Column.SortProperty is null)
            return Source;
        if (IsFirst)
        {
            if (Column.SortDirection!.Value == ListSortDirection.Ascending)
                return Source.OrderBy(Column.SortProperty);
            else
                return Source.OrderByDescending(Column.SortProperty);
        }
        else
        {
            if (Column.SortDirection!.Value == ListSortDirection.Ascending)
                return ((IOrderedQueryable<TGridItem>)Source).ThenBy(Column.SortProperty);
            else
                return ((IOrderedQueryable<TGridItem>)Source).ThenByDescending(Column.SortProperty);
        }
    }

}
