using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.Fast.Components.FluentUI.DesignTokens;
using Microsoft.JSInterop;

namespace FluentUI.Demo.Shared.Pages;

public partial class Index
{
    [Inject]
    private DesignTokens? DesignTokens { get; set; }

    [Inject]
    private IJSRuntime? jsRuntime { get; set; }

    private FluentAnchor ref1 = default!;
    private FluentAnchor ref2 = default!;
    private FluentAnchor ref3 = default!;
    private FluentButton ref4 = default!;

    private int? theValueBeforeDelete;
    private int? theValueAfterDelete;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && DesignTokens is not null)
        {
            await DesignTokens.BaseLayerLuminance.SetValueFor(ref1.Element, 0);

            //Enabling this line below will generate an error because no default is set
            //await DesignTokens.BaseHeightMultiplier.SetValueFor(ref2.Element);

            await DesignTokens.BaseHeightMultiplier.WithDefault(25).SetValueFor(ref3.Element);



            theValueBeforeDelete = await DesignTokens.BaseHeightMultiplier.GetValueFor(ref4.Element);

            await DesignTokens.BaseHeightMultiplier.SetValueFor(ref4.Element, 52);

            //ToDo: Create your own DesingToken 
            //DesignToken<string> specialColor = new(jsRuntime!, "specialColor");
            //await specialColor.SetValueFor(ref3.Element, "#FF0000");
            StateHasChanged();
        }

    }

    public async Task OnClick()
    {
        if (DesignTokens is not null)
        {
            await DesignTokens.BaseHeightMultiplier.DeleteValueFor(ref4.Element);

            theValueAfterDelete = await DesignTokens.BaseHeightMultiplier.GetValueFor(ref4.Element);
        }
    }
}