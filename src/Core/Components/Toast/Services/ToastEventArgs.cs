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

        if (string.Equals(eventType, "toggle", StringComparison.OrdinalIgnoreCase))
        {
            if (string.Equals(newState, "open", StringComparison.OrdinalIgnoreCase))
            {
                State = DialogState.Open;
            }
            else if (string.Equals(newState, "closed", StringComparison.OrdinalIgnoreCase))
            {
                State = DialogState.Closed;
            }
        }
        else if (string.Equals(eventType, "beforetoggle", StringComparison.OrdinalIgnoreCase))
        {
            if (string.Equals(oldState, "closed", StringComparison.OrdinalIgnoreCase))
            {
                State = DialogState.Opening;
            }
            else if (string.Equals(oldState, "open", StringComparison.OrdinalIgnoreCase))
            {
                State = DialogState.Closing;
            }
        }
        else
        {
            State = DialogState.Closed;
        }
    }

    /// <summary />
    internal ToastEventArgs(IToastInstance instance, DialogState state)
    {
        Id = instance.Id;
        Instance = instance;
        State = state;
    }

    /// <summary>
    /// Gets the ID of the FluentToast component.
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Gets the state of the FluentToast component.
    /// </summary>
    public DialogState State { get; }

    /// <summary>
    /// Gets the instance used by the <see cref="ToastService" />.
    /// </summary>
    public IToastInstance? Instance { get; }
}
