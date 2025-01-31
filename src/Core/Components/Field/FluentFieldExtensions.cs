// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public static class FluentFieldExtensions
{
    /// <summary />
    public static FluentFieldCondition When(this IFluentField fluentField, Func<bool> condition, string message, FieldMessageState? state = null, Icon? icon = null)
    {
        return new FluentFieldCondition(fluentField).When(condition, message, state, icon);
    }
}
