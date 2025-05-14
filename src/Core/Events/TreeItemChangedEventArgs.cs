// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Event arguments for the FluentTabs ActiveId changed event.
/// </summary>
internal class TreeItemChangedEventArgs : EventArgs
{
    /// <summary>
    /// Gets or sets the ID of the tree item.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the item is selected.
    /// </summary>
    public bool Selected { get; set; }
}
