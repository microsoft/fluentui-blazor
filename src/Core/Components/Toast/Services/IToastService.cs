// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Interface for ToastService
/// </summary>
public partial interface IToastService : IFluentServiceBase<IToastInstance>
{
    /// <summary>
    /// Closes the toast with the specified result.
    /// </summary>
    /// <param name="Toast">Instance of the toast to close.</param>
    /// <param name="result">Result of closing the toast.</param>
    /// <returns></returns>
    Task CloseAsync(IToastInstance Toast, ToastResult result);

    /// <summary>
    /// Shows a Toast with the component type as the body.
    /// </summary>
    /// <typeparam name="TToast">Type of component to display.</typeparam>
    /// <param name="options">Options to configure the toast component.</param>
    Task<ToastResult> ShowToastAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TToast>(ToastOptions? options = null)
         where TToast : ComponentBase;

    /// <summary>
    /// Shows a toast with the component type as the body.
    /// </summary>
    /// <typeparam name="TToast">Type of component to display.</typeparam>
    /// <param name="options">Options to configure the toast component.</param>
    Task<ToastResult> ShowToastAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TToast>(Action<ToastOptions> options)
         where TToast : ComponentBase;
}
