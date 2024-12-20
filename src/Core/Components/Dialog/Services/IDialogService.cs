// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Interface for DialogService
/// </summary>
public partial interface IDialogService : IFluentServiceBase<FluentDialog>
{
    /// <summary />
    Task CloseAsync(DialogInstance dialog, DialogResult result);

    /// <summary>
    /// Shows a dialog with the component type as the body,
    /// </summary>
    /// <param name="dialogComponent">Type of component to display.</param>
    /// <param name="options">Options to configure the dialog component.</param>
    Task<IDialogInstance> ShowDialogAsync(Type dialogComponent, DialogOptions options);

    /// <summary>
    /// Shows a dialog with the component type as the body.
    /// </summary>
    /// <typeparam name="TDialog">Type of component to display.</typeparam>
    /// <param name="options">Options to configure the dialog component.</param>
    Task<IDialogInstance> ShowDialogAsync<TDialog>(DialogOptions options)
         where TDialog : ComponentBase;

    /// <summary>
    /// Shows a dialog with the component type as the body.
    /// </summary>
    /// <typeparam name="TDialog">Type of component to display.</typeparam>
    /// <param name="options">Options to configure the dialog component.</param>
    Task<IDialogInstance> ShowDialogAsync<TDialog>(Action<DialogOptions> options)
         where TDialog : ComponentBase;
}
