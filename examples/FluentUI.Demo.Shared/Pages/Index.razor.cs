using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.Fast.Components.FluentUI.DesignTokens;

namespace FluentUI.Demo.Shared.Pages;

public partial class Index
{
    [Inject]
    private BaseLayerLuminance BaseLayerLuminance { get; set; } = default!;

    [Inject]
    private BodyFont BodyFont { get; set; } = default!;

    [Inject]
    private StrokeWidth StrokeWidth { get; set; } = default!;

    [Inject]
    private ControlCornerRadius ControlCornerRadius { get; set; } = default!;

    private FluentAnchor ref1 = default!;
    private FluentAnchor ref2 = default!;
    private FluentAnchor ref3 = default!;
    private FluentButton ref4 = default!;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            //Set to dark mode
            await BaseLayerLuminance.SetValueFor(ref1.Element, (float)0.15);

            await BodyFont.SetValueFor(ref3.Element, "Comic Sans MS");

            //Set 'border' width for ref4
            await StrokeWidth.SetValueFor(ref4.Element, 7);
            //And change conrner radius as well
            await ControlCornerRadius.SetValueFor(ref4.Element, 15);

            StateHasChanged();
        }

    }

    public async Task OnClick()
    {
        //Remove the accent color
        await StrokeWidth.DeleteValueFor(ref4.Element);
    }
}