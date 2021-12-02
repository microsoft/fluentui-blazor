using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.JSInterop;

namespace FluentUI.Demo.Shared.Pages
{
    public partial class Index : ComponentBase
    {
        [Inject] IJSRuntime? JSRuntime { get; set; }
        DesignToken<string> specialColor = new DesignToken<string>("special-color");
        ElementReference ancestor;
        FluentAnchor? decendant;
        protected override void OnInitialized()
        {
            //specialColor.SetValueFor(ancestor, "#FFFFFF");
            //specialColor.SetValueFor(decendant!, "#F7F7F7");
            //DesignToken<float> baseLayerLuminance = new();
            //baseLayerLuminance.SetValueFor(ancestor, 0);

            base.OnInitialized();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            DesignToken<int> baseHeightMultiplier = new();
            await baseHeightMultiplier.SetValueFor(ancestor, 48);
            await base.OnAfterRenderAsync(firstRender);
        }
    }
}