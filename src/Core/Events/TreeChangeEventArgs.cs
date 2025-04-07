// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Event arguments for the <see cref="FluentTree"/> events.
/// </summary>
public class TreeChangeEventArgs : EventArgs
{
    /// <summary>
    /// The affected tree item ID.
    /// </summary>
    public string? AffectedId { get; set; }

    /// <summary>
    /// Whether the item is selected or not.
    /// </summary>
    public bool? Selected { get; set; }

    /// <summary>
    /// Whether the item is expanded or not.
    /// </summary>
    public bool? Expanded { get; set; }
}
