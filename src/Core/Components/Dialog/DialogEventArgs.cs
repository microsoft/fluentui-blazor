// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
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
}
