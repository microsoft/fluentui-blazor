// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Interface for a message bar instance managed by the <see cref="INotificationService"/>.
/// </summary>
public interface IMessageBarInstance : INotificationInstance
{
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
    /// Closes the MessageBar with the specified result.
    /// </summary>
    /// <param name="result">Result associated with the close action.</param>
    Task CloseAsync(MessageBarResult result);
}
