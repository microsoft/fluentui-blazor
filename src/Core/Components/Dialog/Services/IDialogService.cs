using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public interface IDialogService
{
    /// <summary>
    /// A event that will be invoked when showing a dialog with a custom component
    /// </summary>
    public event Action<IDialogReference, Type?, DialogParameters, object>? OnShow;
    public event Func<IDialogReference, Type?, DialogParameters, object, Task<IDialogReference>>? OnShowAsync;

    public event Action<string, DialogParameters>? OnUpdate;
    public event Func<string, DialogParameters, Task<IDialogReference>>? OnUpdateAsync;

    public event Action<IDialogReference, DialogResult>? OnDialogCloseRequested;

    void ShowSplashScreen(object receiver, Func<DialogResult, Task> callback, DialogParameters<SplashScreenContent> parameters);

    void ShowSplashScreen<T>(object receiver, Func<DialogResult, Task> callback, DialogParameters<SplashScreenContent> parameters)
        where T : IDialogContentComponent<SplashScreenContent>;

    void ShowSplashScreen(Type component, object receiver, Func<DialogResult, Task> callback, DialogParameters<SplashScreenContent> parameters);


    void ShowSuccess(string message, string? title = null);

    void ShowWarning(string message, string? title = null);

    void ShowError(string message, string? title = null);

    void ShowInfo(string message, string? title = null);

    void ShowConfirmation(object receiver, Func<DialogResult, Task> callback, string message, string primaryText = "Yes", string secondaryText = "No", string? title = null);

    void ShowMessageBox(DialogParameters<MessageBoxContent> parameters);

    void ShowPanel<T, TData>(DialogParameters<TData> parameters)
        where T : IDialogContentComponent<TData>
        where TData : class;

    void ShowPanel<TData>(Type component, DialogParameters<TData> parameters)
        where TData : class;

    void ShowDialog<T, TData>(DialogParameters<TData> parameters)
        where T : IDialogContentComponent<TData>
        where TData : class;

    void ShowDialog<TData>(Type component, TData data, DialogParameters parameters)
        where TData : class;

    void UpdateDialog<TContent>(string id, DialogParameters<TContent> parameters)
        where TContent : class;

    // Async methods
    Task<IDialogReference> ShowSplashScreenAsync(object receiver, Func<DialogResult, Task> callback, DialogParameters<SplashScreenContent> parameters);

    Task<IDialogReference> ShowSplashScreenAsync(DialogParameters<SplashScreenContent> parameters);

    Task<IDialogReference> ShowSplashScreenAsync<T>(object receiver, Func<DialogResult, Task> callback, DialogParameters<SplashScreenContent> parameters)
        where T : IDialogContentComponent<SplashScreenContent>;

    Task<IDialogReference> ShowSplashScreenAsync<T>(DialogParameters<SplashScreenContent> parameters)
        where T : IDialogContentComponent<SplashScreenContent>;

    Task<IDialogReference> ShowSplashScreenAsync(Type component, object receiver, Func<DialogResult, Task> callback, DialogParameters<SplashScreenContent> parameters);

    Task<IDialogReference> ShowSplashScreenAsync(Type component, DialogParameters<SplashScreenContent> parameters);


    Task<IDialogReference> ShowSuccessAsync(string message, string? title = null);

    Task<IDialogReference> ShowWarningAsync(string message, string? title = null);

    Task<IDialogReference> ShowErrorAsync(string message, string? title = null);

    Task<IDialogReference> ShowInfoAsync(string message, string? title = null);

    Task<IDialogReference> ShowConfirmationAsync(object receiver, Func<DialogResult, Task> callback, string message, string primaryText = "Yes", string secondaryText = "No", string? title = null);

    Task<IDialogReference> ShowConfirmationAsync(string message, string primaryText = "Yes", string secondaryText = "No", string? title = null);

    Task<IDialogReference> ShowMessageBoxAsync(DialogParameters<MessageBoxContent> parameters);


    Task<IDialogReference> ShowPanelAsync<T, TData>(DialogParameters<TData> parameters)
        where T : IDialogContentComponent<TData>
        where TData : class;

    Task<IDialogReference> ShowPanelAsync<TData>(Type component, DialogParameters<TData> parameters)
        where TData : class;

    Task<IDialogReference> ShowDialogAsync<T, TData>(DialogParameters<TData> parameters)
        where T : IDialogContentComponent<TData>
        where TData : class;

    Task<IDialogReference> ShowDialogAsync<TData>(Type component, TData data, DialogParameters parameters)
        where TData : class;

    Task<IDialogReference> UpdateDialogAsync<TContent>(string id, DialogParameters<TContent> parameters)
        where TContent : class;

    public EventCallback<DialogResult> CreateDialogCallback(object receiver, Func<DialogResult, Task> callback);

    Task CloseAsync(DialogReference dialog);

    Task CloseAsync(DialogReference dialog, DialogResult result);
}
