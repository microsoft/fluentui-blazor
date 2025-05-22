// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Specifies the selection mode for a <see cref="FluentTreeView"/> component.
/// </summary>
public enum TreeSelectionVisibility
{
    /// <summary>
    /// The checkbox is visible.
    /// </summary>
    Visible,

    /// <summary>
    /// The checkbox is invisible (not drawn), but still affects layout as normal.
    /// </summary>
    Hidden,

    /// <summary>
    /// The checkbox is invisible (not drawn), and the space it would take up is collapsed.
    /// </summary>
    Collapse,
}
