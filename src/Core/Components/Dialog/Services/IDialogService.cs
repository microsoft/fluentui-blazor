// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

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
    Task<DialogResult> ShowDialogAsync(Type dialogComponent, DialogOptions options);

    /// <summary>
    /// Shows a dialog with the component type as the body.
    /// </summary>
    /// <typeparam name="TDialog">Type of component to display.</typeparam>
    /// <param name="options">Options to configure the dialog component.</param>
    Task<DialogResult> ShowDialogAsync<TDialog>(DialogOptions options)
         where TDialog : ComponentBase;

    /// <summary>
    /// Shows a dialog with the component type as the body.
    /// </summary>
    /// <typeparam name="TDialog">Type of component to display.</typeparam>
    /// <param name="options">Options to configure the dialog component.</param>
    Task<DialogResult> ShowDialogAsync<TDialog>(Action<DialogOptions> options)
         where TDialog : ComponentBase;

    /// <summary>
    /// Shows a panel with the component type as the body.
    /// By default, the panel is open at the right (end) of the screen.
    /// </summary>
    /// <typeparam name="TDialog">Type of component to display.</typeparam>
    /// <param name="options">Options to configure the dialog component.</param>
    Task<DialogResult> ShowPanelAsync<TDialog>(DialogOptions options)
         where TDialog : ComponentBase;

    /// <summary>
    /// Shows a panel with the component type as the body.
    /// By default, the panel is open at the right (end) of the screen.
    /// </summary>
    /// <typeparam name="TDialog">Type of component to display.</typeparam>
    /// <param name="options">Options to configure the dialog component.</param>
    Task<DialogResult> ShowPanelAsync<TDialog>(Action<DialogOptions> options)
         where TDialog : ComponentBase;
}
