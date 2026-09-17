// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Specifies the selection mode for a <see cref="FluentTreeView"/> component.
/// </summary>
public enum TreeSelectionMode
{
    /// <summary>
    /// The user can select only one item at a time.
    /// </summary>
    Single,

    /// <summary>
    /// The user can select multiple items at a time, at any level.
    /// </summary>
    Multiple,
}
