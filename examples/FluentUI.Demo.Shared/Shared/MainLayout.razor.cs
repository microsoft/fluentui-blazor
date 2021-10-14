using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.JSInterop;

namespace FluentUI.Demo.Shared
{
    public partial class MainLayout
    {
        [Inject] IJSRuntime? JSRuntime { get; set; }

        FluentDesignSystemProvider fdsp = new();
        LocalizationDirection? dir;
        float? baseLayerLuminance;

        public async Task SwitchDirectionAsync()
        {
            dir = dir == LocalizationDirection.rtl ? LocalizationDirection.ltr : LocalizationDirection.rtl;
            await JSRuntime!.InvokeVoidAsync("switchDirection", dir.ToString());
        }

        public void SwitchTheme()
        {
            baseLayerLuminance = baseLayerLuminance == 0 ? 1 : 0;
        }
    }
}