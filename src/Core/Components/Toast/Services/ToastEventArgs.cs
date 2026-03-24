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
    internal ToastEventArgs(FluentToast toast, DialogToggleEventArgs args)
        : this(toast, args.Id, args.Type, args.OldState, args.NewState)
    {
    }

    /// <summary />
    internal ToastEventArgs(FluentToast toast, string? id, string? eventType, string? oldState, string? newState)
    {
        Id = id ?? string.Empty;
        Instance = toast.Instance;
        Status = ToastStatus.Queued;

        if (string.Equals(eventType, "toggle", StringComparison.OrdinalIgnoreCase))
        {
            if (string.Equals(newState, "open", StringComparison.OrdinalIgnoreCase))
            {
                Status = ToastStatus.Visible;
            }
        }
        else if (string.Equals(eventType, "beforetoggle", StringComparison.OrdinalIgnoreCase))
        {
            if (string.Equals(oldState, "open", StringComparison.OrdinalIgnoreCase))
            {
                Status = ToastStatus.Dismissed;
            }
        }
    }

    /// <summary />
    internal ToastEventArgs(IToastInstance instance, ToastStatus status)
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
    public ToastStatus Status { get; }

    /// <summary>
    /// Gets the instance used by the <see cref="ToastService" />.
    /// </summary>
    public IToastInstance? Instance { get; }
}
