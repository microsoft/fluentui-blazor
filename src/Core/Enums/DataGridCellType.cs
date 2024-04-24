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
    RowHeader
}
