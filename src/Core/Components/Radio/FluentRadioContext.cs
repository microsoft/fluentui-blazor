// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Describes context for an <see cref="FluentRadio{TValue}"/> component.
/// </summary>
internal sealed class FluentRadioContext
{
    private readonly IFluentRadioValueProvider _valueProvider;

    public FluentRadioContext? ParentContext { get; }
    public EventCallback<ChangeEventArgs> ChangeEventCallback { get; }
    public object? CurrentValue => _valueProvider.CurrentValue;

    // Mutable properties that may change any time an FluentRadioGroup is rendered
    public string? GroupName { get; set; }

    public string? FieldClass { get; set; }

    public Orientation? Orientation { get; set; }

    /// <summary>
    /// Instantiates a new <see cref="FluentRadioContext" />.
    /// </summary>
    /// <param name="parentContext">The parent context, if any.</param>
    /// <param name="changeEventCallback">The event callback to be invoked when the selected value is changed.</param>
    /// <param name="valueProvider">The value provider for the radio context.</param>
    /// <param name="orientation">The orientation of the radio group.</param>
    public FluentRadioContext(IFluentRadioValueProvider valueProvider, FluentRadioContext? parentContext, EventCallback<ChangeEventArgs> changeEventCallback, Orientation? orientation)
    {
        _valueProvider = valueProvider;
        ParentContext = parentContext;
        ChangeEventCallback = changeEventCallback;
        Orientation = orientation;
    }

    /// <summary>
    /// Finds an <see cref="FluentRadioContext"/> in the context's ancestors with the matching <paramref name="groupName"/>.
    /// </summary>
    /// <param name="groupName">The group name of the ancestor <see cref="FluentRadioContext"/>.</param>
    /// <returns>The <see cref="FluentRadioContext"/>, or <c>null</c> if none was found.</returns>
    public FluentRadioContext? FindContextInAncestors(string groupName)
        => string.Equals(GroupName, groupName, StringComparison.InvariantCulture) ? this : ParentContext?.FindContextInAncestors(groupName);
}
