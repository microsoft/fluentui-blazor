// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Describes why a toast was closed.
/// </summary>
public enum ToastCloseReason
{
    /// <summary>
    /// The toast was dismissed by the user.
    /// </summary>
    Dismissed,

    /// <summary>
    /// The toast closed after its timeout elapsed.
    /// </summary>
    TimedOut,

    /// <summary>
    /// The toast closed after a quick action was clicked.
    /// </summary>
    QuickAction,

    /// <summary>
    /// The toast was closed programmatically.
    /// </summary>
    Programmatic,
}
