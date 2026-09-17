// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// <see cref="FluentAutocomplete{TOption, TValue}"/> uses this event to return the resolved item for a given value when Value is set.
/// </summary>
/// <typeparam name="TOption"></typeparam>
/// <typeparam name="TValue"></typeparam>
public class SetValueEventArgs<TOption, TValue>
{
    /// <summary>
    /// Gets the value to resolve. The handler should set Item to the resolved item for this value.
    /// </summary>
    public TValue? Value { get; init; }
    
    /// <summary>
    /// Gets or sets the resolved item for the given Value.
    /// </summary>
    public TOption? Item { get; set; }
}