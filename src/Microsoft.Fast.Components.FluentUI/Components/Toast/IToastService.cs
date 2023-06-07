namespace Microsoft.Fast.Components.FluentUI
{
    public interface IToastService
    {
        event Action<bool>? OnClearAll;
        event Action<ToastIntent, bool>? OnClearIntent;
        event Action? OnClearQueue;
        event Action<ToastIntent>? OnClearQueueIntent;
        event Action<Type, ToastParameters, Action<ToastSettings>?>? OnShow;

        void ClearAll(bool includeQueue = true);
        void ClearAvatarToasts(bool includeQueue = true);
        void ClearCustomIntentToasts(bool includeQueue = true);
        void ClearDownloadToasts(bool includeQueue = true);
        void ClearErrorToasts(bool includeQueue = true);
        void ClearEventToasts(bool includeQueue = true);
        void ClearInfoToasts(bool includeQueue = true);
        void ClearIntent(ToastIntent intent, bool includeQueue = true);
        void ClearProgressToasts(bool includeQueue = true);
        void ClearQueue();
        void ClearQueueAvatarToasts();
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
        void ShowAvatar(string title, Action<ToastAction>? action = null, Action<ToastSettings>? settings = null);
        void ShowCustom(string title, Action<ToastAction>? action = null, Action<ToastSettings>? settings = null);
        void ShowDownload(string title, Action<ToastAction>? action = null, Action<ToastSettings>? settings = null);
        void ShowError(string title, Action<ToastAction>? action = null, Action<ToastSettings>? settings = null);
        void ShowEvent(string title, Action<ToastAction>? action = null, Action<ToastSettings>? settings = null);
        void ShowInfo(string title, Action<ToastAction>? action = null, Action<ToastSettings>? settings = null);
        void ShowProgress(string title, Action<ToastAction>? action = null, Action<ToastSettings>? settings = null);
        void ShowSuccess(string title, Action<ToastAction>? action = null, Action<ToastSettings>? settings = null);
        void ShowToast(ToastIntent intent, string title, Action<ToastAction>? action = null, Action<ToastSettings>? settings = null);
        void ShowToast(Type toastComponent, ToastIntent intent, string title, Action<ToastAction>? action = null, Action<ToastSettings>? settings = null);
        void ShowToast(Type toastComponent, ToastParameters parameters, Action<ToastSettings>? settings = null);
        void ShowToast<TComponent>(ToastIntent intent, string title, Action<ToastAction>? action = null, Action<ToastSettings>? settings = null) where TComponent : IToastContentComponent;
        void ShowToast<TComponent>(ToastParameters parameters, Action<ToastSettings>? settings = null) where TComponent : IToastContentComponent;
        void ShowUpload(string title, Action<ToastAction>? action = null, Action<ToastSettings>? settings = null);
        void ShowWarning(string title, Action<ToastAction>? action = null, Action<ToastSettings>? settings = null);
    }
}