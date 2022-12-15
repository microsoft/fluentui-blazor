// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Fast.Components.FluentUI.Infrastructure;

namespace Microsoft.Fast.Components.FluentUI.DataGrid.Infrastructure;

// The grid cascades this so that descendant columns can talk back to it.
// It's an internal type so it doesn't show up in unrelated components by mistake.
internal class InternalGridContext<TGridItem> where TGridItem :class
{
    public Dictionary<string, FluentDataGridRow<TGridItem>> Rows { get; set; } = new();

    public FluentDataGrid<TGridItem> Grid { get; }
    public EventCallbackSubscribable<object?> ColumnsFirstCollected { get; } = new();

    public InternalGridContext(FluentDataGrid<TGridItem> grid)
    {
        Grid = grid;
    }

    internal void Register(FluentDataGridRow<TGridItem> row)
    {
        Rows.Add(row.RowId, row);
    }

    internal void Unregister(FluentDataGridRow<TGridItem> row)
    {
        Rows.Remove(row.RowId);
    }
}
