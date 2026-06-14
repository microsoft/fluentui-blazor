// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Interface for the MessageBar service.
/// </summary>
public interface IMessageBarService : IFluentServiceBase<IMessageBarInstance>
{
    /// <summary>
    /// Closes the message bar with the specified result.
    /// </summary>
    /// <param name="messageBar">Instance of the message bar to close.</param>
    /// <param name="result">Result associated with the close action.</param>
    Task CloseAsync(IMessageBarInstance messageBar, MessageBarResult result);

    /// <summary>
    /// Dismisses the specified message bar instance.
    /// </summary>
    /// <param name="messageBar">Instance of the message bar to dismiss.</param>
    Task DismissAsync(IMessageBarInstance messageBar);

    /// <summary>
    /// Dismisses the message bar with the specified identifier.
    /// </summary>
    /// <param name="messageBarId">The identifier of the message bar to dismiss.</param>
    /// <returns><see langword="true"/> when a matching message bar was found; otherwise <see langword="false"/>.</returns>
    Task<bool> DismissAsync(string messageBarId);

    /// <summary>
    /// Dismisses all current message bars.
    /// </summary>
    /// <returns>The number of message bars that were dismissed.</returns>
    Task<int> DismissAllAsync();

    /// <summary>
    /// Shows a message bar using the supplied options and waits for the close result.
    /// </summary>
    /// <param name="options">Options to configure the message bar.</param>
    Task<MessageBarResult> ShowMessageAsync(MessageBarOptions? options = null);

    /// <summary>
    /// Shows a message bar by configuring an options object and waits for the close result.
    /// </summary>
    /// <param name="options">Action used to configure the message bar.</param>
    Task<MessageBarResult> ShowMessageAsync(Action<MessageBarOptions> options);

    /// <summary>
    /// Shows a custom message bar component and waits for the close result.
    /// The component receives the current <see cref="IMessageBarInstance"/> through a cascading parameter.
    /// </summary>
    /// <typeparam name="TMessageBar">A Blazor component type used to render the message bar.</typeparam>
    /// <param name="options">Options used to configure the message bar.</param>
    Task<MessageBarResult> ShowMessageAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TMessageBar>(MessageBarOptions? options = null)
        where TMessageBar : ComponentBase;

    /// <summary>
    /// Shows a custom message bar component and waits for the close result.
    /// The component receives the current <see cref="IMessageBarInstance"/> through a cascading parameter.
    /// </summary>
    /// <typeparam name="TMessageBar">A Blazor component type used to render the message bar.</typeparam>
    /// <param name="options">Action used to configure the message bar.</param>
    Task<MessageBarResult> ShowMessageAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TMessageBar>(Action<MessageBarOptions> options)
        where TMessageBar : ComponentBase;

    /// <summary>
    /// Updates a shown message bar.
    /// </summary>
    /// <param name="messageBar">The message bar instance to update.</param>
    /// <param name="update">The action that mutates the current options.</param>
    Task UpdateMessageBarAsync(IMessageBarInstance messageBar, Action<MessageBarOptions> update);
}
