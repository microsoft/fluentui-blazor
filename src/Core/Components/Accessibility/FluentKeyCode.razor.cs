using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentKeyCode : FluentComponentBase
{
    private const string JAVASCRIPT_FILE = "./_content/Microsoft.FluentUI.AspNetCore.Components/Components/Accessibility/FluentKeyCode.razor.js";
    private DotNetObjectReference<FluentKeyCode>? _dotNetHelper = null;

    public FluentKeyCode()
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary />
    private IJSObjectReference? Module { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public KeyCode[] IncludeOnly { get; set; } = Array.Empty<KeyCode>();

    [Parameter]
    public KeyCode[] Exclude { get; set; } = Array.Empty<KeyCode>();

    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            Module ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
            _dotNetHelper = DotNetObjectReference.Create(this);

            await Module.InvokeVoidAsync("RegisterKeyCode", Id, IncludeOnly, Exclude, _dotNetHelper);
        }
    }

    [JSInvokable]
    public async Task OnKeyDownRaised(int keyCode)
    {
        Console.WriteLine(keyCode);
    }
}

