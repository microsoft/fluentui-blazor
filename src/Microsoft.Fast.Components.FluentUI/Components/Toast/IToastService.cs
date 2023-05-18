using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public interface IToastService
{
    /// <summary>
    /// A event that will be invoked when showing a toast
    /// </summary>
    event Action<ToastIntent, string, Action<ToastSettings>?> OnShow;

    /// <summary>
    /// A event that will be invoked to clear all toasts
    /// </summary>
    event Action<bool>? OnClearAll;

    /// <summary>
    /// A event that will be invoked to clear toast of specified intent
    /// </summary>
    event Action<ToastIntent, bool>? OnClearToasts;

    /// <summary>
    /// A event that will be invoked to clear custom toast components
    /// </summary>
    event Action<bool>? OnClearCustomToasts;

    /// <summary>
    /// A event that will be invoked when showing a toast with a custom component
    /// </summary>
    event Action<Type, ToastParameters?, Action<ToastSettings>?>? OnShowCustomComponent;

    /// <summary>
    /// A event that will be invoked when showing a toast with a custom component
    /// </summary>
    public event Action<Type, ToastIntent, string, Action<ToastAction>?, Action<ToastSettings>?>? OnShowToastComponent;

    /// <summary>
    /// A event that will be invoked to clear all queued toasts
    /// </summary>
    event Action? OnClearQueue;

    /// <summary>
    /// A event that will be invoked to clear queue toast of specified intent
    /// </summary>
    event Action<ToastIntent>? OnClearQueueToasts;

    /// <summary>
    /// Shows a success confirmation toast 
    /// </summary>
    /// <param name="title">Text to display on the toast</param>
    /// <param name="action">Optional action link for the toast</param>
    /// <param name="settings">Optional settings to configure the toast instance</param>    
    void ShowSuccess(string title, Action<ToastAction>? action = null, Action<ToastSettings>? settings = null);

    /// <summary>
    /// Shows a warning confirmation toast 
    /// </summary>
    /// <param name="title">Text to display on the toast</param>
    /// <param name="action">Optional action link for the toast</param>
    /// <param name="settings">Optional settings to configure the toast instance</param>
    void ShowWarning(string title, Action<ToastAction>? action = null, Action<ToastSettings>? settings = null);

    /// <summary>
    /// Shows an error confirmation toast 
    /// </summary>
    /// <param name="title">Text to display on the toast</param>
    /// <param name="action">Optional action link for the toast</param>
    /// <param name="settings">Optional settings to configure the toast instance</param>
    void ShowError(string title, Action<ToastAction>? action = null, Action<ToastSettings>? settings = null);
    
    /// <summary>
    /// Shows an info confirmation toast 
    /// </summary>
    /// <param name="title">Text to display on the toast</param>
    /// <param name="action">Optional action link for the toast</param>
    /// <param name="settings">Optional settings to configure the toast instance</param>
    void ShowInfo(string title, Action<ToastAction>? action = null, Action<ToastSettings>? settings = null);

    /// <summary>
    /// Shows a progress confirmation toast 
    /// </summary>
    /// <param name="title">Text to display on the toast</param>
    /// <param name="action">Optional action link for the toast</param>
    /// <param name="settings">Optional settings to configure the toast instance</param>
    void ShowProgress(string title, Action<ToastAction>? action = null, Action<ToastSettings>? settings = null);

    /// <summary>
    /// Shows an upload confirmation toast 
    /// </summary>
    /// <param name="title">Text to display on the toast</param>
    /// <param name="action">Optional action link for the toast</param>
    /// <param name="settings">Optional settings to configure the toast instance</param>
    void ShowUpload(string title, Action<ToastAction>? action = null, Action<ToastSettings>? settings = null);

    /// <summary>
    /// Shows a download confirmation toast 
    /// </summary>
    /// <param name="title">Text to display on the toast</param>
    /// <param name="action">Optional action link for the toast</param>
    /// <param name="settings">Optional settings to configure the toast instance</param>
    void ShowDownload(string title, Action<ToastAction>? action = null, Action<ToastSettings>? settings = null);

    /// <summary>
    /// Shows an event confirmation toast 
    /// </summary>
    /// <param name="title">Text to display on the toast</param>
    /// <param name="action">Optional action link for the toast</param>
    /// <param name="settings">Optional settings to configure the toast instance</param>
    void ShowEvent(string title, Action<ToastAction>? action = null, Action<ToastSettings>? settings = null);

    /// <summary>
    /// Shows an avatar confirmation toast 
    /// </summary>
    /// <param name="title">Text to display on the toast</param>
    /// <param name="action">Optional action link for the toast</param>
    /// <param name="settings">Optional settings to configure the toast instance</param>
    void ShowAvatar(string title, Action<ToastAction>? action = null, Action<ToastSettings>? settings = null);

    /// <summary>
    /// Shows a custom confirmation toast 
    /// </summary>
    /// <param name="title">Text to display on the toast</param>
    /// <param name="action">Optional action link for the toast</param>
    /// <param name="settings">Optional settings to configure the toast instance</param>
    void ShowCustom(string title, Action<ToastAction>? action = null, Action<ToastSettings>? settings = null);

    /// <summary>
    /// Shows a toast using the supplied settings
    /// </summary>
    /// <param name="intent">Toast intent to display</param>
    /// <param name="title">Text to display on the toast</param>
    /// <param name="settings">Settings to configure the toast instance</param>
    void ShowToast(ToastIntent intent, string title, Action<ToastSettings>? settings = null);

    /// <summary>
    /// Shows a toast containing the specified <typeparamref name="TComponent"/>.
    /// </summary>
    void ShowToast<TComponent>() where TComponent : IToastComponent;

    /// <summary>
    /// Shows a toast containing a <typeparamref name="TComponent"/> with the specified <paramref name="parameters"/>.
    /// </summary>
    /// <param name="parameters">Key/Value collection of parameters to pass to component being displayed</param>
    void ShowToast<TComponent>(ToastParameters parameters) where TComponent : IToastComponent;

    /// <summary>
    /// Shows a toast containing a <typeparamref name="TComponent"/> with the specified <paramref name="settings"/>.
    /// </summary>
    /// <param name="settings">Settings to configure the toast instance</param>
    void ShowToast<TComponent>(Action<ToastSettings> settings) where TComponent : IToastComponent;

    /// <summary>
    /// Shows a toast containing a <typeparamref name="TComponent"/> with the specified <paramref name="settings" /> and <paramref name="parameters"/>.
    /// </summary>
    /// <param name="parameters">Key/Value collection of parameters to pass to component being displayed</param>
    /// /// <param name="settings">Settings to configure the toast instance</param>
    void ShowToast<TComponent>(ToastParameters parameters, Action<ToastSettings> settings) where TComponent : IToastComponent;

    /// <summary>
    /// Removes all toasts
    /// </summary>
    void ClearAll(bool includeQueue = true);

    /// <summary>
    /// Removes all toasts with a specified <paramref name="toastintent"/>.
    /// </summary>
    void ClearToasts(ToastIntent toastintent, bool includeQueue = true);

    /// <summary>
    /// Removes all toasts with toast intent Success
    /// </summary>
    void ClearSuccessToasts(bool includeQueue = true);

    /// <summary>
    /// Removes all toasts with toast intent warning
    /// </summary>
    void ClearWarningToasts(bool includeQueue = true);

    /// <summary>
    /// Removes all toasts with toast intent Error
    /// </summary>
    void ClearErrorToasts(bool includeQueue = true);

    /// <summary>
    /// Removes all toasts with toast intent Info
    /// </summary>
    void ClearInfoToasts(bool includeQueue = true);

    /// <summary>
    /// Removes all toasts with toast intent Progress
    /// </summary>
    void ClearProgressToasts(bool includeQueue = true);

    /// <summary>
    /// Removes all toasts with toast intent Upload
    /// </summary>
    void ClearUploadToasts(bool includeQueue = true);

    /// <summary>
    /// Removes all toasts with toast intent Download
    /// </summary>
    void ClearDownloadToasts(bool includeQueue = true);

    /// <summary>
    /// Removes all toasts with toast intent Event
    /// </summary>
    void ClearEventToasts(bool includeQueue = true);

    /// <summary>
    /// Removes all toasts with toast intent Avatar
    /// </summary>
    void ClearAvatarToasts(bool includeQueue = true);

    /// <summary>
    /// Removes all toasts with toast intent Custom
    /// </summary>
    void ClearCustomIntentToasts(bool includeQueue = true);

    /// <summary>
    /// Removes all custom toast components
    /// </summary>
    void ClearCustomToasts(bool includeQueue = true);

    /// <summary>
    /// Removes all queued toasts
    /// </summary>
    void ClearQueue();

    /// <summary>
    /// Removes all queued toasts with a specified <paramref name="intent"/>.
    /// </summary>
    void ClearQueueToasts(ToastIntent intent);

    /// <summary>
    /// Removes all queued toasts with toast intent Success
    /// </summary>
    void ClearQueueSuccessToasts();

    /// <summary>
    /// Removes all queued toasts with toast intent warning
    /// </summary>
    void ClearQueueWarningToasts();

    /// <summary>
    /// Removes all queued toasts with toast intent Error
    /// </summary>
    void ClearQueueErrorToasts();

    /// <summary>
    /// Removes all queued toasts with toast intent Info
    /// </summary>
    void ClearQueueInfoToasts();

    /// <summary>
    /// Removes all queued toasts with toast intent Progress
    /// </summary>
    void ClearQueueProgressToasts();

    /// <summary>
    /// Removes all queued toasts with toast intent Upload
    /// </summary>
    void ClearQueueUploadToasts();

    /// <summary>
    /// Removes all queued toasts with toast intent Download
    /// </summary>
    void ClearQueueDownloadToasts();

    /// <summary>
    /// Removes all queued toasts with toast intent Event
    /// </summary>
    void ClearQueueEventToasts();

    /// <summary>
    /// Removes all queued toasts with toast intent Avatar
    /// </summary>
    void ClearQueueAvatarToasts();

    /// <summary>
    /// Removes all queued toasts with toast intent Custom
    /// </summary>
    void ClearQueueCustomIntentToasts();
}
