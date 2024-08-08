using global::Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Demo.Shared.Pages.SplashScreen.Examples;

public partial class DialogSplashScreenDefault
{
    private IDialogReference? _dialog;

    private async Task OpenSplashDefaultAsync()
    {
        DemoLogger.WriteLine($"Open default SplashScreen for 4 seconds");
        DialogParameters<SplashScreenContent> parameters = new()
        {
            Content = new()
            {
                DisplayTime = 0,    // See Task.Delay below
                Title = "Core components",
                SubTitle = "Microsoft Fluent UI Blazor library",
                LoadingText = "Loading...",
                Message = (MarkupString)"some <i>extra</i> text <strong>here</strong>",
                Logo = FluentSplashScreen.LOGO,
            },
            PreventDismissOnOverlayClick = true,
            Modal = false,
            Width = "640px",
            Height = "480px",
        };
        _dialog = await DialogService.ShowSplashScreenAsync(parameters);

        var splashScreen = (SplashScreenContent)_dialog.Instance.Content;

        // Simulate a first task
        await Task.Delay(2000);

        // Update the splash screen content and simulate a second task
        splashScreen.UpdateLabels(loadingText: "Second task...");
        await Task.Delay(2000);

        await _dialog.CloseAsync();

        DialogResult result = await _dialog.Result;
        await HandleDefaultSplashAsync(result);
    }

    private void OpenSplashDefault()
    {
        DemoLogger.WriteLine($"Open default SplashScreen for 4 seconds");
        DialogParameters<SplashScreenContent> parameters = new()
        {
            Content = new()
            {
                Title = "Core components",
                SubTitle = "Microsoft Fluent UI Blazor library",
                LoadingText = "Loading...",
                Message = (MarkupString)"some <i>extra</i> text <strong>here</strong>",
                Logo = FluentSplashScreen.LOGO,
            },
            Width = "640px",
            Height = "480px",
            Modal = true,
        };
        DialogService.ShowSplashScreen(this, HandleDefaultSplashAsync, parameters);
    }

    private async Task HandleDefaultSplashAsync(DialogResult result)
    {
        await Task.Run(() => DemoLogger.WriteLine($"Default splash closed"));
    }
}
