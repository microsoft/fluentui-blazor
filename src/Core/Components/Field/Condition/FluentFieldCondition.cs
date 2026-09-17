// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

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
    public bool Build(FluentFieldConditionOptions? options = null)
    {
        options ??= new FluentFieldConditionOptions();

        // Break on the first condition that is true.
        if (options.BreakOnFirst)
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

        // Display all conditions that are true.
        var messages = new List<RenderFragment>();
        foreach (var item in _listOfConditions)
        {
            if (item.Condition.Invoke())
            {
                var coloredMessage = FluentFieldParameterSelector.StateToMessageTemplate(item.State, item.Message);

                messages.Add(builder =>
                {
                    builder.OpenElement(0, "div");
                    builder.AddContent(1, FluentField.CreateIcon(item.Icon ?? FluentFieldParameterSelector.StateToIcon(item.State)));

                    if (item.State is null)
                    {
                        builder.AddContent(2, item.Message);
                    }
                    else
                    {
                        builder.AddContent(2, coloredMessage);
                    }

                    builder.CloseElement();
                });
            }
        }

        _value.MessageTemplate = builder =>
        {
            foreach (var message in messages)
            {
                message(builder);
            }
        };

        return messages.Count > 0;
    }

    /// <summary>
    /// Build the conditions.
    /// </summary>
    /// <returns></returns>
    public bool Build(Action<FluentFieldConditionOptions> options)
    {
        var config = new FluentFieldConditionOptions();
        options.Invoke(config);
        return Build(config);
    }
}
