// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

// ToDo: remove pragma after next PR
#pragma warning disable CS1574 // XML comment has cref attribute that could not be resolved
/// <summary>
/// How rows can be selected in a <see cref="FluentDataGrid{TGridItem}"/> when using a <see cref="SelectColumn{TGridItem}"/>.
/// </summary>
public enum DataGridSelectMode
{
    /// <summary>
    /// Allow only one selected row.
    /// </summary>
    Single,

    /// <summary>
    /// Allow only one selected row. Keep selection if same row is clicked.
    /// </summary>
    SingleSticky,

    /// <summary>
    /// Allow multiple selected rows.
    /// </summary>
    Multiple,
}
#pragma warning restore CS1574 // XML comment has cref attribute that could not be resolved
