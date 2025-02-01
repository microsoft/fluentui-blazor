// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public class FluentFieldCondition
{
    private readonly IFluentField _value;
    private readonly List<FluentFieldConditionItem> _listOfConditions = [];

    /// <summary />
    public static readonly Func<IFluentField, bool> Always = (i) => true;

    /// <summary />
    public static readonly Func<IFluentField, bool> Never = (i) => false;

    /// <summary />
    internal FluentFieldCondition(IFluentField value)
    {
        _value = value;
    }

    /// <summary />
    public FluentFieldConditionItem When(Func<bool> condition)
    {
        var item = new FluentFieldConditionItem(this, condition);
        _listOfConditions.Add(item);
        return item;
    }

    /// <summary />
    public bool Build()
    {
        foreach (var item in _listOfConditions)
        {
            if (item.Condition.Invoke())
            {
                _value.MessageState = item.State;
                _value.Message = item.Message;
                _value.MessageIcon = item.Icon;
                return true;
            }
        }

        return false;
    }
}
