using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.JSInterop;

namespace FluentUI.Demo.Shared.Pages;

public partial class Index
{
    [Inject]
    private IJSRuntime? jsRuntime { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        DesignToken<int>? dt = new(jsRuntime!, "baseHeightMultiplier");
        await dt.SetValueFor("#secondanchor", 52);

        DesignToken<float>? dt2 = new(jsRuntime!, "baseLayerLuminance");
        await dt2.SetValueFor(".bigbutton", 0);

        await base.OnAfterRenderAsync(firstRender);
    }
}