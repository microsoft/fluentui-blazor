// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public class FluentFieldCondition
{
    private readonly IFluentField _value;
    private readonly List<ConditionItem> _listOfConditions = [];

    /// <summary />
    public FluentFieldCondition(IFluentField value)
    {
        _value = value;
    }

    /// <summary />
    public FluentFieldCondition When(Func<bool> condition, string message, FieldMessageState? state = null, Icon? icon = null)
    {
        _listOfConditions.Add(new ConditionItem(condition, message, state, icon));
        return this;
    }

    /// <summary />
    public bool Build()
    {
        foreach (var item in _listOfConditions)
        {
            if (item.Condition.Invoke())
            {
                _value.MessageState = item.state;
                _value.Message = item.Message;
                _value.MessageIcon = item.icon;
                return true;
            }
        }

        return false;
    }

    private record ConditionItem(Func<bool> Condition, string? Message, FieldMessageState? state, Icon? icon);
}
