// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The semantic state of a <see cref="FluentDataGridRow{TGridItem}"/> in a <see cref="FluentDataGrid{TGridItem}"/>.
/// </summary>
public enum DataGridRowState
{
    /// <summary>
    /// A row that contains empty content.
    /// </summary>
    [Description("empty-content")]
    EmptyContent,

    /// <summary>
    /// A row that contains loading content.
    /// </summary>
    [Description("loading-content")]
    LoadingContent,

    /// <summary>
    /// A row that contains error content.
    /// </summary>
    [Description("error-content")]
    ErrorContent,

    /// <summary>
    /// A row that contains row details content.
    /// </summary>
    [Description("detail-content")]
    RowDetails,
}
