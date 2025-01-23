// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FluentUI.Demo.Client.Documentation.Components.Spacing.Examples;

public partial class SpacingComputedBoxes
{
    [Inject]
    public required IJSRuntime JSRuntime { get; set; }

    private IJSObjectReference? JSModule { get; set; }

    [Parameter]
    public string? Margin { get; set; }

    [Parameter]
    public string? Padding { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            JSModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Documentation/Components/Spacing/Examples/SpacingComputedBoxes.razor.js");
        }

        if (JSModule != null)
        {
            var margin = Margin.ConvertSpacing();
            var padding = Padding.ConvertSpacing();

            await JSModule.InvokeVoidAsync(
                "setSpacing",
                string.IsNullOrEmpty(margin.Style) ? margin.Class : margin.Style,
                string.IsNullOrEmpty(padding.Style) ? padding.Class : padding.Style);
        }
    }
}
