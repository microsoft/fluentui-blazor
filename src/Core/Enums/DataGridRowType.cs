using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

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
    StickyHeader
}
