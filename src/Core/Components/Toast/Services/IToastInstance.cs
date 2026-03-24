// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Interface for ToastReference
/// </summary>
public interface IToastInstance
{
    /// <summary>
    /// Gets the unique identifier for the Toast.
    /// If this value is not set in the <see cref="ToastOptions"/>, a new identifier is generated.
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Gets the index of the Toast (sequential number).
    /// </summary>
    long Index { get; }

    /// <summary>
    /// Gets the options used to configure the Toast.
    /// </summary>
    ToastOptions Options { get; }

    /// <summary>
    /// Gets the close reason of the Toast.
    /// </summary>
    Task<ToastCloseReason> Result { get; }

    /// <summary>
    /// Gets the lifecycle status of the toast.
    /// </summary>
    ToastStatus Status { get; }

    /// <summary>
    /// Closes the Toast as dismissed.
    /// </summary>
    /// <returns></returns>
    Task CancelAsync();

    /// <summary>
    /// Closes the Toast programmatically.
    /// </summary>
    /// <returns></returns>
    Task CloseAsync();

    /// <summary>
    /// Closes the Toast with the specified reason.
    /// </summary>
    /// <param name="reason">Reason to close the Toast with.</param>
    /// <returns></returns>
    Task CloseAsync(ToastCloseReason reason);

    /// <summary>
    /// Updates the toast options while the toast is shown.
    /// </summary>
    /// <param name="update">The action that mutates the current options.</param>
    /// <returns></returns>
    Task UpdateAsync(Action<ToastOptions> update);
}
