// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents a condition to display a message in a field.
/// </summary>
public class FluentFieldConditionItem
{
    internal FluentFieldCondition Owner;
    internal Func<bool> Condition;
    internal string? Message;
    internal MessageState? State;
    internal Icon? Icon;

    /// <summary>
    /// Create a new instance of <see cref="FluentFieldConditionItem"/>.
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="condition"></param>
    internal FluentFieldConditionItem(FluentFieldCondition owner, Func<bool> condition)
    {
        Owner = owner;
        Condition = condition;
    }

    /// <summary>
    /// Display a message in the field.
    /// </summary>
    /// <param name="message">Message to display.</param>
    /// <param name="icon">Optional icon to display.</param>
    /// <returns></returns>
    public FluentFieldCondition Display(string message, Icon? icon = null)
    {
        return InternalDisplay(message, state: null, icon);
    }

    /// <summary>
    /// Display a message in the field.
    /// </summary>
    /// <param name="state">State of the message.</param>
    /// <returns></returns>
    public FluentFieldCondition Display(MessageState state)
    {
        return InternalDisplay(message: null, state, icon: null);
    }

    /// <summary>
    /// Display a message in the field.
    /// </summary>
    /// <param name="message">Message to display.</param>
    /// <param name="state">State of the message.</param>
    /// <param name="icon">Optional icon to display.</param>
    /// <returns></returns>
    public FluentFieldCondition Display(string message, MessageState state, Icon? icon = null)
    {
        return InternalDisplay(message, state, icon);
    }

    /// <summary />
    private FluentFieldCondition InternalDisplay(string? message, MessageState? state, Icon? icon)
    {
        Message = message;
        State = state;
        Icon = icon;
        return Owner;
    }
}
