// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Event arguments for the FluentTabs ActiveId changed event.
/// </summary>
internal class TreeItemChanged : EventArgs
{
    /// <summary>
    /// Gets or sets the ID of the tree item.
    /// </summary>
    public string? Id { get; set; }
}
