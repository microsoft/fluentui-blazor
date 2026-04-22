// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial interface IDialogService
{
    [Obsolete("Use ShowSplashScreenAsync(object, Func, DialogParameters) instead.")]
    void ShowSplashScreen(object receiver, Func<DialogResult, Task> callback, DialogParameters<SplashScreenContent> parameters);

    [Obsolete("Use ShowSplashScreenAsync<T>(object, Func, DialogParameters) instead.")]
    void ShowSplashScreen<T>(object receiver, Func<DialogResult, Task> callback, DialogParameters<SplashScreenContent> parameters)
        where T : IDialogContentComponent<SplashScreenContent>;

    [Obsolete("Use ShowSplashScreenAsync(Type, object, Func, DialogParameters) instead.")]
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
