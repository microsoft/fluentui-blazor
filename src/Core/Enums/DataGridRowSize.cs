// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

// ToDo: remove pragma after next PR
#pragma warning disable CS1574 // XML comment has cref attribute that could not be resolved
/// <summary>
/// The height of each <see cref="FluentDataGridRow{TGridItem}"/> in a <see cref="FluentDataGrid{TGridItem}"/>.
/// Values are in pixels.
/// </summary>
public enum DataGridRowSize
{
    /// <summary>
    /// Small row height (default)
    /// </summary>
    Small = 32,

    /// <summary>
    /// Medium row height
    /// </summary>
    Medium = 44,

    /// <summary>
    /// Smaller row height
    /// </summary>
    Smaller = 24,

    /// <summary>
    /// Large row height
    /// </summary>
    Large = 58,
}
#pragma warning restore CS1574 // XML comment has cref attribute that could not be resolved
