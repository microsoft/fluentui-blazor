using global::Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Demo.Shared.Pages.SplashScreen.Examples;

public partial class DialogSplashScreenCustom
{
    private IDialogReference? _dialog;
    private async Task OpenSplashCustomAsync()
    {
        DemoLogger.WriteLine($"Open custom splashscreen for 7 seconds");
        DialogParameters<SplashScreenContent> parameters = new()
        {
            Content = new SplashScreenContent()
            {
                Title = "Water drinking 101",
                LoadingText = "Filling the re-useable bottles...",
                Message = (MarkupString)"Don't drink <strong>too</strong> much water!",
                Logo = "_content/FluentUI.Demo.Shared/images/Splash_Corporation_logo.png",
            },
            Width = "500px",
            Height = "300px",
        };
        _dialog = await DialogService.ShowSplashScreenAsync<CustomSplashScreen>(parameters);

        for (var i = 0; i < 5; i++)
        {
            await Task.Delay(1000);
            parameters.Content.LoadingText = $"Filling the re-useable bottles... {i + 1}";
            await DialogService.UpdateDialogAsync(_dialog.Id, parameters);
        }

        DialogResult result = await _dialog.Result;
        await HandleCustomSplashAsync(result);
    }
    private void OpenSplashCustom()
    {
        DemoLogger.WriteLine($"Open custom splashscreen for 7 seconds");
        DialogParameters<SplashScreenContent> parameters = new()
        {
            Content = new()
            {
                Title = "Water drinking 101",
                LoadingText = "Filling the re-useable bottles...",
                Message = (MarkupString)"Don't drink <strong>too</strong> much water!",
                Logo = "_content/FluentUI.Demo.Shared/images/Splash_Corporation_logo.png",
            },
            Width = "500px",
            Height = "300px",
        };
        DialogService.ShowSplashScreen<CustomSplashScreen>(this, HandleCustomSplashAsync, parameters);
    }

    private async Task HandleCustomSplashAsync(DialogResult result)
    {
        await Task.Run(() => DemoLogger.WriteLine($"Custom splash closed"));
    }

}
