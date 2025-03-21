// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Event arguments for the FluentDialog toggle event.
/// </summary>
internal class DropdownEventArgs : EventArgs
{
    /// <summary>
    /// Gets or sets the ID of the dialog.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets the type of the dialog.
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// Gets or sets the list of option IDs selected in the drop-down.
    /// </summary>
    public string? SelectedOptions { get; set; }
}
