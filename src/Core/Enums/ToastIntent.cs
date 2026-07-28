// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The intent of the toast.
/// </summary>
public enum ToastIntent
{
    /// <summary>
    /// Indicates that the toast is providing informational messages.
    /// </summary>
    Info,

    /// <summary>
    /// Indicates that the toast is providing success messages.
    /// </summary>
    Success,

    /// <summary>
    /// Indicates that the toast is providing warning messages.
    /// </summary>
    Warning,

    /// <summary>
    /// Indicates that the toast is providing error messages.
    /// </summary>
    Error,

    /// <summary>
    /// Indicates that the toast is displaying progress information, typically used for long-running operations or tasks.
    /// </summary>
    Progress,
}
