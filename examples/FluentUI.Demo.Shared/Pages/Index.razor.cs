using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.JSInterop;

namespace FluentUI.Demo.Shared.Pages;

public partial class Index
{
    [Inject]
    private IJSRuntime? JSRuntime { get; set; }

    private FluentAnchor anchorRef = default!;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        DesignToken<int> dt = new(JSRuntime!, "baseHeightMultiplier");
        await dt.SetValueFor("#secondanchor", 52);

        DesignToken<float> dt2 = new(JSRuntime!, "baseLayerLuminance");
        await dt2.SetValueFor(".bigbutton", 0);

        DesignToken<int> dt3 = new(JSRuntime!, "baseHeightMultiplier");
        await dt3.SetValueFor(anchorRef.Element, 25);

        await base.OnAfterRenderAsync(firstRender);
    }
}