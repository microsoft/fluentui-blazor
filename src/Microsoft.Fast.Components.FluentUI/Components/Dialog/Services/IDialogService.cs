using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public interface IDialogService
{
    /// <summary>
    /// A event that will be invoked when showing a dialog with a custom component
    /// </summary>
    public event Action<Type, DialogParameters, Action<DialogSettings>?>? OnShow;


    //public event Action<DialogInstance> OnDialogInstanceAdded;

    public event Action<string> OnDialogCloseRequested;

    void ShowDialog<T>(string title, DialogParameters parameters, Action<DialogSettings>? settings)
        where T : IDialogContentComponent;

    void ShowDialog(Type component, string title, DialogParameters parameters, Action<DialogSettings>? settings);

    void ShowSplashScreen<T>(object receiver, Func<DialogResult, Task> callback, SplashScreenParameters parameters)
        where T : IDialogContentComponent;

    void ShowSplashScreen(Type component, object receiver, Func<DialogResult, Task> callback, SplashScreenParameters parameters);


    void ShowErrorAsync(string message, string? title = null);

    void ShowInfoAsync(string message, string? title = null);

    void ShowConfirmationAsync(object receiver, Func<DialogResult, Task> callback, string message, string? title = null);

    void ShowMessageBox(MessageBoxParameters parameters);


    //Task CloseAsync(string Id);

    //Task CloseAsync(string Id, DialogResult result);

    public EventCallback<DialogResult> CreateDialogCallback(object receiver, Func<DialogResult, Task> callback);
}
