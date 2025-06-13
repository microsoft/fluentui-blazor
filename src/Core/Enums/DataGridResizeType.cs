// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

// ToDo: remove pragma after next PR
#pragma warning disable CS1574 // XML comment has cref attribute that could not be resolved
/// <summary>
/// The type of <see cref="FluentDataGridRow{TGridItem}"/> in a <see cref="FluentDataGrid{TGridItem}"/>.
/// </summary>
public enum DataGridResizeType
{
    /// <summary>
    /// Resize datagrid columns by discreet steps of 10 pixels
    /// </summary>
    Discrete,

    /// <summary>
    /// Resize datagrid columns by exact pixel values
    /// </summary>
    Exact,
}
#pragma warning restore CS1574 // XML comment has cref attribute that could not be resolved
