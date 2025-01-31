// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public class FluentFieldConditionItem
{
    internal FluentFieldCondition Owner;
    internal Func<bool> Condition;
    internal string? Message;
    internal FieldMessageState? State;
    internal Icon? Icon;

    /// <summary />
    internal FluentFieldConditionItem(FluentFieldCondition owner, Func<bool> condition)
    {
        Owner = owner;
        Condition = condition;
    }

    /// <summary />
    public FluentFieldCondition Display(string message, Icon? icon = null)
    {
        return InternalDisplay(message, state: null, icon);
    }

    /// <summary />
    public FluentFieldCondition Display(FieldMessageState state)
    {
        return InternalDisplay(message: null, state, icon: null);
    }

    /// <summary />
    public FluentFieldCondition Display(string message, FieldMessageState state, Icon? icon = null)
    {
        return InternalDisplay(message, state, icon);
    }

    /// <summary />
    private FluentFieldCondition InternalDisplay(string? message, FieldMessageState? state, Icon? icon)
    {
        Message = message;
        State = state;
        Icon = icon;
        return Owner;
    }
}
