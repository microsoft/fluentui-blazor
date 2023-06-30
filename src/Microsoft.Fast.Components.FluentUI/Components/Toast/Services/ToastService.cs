using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public class ToastService : IToastService

{
    /// <summary>
    /// A event that will be invoked when showing a toast with a custom component
    /// </summary>
    public event Action<Type?, object, ToastParameters>? OnShow;
    public event Action<string, object, Action<ToastParameters>>? OnUpdate;
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

    private static (string Name, Color Color, IconVariant Variant)? GetIntentIcon(ToastIntent intent)
    {
        return intent switch
        {
            ToastIntent.Success => (FluentIcons.CheckmarkCircle, Color.Success, IconVariant.Filled),
            ToastIntent.Warning => (FluentIcons.Warning, Color.Warning, IconVariant.Filled),
            ToastIntent.Error => (FluentIcons.DismissCircle, Color.Error, IconVariant.Filled),
            ToastIntent.Info => (FluentIcons.Info, Color.Info, IconVariant.Filled),
            ToastIntent.Progress => (FluentIcons.Flash, Color.Neutral, IconVariant.Regular),
            ToastIntent.Upload => (FluentIcons.ArrowUpload, Color.Neutral, IconVariant.Regular),
            ToastIntent.Download => (FluentIcons.ArrowDownload, Color.Neutral, IconVariant.Regular),
            ToastIntent.Event => (FluentIcons.CalendarLTR, Color.Neutral, IconVariant.Regular),
            ToastIntent.Mention => (FluentIcons.Person, Color.Neutral, IconVariant.Regular),
            ToastIntent.Custom => null,
            _ => throw new InvalidOperationException()
        };
    }

    private static ToastParameters<ConfirmationToastContent> BuildConfirmationData(ToastIntent intent, string title, int? timeout = null, string? topAction = null, object? receiver = null, Func<ToastResult, Task>? callback = null, (string Name, Color Color, IconVariant Variant)? icon = null)
    {
        ToastTopCTAType topCTAType = receiver is null ? ToastTopCTAType.Dismiss : ToastTopCTAType.Action;

        return new()
        {
            Intent = intent,
            Title = title,
            Icon = icon ?? GetIntentIcon(intent),
            TopCTAType = topCTAType,
            TopAction = topAction,
            OnTopAction = topCTAType == ToastTopCTAType.Action ? EventCallback.Factory.Create(receiver!, callback!) : null,
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
    /// <param name="receiver">Receiver of the TopAction callback</param>
    /// <param name="callback">Callback to invoke when TopAction is clicked</param>
    public void ShowSuccess(string title, int? timeout = null, string? topAction = null, object? receiver = null, Func<ToastResult, Task>? callback = null)
        => ShowConfirmationToast(BuildConfirmationData(ToastIntent.Success, title, timeout, topAction, receiver, callback));

    /// <summary>
    /// Shows a simple warning confirmation toast.
    /// Only shows icon, title and close button or action.
    /// </summary>
    /// <param name="title">Text to display on the toast</param>
    /// <param name="timeout">Duration toast is shown</param>
    /// <param name="topAction">Text to use for the TopAction</param>
    /// <param name="receiver">Receiver of the TopAction callback</param>
    /// <param name="callback">Callback to invoke when TopAction is clicked</param>
    public void ShowWarning(string title, int? timeout = null, string? topAction = null, object? receiver = null, Func<ToastResult, Task>? callback = null)
        => ShowConfirmationToast(BuildConfirmationData(ToastIntent.Warning, title, timeout, topAction, receiver, callback));

    /// <summary>
    /// Shows a simple error confirmation toast.
    /// Only shows icon, title and close button or action.
    /// </summary>
    /// <param name="title">Text to display on the toast</param>
    /// <param name="timeout">Duration toast is shown</param>
    /// <param name="topAction">Text to use for the TopAction</param>
    /// <param name="receiver">Receiver of the TopAction callback</param>
    /// <param name="callback">Callback to invoke when TopAction is clicked</param>
    public void ShowError(string title, int? timeout = null, string? topAction = null, object? receiver = null, Func<ToastResult, Task>? callback = null)
        => ShowConfirmationToast(BuildConfirmationData(ToastIntent.Error, title, timeout, topAction, receiver, callback));

    /// <summary>
    /// Shows a simple information confirmation toast.
    /// Only shows icon, title and close button or action.
    /// </summary>
    /// <param name="title">Text to display on the toast</param>
    /// <param name="timeout">Duration toast is shown</param>
    /// <param name="topAction">Text to use for the TopAction</param>
    /// <param name="receiver">Receiver of the TopAction callback</param>
    /// <param name="callback">Callback to invoke when TopAction is clicked</param>
    public void ShowInfo(string title, int? timeout = null, string? topAction = null, object? receiver = null, Func<ToastResult, Task>? callback = null)
        => ShowConfirmationToast(BuildConfirmationData(ToastIntent.Info, title, timeout, topAction, receiver, callback));

    /// <summary>
    /// Shows a simple progress confirmation toast.
    /// Only shows icon, title and close button or action.
    /// </summary>
    /// <param name="title">Text to display on the toast</param>
    /// <param name="timeout">Duration toast is shown</param>
    /// <param name="topAction">Text to use for the TopAction</param>
    /// <param name="receiver">Receiver of the TopAction callback</param>
    /// <param name="callback">Callback to invoke when TopAction is clicked</param>
    public void ShowProgress(string title, int? timeout = null, string? topAction = null, object? receiver = null, Func<ToastResult, Task>? callback = null)
        => ShowConfirmationToast(BuildConfirmationData(ToastIntent.Progress, title, timeout, topAction, receiver, callback));

    /// <summary>
    /// Shows a simple upload confirmation toast.
    /// Only shows icon, title and close button or action.
    /// </summary>
    /// <param name="title">Text to display on the toast</param>
    /// <param name="timeout">Duration toast is shown</param>
    /// <param name="topAction">Text to use for the TopAction</param>
    /// <param name="receiver">Receiver of the TopAction callback</param>
    /// <param name="callback">Callback to invoke when TopAction is clicked</param>
    public void ShowUpload(string title, int? timeout = null, string? topAction = null, object? receiver = null, Func<ToastResult, Task>? callback = null)
        => ShowConfirmationToast(BuildConfirmationData(ToastIntent.Upload, title, timeout, topAction, receiver, callback));

    /// <summary>
    /// Shows a simple download confirmation toast.
    /// Only shows icon, title and close button or action.
    /// </summary>
    /// <param name="title">Text to display on the toast</param>
    /// <param name="timeout">Duration toast is shown</param>
    /// <param name="topAction">Text to use for the TopAction</param>
    /// <param name="receiver">Receiver of the TopAction callback</param>
    /// <param name="callback">Callback to invoke when TopAction is clicked</param>
    public void ShowDownload(string title, int? timeout = null, string? topAction = null, object? receiver = null, Func<ToastResult, Task>? callback = null)
        => ShowConfirmationToast(BuildConfirmationData(ToastIntent.Download, title, timeout, topAction, receiver, callback));

    /// <summary>
    /// Shows a simple event confirmation toast.
    /// Only shows icon, title and close button or action.
    /// </summary>
    /// <param name="title">Text to display on the toast</param>
    /// <param name="timeout">Duration toast is shown</param>
    /// <param name="topAction">Text to use for the TopAction</param>
    /// <param name="receiver">Receiver of the TopAction callback</param>
    /// <param name="callback">Callback to invoke when TopAction is clicked</param>
    public void ShowEvent(string title, int? timeout = null, string? topAction = null, object? receiver = null, Func<ToastResult, Task>? callback = null)
        => ShowConfirmationToast(BuildConfirmationData(ToastIntent.Event, title, timeout, topAction, receiver, callback));

    /// <summary>
    /// Shows a simple mention confirmation toast.
    /// Only shows icon, title and close button or action.
    /// </summary>
    /// <param name="title">Text to display on the toast</param>
    /// <param name="timeout">Duration toast is shown</param>
    /// <param name="topAction">Text to use for the TopAction</param>
    /// <param name="receiver">Receiver of the TopAction callback</param>
    /// <param name="callback">Callback to invoke when TopAction is clicked</param>
    public void ShowMention(string title, int? timeout = null, string? topAction = null, object? receiver = null, Func<ToastResult, Task>? callback = null)
        => ShowConfirmationToast(BuildConfirmationData(ToastIntent.Mention, title, timeout, topAction, receiver, callback));

    /// <summary>
    /// Shows a simple custom confirmation toast.
    /// Only shows icon, title and close button or action.
    /// </summary>
    /// <param name="title">Text to display on the toast</param>
    /// <param name="timeout">Duration toast is shown</param>
    /// <param name="topAction">Text to use for the TopAction</param>
    /// <param name="receiver">Receiver of the TopAction callback</param>
    /// <param name="callback">Callback to invoke when TopAction is clicked</param>
    /// <param name="icon">Custom icon for this toast</param>
    public void ShowCustom(string title, int? timeout = null, string? topAction = null, object? receiver = null, Func<ToastResult, Task>? callback = null, (string Name, Color Color, IconVariant Variant)? icon = null)
        => ShowConfirmationToast(BuildConfirmationData(ToastIntent.Custom, title, timeout, topAction, receiver, callback, icon));


    public void ShowConfirmationToast(ToastParameters<ConfirmationToastContent> parameters)
        => ShowToast(null, parameters, parameters.ToastContent);

    public void ShowCommunicationToast(ToastParameters<CommunicationToastContent> parameters)
        => ShowToast(typeof(CommunicationToast), parameters, parameters.ToastContent);

    public void ShowProgressToast(ToastParameters<ProgressToastContent> parameters)
        => ShowToast(typeof(ProgressToast), parameters, parameters.ToastContent);

    /// <summary>
    /// Shows a toast using the supplied parameters
    /// </summary>
    /// <param name="intent">Toast intent to display</param>
    /// <param name="title">Text to display on the toast</param>
    /// <param name="timeout">Duration toast is shown</param>
    /// <param name="topAction">Text to use for the TopAction</param>
    /// <param name="receiver">Componente that receivesthe callback</param>
    /// <param name="callback">Callback to execute on the top action</param>
    //public void ShowToast(ToastIntent intent, string title, int? timeout = null)
    public void ShowToast(ToastIntent intent, string title, int? timeout = null, string? topAction = null, object? receiver = null, Func<ToastResult, Task>? callback = null)
        => ShowConfirmationToast(BuildConfirmationData(intent, title, timeout, topAction, receiver, callback));

    /// <summary>
    /// Shows the toast with the component type
    /// </summary>
    public void ShowToast<T, TData>(ToastParameters<TData> parameters)
        where T : IToastContentComponent<TData>
        where TData : class
        => ShowToast(typeof(T), parameters, parameters.ToastContent);

    /// <summary>
    /// Shows a toast with the component type as the body,
    /// passing the specified <paramref name="parameters "/> 
    /// </summary>
    /// <param name="toastComponent">Type of component to display.</param>
    /// <param name="toastContent">Content to be displayed in the toast.</param>
    /// <param name="parameters">Settings to configure the toast component.</param>
    public void ShowToast<TToastContent>(Type? toastComponent, ToastParameters parameters, TToastContent toastContent)
       where TToastContent : class
    {
        if (toastComponent is not null && !typeof(IToastContentComponent).IsAssignableFrom(toastComponent))
        {
            throw new ArgumentException($"{toastComponent.FullName} must be a Toast Component");
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

        //OnShow?.Invoke(toastComponent, parameters.ToastContent, toastSettings);
        parameters.Icon ??= GetIntentIcon(parameters.Intent);
        OnShow?.Invoke(toastComponent, toastContent, parameters);
    }



    /// <summary>
    /// Updates a toast 
    /// </summary>
    /// <param name="id">Id of the toast to update.</param>
    /// <param name="parameters">Parametes used to construct toast.</param>
    /// <param name="settings">Settings to configure the toast component.</param>
    public void UpdateToast<TData>(string id, ToastParameters<TData> parameters, Action<ToastParameters>? settings = null)
        where TData : class
    {
        Action<ToastParameters> toastSettings = settings ?? new(x =>
        {
            x.Id = parameters.Id;
            x.Title = parameters.Title;
            x.Intent = parameters.Intent;
            x.TopCTAType = parameters.TopCTAType;
            x.TopAction = parameters.TopAction;
            x.Timeout = parameters.Timeout;
            x.Icon = parameters.Icon ?? GetIntentIcon(parameters.Intent);
            x.Timestamp = parameters.Timestamp;
            x.PrimaryAction = parameters.PrimaryAction;
            x.SecondaryAction = parameters.SecondaryAction;
            //x.OnToastResult = parameters.OnToastResult;
        });

        OnUpdate?.Invoke(id, parameters.ToastContent, toastSettings);
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
