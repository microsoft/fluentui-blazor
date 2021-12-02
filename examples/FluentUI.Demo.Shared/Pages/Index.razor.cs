using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.JSInterop;

namespace FluentUI.Demo.Shared.Pages;

public partial class Index
{
    [Inject] IJSRuntime? jsRuntime { get; set; } = default!;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        DesignToken<int> baseHeightMultiplier = new();
        await baseHeightMultiplier.SetValueFor("#secondanchor", 52);
        //await baseHeightMultiplier.SetValueFor(".bigbutton", 48);

        await base.OnAfterRenderAsync(firstRender);
    }
}
