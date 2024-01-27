using System.Diagnostics.CodeAnalysis;
using Microsoft.JSInterop;
using static Microsoft.FluentUI.AspNetCore.Components.Utilities.LinkerFlags;

namespace Microsoft.FluentUI.AspNetCore.Components.Utilities;

internal static class LinkerFlags
{
    /// <summary>
    /// Flags for a member that is JSON (de)serialized.
    /// </summary>
    public const DynamicallyAccessedMemberTypes JsonSerialized = DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties;

    /// <summary>
    /// Flags for a component
    /// </summary>
    public const DynamicallyAccessedMemberTypes Component = DynamicallyAccessedMemberTypes.All;

    /// <summary>
    /// Flags for a JSInvokable type.
    /// </summary>
    public const DynamicallyAccessedMemberTypes JSInvokable = DynamicallyAccessedMemberTypes.PublicMethods;
}

/// <summary>
/// Helper for loading any JavaScript (ES6) module and calling its exports
/// </summary>
public abstract class JSModule : IAsyncDisposable
{
    private bool _isDisposed = false;
    private readonly Lazy<Task<IJSObjectReference>> _moduleTask;

    // On construction, we start loading the JSRuntime module
    protected JSModule(IJSRuntime js, string moduleUrl)
    {
        //if (!((AspNetCore.Components.Server.Circuits.RemoteJSRuntime)js).IsInitialized)
        //{
        //}

        ArgumentNullException.ThrowIfNull(js);

        if (moduleUrl != null && string.IsNullOrWhiteSpace(moduleUrl))
        {
            throw new ArgumentException("Argument was empty or whitespace.", nameof(moduleUrl));
        }

        _moduleTask = new(js.InvokeAsync<IJSObjectReference>("import", moduleUrl).AsTask());
    }

    // Methods for invoking exports from the module
    protected async ValueTask InvokeVoidAsync(string identifier, params object[]? args)
        => await (await _moduleTask.Value).InvokeVoidAsync(identifier, args);

    protected async ValueTask<T> InvokeAsync<[DynamicallyAccessedMembers(JsonSerialized)] T>(string identifier, params object[]? args)
        => await (await _moduleTask.Value).InvokeAsync<T>(identifier, args);

    // On disposal, we release the JSRuntime module
    public async ValueTask DisposeAsync()
    {
        await DisposeCoreAsync().ConfigureAwait(false);
        GC.SuppressFinalize(this);
    }

    protected virtual async ValueTask DisposeCoreAsync()
    {
        if (_isDisposed)
        {
            return;
        }

        if (_moduleTask.IsValueCreated && !_moduleTask.Value.IsFaulted)
        {
            try
            {
                IJSObjectReference? module = await _moduleTask.Value;
                await module.DisposeAsync().ConfigureAwait(false);
            }
            catch (InvalidOperationException)
            {
                // This can be called too early when using prerendering
            }
            catch (JSDisconnectedException)
            {
                // The JSRuntime side may routinely be gone already if the reason we're disposing is that
                // the client disconnected. This is not an error.
            }
        }
        _isDisposed = true;
    }
}

