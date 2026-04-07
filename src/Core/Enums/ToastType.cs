// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Describes the type of toast.
/// </summary>
public enum ToastType
{
    /// <summary>
    /// A confirmation toast.
    /// </summary>
    Confirmation,

    /// <summary>
    /// A communication toast.
    /// </summary>
    Communication,

    /// <summary>
    /// A determinate progress toast.
    /// </summary>
    DeterminateProgress,

    /// <summary>
    /// An indeterminate progress toast.
    /// </summary>
    IndeterminateProgress,
}
