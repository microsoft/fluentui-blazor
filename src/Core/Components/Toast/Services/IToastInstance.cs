// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Interface for a toast instance managed by the <see cref="INotificationService"/>.
/// </summary>
public interface IToastInstance : INotificationInstance
{
    /// <summary>
    /// Gets the options used to configure the toast.
    /// </summary>
    ToastOptions Options { get; }

    /// <summary>
    /// Gets the result of the toast.
    /// </summary>
    Task<ToastResult> Result { get; }

    /// <summary>
    /// Gets the lifecycle status of the toast.
    /// </summary>
    ToastLifecycleStatus LifecycleStatus { get; }

    /// <summary>
    /// Closes the toast with the specified result.
    /// </summary>
    /// <param name="result">Result associated with the close action.</param>
    Task CloseAsync(ToastResult result);
}
