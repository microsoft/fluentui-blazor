using global::Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;

namespace FluentUI.Demo.Shared.Pages.SplashScreen.Examples
{
    public partial class DialogSplashScreen
    {
        private void OpenSplashDefault()
        {
            DemoLogger.WriteLine($"Open default splashscreen for 4 seconds");
            DialogParameters<SplashScreenData> parameters = new()
            {
                Data = new()
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
            DialogService.ShowSplashScreen(this, HandleDefaultSplash, parameters);
        }

        private void OpenSplashCustom()
        {
            DemoLogger.WriteLine($"Open custom splashscreen for 7 seconds");
            DialogParameters<SplashScreenData> parameters = new()
            {
                Data = new()
                {
                    Title = "Water drinking 101",
                    LoadingText = "Filling the re-useable bottles...",
                    Message = (MarkupString)"Don't drink <strong>too</strong> much water!",
                    Logo = "_content/FluentUI.Demo.Shared/images/Splash_Corporation_logo.png",
                },
                Width = "500px",
                Height = "300px",
            };
            DialogService.ShowSplashScreen<CustomSplashScreen>(this, HandleCustomSplash, parameters);
        }

        private async Task HandleDefaultSplash(DialogResult result)
        {
            await Task.Run(() => DemoLogger.WriteLine($"Default splash closed"));
        }

        private async Task HandleCustomSplash(DialogResult result)
        {
            await Task.Run(() => DemoLogger.WriteLine($"Custom splash closed"));
        }
    }
}