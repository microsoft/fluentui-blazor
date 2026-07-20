// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The type of <see cref="FluentDataGridCell{TGridItem}"/> in a <see cref="FluentDataGrid{TGridItem}"/>.
/// </summary>
public enum DataGridCellType
{
    /// <summary>
    /// A normal cell.
    /// </summary>
    Default,

    /// <summary>
    /// A header cell.
    /// </summary>
    [Description("columnheader")]
    ColumnHeader,

    /// <summary>
    /// Cell is a row header.
    /// </summary>
    [Description("rowheader")]
    RowHeader,

    /// <summary>
    /// A cell that spans all columns and holds the expanded <see cref="FluentDataGrid{TGridItem}.RowDetails"/> content of a row.
    /// </summary>
    [Description("rowdetails")]
    RowDetails,
}
