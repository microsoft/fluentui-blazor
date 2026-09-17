// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Interface for DialogService
/// </summary>
public partial interface IDialogService : IFluentServiceBase<IDialogInstance>
{
    /// <summary>
    /// Closes the dialog with the specified result.
    /// </summary>
    /// <param name="dialog">Instance of the dialog to close.</param>
    /// <param name="result">Result of closing the dialog box.</param>
    /// <returns></returns>
    Task CloseAsync(IDialogInstance dialog, DialogResult result);

    /// <summary>
    /// Shows a dialog with the component type as the body,
    /// </summary>
    /// <param name="dialogComponent">Type of component to display.</param>
    /// <param name="options">Options to configure the dialog component.</param>
    Task<DialogResult> ShowDialogAsync([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type dialogComponent, DialogOptions options);

    /// <summary>
    /// Shows a dialog with the component type as the body.
    /// </summary>
    /// <typeparam name="TDialog">Type of component to display.</typeparam>
    /// <param name="options">Options to configure the dialog component.</param>
    Task<DialogResult> ShowDialogAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TDialog>(DialogOptions options)
         where TDialog : ComponentBase;

    /// <summary>
    /// Shows a dialog with the component type as the body.
    /// </summary>
    /// <typeparam name="TDialog">Type of component to display.</typeparam>
    /// <param name="options">Options to configure the dialog component.</param>
    Task<DialogResult> ShowDialogAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TDialog>(Action<DialogOptions> options)
         where TDialog : ComponentBase;

    /// <summary>
    /// Shows a drawer (left or right panel) with the component type as the body.
    /// By default, the drawer is open at the right (end) of the screen.
    /// </summary>
    /// <typeparam name="TDialog">Type of component to display.</typeparam>
    /// <param name="options">Options to configure the dialog component.</param>
    Task<DialogResult> ShowDrawerAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TDialog>(DialogOptions options)
         where TDialog : ComponentBase;

    /// <summary>
    /// Shows a drawer (left or right panel) with the component type as the body.
    /// By default, the drawer is open at the right (end) of the screen.
    /// </summary>
    /// <typeparam name="TDialog">Type of component to display.</typeparam>
    /// <param name="options">Options to configure the dialog component.</param>
    Task<DialogResult> ShowDrawerAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TDialog>(Action<DialogOptions> options)
         where TDialog : ComponentBase;
}
