using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.Fast.Components.FluentUI.DesignTokens;
using Microsoft.JSInterop;

namespace FluentUI.Demo.Shared
{
    public partial class MainLayout
    {
        [Inject]
        private IJSRuntime JSRuntime { get; set; } = default!;

        [Inject]
        private BaseLayerLuminance BaseLayerLuminance { get; set; } = default!;

        ElementReference container;

        ErrorBoundary? errorBoundary;

        LocalizationDirection dir;
        float baseLayerLuminance = 1;

        public async Task SwitchDirection()
        {
            dir = dir == LocalizationDirection.rtl ? LocalizationDirection.ltr : LocalizationDirection.rtl;
            await JSRuntime.InvokeVoidAsync("switchDirection", dir.ToString());
        }

        public void SwitchTheme()
        {
            baseLayerLuminance = baseLayerLuminance == 0.15f ? 0.98f : 0.15f;
        }

        protected override void OnParametersSet()
        {
            errorBoundary?.Recover();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {

            if (!firstRender)
            {
                await BaseLayerLuminance.SetValueFor(container, baseLayerLuminance);
                //await DesignTokens.Direction.SetValueFor(container, dir.ToString());
            }
        }
    }
}