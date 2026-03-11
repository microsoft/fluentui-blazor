// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents the result of a Toast.
/// </summary>
public class ToastResult<TContent>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ToastResult{TContent}"/> class.
    /// </summary>
    /// <param name="content"></param>
    /// <param name="cancelled"></param>
    protected internal ToastResult(TContent? content, bool cancelled)
    {
        Value = content;
        Cancelled = cancelled;
    }

    /// <summary>
    /// Gets the content of the Toast result.
    /// </summary>
    public TContent? Value { get; }

    /// <summary>
    /// Gets a value indicating whether the Toast was cancelled.
    /// </summary>
    public bool Cancelled { get; }

    /// <summary>
    /// Creates a Toast result with the specified content.
    /// </summary>
    /// <typeparam name="T">Type of the content.</typeparam>
    /// <param name="result">The content of the Toast result.</param>
    /// <returns>The Toast result.</returns>
    public static ToastResult Ok<T>(T result) => new(result, cancelled: false);

    /// <summary>
    /// Creates a Toast result with the specified content.
    /// </summary>
    /// <returns>The Toast result.</returns>
    public static ToastResult Ok() => Ok<object?>(result: null);

    /// <summary>
    /// Creates a Toast result with the specified content.
    /// </summary>
    /// <param name="result">The content of the Toast result.</param>
    /// <returns>The Toast result.</returns>
    public static ToastResult Cancel<T>(T result) => new(result, cancelled: true);

    /// <summary>
    /// Creates a Toast result with the specified content.
    /// </summary>
    /// <returns>The Toast result.</returns>
    public static ToastResult Cancel() => Cancel<object?>(result: null);
}

/// <summary>
/// Represents the result of a Toast.
/// </summary>
public class ToastResult : ToastResult<object>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ToastResult{TContent}"/> class.
    /// </summary>
    /// <param name="content"></param>
    /// <param name="cancelled"></param>
    protected internal ToastResult(object? content, bool cancelled) : base(content, cancelled)
    {
    }

    /// <summary>
    /// Gets the content of the Toast result.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetValue<T>()
    {
        if (Value is T variable)
        {
            return variable;
        }

        return default(T)!;
    }
}
