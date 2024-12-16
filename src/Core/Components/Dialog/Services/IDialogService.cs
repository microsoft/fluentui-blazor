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
    /// <typeparam name="TData">Type of content to pass to component being displayed.</typeparam>
    /// <param name="dialogComponent">Type of component to display.</param>
    /// <param name="data">Content to pass to component being displayed.</param>
    /// <param name="parameters">Parameters to configure the dialog component.</param>
    Task<IDialogReference> ShowDialogAsync<TData>(Type dialogComponent, TData data, DialogParameters parameters)
        where TData : class;
}
