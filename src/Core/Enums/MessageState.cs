// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents the type of content to display in the message (and the associated icon).
/// </summary>
public enum MessageState
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
