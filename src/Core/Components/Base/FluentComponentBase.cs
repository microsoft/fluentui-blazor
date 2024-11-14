// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
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
    private IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary>
    /// 
    /// </summary>
    internal FluentJSModule JSModule => _jsModule ??= new FluentJSModule(JSRuntime);

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
    /// 
    /// </summary>
    /// <param name="jsModule"></param>
    /// <returns></returns>
    protected virtual async ValueTask DisposeAsync(IJSObjectReference? jsModule)
    {
        await JSModule.DisposeAsync(jsModule);
    }
}
