using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class DialogService
{
    /// <summary>
    /// Shows the standard <see cref="FluentSplashScreen"/> with the given parameters.
    /// </summary>
    /// <param name="receiver">The component that receives the callback</param>
    /// <param name="callback">Name of the callback function</param>
    /// <param name="parameters"><see cref="SplashScreenContent"/> that holds the content to display</param>
    public void ShowSplashScreen(object receiver, Func<DialogResult, Task> callback, DialogParameters<SplashScreenContent> parameters)
        => ShowSplashScreen<FluentSplashScreen>(receiver, callback, parameters);

    /// <summary>
    /// Shows a custom splash screen dialog with the given parameters.
    /// </summary>
    /// <param name="receiver">The component that receives the callback</param>
    /// <param name="callback">Name of the callback function</param>
    /// <param name="parameters"><see cref="SplashScreenContent"/> that holds the content to display</param>
    public void ShowSplashScreen<T>(object receiver, Func<DialogResult, Task> callback, DialogParameters<SplashScreenContent> parameters)
        where T : IDialogContentComponent<SplashScreenContent>
        => ShowSplashScreen(typeof(T), receiver, callback, parameters);

    /// <summary>
    /// Shows a splash screen of the given type with the given parameters.
    /// </summary>
    /// <param name="component">The type of the component to show</param>
    /// <param name="receiver">The component that receives the callback</param>
    /// <param name="callback">Name of the callback function</param>
    /// <param name="parameters"><see cref="SplashScreenContent"/> that holds the content to display</param>
    public void ShowSplashScreen(Type component, object receiver, Func<DialogResult, Task> callback, DialogParameters<SplashScreenContent> parameters)
    {
        DialogParameters dialogParameters = new()
        {
            DialogType = DialogType.SplashScreen,
            Alignment = HorizontalAlignment.Center,
            Modal = false,
            ShowDismiss = false,
            ShowTitle = false,
            PrimaryAction = null,
            SecondaryAction = null,
            Width = parameters.Width ?? "600px",
            Height = parameters.Height ?? "370px",
            DialogBodyStyle = "width: 100%; height: 100%; margin: 0px;",
            AriaLabel = $"{parameters.Content.Title} splashscreen",
            OnDialogResult = EventCallback.Factory.Create(receiver, callback),
        };

#pragma warning disable CS0618 // Type or member is obsolete
        ShowDialog(component, parameters.Content, dialogParameters);
#pragma warning restore CS0618 // Type or member is obsolete
    }

    /// <summary>
    /// Shows the standard <see cref="FluentSplashScreen"/> with the given parameters.
    /// </summary>
    /// <param name="receiver">The component that receives the callback</param>
    /// <param name="callback">Name of the callback function</param>
    /// <param name="parameters"><see cref="SplashScreenContent"/> that holds the content to display</param>
    public async Task<IDialogReference> ShowSplashScreenAsync(object receiver, Func<DialogResult, Task> callback, DialogParameters<SplashScreenContent> parameters)
        => await ShowSplashScreenAsync<FluentSplashScreen>(receiver, callback, parameters);

    /// <summary>
    /// Shows the standard <see cref="FluentSplashScreen"/> with the given parameters.
    /// </summary>
    /// <param name="parameters"><see cref="SplashScreenContent"/> that holds the content to display</param>
    public async Task<IDialogReference> ShowSplashScreenAsync(DialogParameters<SplashScreenContent> parameters)
        => await ShowSplashScreenAsync<FluentSplashScreen>(parameters);

    /// <summary>
    /// Shows a custom splash screen dialog with the given parameters."/>
    /// </summary>
    /// <param name="receiver">The component that receives the callback</param>
    /// <param name="callback">Name of the callback function</param>
    /// <param name="parameters"><see cref="SplashScreenContent"/> that holds the content to display</param>
    public async Task<IDialogReference> ShowSplashScreenAsync<T>(object receiver, Func<DialogResult, Task> callback, DialogParameters<SplashScreenContent> parameters)
        where T : IDialogContentComponent<SplashScreenContent>
        => await ShowSplashScreenAsync(typeof(T), receiver, callback, parameters);

    /// <summary>
    /// Shows a custom splash screen dialog with the given parameters.
    /// </summary>
    /// <param name="parameters"><see cref="SplashScreenContent"/> that holds the content to display</param>
    public async Task<IDialogReference> ShowSplashScreenAsync<T>(DialogParameters<SplashScreenContent> parameters)
        where T : IDialogContentComponent<SplashScreenContent>
        => await ShowSplashScreenAsync(typeof(T), parameters);

    /// <summary>
    /// Shows a splash screen of the given type with the given parameters.
    /// </summary>
    /// <param name="component">The type of the component to show</param>
    /// <param name="receiver">The component that receives the callback</param>
    /// <param name="callback">Name of the callback function</param>
    /// <param name="parameters"><see cref="SplashScreenContent"/> that holds the content to display</param>
    public async Task<IDialogReference> ShowSplashScreenAsync(Type component, object receiver, Func<DialogResult, Task> callback, DialogParameters<SplashScreenContent> parameters)
    {
        DialogParameters dialogParameters = new()
        {
            DialogType = DialogType.SplashScreen,
            Alignment = HorizontalAlignment.Center,
            Modal = false,
            ShowDismiss = false,
            ShowTitle = false,
            PrimaryAction = null,
            SecondaryAction = null,
            Width = parameters.Width ?? "600px",
            Height = parameters.Height ?? "370px",
            DialogBodyStyle = "width: 100%; height: 100%; margin: 0px;",
            AriaLabel = $"{parameters.Content.Title} splashscreen",
            OnDialogResult = EventCallback.Factory.Create(receiver, callback),
        };

        return await ShowDialogAsync(component, parameters.Content, dialogParameters);
    }

    /// <summary>
    /// Shows a splash screen of the given type with the given parameters.
    /// </summary>
    /// <param name="component">The type of the component to show</param>
    /// <param name="parameters"><see cref="SplashScreenContent"/> that holds the content to display</param>
    public async Task<IDialogReference> ShowSplashScreenAsync(Type component, DialogParameters<SplashScreenContent> parameters)
    {
        DialogParameters dialogParameters = new()
        {
            DialogType = DialogType.SplashScreen,
            Alignment = HorizontalAlignment.Center,
            Modal = false,
            ShowDismiss = false,
            ShowTitle = false,
            PrimaryAction = null,
            SecondaryAction = null,
            Width = parameters.Width ?? "600px",
            Height = parameters.Height ?? "370px",
            DialogBodyStyle = "width: 100%; height: 100%; margin: 0px;",
            AriaLabel = $"{parameters.Content.Title} splashscreen",
        };

        return await ShowDialogAsync(component, parameters.Content, dialogParameters);
    }
}
