// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

// ToDo: remove pragma after next PR
#pragma warning disable CS1574 // XML comment has cref attribute that could not be resolved
/// <summary>
/// The type of rendering to use for the <see cref="FluentDataGrid{TGridItem}"/>
/// </summary>
public enum DataGridDisplayMode
{
    /// <summary>
    /// Uses display:grid with HTML table elements to render the DataGrid.
    /// With this mode fr units can be used to set the column widths.
    /// </summary>
    Grid,

    /// <summary>
    /// Uses HTML table elements only to render the DataGrid.
    /// With this mode fr units cannot be used to set the column widths.
    /// </summary>
    Table,
}
#pragma warning restore CS1574 // XML comment has cref attribute that could not be resolved

