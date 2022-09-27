// --------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
// --------------------------------------------------------------

namespace FluentUI.Demo.Shared.Components;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

/// <summary />
public partial class CodeSnippet
{
    private ElementReference codeElement;

    [Inject]
    private IJSRuntime JS { get; set; } = default!;

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [Parameter]
    public string Language { get; set; } = "language-cshtml-razor";

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JS.InvokeVoidAsync("hljs.highlightElement", codeElement);
        }
    }
}
