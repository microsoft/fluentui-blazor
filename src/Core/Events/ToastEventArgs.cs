// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Event arguments for the FluentToast component.
/// </summary>
public class ToastEventArgs : EventArgs
{
    /// <summary />
    internal ToastEventArgs(IToastInstance instance, ToastLifecycleStatus status)
    {
        Id = instance.Id;
        Instance = instance;
        Status = status;
    }

    /// <summary>
    /// Gets the ID of the FluentToast component.
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Gets the lifecycle status of the FluentToast component.
    /// </summary>
    public ToastLifecycleStatus Status { get; }

    /// <summary>
    /// Gets the instance used by the <see cref="NotificationService" />.
    /// This value may be null if the toast is not managed by the <see cref="NotificationService"/>.
    /// </summary>
    public IToastInstance Instance { get; }
}
