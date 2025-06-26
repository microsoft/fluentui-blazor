// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Implementation of <see cref="ITreeViewItem"/>
/// </summary>
[DebuggerDisplay("{DebuggerDisplay}")]
public class TreeViewItem : ITreeViewItem
{
    /// <summary>
    /// Returns an array with a single <see cref="TreeViewItem"/> that represents a loading state.
    /// </summary>
    /// <param name="loadingMessage">The loading message</param>
    public static IEnumerable<TreeViewItem> LoadingTreeViewItems(string loadingMessage) => [new TreeViewItem() { Text = loadingMessage }];

    /// <summary>
    /// Initializes a new instance of the <see cref="TreeViewItem"/> class.
    /// </summary>
    public TreeViewItem()
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TreeViewItem"/> class.
    /// </summary>
    /// <param name="text">Text of the tree item</param>
    /// <param name="items">Sub-items of the tree item.</param>
    public TreeViewItem(string text, IEnumerable<ITreeViewItem>? items = null)
    {
        Id = Identifier.NewId();
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

    /// <inheritdoc cref="ITreeViewItem.Id"/>
    public string Id { get; set; }

    /// <inheritdoc cref="ITreeViewItem.Text"/>
    public string Text { get; set; } = string.Empty;

    /// <inheritdoc cref="ITreeViewItem.Items"/>
    public IEnumerable<ITreeViewItem>? Items { get; set; }

    /// <inheritdoc cref="ITreeViewItem.IconStart"/>
    public Icon? IconStart { get; set; }

    /// <inheritdoc cref="ITreeViewItem.IconEnd"/>
    public Icon? IconEnd { get; set; }

    /// <inheritdoc cref="ITreeViewItem.IconAside"/>
    public Icon? IconAside { get; set; }

    /// <inheritdoc cref="ITreeViewItem.IconCollapsed"/>
    public Icon? IconCollapsed { get; set; }

    /// <inheritdoc cref="ITreeViewItem.IconExpanded"/>
    public Icon? IconExpanded { get; set; }

    /// <inheritdoc cref="ITreeViewItem.Expanded"/>
    public bool Expanded { get; set; }

    /// <inheritdoc cref="ITreeViewItem.OnExpandedAsync"/>
    public Func<TreeViewItemExpandedEventArgs, Task>? OnExpandedAsync { get; set; }

    /// <summary>
    /// Returns the first item with the specified id in the tree view items.
    /// </summary>
    /// <param name="items">The tree view items to search in.</param>
    /// <param name="id">Identifier of the item to find.</param>
    /// <returns></returns>
    internal static ITreeViewItem? FindItemById(IEnumerable<ITreeViewItem>? items, string? id)
    {
        if (items == null)
        {
            return null;
        }

        foreach (var item in items)
        {
            if (string.Equals(item.Id, id, StringComparison.Ordinal))
            {
                return item;
            }

            var nestedItem = FindItemById(item.Items, id);
            if (nestedItem != null)
            {
                return nestedItem;
            }
        }

        return null;
    }

    internal string DebuggerDisplay
    {
        get
        {
            var count = Items?.Count() ?? 0;
            return count > 0
                ? $"[{Id}] {Text} (+ {count} sub-items)"
                : $"[{Id}] {Text}";
        }
    }
}
