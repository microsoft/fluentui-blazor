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

    /// <inheritdoc />
    [Parameter]
    public virtual string? Id { get; set; }

    /// <inheritdoc />
    [Parameter]
    public virtual string? Class { get; set; }

    /// <inheritdoc />
    [Parameter]
    public virtual string? Style { get; set; }

    /// <inheritdoc />
    [Parameter]
    public virtual string? Margin { get; set; }

    /// <inheritdoc />
    [Parameter]
    public virtual string? Padding { get; set; }

    /// <inheritdoc />
    [Parameter]
    public virtual object? Data { get; set; }

    /// <inheritdoc />
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
        await JSModule.DisposeAsync(jsModule);
    }
}
