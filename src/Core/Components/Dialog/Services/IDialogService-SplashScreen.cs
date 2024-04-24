namespace Microsoft.FluentUI.AspNetCore.Components;

public partial interface IDialogService
{
    void ShowSplashScreen(object receiver, Func<DialogResult, Task> callback, DialogParameters<SplashScreenContent> parameters);

    void ShowSplashScreen<T>(object receiver, Func<DialogResult, Task> callback, DialogParameters<SplashScreenContent> parameters)
        where T : IDialogContentComponent<SplashScreenContent>;

    void ShowSplashScreen(Type component, object receiver, Func<DialogResult, Task> callback, DialogParameters<SplashScreenContent> parameters);

    Task<IDialogReference> ShowSplashScreenAsync(object receiver, Func<DialogResult, Task> callback, DialogParameters<SplashScreenContent> parameters);

    Task<IDialogReference> ShowSplashScreenAsync(DialogParameters<SplashScreenContent> parameters);

    Task<IDialogReference> ShowSplashScreenAsync<T>(object receiver, Func<DialogResult, Task> callback, DialogParameters<SplashScreenContent> parameters)
        where T : IDialogContentComponent<SplashScreenContent>;

    Task<IDialogReference> ShowSplashScreenAsync<T>(DialogParameters<SplashScreenContent> parameters)
        where T : IDialogContentComponent<SplashScreenContent>;

    Task<IDialogReference> ShowSplashScreenAsync(Type component, object receiver, Func<DialogResult, Task> callback, DialogParameters<SplashScreenContent> parameters);

    Task<IDialogReference> ShowSplashScreenAsync(Type component, DialogParameters<SplashScreenContent> parameters);

}
