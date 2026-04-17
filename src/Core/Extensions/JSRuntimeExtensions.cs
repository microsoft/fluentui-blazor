// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components.Extensions;

/// <summary>
/// Extension methods for <see cref="IJSRuntime"/>.
/// </summary>
internal static class JSRuntimeExtensions
{
    /// <summary>
    /// Invokes the specified JavaScript function and safely ignores any exceptions that occur if the client has already disconnected.
    /// </summary>
    /// <param name="jsRuntime">The JavaScript runtime instance.</param>
    /// <param name="jsFunctionName">The name of the JavaScript function to invoke.</param>
    /// <param name="args">The arguments to pass to the JavaScript function.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public static async Task SafelyInvokeAsync(this IJSRuntime jsRuntime, string jsFunctionName, params object?[]? args)
    {
        try
        {
            await jsRuntime.InvokeVoidAsync(jsFunctionName, args);
        }
        catch (Exception ex) when (ex is JSDisconnectedException ||
                                   ex is OperationCanceledException)
        {
            // Safely ignore if client already disconnected
        }
    }   
}