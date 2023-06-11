﻿using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public class ToastService : IToastService

{
    /// <summary>
    /// A event that will be invoked when showing a toast with a custom component
    /// </summary>
    public event Action<Type, ToastParameters, Action<ToastSettings>?>? OnShow;

    /// <summary>
    /// A event that will be invoked when clearing all toasts
    /// </summary>
    public event Action<bool>? OnClearAll;

    /// <summary>
    /// A event that will be invoked when clearing toasts of a certain intent
    /// </summary>
    public event Action<ToastIntent, bool>? OnClearIntent;

    /// <summary>
    /// A event that will be invoked to clear all queued toasts
    /// </summary>
    public event Action? OnClearQueue;

    /// <summary>
    /// A event that will be invoked to clear queued toast of specified intent
    /// </summary>
    public event Action<ToastIntent>? OnClearQueueIntent;

    /// <summary>
    /// Shows a simple succes confirmation toast.
    /// Only shows icon, title and close button or action.
    /// </summary>
    /// <param name="title">Text to display on the toast</param>
    /// <param name="action">Action to use for this toast</param>
    /// <param name="settings">Settings to configure the toast instance</param>
    public void ShowSuccess(string title, Action<ToastAction>? action = null, Action<ToastSettings>? settings = null)
        => ShowToast<ConfirmationToast>(ToastIntent.Success, title, action, settings);

    /// <summary>
    /// Shows a simple warning confirmation toast.
    /// Only shows icon, title and close button or action.
    /// </summary>
    /// <param name="title">Text to display on the toast</param>
    /// <param name="action">Action to use for this toast</param>
    /// <param name="settings">Settings to configure the toast instance</param>
    public void ShowWarning(string title, Action<ToastAction>? action = null, Action<ToastSettings>? settings = null)
        => ShowToast<ConfirmationToast>(ToastIntent.Warning, title, action, settings);

    /// <summary>
    /// Shows a simple error confirmation toast.
    /// Only shows icon, title and close button or action.
    /// </summary>
    /// <param name="title">Text to display on the toast</param>
    /// <param name="action">Action to use for this toast</param>
    /// <param name="settings">Settings to configure the toast instance</param>
    public void ShowError(string title, Action<ToastAction>? action = null, Action<ToastSettings>? settings = null)
        => ShowToast<ConfirmationToast>(ToastIntent.Error, title, action, settings);

    /// <summary>
    /// Shows a simple information confirmation toast.
    /// Only shows icon, title and close button or action.
    /// </summary>
    /// <param name="title">Text to display on the toast</param>
    /// <param name="action">Action to use for this toast</param>
    /// <param name="settings">Settings to configure the toast instance</param>
    public void ShowInfo(string title, Action<ToastAction>? action = null, Action<ToastSettings>? settings = null)
        => ShowToast<ConfirmationToast>(ToastIntent.Info, title, action, settings);

    /// <summary>
    /// Shows a simple progress confirmation toast.
    /// Only shows icon, title and close button or action.
    /// </summary>
    /// <param name="title">Text to display on the toast</param>
    /// <param name="action">Action to use for this toast</param>
    /// <param name="settings">Settings to configure the toast instance</param>
    public void ShowProgress(string title, Action<ToastAction>? action = null, Action<ToastSettings>? settings = null)
        => ShowToast<ConfirmationToast>(ToastIntent.Progress, title, action, settings);

    /// <summary>
    /// Shows a simple upload confirmation toast.
    /// Only shows icon, title and close button or action.
    /// </summary>
    /// <param name="title">Text to display on the toast</param>
    /// <param name="action">Action to use for this toast</param>
    /// <param name="settings">Settings to configure the toast instance</param>
    public void ShowUpload(string title, Action<ToastAction>? action = null, Action<ToastSettings>? settings = null)
        => ShowToast<ConfirmationToast>(ToastIntent.Upload, title, action, settings);

    /// <summary>
    /// Shows a simple download confirmation toast.
    /// Only shows icon, title and close button or action.
    /// </summary>
    /// <param name="title">Text to display on the toast</param>
    /// <param name="action">Action to use for this toast</param>
    /// <param name="settings">Settings to configure the toast instance</param>
    public void ShowDownload(string title, Action<ToastAction>? action = null, Action<ToastSettings>? settings = null)
        => ShowToast<ConfirmationToast>(ToastIntent.Download, title, action, settings);

    /// <summary>
    /// Shows a simple event confirmation toast.
    /// Only shows icon, title and close button or action.
    /// </summary>
    /// <param name="title">Text to display on the toast</param>
    /// <param name="action">Action to use for this toast</param>
    /// <param name="settings">Settings to configure the toast instance</param>
    public void ShowEvent(string title, Action<ToastAction>? action = null, Action<ToastSettings>? settings = null)
        => ShowToast<ConfirmationToast>(ToastIntent.Event, title, action, settings);

    /// <summary>
    /// Shows a simple avatar confirmation toast.
    /// Only shows icon, title and close button or action.
    /// </summary>
    /// <param name="title">Text to display on the toast</param>
    /// <param name="action">Action to use for this toast</param>
    /// <param name="settings">Settings to configure the toast instance</param>
    public void ShowAvatar(string title, Action<ToastAction>? action = null, Action<ToastSettings>? settings = null)
        => ShowToast<ConfirmationToast>(ToastIntent.Avatar, title, action, settings);

    /// <summary>
    /// Shows a simple custom confirmation toast.
    /// Only shows icon, title and close button or action.
    /// </summary>
    /// <param name="title">Text to display on the toast</param>
    /// <param name="action">Action to use for this toast</param>
    /// <param name="settings">Settings to configure the toast instance</param>
    public void ShowCustom(string title, Action<ToastAction>? action = null, Action<ToastSettings>? settings = null)
        => ShowToast<ConfirmationToast>(ToastIntent.Custom, title, action, settings);

    /// <summary>
    /// Shows a toast using the supplied settings
    /// </summary>
    /// <param name="intent">Toast intent to display</param>
    /// <param name="title">Text to display on the toast</param>
    /// <param name="action">Action to use for this toast</param>
    /// <param name="settings">Settings to configure the toast instance</param>
    public void ShowToast(ToastIntent intent, string title, Action<ToastAction>? action = null, Action<ToastSettings>? settings = null)
        => ShowToast<ConfirmationToast>(intent, title, action, settings);

    /// <summary>
    /// Shows a toast using the supplied settings
    /// </summary>
    /// <param name="intent">Toast intent to display</param>
    /// <param name="title">Text to display on the toast</param>
    /// <param name="action">Action to show (instead of close button)</param>
    /// <param name="settings">Settings to configure the toast instance</param>
    public void ShowToast<TComponent>(ToastIntent intent, string title, Action<ToastAction>? action = null, Action<ToastSettings>? settings = null) where TComponent : IToastContentComponent
        => ShowToast(typeof(TComponent), intent, title, action, settings);

    /// <summary>
    /// Shows the toast with the component type
    /// </summary>
    public void ShowToast<TComponent>(ToastParameters parameters, Action<ToastSettings>? settings = null) where TComponent : IToastContentComponent
        => ShowToast(typeof(TComponent), parameters, settings);

    /// <summary>
    /// Shows the specified toast component type />,
    /// </summary>
    /// <param name="toastComponent">Type of toast component to display.</param>
    /// <param name="intent">The intent of the notification.</param>
    /// <param name="title">The title of the notification</param>
    /// <param name="action">The action to use for this toast.</param>
    /// <param name="settings">Settings to configure the toast component.</param>
    public void ShowToast(Type toastComponent, ToastIntent intent, string title, Action<ToastAction>? action = null, Action<ToastSettings>? settings = null)
    {
        if (!typeof(IToastContentComponent).IsAssignableFrom(toastComponent))
        {
            throw new ArgumentException($"{toastComponent.FullName} must be a Toast Component");
        }

        ToastParameters parameters = new()
        {
            Intent = intent,
            Title = title,
            EndContentType = ToastEndContentType.Dismiss
        };

        if (action is not null)
        {
            ToastAction act = new();
            action.Invoke(act);

            parameters.Add("PrimaryAction", act);
            parameters.EndContentType = ToastEndContentType.Action;
        }
        ShowToast(toastComponent, parameters, settings);
    }


    /// <summary>
    /// Shows the toast with the component type />,
    /// passing the specified <paramref name="parameters"/> 
    /// </summary>
    /// <param name="toastComponent">Type of component to display.</param>
    /// <param name="parameters">Key/Value collection of parameters to pass to component being displayed.</param>
    /// <param name="settings">Settings to configure the toast component.</param>
    public void ShowToast(Type toastComponent, ToastParameters parameters, Action<ToastSettings>? settings = null)
    {
        if (!typeof(IComponent).IsAssignableFrom(toastComponent))
        {
            throw new ArgumentException($"{toastComponent.FullName} must be a Blazor Component");
        }

        OnShow?.Invoke(toastComponent, parameters, settings);
    }

    /// <summary>
    /// Removes all toasts
    /// </summary>
    public void ClearAll(bool includeQueue = true)
        => OnClearAll?.Invoke(includeQueue);

    /// <summary>
    /// Removes all toasts with a specified <paramref name="intent"/>.
    /// </summary>
    public void ClearIntent(ToastIntent intent, bool includeQueue = true)
        => OnClearIntent?.Invoke(intent, includeQueue);

    /// <summary>
    /// Removes all toasts with toast intent Success
    /// </summary>
    public void ClearSuccessToasts(bool includeQueue = true)
        => OnClearIntent?.Invoke(ToastIntent.Success, includeQueue);

    /// <summary>
    /// Removes all toasts with toast intent Warning
    /// </summary>
    public void ClearWarningToasts(bool includeQueue = true)
        => OnClearIntent?.Invoke(ToastIntent.Warning, includeQueue);

    /// <summary>
    /// Removes all toasts with toast intent Error
    /// </summary>
    public void ClearErrorToasts(bool includeQueue = true)
        => OnClearIntent?.Invoke(ToastIntent.Error, includeQueue);

    /// <summary>
    /// Removes all toasts with toast intent Info
    /// </summary>
    public void ClearInfoToasts(bool includeQueue = true)
        => OnClearIntent?.Invoke(ToastIntent.Info, includeQueue);

    /// <summary>
    /// Removes all toasts with toast intent Progress
    /// </summary>
    public void ClearProgressToasts(bool includeQueue = true)
        => OnClearIntent?.Invoke(ToastIntent.Progress, includeQueue);

    /// <summary>
    /// Removes all toasts with toast intent Upload
    /// </summary>
    public void ClearUploadToasts(bool includeQueue = true)
        => OnClearIntent?.Invoke(ToastIntent.Upload, includeQueue);

    /// <summary>
    /// Removes all toasts with toast intent Download
    /// </summary>
    public void ClearDownloadToasts(bool includeQueue = true)
        => OnClearIntent?.Invoke(ToastIntent.Download, includeQueue);

    /// <summary>
    /// Removes all toasts with toast intent Event
    /// </summary>
    public void ClearEventToasts(bool includeQueue = true)
        => OnClearIntent?.Invoke(ToastIntent.Event, includeQueue);

    /// <summary>
    /// Removes all toasts with toast intent Avatar
    /// </summary>
    public void ClearAvatarToasts(bool includeQueue = true)
        => OnClearIntent?.Invoke(ToastIntent.Avatar, includeQueue);

    /// <summary>
    /// Removes all toasts with toast intent Custom
    /// </summary>
    public void ClearCustomIntentToasts(bool includeQueue = true)
        => OnClearIntent?.Invoke(ToastIntent.Custom, includeQueue);

    /// <summary>
    /// Removes all queued toasts
    /// </summary>
    /// 
    public void ClearQueue()
        => OnClearQueue?.Invoke();

    /// <summary>
    /// Removes all queued toasts with a specified <paramref name="intent"/>.
    /// </summary>
    public void ClearQueueToasts(ToastIntent intent)
        => OnClearQueueIntent?.Invoke(intent);
    /// <summary>
    /// Removes all queued toasts with toast intent Success
    /// </summary>
    public void ClearQueueSuccessToasts()
        => OnClearQueueIntent?.Invoke(ToastIntent.Success);

    /// <summary>
    /// Removes all queued toasts with toast intent Warning
    /// </summary>
    public void ClearQueueWarningToasts()
        => OnClearQueueIntent?.Invoke(ToastIntent.Warning);

    /// <summary>
    /// Removes all queued toasts with toast intent Error
    /// </summary>
    public void ClearQueueErrorToasts()
        => OnClearQueueIntent?.Invoke(ToastIntent.Error);

    /// <summary>
    /// Removes all queued toasts with toast intent o
    /// </summary>
    public void ClearQueueInfoToasts()
        => OnClearQueueIntent?.Invoke(ToastIntent.Info);

    /// <summary>
    /// Removes all queued toasts with toast intent Progress
    /// </summary>
    public void ClearQueueProgressToasts()
        => OnClearQueueIntent?.Invoke(ToastIntent.Progress);

    /// <summary>
    /// Removes all queued toasts with toast intent Upload
    /// </summary>
    public void ClearQueueUploadToasts()
        => OnClearQueueIntent?.Invoke(ToastIntent.Upload);

    /// <summary>
    /// Removes all queued toasts with toast intent Download
    /// </summary>
    public void ClearQueueDownloadToasts()
        => OnClearQueueIntent?.Invoke(ToastIntent.Download);

    /// <summary>
    /// Removes all queued toasts with toast intent Event
    /// </summary>
    public void ClearQueueEventToasts()
        => OnClearQueueIntent?.Invoke(ToastIntent.Event);

    /// <summary>
    /// Removes all queued toasts with toast intent Avatar
    /// </summary>
    public void ClearQueueAvatarToasts()
        => OnClearQueueIntent?.Invoke(ToastIntent.Avatar);

    /// <summary>
    /// Removes all queued toasts with toast intent Custom
    /// </summary>
    public void ClearQueueCustomIntentToasts()
        => OnClearQueueIntent?.Invoke(ToastIntent.Custom);
}
