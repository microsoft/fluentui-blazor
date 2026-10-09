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
    /// Invokes a JavaScript function that returns void, ignoring the exceptions handled by <see cref="TryInvokeFluentVoidAsync"/>.
    /// </summary>
    /// <param name="jsRuntime"></param>
    /// <param name="identifier"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static async ValueTask InvokeFluentVoidAsync(this IJSRuntime jsRuntime, string identifier, params object?[] args)
    {
        await jsRuntime.TryInvokeFluentVoidAsync(identifier, args);
    }

    /// <summary>
    /// Attempts to invoke a JavaScript function that returns void.
    /// </summary>
    /// <param name="jsRuntime">The JavaScript runtime.</param>
    /// <param name="identifier">The JavaScript function identifier.</param>
    /// <param name="args">The arguments passed to the function.</param>
    /// <returns>
    /// <see langword="true"/> if the invocation completes normally; <see langword="false"/> if
    /// <see cref="JSDisconnectedException"/>, <see cref="OperationCanceledException"/>, or
    /// <see cref="InvalidOperationException"/> is caught.
    /// </returns>
    /// <remarks>All other exceptions propagate to the caller.</remarks>
    public static async ValueTask<bool> TryInvokeFluentVoidAsync(this IJSRuntime jsRuntime, string identifier, params object?[] args)
    {
        try
        {
            await jsRuntime.InvokeVoidAsync(identifier, args);
            return true;
        }
        catch (Exception ex) when (ex is JSDisconnectedException ||
                                   ex is OperationCanceledException ||
                                   ex is InvalidOperationException)
        {
            return false;
        }
    }

    /// <summary>
    /// Invokes a JavaScript function that returns a value, ignoring the exceptions handled by <see cref="TryInvokeFluentAsync{T}"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="jsRuntime"></param>
    /// <param name="identifier"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static async ValueTask<T?> InvokeFluentAsync<[DynamicallyAccessedMembers(
        DynamicallyAccessedMemberTypes.PublicConstructors |
        DynamicallyAccessedMemberTypes.PublicFields |
        DynamicallyAccessedMemberTypes.PublicProperties)] T>(this IJSRuntime jsRuntime, string identifier, params object?[] args)
    {
        var result = await jsRuntime.TryInvokeFluentAsync<T>(identifier, args);
        return result.Value;
    }

    /// <summary>
    /// Attempts to invoke a JavaScript function that returns a value.
    /// </summary>
    /// <typeparam name="T">The type of the returned value.</typeparam>
    /// <param name="jsRuntime">The JavaScript runtime.</param>
    /// <param name="identifier">The JavaScript function identifier.</param>
    /// <param name="args">The arguments passed to the function.</param>
    /// <returns>
    /// A tuple containing <c>Success</c> and <c>Value</c>. Success is <see langword="true"/> if the invocation
    /// completes normally, even when the returned value is <see langword="default"/>. If
    /// <see cref="JSDisconnectedException"/>, <see cref="OperationCanceledException"/>, or
    /// <see cref="InvalidOperationException"/> is caught, Success is <see langword="false"/>
    /// and Value is <see langword="default"/>.
    /// </returns>
    /// <remarks>All other exceptions propagate to the caller.</remarks>
    public static async ValueTask<(bool Success, T? Value)> TryInvokeFluentAsync<[DynamicallyAccessedMembers(
        DynamicallyAccessedMemberTypes.PublicConstructors |
        DynamicallyAccessedMemberTypes.PublicFields |
        DynamicallyAccessedMemberTypes.PublicProperties)] T>(this IJSRuntime jsRuntime, string identifier, params object?[] args)
    {
        try
        {
            var value = await jsRuntime.InvokeAsync<T>(identifier, args);
            return (true, value);
        }
        catch (Exception ex) when (ex is JSDisconnectedException ||
                                   ex is OperationCanceledException ||
                                   ex is InvalidOperationException)
        {
            return (false, default);
        }
    }
}