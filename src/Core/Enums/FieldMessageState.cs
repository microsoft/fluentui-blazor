// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents the type of content to display in the message (and the associated icon).
/// </summary>
public enum FieldMessageState
{
    /// <summary>
    /// Display a red error message text and red error icon.
    /// </summary>
    Error,

    /// <summary>
    /// Display a gray success message text and green checkmark icon.
    /// </summary>
    Success,

    /// <summary>
    /// Display a gray message text and yellow exclamation icon.
    /// </summary>
    Warning,
}
