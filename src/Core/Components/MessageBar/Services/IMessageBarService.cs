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
    /// Closes the specified message bar instance.
    /// </summary>
    /// <param name="messageBar">Instance of the message bar to close.</param>
    /// <param name="data">Optional data to include with the close result.</param>
    Task CloseAsync(IMessageBarInstance messageBar, object? data = null);

    /// <summary>
    /// Closes the message bar with the specified identifier.
    /// </summary>
    /// <param name="messageBarId">The identifier of the message bar to close.</param>
    /// <param name="data">Optional data to include with the close result.</param>
    /// <returns><see langword="true"/> when a matching message bar was found; otherwise <see langword="false"/>.</returns>
    Task<bool> CloseAsync(string messageBarId, object? data = null);

    /// <summary>
    /// Closes all current message bars.
    /// </summary>
    /// <returns>The number of message bars that were closed.</returns>
    Task<int> CloseAllAsync();

    /// <summary>
    /// Shows a message bar using the supplied options and waits for the close result.
    /// </summary>
    /// <param name="options">Options to configure the message bar.</param>
    Task<MessageBarResult> ShowMessageAsync(MessageBarOptions options);

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
    Task<MessageBarResult> ShowMessageAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TMessageBar>(MessageBarOptions options)
        where TMessageBar : ComponentBase;

    /// <summary>
    /// Shows a custom message bar component and waits for the close result.
    /// The component receives the current <see cref="IMessageBarInstance"/> through a cascading parameter.
    /// </summary>
    /// <typeparam name="TMessageBar">A Blazor component type used to render the message bar.</typeparam>
    /// <param name="options">Action used to configure the message bar.</param>
    Task<MessageBarResult> ShowMessageAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TMessageBar>(Action<MessageBarOptions> options)
        where TMessageBar : ComponentBase;
}
