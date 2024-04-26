// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

public interface ITreeViewItem
{
    /// <summary>
    /// Gets or sets the text of the tree item
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// Gets or sets the sub-items of the tree item
    /// </summary>
    public IEnumerable<ITreeViewItem>? Items { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="Icon"/> displayed at the start of tree item,
    /// when the node is collapsed.
    /// If this icon is not set, the <see cref="IconExpanded"/> will be used.
    /// </summary>
    public Icon? IconCollapsed { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="Icon"/> displayed at the start of tree item,
    /// when the node is expanded.
    /// If this icon is not set, the <see cref="IconCollapsed"/> will be used.
    /// </summary>
    public Icon? IconExpanded { get; set; }

    /// <summary>
    /// When true, the control will be immutable by user interaction.
    /// See <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Attributes/disabled">disabled</see> HTML attribute for more information.
    /// </summary>
    public bool Disabled { get; set; }
}
