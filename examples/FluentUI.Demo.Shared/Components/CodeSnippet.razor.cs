// --------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
// --------------------------------------------------------------

namespace FluentUI.Demo.Shared.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.JSInterop;

/// <summary />
public partial class CodeSnippet
{
    [Inject]
    private IJSRuntime _js { get; set; } = default!;

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [Parameter]
    public string Language { get; set; } = "language-razor";

    public string Id { get; } = Identifier.NewId();

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                await _js.InvokeVoidAsync("HighlightCode", Id);
            }
            catch
            {
            }
        }
    }
}
