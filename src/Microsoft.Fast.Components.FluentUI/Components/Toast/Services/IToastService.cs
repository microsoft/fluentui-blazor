namespace Microsoft.Fast.Components.FluentUI
{
    public interface IToastService
    {
        event Action<bool>? OnClearAll;
        event Action<ToastIntent, bool>? OnClearIntent;
        event Action? OnClearQueue;
        event Action<ToastIntent>? OnClearQueueIntent;
        event Action<Type?, object, ToastParameters>? OnShow;
        event Action<string?, object, Action<ToastParameters>>? OnUpdate;
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
        void ShowSuccess(string title, int? timeout = null, string? topAction = null, object? receiver = null, Func<ToastResult, Task>? callback = null);
        void ShowWarning(string title, int? timeout = null, string? topAction = null, object? receiver = null, Func<ToastResult, Task>? callback = null);
        void ShowError(string title, int? timeout = null, string? topAction = null, object? receiver = null, Func<ToastResult, Task>? callback = null);
        void ShowInfo(string title, int? timeout = null, string? topAction = null, object? receiver = null, Func<ToastResult, Task>? callback = null);
        void ShowProgress(string title, int? timeout = null, string? topAction = null, object? receiver = null, Func<ToastResult, Task>? callback = null);
        void ShowUpload(string title, int? timeout = null, string? topAction = null, object? receiver = null, Func<ToastResult, Task>? callback = null);
        void ShowDownload(string title, int? timeout = null, string? topAction = null, object? receiver = null, Func<ToastResult, Task>? callback = null);
        void ShowEvent(string title, int? timeout = null, string? topAction = null, object? receiver = null, Func<ToastResult, Task>? callback = null);
        void ShowMention(string title, int? timeout = null, string? topAction = null, object? receiver = null, Func<ToastResult, Task>? callback = null);
        void ShowCustom(string title, int? timeout = null, string? topAction = null, object? receiver = null, Func<ToastResult, Task>? callback = null, (string Name, Color Color, IconVariant Variant)? icon = null);
        void ShowConfirmationToast(ToastParameters<ConfirmationToastContent> parameters);


        //Communication toasts.
        void ShowCommunicationToast(ToastParameters<CommunicationToastContent> parameters);

        //Progress toasts.
        void ShowProgressToast(ToastParameters<ProgressToastContent> parameters);

        // No type given, defaults to ConfirmationToast with timeout set by <see cref="FluentToastContainer"/>.
        //void ShowToast(ToastIntent intent, string title, Action<ToastAction>? action = null, int? timout = null);
        void ShowToast(ToastIntent intent, string title, int? timout = null, string? topAction = null, object? receiver = null, Func<ToastResult, Task>? callback = null);


        //void ShowToast<TToastContent>(Type? toastComponent, ToastParameters<TToastContent> data, Action<ToastParameters>? settings = null)
        //    where TToastContent : class;

        void ShowToast<TToastContent>(Type? toastComponent, ToastParameters parameters, TToastContent toastContent)
            where TToastContent : class;

        void ShowToast<T, TToastContent>(ToastParameters<TToastContent> parameters)
            where T : IToastContentComponent<TToastContent>
            where TToastContent : class;

        void UpdateToast<TToastContent>(string id, ToastParameters<TToastContent> parameters, Action<ToastParameters>? settings = null)
            where TToastContent : class;

        void CloseToast(string id);
    }
}