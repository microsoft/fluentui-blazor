using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI;

internal class DesignTokenManagerJSObjectReference
{
    private const string ScriptPath = "./_content/Microsoft.Fast.Components.FluentUI/designTokenManager.js";
    private readonly IJSRuntime _jsRuntime;
    private Task<IJSObjectReference>? _scriptManager;

    public DesignTokenManagerJSObjectReference(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public Task<IJSObjectReference> ScriptManager =>
        _scriptManager ??= _jsRuntime.InvokeAsync<IJSObjectReference>("import", ScriptPath).AsTask();

    public async ValueTask SetTitleAsync(string title)
    {

        var scriptManager = await ScriptManager;
        await scriptManager.InvokeVoidAsync("setTitle", title);
    }

    public async ValueTask SetValueFor(ElementReference element, object value)
    {
        var scriptManager = await ScriptManager;
        await scriptManager.InvokeVoidAsync("setBaseHeightMultiplier", element, value);
    }
}

