// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel;
using System.Linq.Expressions;
using Microsoft.Fast.Components.FluentUI.DataGrid.Infrastructure;

namespace Microsoft.Fast.Components.FluentUI.DataGrid.Infrastructure;

/// <summary>
/// provides a sort order specification used within <see cref="FluentDataGrid{TGridItem}"/>.
/// </summary>
internal class SortProvider
{
    public IQueryable<TGridItem> ApplySort<TGridItem, TValue>(ISortableColumn<TGridItem, TValue> Column, IQueryable<TGridItem> Source, bool IsFirst)
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
