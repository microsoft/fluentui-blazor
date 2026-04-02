// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Defines the pinning behavior of a column in a <see cref="FluentDataGrid{TGridItem}"/>.
/// Pinned columns remain visible during horizontal scrolling.
/// </summary>
public enum DataGridColumnPin
{
    /// <summary>
    /// The column is not pinned (default).
    /// </summary>
    None,

    /// <summary>
    /// The column is pinned to the left edge of the grid.
    /// The column will remain visible when the user scrolls right.
    /// </summary>
    Left,

    /// <summary>
    /// The column is pinned to the right edge of the grid.
    /// The column will remain visible when the user scrolls left.
    /// </summary>
    Right,
}
