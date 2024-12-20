// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Event arguments for the FluentDialog toggle event.
/// </summary>
internal class DialogToggleEventArgs : EventArgs
{
    /// <summary>
    /// Gets or sets the ID of the dialog.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets the old state of the dialog.
    /// </summary>
    public string? OldState { get; set; }

    /// <summary>
    /// Gets or sets the new state of the dialog.
    /// </summary>
    public string? NewState { get; set; }

    /// <summary>
    /// Gets or sets the type of the dialog.
    /// </summary>
    public string? Type { get; set; }
}
