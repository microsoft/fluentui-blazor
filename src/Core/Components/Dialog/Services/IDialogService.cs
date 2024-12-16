// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Interface for DialogService
/// </summary>
public partial interface IDialogService : IFluentServiceBase<FluentDialog>
{
    /// <summary />
    Task CloseAsync(DialogReference dialog, DialogResult result);

    /// <summary>
    /// Shows a dialog with the component type as the body,
    /// passing the specified <paramref name="data"/> 
    /// </summary>
    /// <param name="dialogComponent">Type of component to display.</param>
    /// <param name="data">Content to pass to component being displayed.</param>
    /// <param name="parameters">Parameters to configure the dialog component.</param>
    Task<IDialogReference> ShowDialogAsync(Type dialogComponent, object data, DialogParameters parameters);

    /// <summary>
    /// Shows a dialog with the component type as the body,
    /// passing the specified <paramref name="data"/> 
    /// </summary>
    /// <typeparam name="TDialog">Type of component to display.</typeparam>
    /// <param name="data">Content to pass to component being displayed.</param>
    /// <param name="parameters">Parameters to configure the dialog component.</param>
    Task<IDialogReference> ShowDialogAsync<TDialog>(object data, DialogParameters parameters)
         where TDialog : IDialogContentComponent;
}
