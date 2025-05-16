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
    private bool _expanded;

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

    /// <inheritdoc cref="ITreeViewItem.Id"/>
    public string Id { get; set; } = Identifier.NewId();

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
    public bool Expanded
    {
        get => _expanded;
        set
        {
            _expanded = value;

            // Invoke the event asynchronously
            _ = Task.Run(async () =>
            {
                if (OnExpandedAsync is not null)
                {
                    await OnExpandedAsync.Invoke(new TreeViewItemExpandedEventArgs(this, _expanded));
                }
            });
        }
    }

    /// <inheritdoc cref="ITreeViewItem.OnExpandedAsync"/>
    public Func<TreeViewItemExpandedEventArgs, Task>? OnExpandedAsync { get; set; }
}
