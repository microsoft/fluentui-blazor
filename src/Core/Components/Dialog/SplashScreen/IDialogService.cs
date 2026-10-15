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
    /// Use <see cref="ISplashScreenInstance.UpdateAsync(Action{SplashScreenContent})"/> to update its content
    /// and <see cref="IDialogInstance.CloseAsync()"/> to close it.
    /// The component must declare a parameter named <c>SplashScreen</c> of type <see cref="SplashScreenOptions"/>.
    /// </summary>
    /// <param name="dialogComponent">Type of component to display.</param>
    /// <param name="options">Options to configure the dialog component.</param>
    /// <returns>The instance of the displayed dialog.</returns>
    Task<ISplashScreenInstance> ShowSplashScreenAsync([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type dialogComponent, SplashScreenOptions options);

    /// <summary>
    /// Shows a splash screen dialog with the component type as the body,
    /// and returns the dialog instance as soon as the dialog is displayed (without waiting for it to be closed).
    /// Use <see cref="ISplashScreenInstance.UpdateAsync(Action{SplashScreenContent})"/> to update its content
    /// and <see cref="IDialogInstance.CloseAsync()"/> to close it.
    /// The component must declare a parameter named <c>SplashScreen</c> of type <see cref="SplashScreenOptions"/>.
    /// </summary>
    /// <typeparam name="TDialog">Type of component to display.</typeparam>
    /// <param name="options">Options to configure the dialog component.</param>
    /// <returns>The instance of the displayed dialog.</returns>
    Task<ISplashScreenInstance> ShowSplashScreenAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TDialog>(SplashScreenOptions options)
         where TDialog : ComponentBase;

    /// <summary>
    /// Shows a splash screen dialog with the component type as the body,
    /// and returns the dialog instance as soon as the dialog is displayed (without waiting for it to be closed).
    /// Use <see cref="ISplashScreenInstance.UpdateAsync(Action{SplashScreenContent})"/> to update its content
    /// and <see cref="IDialogInstance.CloseAsync()"/> to close it.
    /// The component must declare a parameter named <c>SplashScreen</c> of type <see cref="SplashScreenOptions"/>.
    /// </summary>
    /// <typeparam name="TDialog">Type of component to display.</typeparam>
    /// <param name="options">Options to configure the dialog component.</param>
    /// <returns>The instance of the displayed dialog.</returns>
    Task<ISplashScreenInstance> ShowSplashScreenAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TDialog>(Action<SplashScreenOptions>? options = null)
         where TDialog : ComponentBase;

    /// <summary>
    /// Shows a splash screen dialog with the default component as the body,
    /// and returns the dialog instance as soon as the dialog is displayed (without waiting for it to be closed).
    /// Use <see cref="ISplashScreenInstance.UpdateAsync(Action{SplashScreenContent})"/> to update its content
    /// and <see cref="IDialogInstance.CloseAsync()"/> to close it.
    /// </summary>
    /// <param name="content">Action used to configure the dialog component.</param>
    /// <returns>The instance of the displayed dialog.</returns>
    Task<ISplashScreenInstance> ShowSplashScreenAsync(Action<SplashScreenContent> content);

    /// <summary>
    /// Updates the content of an opened splash screen and re-renders it with the new values.
    /// </summary>
    /// <param name="splashScreen">Instance of the splash screen to update.</param>
    /// <param name="content">Action used to update the splash screen content.</param>
    /// <returns>A task that represents the asynchronous update operation.</returns>
    internal Task UpdateSplashScreenAsync(ISplashScreenInstance splashScreen, Action<SplashScreenContent> content);
}
