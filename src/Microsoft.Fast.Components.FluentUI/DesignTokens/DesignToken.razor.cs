using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

public partial class DesignToken<T> : ComponentBase, IDesignToken<T>, IAsyncDisposable
{
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    [Inject]
    private IConfiguration Configuration { get; set; } = default!;

    private Lazy<Task<IJSObjectReference>>? _jsModuleTask;
    private Reference Target { get; set; } = new Reference();

    private T? _defaultValue;

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
    public DesignToken()
    {

    }

    /// <summary>
    /// Constructs an instance of a DesignToken.
    /// </summary>
    public DesignToken(IJSRuntime jsRuntime, IConfiguration configuration)
    {
        JSRuntime = jsRuntime;
        Configuration = configuration;

        Initialize();
    }

    /// <inheritdoc/>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && Value is not null)
        {
            Initialize();

            await SetValueFor(Target.Current, Value);
            StateHasChanged();
        }
    }

    private void Initialize()
    {
        string scriptSource = Configuration["FluentWebComponentsScriptSource"] ?? "https://cdn.jsdelivr.net/npm/@fluentui/web-components/dist/web-components.min.js";

        _jsModuleTask = new(() => JSRuntime.InvokeAsync<IJSObjectReference>(
            "import", scriptSource).AsTask());
    }

    /// <summary>
    /// Sets the default value of this token
    /// </summary>
    public DesignToken<T> WithDefault(T value)
    {
        _defaultValue = value;
        return this;
    }

    //ToDo Create method
    ///// <summary>
    ///// Create a new token
    ///// </summary>
    ///// /// <param name="name">The name of the Design Token</param>
    //public async ValueTask<DesignToken<T>> Create(string name)
    //{
    //    IJSObjectReference module = await _jsModuleTask!.Value;
    //    await module.InvokeAsync<DesignToken<T>>("DesignToken.create", name);
    //    return this;
    //}

    /// <summary>
    /// Sets the value of the for the associated <see cref="ElementReference"/> to the supplied value
    /// </summary>
    /// <param name="element">the associated <see cref="ElementReference"/></param>
    /// <param name="value">the value to set</param>
    /// <returns></returns>
    public async ValueTask SetValueFor(ElementReference element, T value)
    {
        IJSObjectReference module = await _jsModuleTask!.Value;
        await module.InvokeVoidAsync(Name + ".setValueFor", element, value);
    }

    /// <summary>
    /// Deletes the set value for the associated <see cref="ElementReference"/>
    /// </summary>
    /// <param name="element">the associated <see cref="ElementReference"/></param>
    /// <returns></returns>
    public async ValueTask DeleteValueFor(ElementReference element)
    {
        IJSObjectReference module = await _jsModuleTask!.Value;
        await module.InvokeVoidAsync(Name + ".deleteValueFor", element);
    }

    /// <summary>
    /// Gets the set value for the associated <see cref="ElementReference"/>
    /// </summary>
    /// <param name="element">the associated <see cref="ElementReference"/></param>
    /// <returns>the value</returns>
    public async ValueTask<T> GetValueFor(ElementReference element)
    {
        IJSObjectReference module = await _jsModuleTask!.Value;
        return await module.InvokeAsync<T>(Name + ".getValueFor", element);
    }

    /// <summary>
    /// Convert a hex color string to a value the DesignToken can work with
    /// </summary>
    /// <returns>the value</returns>
    public async ValueTask<object> ParseColorHex(string color)
    {
        IJSObjectReference module = await _jsModuleTask!.Value;
        return await module.InvokeAsync<object>("parseColorHexRGB", color);
    }


    [SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "Not needed")]
    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_jsModuleTask is not null && _jsModuleTask.IsValueCreated)
            {
                IJSObjectReference module = await _jsModuleTask.Value;
                await module.DisposeAsync();
            }
        }
        catch (JSDisconnectedException)
        {
            // The JSRuntime side may routinely be gone already if the reason we're disposing is that
            // the client disconnected. This is not an error.
        }
    }
}
