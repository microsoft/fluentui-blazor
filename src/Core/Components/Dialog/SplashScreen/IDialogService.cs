// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public partial interface IDialogService
{
    /// <summary>
    /// Shows a splash screen dialog with the component type as the body,
    /// and returns the dialog instance as soon as the dialog is displayed (without waiting for it to be closed).
    /// Use <see cref="ISplashScreenInstance.UpdateAsync(Action{DialogOptions})"/> to update its content
    /// and <see cref="IDialogInstance.CloseAsync()"/> to close it.
    /// </summary>
    /// <param name="dialogComponent">Type of component to display.</param>
    /// <param name="options">Options to configure the dialog component.</param>
    /// <returns>The instance of the displayed dialog.</returns>
    Task<ISplashScreenInstance> ShowSplashScreenAsync([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type dialogComponent, DialogOptions options);

    /// <summary>
    /// Shows a splash screen dialog with the component type as the body,
    /// and returns the dialog instance as soon as the dialog is displayed (without waiting for it to be closed).
    /// Use <see cref="ISplashScreenInstance.UpdateAsync(Action{DialogOptions})"/> to update its content
    /// and <see cref="IDialogInstance.CloseAsync()"/> to close it.
    /// </summary>
    /// <typeparam name="TDialog">Type of component to display.</typeparam>
    /// <param name="options">Options to configure the dialog component.</param>
    /// <returns>The instance of the displayed dialog.</returns>
    Task<ISplashScreenInstance> ShowSplashScreenAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TDialog>(DialogOptions options)
         where TDialog : ComponentBase;

    /// <summary>
    /// Shows a splash screen dialog with the component type as the body,
    /// and returns the dialog instance as soon as the dialog is displayed (without waiting for it to be closed).
    /// Use <see cref="ISplashScreenInstance.UpdateAsync(Action{DialogOptions})"/> to update its content
    /// and <see cref="IDialogInstance.CloseAsync()"/> to close it.
    /// </summary>
    /// <typeparam name="TDialog">Type of component to display.</typeparam>
    /// <param name="options">Options to configure the dialog component.</param>
    /// <returns>The instance of the displayed dialog.</returns>
    Task<ISplashScreenInstance> ShowSplashScreenAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TDialog>(Action<DialogOptions>? options = null)
         where TDialog : ComponentBase;

    /// <summary>
    /// Updates the options (including the <see cref="DialogOptions.Parameters"/>) of an opened splash screen,
    /// and re-renders the splash screen content with these new values.
    /// </summary>
    /// <param name="splashScreen">Instance of the splash screen to update.</param>
    /// <param name="options">Action used to update the splash screen options.</param>
    /// <returns></returns>
    Task UpdateSplashScreenAsync(ISplashScreenInstance splashScreen, Action<DialogOptions> options);
}
