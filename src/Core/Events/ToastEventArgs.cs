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
    internal ToastEventArgs(IToastInstance? instance, ToastLifecycleStatus status)
    {
        Id = instance?.Id ?? string.Empty;
        Instance = instance;
        Status = status;
    }

    /// <summary />
    internal ToastEventArgs(IToastInstance? instance, DialogToggleEventArgs args)
        : this(instance, args.Id, args.Type, args.OldState, args.NewState)
    {
    }

    /// <summary />
    internal ToastEventArgs(IToastInstance? instance, string? id, string? eventType, string? oldState, string? newState)
    {
        Id = id ?? string.Empty;
        Instance = instance;

        Status = DialogEventArgs.GetDialogState(eventType, oldState, newState) switch
        {
            DialogState.Open => ToastLifecycleStatus.Visible,
            DialogState.Closing => ToastLifecycleStatus.Dismissed,
            _ => ToastLifecycleStatus.Queued,
        };
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
    /// Gets the instance used by the <see cref="ToastService" />.
    /// This value may be null if the toast is not managed by the <see cref="ToastService"/>.
    /// </summary>
    public IToastInstance? Instance { get; }
}
