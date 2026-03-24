// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Interface for ToastService
/// </summary>
public partial interface IToastService : IFluentServiceBase<IToastInstance>
{
    /// <summary>
    /// Closes the toast with the specified reason.
    /// </summary>
    /// <param name="Toast">Instance of the toast to close.</param>
    /// <param name="reason">Reason for closing the toast.</param>
    /// <returns></returns>
    Task CloseAsync(IToastInstance Toast, ToastCloseReason reason);

    /// <summary>
    /// Shows a toast using the supplied options.
    /// </summary>
    /// <param name="options">Options to configure the toast.</param>
    Task<ToastCloseReason> ShowToastAsync(ToastOptions? options = null);

    /// <summary>
    /// Shows a toast by configuring an options object.
    /// </summary>
    /// <param name="options">Action used to configure the toast.</param>
    Task<ToastCloseReason> ShowToastAsync(Action<ToastOptions> options);

    /// <summary>
    /// Updates a shown toast.
    /// </summary>
    /// <param name="toast">The toast instance to update.</param>
    /// <param name="update">The action that mutates the current options.</param>
    /// <returns></returns>
    Task UpdateToastAsync(IToastInstance toast, Action<ToastOptions> update);
}
