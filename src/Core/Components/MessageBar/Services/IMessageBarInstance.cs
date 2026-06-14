// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Interface for a message bar instance managed by the <see cref="IMessageBarService"/>.
/// </summary>
public interface IMessageBarInstance
{
    /// <summary>
    /// Gets the optional component type rendered for this message bar.
    /// When <see langword="null"/>, the default <see cref="FluentMessageBar"/> is rendered.
    /// </summary>
    internal Type? ComponentType { get; }

    /// <summary>
    /// Gets the unique identifier for the MessageBar. If this value is not set in the <see cref="MessageBarOptions"/>,
    /// a new identifier is generated.
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Gets the index of the MessageBar (sequential number).
    /// </summary>
    long Index { get; }

    /// <summary>
    /// Gets the options used to configure the MessageBar.
    /// </summary>
    MessageBarOptions Options { get; }

    /// <summary>
    /// Gets the result of the MessageBar.
    /// </summary>
    Task<MessageBarResult> Result { get; }

    /// <summary>
    /// Gets the lifecycle status of the MessageBar.
    /// </summary>
    MessageBarLifecycleStatus LifecycleStatus { get; }

    /// <summary>
    /// Closes the MessageBar programmatically.
    /// </summary>
    Task CloseAsync();

    /// <summary>
    /// Closes the MessageBar with the specified result.
    /// </summary>
    /// <param name="result">Result associated with the close action.</param>
    Task CloseAsync(MessageBarResult result);

    /// <summary>
    /// Dismisses the MessageBar (close reason <see cref="MessageBarCloseReason.Dismissed"/>).
    /// </summary>
    Task DismissAsync();

    /// <summary>
    /// Updates the message bar options while the message bar is shown.
    /// </summary>
    /// <param name="update">The action that mutates the current options.</param>
    Task UpdateAsync(Action<MessageBarOptions> update);
}
