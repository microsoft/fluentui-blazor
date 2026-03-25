// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Describes the current lifecycle status of a toast.
/// </summary>
public enum ToastLifecycleStatus
{
    /// <summary>
    /// The toast has been queued for display.
    /// </summary>
    Queued,

    /// <summary>
    /// The toast is visible.
    /// </summary>
    Visible,

    /// <summary>
    /// The toast has been dismissed and is leaving the active surface.
    /// </summary>
    Dismissed,

    /// <summary>
    /// The toast has been unmounted from the provider.
    /// </summary>
    Unmounted,
}
