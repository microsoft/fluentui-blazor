using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.JSInterop;

namespace FluentUI.Demo.Shared
{
    public partial class MainLayout
    {
        [Inject] IJSRuntime? JSRuntime { get; set; }

        static FluentDesignSystemProvider fdsp = new();
        static LocalizationDirection? dir = fdsp.Direction;
        static float? baseLayerLuminance = fdsp.BaseLayerLuminance;

        bool darkTheme = baseLayerLuminance == 1;
        bool isRTL = dir == LocalizationDirection.rtl;

        public async Task SwitchDirectionAsync()
        {
            isRTL = !isRTL;

            if (isRTL)
                dir = LocalizationDirection.rtl;
            else
                dir = LocalizationDirection.ltr;

            await JSRuntime!.InvokeVoidAsync("SwitchDirection", dir.Value.ToString());
        }

        public void SwitchTheme()
        {
            darkTheme = !darkTheme;
            baseLayerLuminance = darkTheme ? 0 : 1;
        }
    }
}