// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents the state of a dialog.
/// </summary>
public enum DialogState
{
    /// <summary>
    /// The dialog is hidden.
    /// </summary>
    Closed,

    /// <summary>
    /// The dialog is showing.
    /// </summary>
    Opening,

    /// <summary>
    /// The dialog is shown.
    /// </summary>
    Open,

    /// <summary>
    /// The dialog is closing.
    /// </summary>
    Closing,
}
