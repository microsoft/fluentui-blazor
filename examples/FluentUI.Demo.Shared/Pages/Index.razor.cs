using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.JSInterop;

namespace FluentUI.Demo.Shared.Pages;

public partial class Index
{
    [Inject]
    private DesignTokens? DesignTokens { get; set; }

    [Inject]
    private IJSRuntime? JSRuntime { get; set; }

    private FluentAnchor anchorRef = default!;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (DesignTokens is not null)
        {
            await DesignTokens.BaseHeightMultiplier.SetValueFor("#secondanchor", 52);
            await DesignTokens.BaseLayerLuminance.SetValueFor(".bigbutton", 0);

            await DesignTokens.BaseHeightMultiplier.WithDefault(25).SetValueFor(anchorRef.Element);

            //int x = await DesignTokens.BaseHeightMultiplier.GetValueFor(anchorRef.Element);

            //await DesignTokens.BaseHeightMultiplier.DeleteValueFor(anchorRef.Element);
            //await DesignTokens.BaseHeightMultiplier.SetValueFor(anchorRef.Element);

            //Create your own DesingToken
            //DesignToken<string> specialColor = new(JSRuntime!, "special-color");
            //await specialColor.SetValueFor("#secondanchor", "green");
        }
        await base.OnAfterRenderAsync(firstRender);
    }
}