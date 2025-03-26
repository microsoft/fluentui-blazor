// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents the event arguments for a menu item change, indicating whether the item is checked or not.
/// </summary>
public class MenuItemEventArgs : EventArgs
{
    /// <summary>
    /// Gets the item that was changed.
    /// </summary>
    public FluentMenuItem? Item { get; internal set; }

    /// <summary>
    /// Gets whether an item is checked or not, represented as a boolean value.
    /// Value is null if the item has a role of 'menuitem'.
    /// </summary>
    public bool? Checked { get; init; }

    /// <summary>
    /// Gets the menu item text.
    /// </summary>
    public string? Text { get; init; }
}
