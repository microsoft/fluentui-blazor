using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public interface IDialogService
{
    /// <summary>
    /// A event that will be invoked when showing a dialog with a custom component
    /// </summary>
    public event Action<Type, object, Action<DialogSettings>?>? OnShow;

    void ShowSplashScreen(object receiver, Func<DialogResult, Task> callback, DialogParameters<SplashScreenData> parameters);

    void ShowSplashScreen<T>(object receiver, Func<DialogResult, Task> callback, DialogParameters<SplashScreenData> parameters)
        where T : IDialogContentComponent<SplashScreenData>;

    void ShowSplashScreen(Type component, object receiver, Func<DialogResult, Task> callback, DialogParameters<SplashScreenData> parameters);


    void ShowSuccess(string message, string? title = null);

    void ShowWarning(string message, string? title = null);

    void ShowError(string message, string? title = null);

    void ShowInfo(string message, string? title = null);

    void ShowConfirmation(object receiver, Func<DialogResult, Task> callback, string message, string primaryText = "Yes", string secondaryText = "No", string? title = null);

    void ShowMessageBox(DialogParameters<MessageBoxData> parameters);

    void ShowPanel<T, TData>(DialogParameters<TData> parameters)
        where T : IDialogContentComponent<TData>
        where TData : class;

    void ShowPanel<TData>(Type component, DialogParameters<TData> parameters)
        where TData : class;

    //void ShowDialog<T, TContent>(DialogParameters<TContent> parameters)
    //    where T : IDialogContentComponent<TContent>
    //    where TContent : class;

    void ShowDialog<T, TData>(DialogParameters<TData> parameters, Action<DialogSettings>? settings = null)
        where T : IDialogContentComponent<TData>
        where TData : class;

    //void ShowDialog<TContent>(Type component, IDialogContentParameters<TContent> parameters, Action<DialogSettings> settings)
    //    where TContent : class;

    void ShowDialog<TData>(Type component, TData data, Action<DialogSettings>? settings = null)
        where TData : class;

    public EventCallback<DialogResult> CreateDialogCallback(object receiver, Func<DialogResult, Task> callback);
}
