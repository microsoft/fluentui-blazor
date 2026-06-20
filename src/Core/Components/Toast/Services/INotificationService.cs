// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Interface for the Notification service.
/// </summary>
public partial interface INotificationService : IFluentServiceBase<INotificationInstance>
{
    /// <summary>
    /// Shows a toast using the supplied options and waits for the close result.
    /// </summary>
    /// <param name="options">Options to configure the toast.</param>
    Task<ToastResult> ShowToastAsync(ToastOptions options);

    /// <summary>
    /// Shows a toast by configuring an options object and waits for the close result.
    /// </summary>
    /// <param name="options">Action used to configure the toast.</param>
    Task<ToastResult> ShowToastAsync(Action<ToastOptions> options);

    /// <summary>
    /// Shows a custom toast component and waits for the close result.
    /// The component receives the current <see cref="INotificationInstance"/> through a cascading parameter.
    /// </summary>
    /// <typeparam name="TToast">A Blazor component type used to render the toast.</typeparam>
    /// <param name="options">Options used to configure the toast.</param>
    Task<ToastResult> ShowToastAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TToast>(ToastOptions options)
        where TToast : ComponentBase;

    /// <summary>
    /// Shows a custom toast component and waits for the close result.
    /// The component receives the current <see cref="INotificationInstance"/> through a cascading parameter.
    /// </summary>
    /// <typeparam name="TToast">A Blazor component type used to render the toast.</typeparam>
    /// <param name="options">Action used to configure the toast.</param>
    Task<ToastResult> ShowToastAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TToast>(Action<ToastOptions> options)
        where TToast : ComponentBase;

    /// <summary>
    /// Gets the toast instance with the specified ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    IToastInstance? GetToastInstance(string id);

    /// <summary>
    /// Closes the specified toast instance.
    /// </summary>
    /// <param name="toast">Instance of the toast to close.</param>
    /// <param name="data">Optional data to include with the close result.</param>
    Task CloseAsync(IToastInstance toast, object? data = null);

    /// <summary>
    /// Closes all current toasts.
    /// </summary>
    /// <returns>The number of toasts that were closed.</returns>
    Task<int> CloseAllToastsAsync();
}