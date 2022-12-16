// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Data.SqlTypes;
using Microsoft.Fast.Components.FluentUI.DataGrid.Infrastructure;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary>
/// Parameters for data to be supplied by a <see cref="FluentDataGrid{TGridItem}"/>'s <see cref="FluentDataGrid{TGridItem}.RowsDataProvider"/>.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
public struct GridItemsProviderRequest<TGridItem> where TGridItem : class
{
    /// <summary>
    /// The zero-based index of the first item to be supplied.
    /// </summary>
    public int StartIndex { get; }

    /// <summary>
    /// If set, the maximum number of items to be supplied. If not set, the maximum number is unlimited.
    /// </summary>
    public int? Count { get; }

    /// <summary>
    /// Specifies which columns represents the sort order.
    ///
    /// Rather than inferring the sort rules manually, you should normally call either <see cref="ISortableColumn{TGridItem}.ApplySort(IQueryable{TGridItem}, bool)"/>
    /// since they also account for <see cref="SortByColumns" /> automatically.
    /// </summary>
    public IEnumerable<ColumnBase<TGridItem>>? SortByColumns { get; }

    /// <summary>
    /// Specifies which columns represents filter.
    /// </summary>
    public IEnumerable<ColumnBase<TGridItem>>? FilterByColumns { get; }

    internal DataGridSortProvider _sortProvider;

    /// <summary>
    /// A token that indicates if the request should be cancelled.
    /// </summary>
    public CancellationToken CancellationToken { get; }

    internal GridItemsProviderRequest(int startIndex
        , int? count
        , IEnumerable<ColumnBase<TGridItem>>? sortByColumn
        , IEnumerable<ColumnBase<TGridItem>>? filterByColumns
        , CancellationToken cancellationToken)
    {
        StartIndex = startIndex;
        Count = count;
        SortByColumns = sortByColumn;
        FilterByColumns = filterByColumns;
        CancellationToken = cancellationToken;
        _sortProvider = new DataGridSortProvider();
    }

    /// <summary>
    /// Applies the request's filters and sorting rules to the supplied <see cref="IQueryable{TGridItem}"/>.
    ///
    /// </summary>
    /// <param name="source">An <see cref="IQueryable{TGridItem}"/>.</param>
    /// <returns>A new <see cref="IQueryable{TGridItem}"/> representing the <paramref name="source"/> with sorting and filter rules applied.</returns>
    public IQueryable<TGridItem>? ApplyFilterAndSorting(IQueryable<TGridItem>? source)
    {
        if (source is null)
            return null;
        var filteredSource = ApplyFilter(source);
        return ApplySorting(filteredSource);
    }

    /// <summary>
    /// Applies the request's sorting rules to the supplied <see cref="IQueryable{TGridItem}"/>.
    ///
    /// Note that this only works if the current <see cref="SortByColumns"/> implements <see cref="ISortableColumn{TGridItem}"/>,
    /// otherwise it will throw.
    /// </summary>
    /// <param name="source">An <see cref="IQueryable{TGridItem}"/>.</param>
    /// <returns>A new <see cref="IQueryable{TGridItem}"/> representing the <paramref name="source"/> with sorting rules applied.</returns>
    public IQueryable<TGridItem>? ApplySorting(IQueryable<TGridItem> source)
    {
        if (source is null)
            return null;
        if (SortByColumns is not null)
        {
            IQueryable<TGridItem>? orderedItems = null;
            foreach (var col in SortByColumns.Cast<ISortableColumn<TGridItem>>().OrderBy(s => s.SortOrder))
            {
                if (col.SortDirection.HasValue && col.SortDirection.HasValue)
                {
                    orderedItems = col.ApplySort(orderedItems is null ? source : orderedItems, orderedItems is null);
                }
            }
            return orderedItems ?? source;
        }
        return null;
    }

    private IQueryable<TGridItem> ApplyFilter(IQueryable<TGridItem> source)
    {
        if (FilterByColumns is not null)
        {
            foreach (var col in FilterByColumns.Cast<IFilterableColumn<TGridItem>>())
            {
                source = col.ApplyFilter(source);
            }
            return source;
        }
        return source;
    }

}
