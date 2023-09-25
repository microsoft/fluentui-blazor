using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.JSInterop;

namespace FluentUI.Demo.Shared.Components;

public partial class SiteSettings
{
    private IDialogReference? _dialog;
    
    [Inject]
    private GlobalState GlobalState { get; set; } = default!;

    private async Task OpenSiteSettingsAsync()
    {
        DemoLogger.WriteLine($"Open site settings");
        _dialog = await DialogService.ShowPanelAsync<SiteSettingsPanel>(GlobalState, new DialogParameters<GlobalState>()
        {
            ShowTitle = true,
            Title = "Site settings",
            Content = GlobalState,
            Alignment = HorizontalAlignment.Right,
            PrimaryAction = "OK",
            SecondaryAction = null,
            ShowDismiss = true
        });

        DialogResult result = await _dialog.Result;
        HandlePanel(result);
    }

    private void HandlePanel(DialogResult result)
    {
        if (result.Cancelled)
        {
            DemoLogger.WriteLine($"Site settings panel dismissed");
            return;
        }

        if (result.Data is not null)
        {
            GlobalState? state = result.Data as GlobalState;

            GlobalState.SetDirection(state!.Dir);
            GlobalState.SetLuminance(state.Luminance);
            GlobalState.SetColor(state!.Color);


            DemoLogger.WriteLine($"Site settings panel closed");
            return;
        }
    }
}