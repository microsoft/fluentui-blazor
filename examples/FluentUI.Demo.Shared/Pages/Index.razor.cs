using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.Fast.Components.FluentUI.DesignTokens;

namespace FluentUI.Demo.Shared.Pages;

public partial class Index
{
    [Inject]
    private BaseLayerLuminance BaseLayerLuminance { get; set; } = default!;

    [Inject]
    private BaseHeightMultiplier BaseHeightMultiplier { get; set; } = default!;

    [Inject]
    private ControlCornerRadius ControlCornerRadius { get; set; } = default!;

    private FluentAnchor ref1 = default!;
    private FluentAnchor ref2 = default!;
    private FluentAnchor ref3 = default!;
    private FluentButton ref4 = default!;

    private int? theValueBeforeDelete;
    private int? theValueAfterDelete;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await BaseLayerLuminance.SetValueFor(ref1.Element, 0);

            //Enabling this line below will generate an error because no default is set
            //await DesignTokens.BaseHeightMultiplier.SetValueFor(ref2.Element);

            //await DesignTokens.BaseHeightMultiplier.WithDefault(25).SetValueFor(ref3.Element);
            await BaseHeightMultiplier.WithDefault(25).SetValueFor(ref3.Element);



            theValueBeforeDelete = await BaseHeightMultiplier.GetValueFor(ref4.Element);

            await BaseHeightMultiplier.SetValueFor(ref4.Element, 52);

            await ControlCornerRadius.SetValueFor(ref4.Element, 15);


            StateHasChanged();
        }

    }

    public async Task OnClick()
    {
        await BaseHeightMultiplier.DeleteValueFor(ref4.Element);

        theValueAfterDelete = await BaseHeightMultiplier.GetValueFor(ref4.Element);
    }
}