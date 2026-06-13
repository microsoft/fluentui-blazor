// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Event arguments for the FluentMessageBar component when displayed by the <see cref="IMessageBarService"/>.
/// </summary>
public class MessageBarEventArgs : EventArgs
{
    /// <summary />
    internal MessageBarEventArgs(IMessageBarInstance instance, MessageBarLifecycleStatus status)
    {
        Id = instance.Id;
        Instance = instance;
        Status = status;
    }

    /// <summary>
    /// Gets the ID of the FluentMessageBar component.
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Gets the lifecycle status of the FluentMessageBar component.
    /// </summary>
    public MessageBarLifecycleStatus Status { get; }

    /// <summary>
    /// Gets the instance used by the <see cref="MessageBarService" />.
    /// </summary>
    public IMessageBarInstance? Instance { get; }
}
