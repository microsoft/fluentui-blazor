// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Interface for the Notification service.
/// </summary>
public partial interface INotificationService : IFluentServiceBase<INotificationInstance>
{
    /// <summary>
    /// Shows a success message bar with the specified title and message and waits for the close result.
    /// </summary>
    /// <param name="section">Section used to target a specific <see cref="FluentMessageBarProvider"/>.</param>
    /// <param name="title">The title of the message bar.</param>
    /// <param name="message">The message content of the message bar.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the close result of the message bar.</returns>
    Task<MessageBarResult> ShowSuccessBarAsync(string section, string? title = null, string? message = null);

    /// <summary>
    /// Shows a warning message bar with the specified title and message and waits for the close result.
    /// </summary>
    /// <param name="section">Section used to target a specific <see cref="FluentMessageBarProvider"/>.</param>
    /// <param name="title">The title of the message bar.</param>
    /// <param name="message">The message content of the message bar.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the close result of the message bar.</returns>
    Task<MessageBarResult> ShowWarningBarAsync(string section, string? title = null, string? message = null);

    /// <summary>
    /// Shows an error message bar with the specified title and message and waits for the close result.
    /// </summary>
    /// <param name="section">Section used to target a specific <see cref="FluentMessageBarProvider"/>.</param>
    /// <param name="title">The title of the message bar.</param>
    /// <param name="message">The message content of the message bar.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the close result of the message bar.</returns>
    Task<MessageBarResult> ShowErrorBarAsync(string section, string? title = null, string? message = null);

    /// <summary>
    /// Shows an informational message bar with the specified title and message and waits for the close result.
    /// </summary>
    /// <param name="section">Section used to target a specific <see cref="FluentMessageBarProvider"/>.</param>
    /// <param name="title">The title of the message bar.</param>
    /// <param name="message">The message content of the message bar.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the close result of the message bar.</returns>
    Task<MessageBarResult> ShowInfoBarAsync(string section, string? title = null, string? message = null);

    /// <summary>
    /// Shows a message bar using the supplied options.
    /// </summary>
    /// <param name="options">Options to configure the message bar.</param>
    Task<MessageBarResult> ShowMessageBarAsync(MessageBarOptions options);

    /// <summary>
    /// Shows a message bar by configuring an options object.
    /// </summary>
    /// <param name="options">Action used to configure the message bar.</param>
    Task<MessageBarResult> ShowMessageBarAsync(Action<MessageBarOptions> options);

    /// <summary>
    /// Shows a custom message bar component.
    /// The component receives the current <see cref="IMessageBarInstance"/> through a cascading parameter.
    /// </summary>
    /// <typeparam name="TMessageBar">A Blazor component type used to render the message bar.</typeparam>
    /// <param name="options">Options used to configure the message bar.</param>
    Task<MessageBarResult> ShowMessageBarAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TMessageBar>(MessageBarOptions options)
        where TMessageBar : ComponentBase;

    /// <summary>
    /// Shows a custom message bar component.
    /// The component receives the current <see cref="IMessageBarInstance"/> through a cascading parameter.
    /// </summary>
    /// <typeparam name="TMessageBar">A Blazor component type used to render the message bar.</typeparam>
    /// <param name="options">Action used to configure the message bar.</param>
    Task<MessageBarResult> ShowMessageBarAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TMessageBar>(Action<MessageBarOptions> options)
        where TMessageBar : ComponentBase;

    /// <summary>
    /// Closes the specified message bar instance.
    /// </summary>
    /// <param name="messageBar">Instance of the message bar to close.</param>
    /// <param name="data">Optional data to include with the close result.</param>
    Task CloseAsync(IMessageBarInstance messageBar, object? data = null);

    /// <summary>
    /// Closes the notification (message bar or toast) with the specified identifier.
    /// </summary>
    /// <param name="id">The identifier of the notification to close.</param>
    /// <param name="data">Optional data to include with the close result.</param>
    /// <returns><see langword="true"/> when a matching notification was found; otherwise <see langword="false"/>.</returns>
    Task<bool> CloseAsync(string id, object? data = null);

    /// <summary>
    /// Closes all current message bars.
    /// </summary>
    /// <returns>The number of message bars that were closed.</returns>
    Task<int> CloseAllMessageBarsAsync();
}
