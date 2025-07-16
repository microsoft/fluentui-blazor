// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

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
