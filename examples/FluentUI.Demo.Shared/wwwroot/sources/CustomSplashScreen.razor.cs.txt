using Microsoft.Fast.Components.FluentUI;

namespace FluentUI.Demo.Shared.Pages.Dialog.Examples;

public partial class CustomSplashScreen : FluentSplashScreen
{
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            // Simulation of loading process
            await Task.Delay(7000);

            // Close the dialog
            await Dialog.CloseAsync();
        }
    }
}
