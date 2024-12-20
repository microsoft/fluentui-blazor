// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents the result of a dialog.
/// </summary>
public class DialogResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DialogResult"/> class.
    /// </summary>
    /// <param name="content"></param>
    /// <param name="cancelled"></param>
    protected internal DialogResult(object? content, bool cancelled)
    {
        Value = content;
        Cancelled = cancelled;
    }

    /// <summary>
    /// Gets the content of the dialog result.
    /// </summary>
    public object? Value { get; }

    /// <summary>
    /// Gets a value indicating whether the dialog was cancelled.
    /// </summary>
    public bool Cancelled { get; }

    /// <summary>
    /// Creates a dialog result with the specified content.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="result"></param>
    /// <returns></returns>
    public static DialogResult Ok<T>(T result) => new(result, cancelled: false);

    /// <summary>
    /// Creates a dialog result with the specified content.
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public static DialogResult Cancel(object? content = null) => new(content ?? default, cancelled: true);
}
