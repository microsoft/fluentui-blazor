// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.FluentUI.AspNetCore.Components;

#pragma warning disable MA0004 // Use Task.ConfigureAwait

/// <summary>
/// Base class to manage the JavaScript function from the FluentUI Blazor components.
/// </summary>
internal class FluentJSModule : IAsyncDisposable
{
    private IJSObjectReference? _jsModule;

    /// <summary>
    /// Gets the root path for the JavaScript files.
    /// </summary>
    public const string JAVASCRIPT_ROOT = "./_content/Microsoft.FluentUI.AspNetCore.Components/Components/";

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentJSModule"/> class.
    /// </summary>
    /// <param name="jsRuntime"></param>
    public FluentJSModule(IJSRuntime jsRuntime)
    {
        JSRuntime = jsRuntime;
    }

    /// <summary>
    /// Gets or sets a reference to the JavaScript runtime.
    /// This property is injected by the Blazor framework.
    /// </summary>
    protected virtual IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary>
    /// Gets the JavaScript module imported with the <see cref="ImportJavaScriptModuleAsync"/> method.
    /// You need to call this method (in the `OnAfterRenderAsync` method) before using the module.
    /// </summary>
    public IJSObjectReference ObjectReference => _jsModule ?? throw new InvalidOperationException("You must call `ImportJavaScriptModuleAsync` method before accessing the JSModule property.");

    /// <summary>
    /// Invoke the JavaScript runtime to import the JavaScript module.
    /// </summary>
    /// <param name="file">Name of the JavaScript file to import (e.g. JAVASCRIPT_ROOT + "Button/FluentButton.razor.js").</param>
    /// <returns></returns>
    public async Task<IJSObjectReference> ImportJavaScriptModuleAsync(string file)
    {
        _jsModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", file);  // TO ADD: .FormatCollocatedUrl(LibraryConfiguration)
        return _jsModule;
    }

    /// <summary>
    /// Dispose the <see cref="ObjectReference"/> object.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [ExcludeFromCodeCoverage]
    public virtual async ValueTask DisposeAsync()
    {

        await DisposeAsync(_jsModule);

        if (_jsModule != null)
        {
            try
            {
                await _jsModule.DisposeAsync();
            }
            catch (Exception ex) when (ex is JSDisconnectedException ||
                                       ex is OperationCanceledException)
            {
                // The JSRuntime side may routinely be gone already if the reason we're disposing is that
                // the client disconnected. This is not an error.
            }
        }

        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Dispose the <see cref="ObjectReference"/> object.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [ExcludeFromCodeCoverage]
    internal virtual ValueTask DisposeAsync(IJSObjectReference? jsModule)
    {
        return ValueTask.CompletedTask;
    }
}

#pragma warning restore MA0004 // Use Task.ConfigureAwait
