// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Interface for a notification instance managed by the <see cref="INotificationService"/>.
/// </summary>
public partial interface INotificationInstance
{
    /// <summary>
    /// Gets the optional component type rendered for this notification.
    /// When <see langword="null"/>, the default notification component is rendered.
    /// </summary>
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
    internal Type? ComponentType { get; }

    /// <summary>
    /// Gets the unique identifier for the notification. If this value is not set in the options,
    /// a new identifier is generated.
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Gets the index of the notification (sequential number).
    /// </summary>
    long Index { get; }

    /// <summary>
    /// Closes the notification programmatically.
    /// </summary>
    Task CloseAsync();
}
