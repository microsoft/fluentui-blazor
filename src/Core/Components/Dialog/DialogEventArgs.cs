// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Event arguments for the FluentDialog component.
/// </summary>
public class DialogEventArgs : EventArgs
{
    /// <summary />
    internal DialogEventArgs(FluentDialog dialog, DialogToggleEventArgs args)
        : this(dialog, args.Id, args.Type, args.OldState, args.NewState)
    {
    }

    /// <summary />
    internal DialogEventArgs(FluentDialog dialog, string? id, string? eventType, string? oldState, string? newState)
    {
        Id = id ?? string.Empty;
        Instance = dialog.Instance;
        State = GetDialogState(eventType, oldState, newState);
    }

    /// <summary />
    internal DialogEventArgs(IDialogInstance instance, DialogState state)
    {
        Id = instance.Id;
        Instance = instance;
        State = state;
    }

    /// <summary>
    /// Gets the ID of the FluentDialog component.
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Gets the state of the FluentDialog component.
    /// </summary>
    public DialogState State { get; }

    /// <summary>
    /// Gets the instance used by the <see cref="DialogService" />.
    /// </summary>
    public IDialogInstance? Instance { get; }

    /// <summary>
    /// Determines the <see cref="DialogState"/> based on the provided event type and states.
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="oldState"></param>
    /// <param name="newState"></param>
    /// <returns></returns>
    internal static DialogState GetDialogState(string? eventType, string? oldState, string? newState)
    {
        if (string.Equals(eventType, "toggle", StringComparison.OrdinalIgnoreCase))
        {
            if (string.Equals(newState, "open", StringComparison.OrdinalIgnoreCase))
            {
                return DialogState.Open;
            }

            if (string.Equals(newState, "closed", StringComparison.OrdinalIgnoreCase))
            {
                return DialogState.Closed;
            }
        }

        if (string.Equals(eventType, "beforetoggle", StringComparison.OrdinalIgnoreCase))
        {
            if (string.Equals(oldState, "closed", StringComparison.OrdinalIgnoreCase))
            {
                return DialogState.Opening;
            }

            if (string.Equals(oldState, "open", StringComparison.OrdinalIgnoreCase))
            {
                return DialogState.Closing;
            }
        }

        return DialogState.Closed;
    }
}
