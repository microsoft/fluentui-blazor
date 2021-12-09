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

        DesignToken<int> dt3 = new DesignToken<int>(JSRuntime!, "baseHeightMultiplier").WithDefault(25);
        await dt3.SetValueFor(anchorRef.Element);

        int x = await dt3.GetValueFor(anchorRef.Element);

        await dt3.DeleteValueFor(anchorRef.Element);

        //DesignToken<string> dt4 = new DesignToken<string>(JSRuntime!, "baseHeightMultiplier");
        // Throws error; no DefaultValue set and no Value supplied as parameter
        //await dt4.SetValueFor(anchorRef.Element);

        await base.OnAfterRenderAsync(firstRender);
    }
}