// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Class that contains the event arguments for the <see cref="ITreeViewItem.OnExpandedAsync"/> event.
/// </summary>
public class TreeViewItemExpandedEventArgs
{
    /// <summary />
    internal TreeViewItemExpandedEventArgs(ITreeViewItem item, bool expanded)
    {
        CurrentItem = item;
        Expanded = expanded;
    }

    /// <summary>
    /// Gets the <see cref="ITreeViewItem"/> that was expanded or collapsed.
    /// </summary>
    public ITreeViewItem CurrentItem { get; }

    /// <summary>
    /// Gets a value indicating whether the item was expanded or collapsed.
    /// </summary>
    public bool Expanded { get; }
}
