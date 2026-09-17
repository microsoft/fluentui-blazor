// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Describes why a message bar was closed.
/// </summary>
public enum MessageBarCloseReason
{
    /// <summary>
    /// The message bar was dismissed by the user.
    /// </summary>
    Dismissed,

    /// <summary>
    /// The message bar closed after its lifetime elapsed.
    /// </summary>
    TimedOut,

    /// <summary>
    /// The message bar was closed programmatically.
    /// </summary>
    Programmatic,
}
