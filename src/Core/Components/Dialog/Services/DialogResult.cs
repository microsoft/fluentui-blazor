// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents the result of a dialog.
/// </summary>
public class DialogResult<TContent>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DialogResult{TContent}"/> class.
    /// </summary>
    /// <param name="content"></param>
    /// <param name="cancelled"></param>
    protected internal DialogResult(TContent? content, bool cancelled)
    {
        Value = content;
        Cancelled = cancelled;
    }

    /// <summary>
    /// Gets the content of the dialog result.
    /// </summary>
    public TContent? Value { get; }

    /// <summary>
    /// Gets a value indicating whether the dialog was cancelled.
    /// </summary>
    public bool Cancelled { get; }

    /// <summary>
    /// Creates a dialog result with the specified content.
    /// </summary>
    /// <typeparam name="T">Type of the content.</typeparam>
    /// <param name="result">The content of the dialog result.</param>
    /// <returns>The dialog result.</returns>
    public static DialogResult Ok<T>(T result) => new(result, cancelled: false);

    /// <summary>
    /// Creates a dialog result with the specified content.
    /// </summary>
    /// <returns>The dialog result.</returns>
    public static DialogResult Ok() => Ok<object?>(result: null);

    /// <summary>
    /// Creates a dialog result with the specified content.
    /// </summary>
    /// <param name="result">The content of the dialog result.</param>
    /// <returns>The dialog result.</returns>
    public static DialogResult Cancel<T>(T result) => new(result, cancelled: true);

    /// <summary>
    /// Creates a dialog result with the specified content.
    /// </summary>
    /// <returns>The dialog result.</returns>
    public static DialogResult Cancel() => Cancel<object?>(result: null);
}

/// <summary>
/// Represents the result of a dialog.
/// </summary>
public class DialogResult : DialogResult<object>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DialogResult{TContent}"/> class.
    /// </summary>
    /// <param name="content"></param>
    /// <param name="cancelled"></param>
    protected internal DialogResult(object? content, bool cancelled) : base(content, cancelled)
    {
    }

    /// <summary>
    /// Gets the content of the dialog result.
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
