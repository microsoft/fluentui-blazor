// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Event arguments for the FluentTabs ActiveId changed event.
/// </summary>
internal class TabChangeEventArgs : EventArgs
{
    /// <summary>
    /// Gets or sets the ID of the dialog.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets new active tab ID.
    /// </summary>
    public string? ActiveId { get; set; }
}
