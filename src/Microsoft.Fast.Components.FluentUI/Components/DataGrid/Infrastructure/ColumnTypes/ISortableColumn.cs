using System.ComponentModel;
using System.Linq.Expressions;

namespace Microsoft.Fast.Components.FluentUI.DataGrid.Infrastructure;

internal interface ISortableColumn<TGridItem>
{

    public Nullable<ListSortDirection> SortDirection { get; set; }

    public short SortOrder { get; set; }

    public IQueryable<TGridItem> ApplySort(IQueryable<TGridItem> Source, bool IsFirst);

}

internal interface ISortableColumn<TGridItem, TValue> : ISortableColumn<TGridItem>
{
    public Expression<Func<TGridItem, TValue>>? SortProperty { get; set; }

    
}
