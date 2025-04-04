// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
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
    /// Dispose the <see cref="JSModule"/> object.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [ExcludeFromCodeCoverage]
    public virtual async ValueTask DisposeAsync()
    {
        _cachedServices?.DisposeTooltipAsync(this);
        _cachedServices?.Dispose();
        await JSModule.DisposeAsync();
    }

    /// <summary>
    /// Dispose the <paramref name="jsModule"/> object.
    /// </summary>
    /// <param name="jsModule"></param>
    /// <returns></returns>
    [ExcludeFromCodeCoverage]
    protected virtual async ValueTask DisposeAsync(IJSObjectReference? jsModule)
    {
        _cachedServices?.DisposeTooltipAsync(this);
        _cachedServices?.Dispose();
        await JSModule.DisposeAsync(jsModule);
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
