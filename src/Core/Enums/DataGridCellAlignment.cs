// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The horizontal alignment of content in a <see cref="FluentDataGridCell{TGridItem}"/>.
/// </summary>
public enum DataGridCellAlignment
{
    /// <summary>
    /// A start aligned cell.
    /// </summary>
    [Description("flex-start")]
    Start,

    /// <summary>
    /// A center aligned cell.
    /// </summary>
    [Description("center")]
    Center,

    /// <summary>
    /// An end aligned cell.
    /// </summary>
    [Description("flex-end")]
    End,
}
