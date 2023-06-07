using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public interface IDialogService
{
    public event Action<IDialogReference> OnDialogInstanceAdded;

    public event Action<IDialogReference, DialogResult> OnDialogCloseRequested;

    Task<IDialogReference> ShowAsync<TComponent>()
        where TComponent : ComponentBase;

    Task<IDialogReference> ShowAsync<TComponent>(string title)
        where TComponent : ComponentBase;

    Task<IDialogReference> ShowAsync<TComponent>(string title, DialogOptions options)
        where TComponent : ComponentBase;

    Task<IDialogReference> ShowAsync<TComponent>(string title, DialogParameters parameters)
        where TComponent : ComponentBase;

    Task<IDialogReference> ShowAsync<TComponent>(string title, DialogParameters parameters, DialogOptions options)
        where TComponent : ComponentBase;

    Task<IDialogReference> ShowAsync(Type component);

    Task<IDialogReference> ShowAsync(Type component, string title);

    Task<IDialogReference> ShowAsync(Type component, string title, DialogOptions options);

    Task<IDialogReference> ShowAsync(Type component, string title, DialogParameters parameters);

    Task<IDialogReference> ShowAsync(Type component, string title, DialogParameters parameters, DialogOptions options);

    Task<IDialogReference> ShowSplashScreenAsync(string title, string subTitle, string? loading = null);

    Task<bool> ShowMessageBoxErrorAsync(string message, string? title = null);

    Task<bool> ShowMessageBoxInformationAsync(string message, string? title = null);

    Task<bool> ShowMessageBoxConfirmationAsync(string message, string? title = null);

    Task<bool> ShowMessageBoxAsync(MessageBoxOptions options);

    Task CloseAsync(DialogReference dialog);

    Task CloseAsync(DialogReference dialog, DialogResult result);
}
