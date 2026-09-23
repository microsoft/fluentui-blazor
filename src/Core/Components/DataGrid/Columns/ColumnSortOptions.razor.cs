// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.DataGrid.Infrastructure;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Holds the multi-column sort actions of a <see cref="FluentDataGrid{TGridItem}" /> column in the column header
/// popup, for grids that do not use <see cref="FluentDataGrid{TGridItem}.HeaderCellAsButtonWithMenu"/> and therefore
/// have no header menu to put them in.
/// </summary>
/// <typeparam name="TGridItem">The type of the data items displayed in the grid.</typeparam>
public partial class ColumnSortOptions<TGridItem>
{
    [CascadingParameter]
    internal InternalGridContext<TGridItem> InternalGridContext { get; set; } = default!;

    /// <summary>
    /// Gets the column whose sort should be changed.
    /// </summary>
    [Parameter, EditorRequired]
    public ColumnBase<TGridItem> Column { get; set; } = default!;

    private FluentDataGrid<TGridItem> Grid => InternalGridContext.Grid;
}
