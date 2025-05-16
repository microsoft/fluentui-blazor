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
    /// If this text is too long, it will be truncated with ellipsis.
    /// </summary>
    string Text { get; set; }

    /// <summary>
    /// Gets or sets the sub-items of the tree item.
    /// </summary>
    IEnumerable<ITreeViewItem>? Items { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="Icon"/> displayed at the start of item content.
    /// We recommend using an icon size of 16px.
    /// </summary>
    Icon? IconStart { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="Icon"/> displayed at the end of item content.
    /// We recommend using an icon size of 16px.
    /// </summary>
    Icon? IconEnd { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="Icon"/> displayed at the "far end" of item content.
    /// We recommend using an icon size of 16px.
    /// </summary>
    Icon? IconAside { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="Icon"/> displayed to indicate the tree item is collapsed.
    /// If this icon is not set, the <see cref="IconExpanded"/> will be used.
    /// We recommend using an icon size of 16px.
    /// </summary>
    Icon? IconCollapsed { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="Icon"/> displayed to indicate the tree item is expanded.
    /// If this icon is not set, the <see cref="IconCollapsed"/> will be used.
    /// A 90-degree rotation effect is applied to the icon.
    /// Please select an icon that will look correct after rotation.
    /// We recommend using an icon size of 16px.
    /// </summary>
    Icon? IconExpanded { get; set; }

    /// <summary>
    /// Returns <see langword="true"/> if the tree item is expanded,
    /// and <see langword="false"/> if collapsed.
    /// </summary>
    bool Expanded { get; set; }

    /// <summary>
    /// Gets or sets the action to be performed when the tree item is expanded or collapsed
    /// </summary>
    Func<TreeViewItemExpandedEventArgs, Task>? OnExpandedAsync { get; set; }
}
