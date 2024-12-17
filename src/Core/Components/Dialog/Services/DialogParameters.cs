// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Parameters for configuring a dialog.
/// </summary>
public class DialogParameters
{
    /// <summary>
    /// Gets or sets the title of the dialog.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the action raised when the dialog is opened or closed.
    /// </summary>
    public Action<DialogEventArgs>? OnStateChange { get; set; }
}
