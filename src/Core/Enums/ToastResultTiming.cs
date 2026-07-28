// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Controls when <see cref="IToastInstance.Result"/> is completed.
/// </summary>
public enum ToastResultTiming
{
    /// <summary>
    /// Complete the result when the toast is dismissed or closed.
    /// </summary>
    Closed,

    /// <summary>
    /// Complete the result when the toast becomes visible.
    /// </summary>
    Visible,

    /// <summary>
    /// Complete the result when the toast is queued.
    /// </summary>
    Queued,
}
