using global::Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Demo.Shared.Pages.SplashScreen.Examples;

public partial class DialogSplashScreenDefault
{
    private IDialogReference? _dialog;

    private async Task OpenSplashDefaultAsync()
    {
        DemoLogger.WriteLine($"Open default splashscreen for 4 seconds");
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
        };
        _dialog = await DialogService.ShowSplashScreenAsync(parameters);
        DialogResult result = await _dialog.Result;
        await HandleDefaultSplashAsync(result);
    }

    private void OpenSplashDefault()
    {
        DemoLogger.WriteLine($"Open default splashscreen for 4 seconds");
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
        };
        DialogService.ShowSplashScreen(this, HandleDefaultSplashAsync, parameters);
    }

    private async Task HandleDefaultSplashAsync(DialogResult result)
    {
        await Task.Run(() => DemoLogger.WriteLine($"Default splash closed"));
    }
}
