// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.DataGrid.Infrastructure;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Provides accessible column reorder actions for a <see cref="FluentDataGrid{TGridItem}" /> column.
/// </summary>
/// <typeparam name="TGridItem">The type of the data items displayed in the grid.</typeparam>
public partial class ColumnReorderOptions<TGridItem>
{
    /// <summary />
    [Inject]
    public IFluentLocalizer Localizer { get; set; } = default!;

    [CascadingParameter]
    internal InternalGridContext<TGridItem> InternalGridContext { get; set; } = default!;

    /// <summary>
    /// Gets the column whose order should be changed.
    /// </summary>
    [Parameter, EditorRequired]
    public ColumnBase<TGridItem> Column { get; set; } = default!;

    private FluentDataGrid<TGridItem> Grid => InternalGridContext.Grid;

    private Task HandleMoveToStartAsync() => Grid.MoveColumnToStartAsync(Column);

    private Task HandleMoveLeftAsync() => Grid.MoveColumnLeftAsync(Column);

    private Task HandleMoveRightAsync() => Grid.MoveColumnRightAsync(Column);

    private Task HandleMoveToEndAsync() => Grid.MoveColumnToEndAsync(Column);
}
