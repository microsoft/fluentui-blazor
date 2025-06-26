// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

// ToDo: remove pragma after next PR
#pragma warning disable CS1574 // XML comment has cref attribute that could not be resolved

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
}
#pragma warning restore CS1574 // XML comment has cref attribute that could not be resolved

