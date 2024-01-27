using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components.DesignTokens;

public partial class DesignToken<T> : ComponentBase, IDesignToken<T>, IAsyncDisposable
{
    private IJSObjectReference _jsModule = default!;

    private Reference Target { get; set; } = new();

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    //private T? _defaultValue;

    /// <summary>
    /// Gets the name of this design token 
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Gets or sets the value of the design token
    /// </summary>
    [Parameter]
    public T? Value { get; set; }

    /// <summary>
    /// Gets or sets the content to apply this design token on
    /// </summary>
    [Parameter]
    public RenderFragment<Reference>? ChildContent { get; set; }

    /// <summary>
    /// Constructs an instance of a DesignToken.
    /// </summary>
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(Swatch))]

    public DesignToken()
    {

    }

    /// <summary>
    /// Constructs an instance of a DesignToken.
    /// </summary>
    public DesignToken(IJSRuntime jsRuntime)
    {
        JSRuntime = jsRuntime;
    }

    /// <inheritdoc/>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await InitJSReferenceAsync();

            if (Value is not null)
            {
                await SetValueFor(Target.Current, Value);
                StateHasChanged();
            }
        }
    }

    private async Task InitJSReferenceAsync()
    {
        _jsModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Microsoft.FluentUI.AspNetCore.Components/Microsoft.FluentUI.AspNetCore.Components.lib.module.js");
    }

    /// <summary>
    /// Sets the default value of this token
    /// </summary>
    public async ValueTask<DesignToken<T>> WithDefault(T value)
    {
        await InitJSReferenceAsync();
        await _jsModule.InvokeVoidAsync(Name + ".withDefault", value);
        return this;
    }

    /// <summary>
    /// Create a new token
    /// </summary>
    /// <param name="name">The name of the Design Token</param>
    public async ValueTask<DesignToken<T>> Create(string name)
    {
        await InitJSReferenceAsync();
        return await _jsModule.InvokeAsync<DesignToken<T>>("DesignToken.create", name);
    }

    /// <summary>
    /// Sets the value of the for the associated <see cref="ElementReference"/> to the supplied value
    /// </summary>
    /// <param name="element">the associated <see cref="ElementReference"/></param>
    /// <param name="value">the value to set</param>
    /// <returns></returns>
    public async ValueTask SetValueFor(ElementReference element, T value)
    {
        await InitJSReferenceAsync();
        await _jsModule.InvokeVoidAsync(Name + ".setValueFor", element, value);
    }

    /// <summary>
    /// Deletes the set value for the associated <see cref="ElementReference"/>
    /// </summary>
    /// <param name="element">the associated <see cref="ElementReference"/></param>
    /// <returns></returns>
    public async ValueTask DeleteValueFor(ElementReference element)
    {
        await InitJSReferenceAsync();
        await _jsModule.InvokeVoidAsync(Name + ".deleteValueFor", element);
    }

    /// <summary>
    /// Gets the set value for the associated <see cref="ElementReference"/>
    /// </summary>
    /// <param name="element">the associated <see cref="ElementReference"/></param>
    /// <returns>the value</returns>
    public async ValueTask<T> GetValueFor(ElementReference element)
    {
        await InitJSReferenceAsync();
        return await _jsModule.InvokeAsync<T>(Name + ".getValueFor", element);
    }

    /// <summary>
    /// Convert a hex color string to a value the DesignToken can work with
    /// </summary>
    /// <returns>the value</returns>
    [SuppressMessage("Style", "VSTHRD200:Use `Async` suffix for async methods", Justification = "#vNext: To update in the next version")]
    public async ValueTask<object> ParseColorHex(string color)
    {
        await InitJSReferenceAsync();
        return await _jsModule.InvokeAsync<object>("parseColorHexRGB", color);
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_jsModule is not null)
            {
                await _jsModule.DisposeAsync();
            }

        }
        catch (JSDisconnectedException)
        {
            // The JSRuntime side may routinely be gone already if the reason we're disposing is that
            // the client disconnected. This is not an error.
        }
    }
}
