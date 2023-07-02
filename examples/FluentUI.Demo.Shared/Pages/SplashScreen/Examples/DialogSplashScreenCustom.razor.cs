using global::Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;

namespace FluentUI.Demo.Shared.Pages.SplashScreen.Examples
{
    public partial class DialogSplashScreenCustom
    {
        private void OpenSplashCustom()
        {
            DemoLogger.WriteLine($"Open custom splashscreen for 7 seconds");
            DialogParameters<SplashScreenData> parameters = new()
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
            DialogService.ShowSplashScreen<CustomSplashScreen>(this, HandleCustomSplash, parameters);
        }

        private async Task HandleCustomSplash(DialogResult result)
        {
            await Task.Run(() => DemoLogger.WriteLine($"Custom splash closed"));
        }
    }
}