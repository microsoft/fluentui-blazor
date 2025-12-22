// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

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
}
