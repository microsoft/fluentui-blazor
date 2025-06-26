// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

// ToDo: remove pragma after next PR
#pragma warning disable CS1574 // XML comment has cref attribute that could not be resolved
/// <summary>
/// The type of <see cref="FluentDataGridRow{TGridItem}"/> in a <see cref="FluentDataGrid{TGridItem}"/>.
/// </summary>
public enum DataGridRowType
{
    /// <summary>
    /// A normal row .
    /// </summary>
    Default,

    /// <summary>
    /// A header row.
    /// </summary>
    Header,

    /// <summary>
    /// A sticky header row.
    /// </summary>
    [Description("sticky-header")]
    StickyHeader,
}
#pragma warning restore CS1574 // XML comment has cref attribute that could not be resolved
