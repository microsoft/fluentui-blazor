using global::Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;

namespace FluentUI.Demo.Shared.Pages.SplashScreen.Examples
{
    public partial class DialogSplashScreenDefault
    {
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
            DialogService.ShowSplashScreen(this, HandleDefaultSplash, parameters);
        }

        private async Task HandleDefaultSplash(DialogResult result)
        {
            await Task.Run(() => DemoLogger.WriteLine($"Default splash closed"));
        }
    }
}