// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Implementation of <see cref="ITreeViewItem"/>
/// </summary>
public record TreeViewItem : ITreeViewItem
{ 
    /// <summary>
    /// <inheritdoc cref="ITreeViewItem.Id" />
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// <inheritdoc cref="ITreeViewItem.Text" />
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// <inheritdoc cref="ITreeViewItem.Items" />
    /// </summary>
    public IEnumerable<ITreeViewItem>? Items { get; set; }

    /// <summary>
    /// <inheritdoc cref="ITreeViewItem.IconCollapsed" />
    /// </summary>
    public Icon? IconCollapsed { get; set; }

    /// <summary>
    /// <inheritdoc cref="ITreeViewItem.IconExpanded" />
    /// </summary>
    public Icon? IconExpanded { get; set; }

    /// <summary>
    /// <inheritdoc cref="ITreeViewItem.Disabled" />
    /// </summary>
    public bool Disabled { get; set; } = false;
}
