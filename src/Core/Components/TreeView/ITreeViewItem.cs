// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Interface for tree view items
/// </summary>
public interface ITreeViewItem
{
    /// <summary>
    /// Gets or sets the unique identifier of the tree item.
    /// </summary>
    string Id { get; set; }

    /// <summary>
    /// Gets or sets the text of the tree item.
    /// </summary>
    string Text { get; set; }

    /// <summary>
    /// Gets or sets the sub-items of the tree item.
    /// </summary>
    IEnumerable<ITreeViewItem>? Items { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="Icon"/> displayed at the start of tree item,
    /// when the node is collapsed.
    /// If this icon is not set, the <see cref="IconStartExpanded"/> will be used.
    /// </summary>
    Icon? IconStartCollapsed { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="Icon"/> displayed at the start of tree item,
    /// when the node is expanded.
    /// If this icon is not set, the <see cref="IconStartCollapsed"/> will be used.
    /// </summary>
    Icon? IconStartExpanded { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="Icon"/> displayed at the end of tree item,
    /// when the node is collapsed.
    /// If this icon is not set, the <see cref="IconEndExpanded"/> will be used.
    /// </summary>
    Icon? IconEndCollapsed { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="Icon"/> displayed at the end of tree item,
    /// when the node is expanded.
    /// If this icon is not set, the <see cref="IconEndCollapsed"/> will be used.
    /// </summary>
    Icon? IconEndExpanded { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="Icon"/> displayed at the aside slot of tree item,
    /// when the node is collapsed.
    /// If this icon is not set, the <see cref="IconAsideExpanded"/> will be used.
    /// </summary>
    Icon? IconAsideCollapsed { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="Icon"/> displayed at the aside slot of tree item,
    /// when the node is expanded.
    /// If this icon is not set, the <see cref="IconAsideCollapsed"/> will be used.
    /// </summary>
    Icon? IconAsideExpanded { get; set; }

    /// <summary>
    /// When true, the control will be immutable by user interaction.
    /// See <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Attributes/disabled">disabled</see> HTML attribute for more information.
    /// </summary>
    bool Disabled { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the tree item is expanded.
    /// </summary>
    bool Expanded { get; set; }

    /// <summary>
    /// Gets or sets the action to be performed when the tree item is expanded or collapsed
    /// </summary>
    Func<TreeViewItemExpandedEventArgs, Task>? OnExpandedAsync { get; set; }
}
