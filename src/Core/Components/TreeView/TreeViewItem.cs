// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Implementation of <see cref="ITreeViewItem"/>
/// </summary>
public class TreeViewItem : ITreeViewItem
{
    /// <summary>
    /// Returns a <see cref="TreeViewItem"/> that represents a loading state.
    /// </summary>
    public static TreeViewItem LoadingTreeViewItem => new TreeViewItem() { Text = FluentTree.LoadingMessage, Disabled = true };

    /// <summary>
    /// Returns an array with a single <see cref="TreeViewItem"/> that represents a loading state.
    /// </summary>
    public static IEnumerable<TreeViewItem> LoadingTreeViewItems => new[] { new TreeViewItem() { Text = FluentTree.LoadingMessage, Disabled = true } };

    /// <summary>
    /// Initializes a new instance of the <see cref="TreeViewItem"/> class.
    /// </summary>
    public TreeViewItem()
    {

    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TreeViewItem"/> class.
    /// </summary>
    /// <param name="text">Text of the tree item</param>
    /// <param name="items">Sub-items of the tree item.</param>
    public TreeViewItem(string text, IEnumerable<ITreeViewItem>? items = null)
    {
        Text = text;
        Items = items;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TreeViewItem"/> class.
    /// </summary>
    /// <param name="id">Unique identifier of the tree item</param>
    /// <param name="text">Text of the tree item</param>
    /// <param name="items">Sub-items of the tree item.</param>
    public TreeViewItem(string id, string text, IEnumerable<ITreeViewItem>? items = null)
    {
        Id = id;
        Text = text;
        Items = items;
    }

    /// <summary>
    /// <inheritdoc cref="ITreeViewItem.Id" />
    /// </summary>
    public string Id { get; set; } = Identifier.NewId();

    /// <summary>
    /// <inheritdoc cref="ITreeViewItem.Text" />
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// <inheritdoc cref="ITreeViewItem.Items" />
    /// </summary>
    public IEnumerable<ITreeViewItem>? Items { get; set; }

    /// <summary>
    /// <inheritdoc cref="ITreeViewItem.IconStartCollapsed" />
    /// </summary>
    public Icon? IconStartCollapsed { get; set; }

    /// <summary>
    /// <inheritdoc cref="ITreeViewItem.IconStartExpanded" />
    /// </summary>
    public Icon? IconStartExpanded { get; set; }

    /// <summary>
    /// <inheritdoc cref="ITreeViewItem.IconEndCollapsed" />
    /// </summary>
    public Icon? IconEndCollapsed { get; set; }

    /// <summary>
    /// <inheritdoc cref="ITreeViewItem.IconEndExpanded" />
    /// </summary>
    public Icon? IconEndExpanded { get; set; }

    /// <summary>
    /// <inheritdoc cref="ITreeViewItem.IconAsideCollapsed" />
    /// </summary>
    public Icon? IconAsideCollapsed { get; set; }

    /// <summary>
    /// <inheritdoc cref="ITreeViewItem.IconAsideExpanded" />
    /// </summary>
    public Icon? IconAsideExpanded { get; set; }

    /// <summary>
    /// <inheritdoc cref="ITreeViewItem.Disabled" />
    /// </summary>
    public bool Disabled { get; set; }

    /// <summary>
    /// <inheritdoc cref="ITreeViewItem.Expanded"/>
    /// </summary>
    public bool Expanded { get; set; }

    /// <summary>
    /// <inheritdoc cref="ITreeViewItem.OnExpandedAsync" />
    /// </summary>
    public Func<TreeViewItemExpandedEventArgs, Task>? OnExpandedAsync { get; set; }
}
