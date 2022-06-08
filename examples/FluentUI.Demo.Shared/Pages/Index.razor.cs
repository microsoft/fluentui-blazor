using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.Fast.Components.FluentUI.DesignTokens;

namespace FluentUI.Demo.Shared.Pages;

public partial class Index
{
    [Inject]
    private BaseLayerLuminance BaseLayerLuminance { get; set; } = default!;

    [Inject]
    private AccentBaseColor AccentBaseColor { get; set; } = default!;

    [Inject]
    private BodyFont BodyFont { get; set; } = default!;

    [Inject]
    private StrokeWidth StrokeWidth { get; set; } = default!;

    [Inject]
    private ControlCornerRadius ControlCornerRadius { get; set; } = default!;

    private FluentAnchor? ref1;
    private FluentButton? ref2;
    private FluentAnchor? ref3;
    private FluentButton? ref4;

    private FluentButton? cref1;
    private FluentButton? cref2;
    private FluentButton? cref3;
    private FluentButton? cref4;
    private FluentButton? cref5;
    private FluentButton? cref6;
    private FluentButton? cref7;
    private FluentButton? cref8;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            //Set to dark mode
            await BaseLayerLuminance.SetValueFor(ref1!.Element, (float)0.15);

            await AccentBaseColor.SetValueFor(ref2!.Element, "#107C41".ToSwatch());

            await BodyFont.SetValueFor(ref3!.Element, "Comic Sans MS");

            //Set 'border' width for ref4
            await StrokeWidth.SetValueFor(ref4!.Element, 7);
            //And change conrner radius as well
            await ControlCornerRadius.SetValueFor(ref4!.Element, 15);


            await AccentBaseColor.SetValueFor(cref1!.Element, "#D83B01".ToSwatch());
            await AccentBaseColor.SetValueFor(cref2!.Element, "#185ABD".ToSwatch());
            await AccentBaseColor.SetValueFor(cref3!.Element, "#107C41".ToSwatch());
            await AccentBaseColor.SetValueFor(cref4!.Element, "#C43E1C".ToSwatch());
            await AccentBaseColor.SetValueFor(cref5!.Element, "#6264A7".ToSwatch());
            await AccentBaseColor.SetValueFor(cref6!.Element, "#7719AA".ToSwatch());
            await AccentBaseColor.SetValueFor(cref7!.Element, "#03787C".ToSwatch());
            await AccentBaseColor.SetValueFor(cref8!.Element, "#BC1948".ToSwatch());

            StateHasChanged();
        }

    }

    public async Task OnClick()
    {
        //Remove the accent color
        await StrokeWidth.DeleteValueFor(ref4!.Element);
    }
}