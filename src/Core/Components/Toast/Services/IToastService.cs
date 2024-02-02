using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

public interface IToastService
{
    event Action<bool>? OnClearAll;
    event Action<ToastIntent, bool>? OnClearIntent;
    event Action? OnClearQueue;
    event Action<ToastIntent>? OnClearQueueIntent;
    event Action<Type?, ToastParameters, object>? OnShow;
    event Action<string?, ToastParameters>? OnUpdate;
    event Action<string>? OnClose;

    void ClearAll(bool includeQueue = true);
    void ClearMentionToasts(bool includeQueue = true);
    void ClearCustomIntentToasts(bool includeQueue = true);
    void ClearDownloadToasts(bool includeQueue = true);
    void ClearErrorToasts(bool includeQueue = true);
    void ClearEventToasts(bool includeQueue = true);
    void ClearInfoToasts(bool includeQueue = true);
    void ClearIntent(ToastIntent intent, bool includeQueue = true);
    void ClearProgressToasts(bool includeQueue = true);
    void ClearQueue();
    void ClearQueueMentionToasts();
    void ClearQueueCustomIntentToasts();
    void ClearQueueDownloadToasts();
    void ClearQueueErrorToasts();
    void ClearQueueEventToasts();
    void ClearQueueInfoToasts();
    void ClearQueueProgressToasts();
    void ClearQueueSuccessToasts();
    void ClearQueueToasts(ToastIntent intent);
    void ClearQueueUploadToasts();
    void ClearQueueWarningToasts();
    void ClearSuccessToasts(bool includeQueue = true);
    void ClearUploadToasts(bool includeQueue = true);
    void ClearWarningToasts(bool includeQueue = true);

    // Confirmation toasts.
    void ShowSuccess(string title, int? timeout = null, string? topAction = null, EventCallback<ToastResult>? callback = null);
    void ShowWarning(string title, int? timeout = null, string? topAction = null, EventCallback<ToastResult>? callback = null);
    void ShowError(string title, int? timeout = null, string? topAction = null, EventCallback<ToastResult>? callback = null);
    void ShowInfo(string title, int? timeout = null, string? topAction = null, EventCallback<ToastResult>? callback = null);
    void ShowProgress(string title, int? timeout = null, string? topAction = null, EventCallback<ToastResult>? callback = null);
    void ShowUpload(string title, int? timeout = null, string? topAction = null, EventCallback<ToastResult>? callback = null);
    void ShowDownload(string title, int? timeout = null, string? topAction = null, EventCallback<ToastResult>? callback = null);
    void ShowEvent(string title, int? timeout = null, string? topAction = null, EventCallback<ToastResult>? callback = null);
    void ShowMention(string title, int? timeout = null, string? topAction = null, EventCallback<ToastResult>? callback = null);
    void ShowCustom(string title, int? timeout = null, string? topAction = null, EventCallback<ToastResult>? callback = null, (Icon Value, Color Color)? icon = null);
    void ShowConfirmationToast(ToastParameters<ConfirmationToastContent> parameters);

    //Communication toasts.
    void ShowCommunicationToast(ToastParameters<CommunicationToastContent> parameters);

    //Progress toasts.
    void ShowProgressToast(ToastParameters<ProgressToastContent> parameters);

    // No type given, defaults to ConfirmationToast with timeout set by <see cref="FluentToastContainer"/>.
    //void ShowToast(ToastIntent intent, string title, Action<ToastAction>? action = null, int? timout = null);
    void ShowToast(ToastIntent intent, string title, int? timeout = null, string? topAction = null, EventCallback<ToastResult>? callback = null);

    //void ShowToast<TContent>(Type? toastComponent, ToastParameters<TContent> data, Action<ToastParameters>? settings = null)
    //    where TContent : class;

    void ShowToast<TContent>(Type? toastComponent, ToastParameters parameters, TContent content)
        where TContent : class;

    void ShowToast<T, TContent>(ToastParameters<TContent> parameters)
        where T : IToastContentComponent<TContent>
        where TContent : class;

    void UpdateToast<TContent>(string id, ToastParameters<TContent> parameters)
        where TContent : class;

    void CloseToast(string id);
}
