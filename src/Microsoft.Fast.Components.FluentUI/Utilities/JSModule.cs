using System.Diagnostics.CodeAnalysis;
using Microsoft.JSInterop;
using static Microsoft.Fast.Components.FluentUI.Utilities.LinkerFlags;


namespace Microsoft.Fast.Components.FluentUI.Utilities;

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
    private readonly Task<IJSObjectReference> moduleTask;

    // On construction, we start loading the JS module
    protected JSModule(IJSRuntime js, string moduleUrl)
        => moduleTask = js.InvokeAsync<IJSObjectReference>("import", moduleUrl).AsTask();

    // Methods for invoking exports from the module
    protected async ValueTask InvokeVoidAsync(string identifier, params object[]? args)
        => await (await moduleTask).InvokeVoidAsync(identifier, args);
    protected async ValueTask<T> InvokeAsync<[DynamicallyAccessedMembers(JsonSerialized)] T>(string identifier, params object[]? args)
        => await (await moduleTask).InvokeAsync<T>(identifier, args);


    // On disposal, we release the JS module
    [SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "Not needed")]
    public async ValueTask DisposeAsync()
    {
        try
        {
            await (await moduleTask).DisposeAsync();
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
}

