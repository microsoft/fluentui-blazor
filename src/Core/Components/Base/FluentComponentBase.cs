// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Base class for FluentUI Blazor components.
/// </summary>
public abstract class FluentComponentBase : ComponentBase, IAsyncDisposable, IFluentComponentBase
{
    private FluentJSModule? _jsModule;
    private CachedServices? _cachedServices;

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentComponentBase"/> class with the specified configuration.
    /// </summary>
    /// <param name="configuration">The configuration object used to apply default values to the component.</param>
    protected FluentComponentBase(LibraryConfiguration configuration)
    {
        configuration?.DefaultValues.ApplyDefaults(this);
    }

    [Inject]
    private IServiceProvider ServiceProvider { get; set; } = default!;

    /// <summary />
    [Inject]
    protected IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary />
    [Inject]
    protected IFluentLocalizer Localizer { get; set; } = FluentLocalizerInternal.Default;

    /// <summary>
    /// Gets the JavaScript module imported with the <see cref="FluentJSModule.ImportJavaScriptModuleAsync"/> method.
    /// You need to call this method (in the `OnAfterRenderAsync` method) before using the module.
    /// </summary>
    internal FluentJSModule JSModule => _jsModule ??= new FluentJSModule(JSRuntime);

    /// <summary>
    /// Gets the class builder, containing the default margin and padding values.
    /// </summary>
    protected virtual CssBuilder DefaultClassBuilder => new CssBuilder(Class)
        .AddClass(Margin.ConvertSpacing().Class)
        .AddClass(Padding.ConvertSpacing().Class);

    /// <summary>
    /// Gets the style builder, containing the default margin and padding values.
    /// </summary>
    protected virtual StyleBuilder DefaultStyleBuilder => new StyleBuilder(Style)
        .AddStyle("margin", Margin.ConvertSpacing().Style)
        .AddStyle("padding", Padding.ConvertSpacing().Style);

    /// <inheritdoc cref="IFluentComponentBase.Id" />
    [Parameter]
    public virtual string? Id { get; set; }

    /// <inheritdoc cref="IFluentComponentBase.Class" />
    [Parameter]
    public virtual string? Class { get; set; }

    /// <inheritdoc cref="IFluentComponentBase.Style" />
    [Parameter]
    public virtual string? Style { get; set; }

    /// <inheritdoc cref="IFluentComponentBase.Margin" />
    [Parameter]
    public virtual string? Margin { get; set; }

    /// <inheritdoc cref="IFluentComponentBase.Padding" />
    [Parameter]
    public virtual string? Padding { get; set; }

    /// <inheritdoc cref="IFluentComponentBase.Data" />
    [Parameter]
    public virtual object? Data { get; set; }

    /// <inheritdoc cref="IFluentComponentBase.AdditionalAttributes" />
    [Parameter(CaptureUnmatchedValues = true)]
    public virtual IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    /// <summary>
    /// Dispose the current object.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [ExcludeFromCodeCoverage]
    public virtual async ValueTask DisposeAsync()
    {
        if (_jsModule != null && _jsModule.Imported)
        {
            try
            {
                await DisposeAsync(_jsModule.ObjectReference);
            }
            catch (Exception ex) when (ex is JSDisconnectedException ||
                                       ex is OperationCanceledException ||
                                       ex is InvalidOperationException)
            {
                // The JSRuntime side may routinely be gone already if the reason we're disposing is that
                // the client disconnected. This is not an error.
            }
        }

        _cachedServices?.DisposeTooltipAsync(this);
        _cachedServices?.Dispose();
        await JSModule.DisposeAsync();
    }

    /// <summary>
    /// Override this method to call your custom dispose logic, using the <see cref="IJSObjectReference"/> object.
    /// </summary>
    /// <param name="jsModule"></param>
    /// <returns></returns>
    [ExcludeFromCodeCoverage]
    protected virtual ValueTask DisposeAsync(IJSObjectReference jsModule)
    {
        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// Get service of type <typeparamref name="T"/> from the <see cref="IServiceProvider"/> or null if not found.
    /// Keep in mind that this method will cache the service in the component memory for future use.
    /// </summary>
    /// <typeparam name="T">The type of service object to get.</typeparam>
    /// <returns></returns>
    protected virtual T? GetCachedServiceOrNull<T>() => (_cachedServices ??= new CachedServices(ServiceProvider)).GetCachedServiceOrNull<T>();

    /// <summary>
    /// Renders the label in a FluentTooltipProvider.
    /// </summary>
    /// <param name="label"></param>
    /// <returns></returns>
    protected Task RenderTooltipAsync(string? label) => (_cachedServices ??= new CachedServices(ServiceProvider)).RenderTooltipAsync(this, label);
}
