// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Interface for ToastReference
/// </summary>
public interface IToastInstance
{
    /// <summary>
    /// Gets the unique identifier for the Toast.
    /// If this value is not set in the <see cref="ToastOptions"/>, a new identifier is generated.
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Gets the index of the Toast (sequential number).
    /// </summary>
    long Index { get; }

    /// <summary>
    /// Gets the options used to configure the Toast.
    /// </summary>
    ToastOptions Options { get; }

    /// <summary>
    /// Gets the result of the Toast.
    /// </summary>
    Task<ToastResult> Result { get; }

    /// <summary>
    /// Closes the Toast with a Cancel result.
    /// </summary>
    /// <returns></returns>
    Task CancelAsync();

    /// <summary>
    /// Closes the Toast with the specified result.
    /// </summary>
    /// <returns></returns>
    Task CloseAsync();

    /// <summary>
    /// Closes the Toast with the specified result.
    /// </summary>
    /// <param name="result">Result to close the Toast with.</param>
    /// <returns></returns>
    Task CloseAsync(ToastResult result);

    /// <summary>
    /// Closes the Toast with the specified result.
    /// </summary>
    /// <param name="result">Result to close the Toast with.</param>
    /// <returns></returns>
    Task CloseAsync<T>(T result);

    /// <summary>
    /// Updates the toast options while the toast is shown.
    /// </summary>
    /// <param name="update">The action that mutates the current options.</param>
    /// <returns></returns>
    Task UpdateAsync(Action<ToastOptions> update);
}
