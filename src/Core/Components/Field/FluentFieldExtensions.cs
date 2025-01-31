// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public static class FluentFieldExtensions
{
    /// <summary />
    public static FluentFieldConditionItem When(this IFluentField fluentField, Func<bool> condition)
    {
        return new FluentFieldCondition(fluentField).When(condition);
    }
}
