using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public interface IDialogService
{
    /// <summary>
    /// A event that will be invoked when showing a dialog with a custom component
    /// </summary>
    public event Action<Type, DialogParameters, Action<DialogSettings>?>? OnShow;

    void ShowSplashScreen(object receiver, Func<DialogResult, Task> callback, SplashScreenParameters parameters);

    void ShowSplashScreen<T>(object receiver, Func<DialogResult, Task> callback, SplashScreenParameters parameters)
        where T : IDialogContentComponent;

    void ShowSplashScreen(Type component, object receiver, Func<DialogResult, Task> callback, SplashScreenParameters parameters);


    void ShowError(string message, string? title = null);

    void ShowInfo(string message, string? title = null);

    void ShowConfirmation(object receiver, Func<DialogResult, Task> callback, string message, string primaryText = "Yes", string secondaryText = "No", string? title = null);

    void ShowMessageBox(MessageBoxParameters parameters);

    void ShowPanel<T>(DialogParameters parameters)
        where T : IDialogContentComponent;

    void ShowPanel(Type component, DialogParameters parameters);

    void ShowDialog<T>(DialogParameters parameters)
        where T : IDialogContentComponent;

    void ShowDialog<T>(DialogParameters parameters, Action<DialogSettings> settings)
        where T : IDialogContentComponent;

    void ShowDialog(Type component, DialogParameters parameters, Action<DialogSettings> settings);

    public EventCallback<DialogResult> CreateDialogCallback(object receiver, Func<DialogResult, Task> callback);
}
