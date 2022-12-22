using System.ComponentModel;
using System.Linq.Expressions;
namespace Microsoft.Fast.Components.FluentUI.DataGrid.Infrastructure;

/// <summary>
/// This interface used for internal purpose, Implement <see cref="ISortableColumn{TGridItem, TValue}"/> instead.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
public interface ISortableColumn<TGridItem>
{

    /// <summary>
    ///  datagrid calcuated value of this property every time that sort changed.
    ///  dont use it for
    /// </summary>
    public ListSortDirection? SortDirection { get; set; }

    /// <summary>
    ///  datagrid calcuated value of this property every time that sort changed.
    ///  dont use it for
    /// </summary>
    public short SortOrder { get; set; }

    /// <summary>
    /// Datagrid call this function after sort of every column changed to
    /// this function call for all sortable columns to apply sorting to datasource. <br />
    /// custom columns can override this function, or simply call <see cref="SortProvider.ApplySort{TGridItem, TValue}(ISortableColumn{TGridItem, TValue}, IQueryable{TGridItem}, bool)"/> 
    /// </summary>
    /// <param name="Source">source of datagrid</param>
    /// <param name="IsFirst">if true, <see cref="SortProvider"/> use <see cref="Queryable.OrderBy{TSource, TKey}(IQueryable{TSource}, Expression{Func{TSource, TKey}})"/>, or <see cref="Queryable.OrderByDescending{TSource, TKey}(IQueryable{TSource}, Expression{Func{TSource, TKey}})"/>, otherwise use <see cref="Queryable.ThenBy{TSource, TKey}(IOrderedQueryable{TSource}, Expression{Func{TSource, TKey}})"/> or <see cref="Queryable.ThenByDescending{TSource, TKey}(IOrderedQueryable{TSource}, Expression{Func{TSource, TKey}})"/> based on sort direction </param>
    /// <returns></returns>
    public IQueryable<TGridItem> ApplySort(IQueryable<TGridItem> Source, bool IsFirst);

}

/// <summary>
/// An interface that, if implemented by a <see cref="ColumnBase{TGridItem}"/> subclass, allows a <see cref="FluentDataGrid{TGridItem}"/>
/// to understand the sorting rules associated with that column.
///
/// If a <see cref="ColumnBase{TGridItem}"/> subclass does not implement this, that column does not marked as sortable and can
/// be the current sort column and its sorting logic cannot be applied to the data queries automatically. The developer would be
/// responsible for implementing that sorting logic separately inside their <see cref="GridItemsProvider{TGridItem}"/>.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
/// <typeparam name="TValue">if  <see cref="ISortableColumn{TGridItem,TValue}.SortProperty"/> does not provided, <see cref="IBindableColumn{TGridItem,TValue}.Property"/> will used.</typeparam>
public interface ISortableColumn<TGridItem, TValue> : ISortableColumn<TGridItem>
{
    /// <summary>
    /// Defines the property to use for sort functionality
    /// if it is not provided, <see cref="PropertyColumn{TGridItem, TValue}.Property"/> used for sort.
    /// </summary>
    public Expression<Func<TGridItem, TValue>>? SortProperty { get; set; }
}

internal static class ISortableColumnExtensions
{

    public static bool SortingIsEnable<TGridItem>(this ColumnBase<TGridItem> col)
    {
        return col.IsSortableByDefault() && (!col.Sortable.HasValue || col.Sortable.Value);
    }

}