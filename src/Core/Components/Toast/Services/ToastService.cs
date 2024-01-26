using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

public class ToastService : IToastService

{
    /// <summary>
    /// A event that will be invoked when showing a toast with a custom component
    /// </summary>
    public event Action<Type?, ToastParameters, object>? OnShow;
    public event Action<string, ToastParameters>? OnUpdate;
    public event Action<string>? OnClose;

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

    private static (Icon Icon, Color Color)? GetIntentIcon(ToastIntent intent)
    {
        return intent switch
        {
            ToastIntent.Success => (new CoreIcons.Filled.Size24.CheckmarkCircle(), Color.Success),
            ToastIntent.Warning => (new CoreIcons.Filled.Size24.Warning(), Color.Warning),
            ToastIntent.Error => (new CoreIcons.Filled.Size24.DismissCircle(), Color.Error),
            ToastIntent.Info => (new CoreIcons.Filled.Size24.Info(), Color.Info),
            ToastIntent.Progress => (new CoreIcons.Regular.Size24.Flash(), Color.Neutral),
            ToastIntent.Upload => (new CoreIcons.Regular.Size24.ArrowUpload(), Color.Neutral),
            ToastIntent.Download => (new CoreIcons.Regular.Size24.ArrowDownload(), Color.Neutral),
            ToastIntent.Event => (new CoreIcons.Regular.Size24.CalendarLtr(), Color.Neutral),
            ToastIntent.Mention => (new CoreIcons.Regular.Size24.Person(), Color.Neutral),
            ToastIntent.Custom => null,
            _ => throw new InvalidOperationException()
        };
    }

    private static ToastParameters<ConfirmationToastContent> BuildConfirmationData(ToastIntent intent, string title, int? timeout = null, string? topAction = null, EventCallback<ToastResult>? callback = null, (Icon Value, Color Color)? icon = null)
    {
        return new()
        {
            Intent = intent,
            Title = title,
            Icon = icon ?? GetIntentIcon(intent),
            TopCTAType = callback is null ? ToastTopCTAType.Dismiss : ToastTopCTAType.Action,
            TopAction = topAction,
            OnTopAction = callback,
            Timeout = timeout,
        };
    }

    /// <summary>
    /// Shows a simple succes confirmation toast.
    /// Only shows icon, title and close button or action.
    /// </summary>
    /// <param name="title">Text to display on the toast</param>
    /// <param name="timeout">Duration toast is shown</param>
    /// <param name="topAction">Text to use for the TopAction</param>
    /// <param name="callback">Callback to invoke when TopAction is clicked</param>
    public void ShowSuccess(string title, int? timeout = null, string? topAction = null, EventCallback<ToastResult>? callback = null)
        => ShowConfirmationToast(BuildConfirmationData(ToastIntent.Success, title, timeout, topAction, callback));

    /// <summary>
    /// Shows a simple warning confirmation toast.
    /// Only shows icon, title and close button or action.
    /// </summary>
    /// <param name="title">Text to display on the toast</param>
    /// <param name="timeout">Duration toast is shown</param>
    /// <param name="topAction">Text to use for the TopAction</param>
    /// <param name="callback">Callback to invoke when TopAction is clicked</param>
    public void ShowWarning(string title, int? timeout = null, string? topAction = null, EventCallback<ToastResult>? callback = null)
        => ShowConfirmationToast(BuildConfirmationData(ToastIntent.Warning, title, timeout, topAction, callback));

    /// <summary>
    /// Shows a simple error confirmation toast.
    /// Only shows icon, title and close button or action.
    /// </summary>
    /// <param name="title">Text to display on the toast</param>
    /// <param name="timeout">Duration toast is shown</param>
    /// <param name="topAction">Text to use for the TopAction</param>
    /// <param name="callback">Callback to invoke when TopAction is clicked</param>
    public void ShowError(string title, int? timeout = null, string? topAction = null, EventCallback<ToastResult>? callback = null)
        => ShowConfirmationToast(BuildConfirmationData(ToastIntent.Error, title, timeout, topAction, callback));

    /// <summary>
    /// Shows a simple information confirmation toast.
    /// Only shows icon, title and close button or action.
    /// </summary>
    /// <param name="title">Text to display on the toast</param>
    /// <param name="timeout">Duration toast is shown</param>
    /// <param name="topAction">Text to use for the TopAction</param>
    /// <param name="callback">Callback to invoke when TopAction is clicked</param>
    public void ShowInfo(string title, int? timeout = null, string? topAction = null, EventCallback<ToastResult>? callback = null)
        => ShowConfirmationToast(BuildConfirmationData(ToastIntent.Info, title, timeout, topAction, callback));

    /// <summary>
    /// Shows a simple progress confirmation toast.
    /// Only shows icon, title and close button or action.
    /// </summary>
    /// <param name="title">Text to display on the toast</param>
    /// <param name="timeout">Duration toast is shown</param>
    /// <param name="topAction">Text to use for the TopAction</param>
    /// <param name="callback">Callback to invoke when TopAction is clicked</param>
    public void ShowProgress(string title, int? timeout = null, string? topAction = null, EventCallback<ToastResult>? callback = null)
        => ShowConfirmationToast(BuildConfirmationData(ToastIntent.Progress, title, timeout, topAction, callback));

    /// <summary>
    /// Shows a simple upload confirmation toast.
    /// Only shows icon, title and close button or action.
    /// </summary>
    /// <param name="title">Text to display on the toast</param>
    /// <param name="timeout">Duration toast is shown</param>
    /// <param name="topAction">Text to use for the TopAction</param>
    /// <param name="callback">Callback to invoke when TopAction is clicked</param>
    public void ShowUpload(string title, int? timeout = null, string? topAction = null, EventCallback<ToastResult>? callback = null)
        => ShowConfirmationToast(BuildConfirmationData(ToastIntent.Upload, title, timeout, topAction, callback));

    /// <summary>
    /// Shows a simple download confirmation toast.
    /// Only shows icon, title and close button or action.
    /// </summary>
    /// <param name="title">Text to display on the toast</param>
    /// <param name="timeout">Duration toast is shown</param>
    /// <param name="topAction">Text to use for the TopAction</param>
    /// <param name="callback">Callback to invoke when TopAction is clicked</param>
    public void ShowDownload(string title, int? timeout = null, string? topAction = null, EventCallback<ToastResult>? callback = null)
        => ShowConfirmationToast(BuildConfirmationData(ToastIntent.Download, title, timeout, topAction, callback));

    /// <summary>
    /// Shows a simple event confirmation toast.
    /// Only shows icon, title and close button or action.
    /// </summary>
    /// <param name="title">Text to display on the toast</param>
    /// <param name="timeout">Duration toast is shown</param>
    /// <param name="topAction">Text to use for the TopAction</param>
    /// <param name="callback">Callback to invoke when TopAction is clicked</param>
    public void ShowEvent(string title, int? timeout = null, string? topAction = null, EventCallback<ToastResult>? callback = null)
        => ShowConfirmationToast(BuildConfirmationData(ToastIntent.Event, title, timeout, topAction, callback));

    /// <summary>
    /// Shows a simple mention confirmation toast.
    /// Only shows icon, title and close button or action.
    /// </summary>
    /// <param name="title">Text to display on the toast</param>
    /// <param name="timeout">Duration toast is shown</param>
    /// <param name="topAction">Text to use for the TopAction</param>
    /// <param name="callback">Callback to invoke when TopAction is clicked</param>
    public void ShowMention(string title, int? timeout = null, string? topAction = null, EventCallback<ToastResult>? callback = null)
        => ShowConfirmationToast(BuildConfirmationData(ToastIntent.Mention, title, timeout, topAction, callback));

    /// <summary>
    /// Shows a simple custom confirmation toast.
    /// Only shows icon, title and close button or action.
    /// </summary>
    /// <param name="title">Text to display on the toast</param>
    /// <param name="timeout">Duration toast is shown</param>
    /// <param name="topAction">Text to use for the TopAction</param>
    /// <param name="callback">Callback to invoke when TopAction is clicked</param>
    /// <param name="icon">Custom icon for this toast</param>
    public void ShowCustom(string title, int? timeout = null, string? topAction = null, EventCallback<ToastResult>? callback = null, (Icon Value, Color Color)? icon = null)
        => ShowConfirmationToast(BuildConfirmationData(ToastIntent.Custom, title, timeout, topAction, callback, icon));

    public void ShowConfirmationToast(ToastParameters<ConfirmationToastContent> parameters)
        => ShowToast(null, parameters, parameters.Content);

    public void ShowCommunicationToast(ToastParameters<CommunicationToastContent> parameters)
        => ShowToast(typeof(CommunicationToast), parameters, parameters.Content);

    public void ShowProgressToast(ToastParameters<ProgressToastContent> parameters)
        => ShowToast(typeof(ProgressToast), parameters, parameters.Content);

    /// <summary>
    /// Shows a toast using the supplied parameters
    /// </summary>
    /// <param name="intent">Toast intent to display</param>
    /// <param name="title">Text to display on the toast</param>
    /// <param name="timeout">Duration toast is shown</param>
    /// <param name="topAction">Text to use for the TopAction</param>
    /// <param name="callback">Callback to execute on the top action</param>
    //public void ShowToast(ToastIntent intent, string title, int? timeout = null)
    public void ShowToast(ToastIntent intent, string title, int? timeout = null, string? topAction = null, EventCallback<ToastResult>? callback = null)
        => ShowConfirmationToast(BuildConfirmationData(intent, title, timeout, topAction, callback));

    /// <summary>
    /// Shows the toast with the component type
    /// </summary>
    public void ShowToast<T, TData>(ToastParameters<TData> parameters)
        where T : IToastContentComponent<TData>
        where TData : class
        => ShowToast(typeof(T), parameters, parameters.Content);

    /// <summary>
    /// Shows a toast with the component type as the body,
    /// passing the specified <paramref name="parameters "/> 
    /// </summary>
    /// <param name="component">Type of component to display.</param>
    /// <param name="content">Content to be displayed in the toast.</param>
    /// <param name="parameters">Parameters to configure the toast component.</param>
    public void ShowToast<TContent>(Type? component, ToastParameters parameters, TContent content)
       where TContent : class
    {
        if (component is not null && !typeof(IToastContentComponent).IsAssignableFrom(component))
        {
            throw new ArgumentException($"{component.FullName} must be a Toast Component");
        }

        //Action<ToastParameters> toastSettings = parameters ?? new(x =>
        //{
        //    x.Id = parameters.Id;
        //    x.Title = parameters.Title;
        //    x.Intent = parameters.Intent;
        //    x.TopCTAType = parameters.TopCTAType;
        //    x.TopAction = parameters.TopAction; //EventCallback.Factory.Create(receiver, callback);
        //    x.Timeout = parameters.Timeout;
        //    x.Icon = parameters.Icon ?? GetIntentIcon(parameters.Intent);
        //    x.Timestamp = parameters.Timestamp;
        //    x.PrimaryAction = parameters.PrimaryAction;
        //    x.SecondaryAction = parameters.SecondaryAction;
        //    //x.OnToastResult = parameters.OnToastResult;
        //});

        //OnShow?.Invoke(component, parameters.Content, toastSettings);
        parameters.Icon ??= GetIntentIcon(parameters.Intent);
        OnShow?.Invoke(component, parameters, content);
    }

    /// <summary>
    /// Updates a toast 
    /// </summary>
    /// <param name="id">Id of the toast to update.</param>
    /// <param name="parameters">Parameters to configure the toast component.</param>
    public void UpdateToast<TContent>(string id, ToastParameters<TContent> parameters)
        where TContent : class
    {
        parameters.Icon ??= GetIntentIcon(parameters.Intent);
        OnUpdate?.Invoke(id, parameters);
    }

    public void CloseToast(string id)
        => OnClose?.Invoke(id);

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
    /// Removes all toasts with toast intent Mention
    /// </summary>
    public void ClearMentionToasts(bool includeQueue = true)
        => OnClearIntent?.Invoke(ToastIntent.Mention, includeQueue);

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
    /// Removes all queued toasts with toast intent Mention
    /// </summary>
    public void ClearQueueMentionToasts()
        => OnClearQueueIntent?.Invoke(ToastIntent.Mention);

    /// <summary>
    /// Removes all queued toasts with toast intent Custom
    /// </summary>
    public void ClearQueueCustomIntentToasts()
        => OnClearQueueIntent?.Invoke(ToastIntent.Custom);
}
