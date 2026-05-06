// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Extension methods for <see cref="IFluentField"/>.
/// </summary>
public static class FluentFieldExtensions
{
    /// <summary>
    /// Add a condition to the field.
    /// </summary>
    /// <param name="fluentField">The field to add the condition to.</param>
    /// <param name="condition">The condition to add.</param>
    /// <returns></returns>
    public static FluentFieldConditionItem When(this IFluentField fluentField, Func<bool> condition)
    {
        return new FluentFieldCondition(fluentField).When(condition);
    }

    /// <summary>
    /// Combines two <see cref="RenderFragment"/> instances into one. If both fragments are null, returns null.
    /// </summary>
    /// <param name="item1"></param>
    /// <param name="item2"></param>
    /// <returns></returns>
    public static RenderFragment? CombinedWith(this RenderFragment? item1, RenderFragment? item2)
    {
        if (item1 is null && item2 is null)
        {
            return null;
        }

        return builder =>
        {
            if (item1 is not null)
            {
                builder.AddContent(0, item1);
            }

            if (item2 is not null)
            {
                builder.AddContent(1, item2);
            }
        };
    }
}
