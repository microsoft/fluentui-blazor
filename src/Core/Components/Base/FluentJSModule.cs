// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Base class to manage the JavaScript function from the FluentUI Blazor components.
/// </summary>
public class FluentJSModule : IAsyncDisposable
{
    private readonly IFluentComponentBase _component;
    private IJSObjectReference? _jsModule;
    private bool _disposalClaimed;

    /// <summary>
    /// Gets the root path for the JavaScript files.
    /// </summary>
    internal const string JAVASCRIPT_ROOT = "./_content/Microsoft.FluentUI.AspNetCore.Components/Components/";

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentJSModule"/> class.
    /// </summary>
    /// <param name="jsRuntime"></param>
    /// <param name="component">The component that owns the JavaScript module.</param>
    public FluentJSModule(IJSRuntime jsRuntime, IFluentComponentBase component)
    {
        ArgumentNullException.ThrowIfNull(component);
        _component = component;
        JSRuntime = jsRuntime;
    }

    /// <summary>
    /// Gets or sets a reference to the JavaScript runtime.
    /// This property is injected by the Blazor framework.
    /// </summary>
    protected virtual IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary>
    /// Gets a value indicating whether an imported JavaScript module reference is present.
    /// </summary>
    /// <remarks>
    /// This does not indicate whether the component or module has been disposed.
    /// Use <see cref="TryImportJavaScriptModuleAsync"/> to check whether initialization can continue.
    /// </remarks>
    public bool Imported => _jsModule is not null;

    /// <summary>
    /// Gets the imported JavaScript module reference.
    /// </summary>
    /// <remarks>
    /// Await <see cref="TryImportJavaScriptModuleAsync"/> in
    /// <see cref="Microsoft.AspNetCore.Components.ComponentBase.OnAfterRenderAsync"/>
    /// and check that it returns <see langword="true"/> before using the reference.
    /// </remarks>
    public IJSObjectReference ObjectReference => _jsModule ?? throw new InvalidOperationException("The JavaScript module has not been imported.");

    /// <summary>
    /// Imports the JavaScript module and cleans up a late import if the owning component has been disposed.
    /// </summary>
    /// <param name="file">The path of the JavaScript module to import.</param>
    /// <returns><see langword="true"/> if the component is still active; otherwise, <see langword="false"/>.</returns>
    public async Task<bool> TryImportJavaScriptModuleAsync(string file)
    {
        await ImportJavaScriptModuleAsync(file);
        if (_component.IsDisposed)
        {
            // Only claim a late module if component disposal had no module to clean up.
            // Otherwise component-specific cleanup may still be using it.
            if (TryClaimDisposal())
            {
                await DisposeAsync();
            }

            return false;
        }

        return true;
    }

    internal bool TryClaimDisposal()
    {
        if (!Imported || _disposalClaimed)
        {
            return false;
        }

        _disposalClaimed = true;
        return true;
    }

    /// <summary>
    /// Invoke the JavaScript runtime to import the JavaScript module.
    /// </summary>
    /// <param name="file">Name of the JavaScript file to import (e.g. JAVASCRIPT_ROOT + "Button/FluentButton.razor.js").</param>
    /// <returns></returns>
    internal async Task<IJSObjectReference> ImportJavaScriptModuleAsync(string file)
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
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Dispose the <see cref="ObjectReference"/> object.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [ExcludeFromCodeCoverage]
    protected virtual async ValueTask DisposeAsync(IJSObjectReference? jsModule)
    {
        if (jsModule != null)
        {
            try
            {
                await jsModule.DisposeAsync();
            }
            catch (Exception ex) when (ex is JSDisconnectedException ||
                                       ex is OperationCanceledException)
            {
                // The JSRuntime side may routinely be gone already if the reason we're disposing is that
                // the client disconnected. This is not an error.
            }
        }
    }
}
