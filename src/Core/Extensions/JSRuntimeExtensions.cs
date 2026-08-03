// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Extension methods for <see cref="IJSRuntime"/>.
/// </summary>
internal static class JSRuntimeExtensions
{
    /// <summary>
    /// Invokes a JavaScript function that returns void, and ignores exceptions that occur when the JS runtime is disconnected or the operation is canceled.
    /// </summary>
    /// <param name="jsRuntime"></param>
    /// <param name="identifier"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static async ValueTask InvokeFluentVoidAsync(this IJSRuntime jsRuntime, string identifier, params object?[] args)
    {
        try
        {
            await jsRuntime.InvokeVoidAsync(identifier, args);
        }
        catch (Exception ex) when (ex is JSDisconnectedException ||
                                   ex is OperationCanceledException ||
                                   ex is InvalidOperationException)
        {
            // Ignore exceptions that occur when the JS runtime is disconnected or the operation is canceled.
        }
    }

    /// <summary>
    /// Invokes a JavaScript function that returns a value, and ignores exceptions that occur when the JS runtime is disconnected or the operation is canceled.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="jsRuntime"></param>
    /// <param name="identifier"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static async ValueTask<T> InvokeFluentAsync<[DynamicallyAccessedMembers(
        DynamicallyAccessedMemberTypes.PublicConstructors |
        DynamicallyAccessedMemberTypes.PublicFields |
        DynamicallyAccessedMemberTypes.PublicProperties)] T>(this IJSRuntime jsRuntime, string identifier, params object?[] args)
    {
        try
        {
            return await jsRuntime.InvokeAsync<T>(identifier, args);
        }
        catch (Exception ex) when (ex is JSDisconnectedException ||
                                   ex is OperationCanceledException ||
                                   ex is InvalidOperationException)
        {
            // Ignore exceptions that occur when the JS runtime is disconnected or the operation is canceled.
            return default!;
        }
    }
}