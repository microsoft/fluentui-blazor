// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Describes the current lifecycle status of a message bar.
/// </summary>
public enum MessageBarLifecycleStatus
{
    /// <summary>
    /// The message bar is visible.
    /// </summary>
    Visible,

    /// <summary>
    /// The message bar has been dismissed and is leaving the active surface.
    /// </summary>
    Dismissed,

    /// <summary>
    /// The message bar has been unmounted from the provider.
    /// </summary>
    Unmounted,
}
