// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The type of rendering to use for the <see cref="FluentDataGrid{TGridItem}"/>
/// </summary>
public enum DataGridDisplayMode
{
    /// <summary>
    /// Uses display:grid with HTML table elements to render the DataGrid.
    /// With this mode fr units canbe used to set the column widths.
    /// </summary>
    Grid,

    /// <summary>
    /// Uses HTML table elements only to render the DataGrid.
    /// With this mode fr units cannot be used to set the column widths.
    /// </summary>
    Table,

}
