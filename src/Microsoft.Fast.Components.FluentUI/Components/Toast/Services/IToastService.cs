namespace Microsoft.Fast.Components.FluentUI
{
    public interface IToastService
    {
        event Action<bool>? OnClearAll;
        event Action<ToastIntent, bool>? OnClearIntent;
        event Action? OnClearQueue;
        event Action<ToastIntent>? OnClearQueueIntent;
        event Action<Type?, object, Action<ToastParameters>>? OnShow;
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
        void ShowSuccess(string title, Action<ToastAction>? action = null, int? timeout = null);
        void ShowWarning(string title, Action<ToastAction>? action = null, int? timeout = null);
        void ShowError(string title, Action<ToastAction>? action = null, int? timeout = null);
        void ShowInfo(string title, Action<ToastAction>? action = null, int? timeout = null);
        void ShowProgress(string title, Action<ToastAction>? action = null, int? timeout = null);
        void ShowUpload(string title, Action<ToastAction>? action = null, int? timeout = null);
        void ShowDownload(string title, Action<ToastAction>? action = null, int? timeout = null);
        void ShowEvent(string title, Action<ToastAction>? action = null, int? timeout = null);
        void ShowMention(string title, Action<ToastAction>? action = null, int? timeout = null);
        void ShowCustom(string title, Action<ToastAction>? action = null, int? timeout = null, (string Name, Color Color, IconVariant Variant)? icon = null);
        void ShowConfirmationToast(ToastParameters<ConfirmationToastContent> parameters);


        //Communication toasts.
        void ShowCommunicationToast(ToastParameters<CommunicationToastContent> parameters);

        //Progress toasts.
        void ShowProgressToast(ToastParameters<ProgressToastContent> parameters);

        // No type given, defaults to ConfirmationToast with timeout set by <see cref="FluentToastContainer"/>.
        void ShowToast(ToastIntent intent, string title, Action<ToastAction>? action = null, int? timout = null);

        void ShowToast<TData>(Type? toastComponent, ToastParameters<TData> data, Action<ToastParameters>? settings = null)
            where TData : class;

        void ShowToast<T, TData>(ToastParameters<TData> parameters)
            where T : IToastContentComponent<TData>
            where TData : class;

        void UpdateToast<TData>(string id, ToastParameters<TData> parameters, Action<ToastParameters>? settings = null)
            where TData : class;

        void CloseToast(string id);
    }
}