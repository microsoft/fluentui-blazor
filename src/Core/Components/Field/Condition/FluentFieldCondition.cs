// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Add or build conditions for a field.
/// </summary>
public class FluentFieldCondition
{
    private readonly IFluentField _value;
    private readonly List<FluentFieldConditionItem> _listOfConditions = [];

    /// <summary>
    /// Gets a condition that is always true.
    /// </summary>
    public static readonly Func<IFluentField, bool> Always = (i) => true;

    /// <summary>
    /// Gets a condition that is always false.
    /// </summary>
    public static readonly Func<IFluentField, bool> Never = (i) => false;

    /// <summary>
    /// Create a new instance of <see cref="FluentFieldCondition"/>.
    /// </summary>
    /// <param name="value"></param>
    internal FluentFieldCondition(IFluentField value)
    {
        _value = value;
    }

    /// <summary>
    /// Add a condition to the field.
    /// </summary>
    /// <param name="condition">The condition to add.</param>
    /// <returns></returns>
    public FluentFieldConditionItem When(Func<bool> condition)
    {
        var item = new FluentFieldConditionItem(this, condition);
        _listOfConditions.Add(item);
        return item;
    }

    /// <summary>
    /// Build the conditions.
    /// </summary>
    /// <returns></returns>
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
