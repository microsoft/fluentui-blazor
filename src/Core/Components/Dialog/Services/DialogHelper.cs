// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Helper class for showing dialogs with typesafe parameters
/// </summary>
public static class DialogHelper
{
    /// <summary>
    /// Create a dialog helper for the specified dialog type
    /// </summary>
    /// <typeparam name="TDialog">Type of component to display.</typeparam>
    public static DialogHelper<TDialog> From<TDialog>() where TDialog : IDialogContentComponent => default;

    /// <summary>
    /// Shows a dialog with the component type as the body,
    /// passing the specified <paramref name="data"/>
    /// </summary>
    /// <typeparam name="TDialog">Type of component to display.</typeparam>
    /// <typeparam name="TData">Type of content to pass to component being displayed.</typeparam>
    /// <param name="svc">The Dialog service.</param>
    /// <param name="dialogHelper">The dialog helper from which we can infer the TData.</param>
    /// <param name="data">Content to pass to component being displayed.</param>
    /// <param name="parameters">Parameters to configure the dialog component.</param>
    public static Task<IDialogReference> ShowDialogAsync<TDialog, TData>(this IDialogService svc, DialogHelper<TDialog> dialogHelper, TData data, DialogParameters parameters)
       where TDialog : IDialogContentComponent<TData> => svc.ShowDialogAsync<TDialog, TData>(data, parameters);

    /// <summary>
    /// Shows a dialog with the component type as the body,
    /// passing the specified <paramref name="data"/>
    /// </summary>
    /// <typeparam name="TDialog">Type of component to display.</typeparam>
    /// <typeparam name="TData">Type of content to pass to component being displayed.</typeparam>
    /// <param name="dialogHelper">The dialog helper from which we can infer the TData.</param>
    /// <param name="svc">The Dialog service.</param>
    /// <param name="data">Content to pass to component being displayed.</param>
    /// <param name="parameters">Parameters to configure the dialog component.</param>
    public static Task<IDialogReference> ShowDialogAsync<TDialog, TData>(this DialogHelper<TDialog> dialogHelper, TData data, IDialogService svc, DialogParameters parameters)
       where TDialog : IDialogContentComponent<TData> => svc.ShowDialogAsync<TDialog, TData>(data, parameters);
}

/// <summary>
/// Helper struct that lets us infer the type of the dialog content
/// </summary>
public readonly struct DialogHelper<TDialog>
   where TDialog : IDialogContentComponent;
