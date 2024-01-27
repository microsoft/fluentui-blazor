using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Describes context for an <see cref="FluentRadio{TValue}"/> component. 
/// </summary>
internal sealed class FluentRadioContext
{
    public FluentRadioContext? ParentContext { get; }
    public EventCallback<ChangeEventArgs> ChangeEventCallback { get; }

    // Mutable properties that may change any time an FluentRadioGroup is rendered
    public string? GroupName { get; set; }
    public object? CurrentValue { get; set; }

    public string? FieldClass { get; set; }

    /// <summary>
    /// Instantiates a new <see cref="FluentRadioContext" />.
    /// </summary>
    /// <param name="parentContext">The parent context, if any.</param>
    /// <param name="changeEventCallback">The event callback to be invoked when the selected value is changed.</param>
    public FluentRadioContext(FluentRadioContext? parentContext, EventCallback<ChangeEventArgs> changeEventCallback)
    {
        ParentContext = parentContext;
        ChangeEventCallback = changeEventCallback;
    }

    /// <summary>
    /// Finds an <see cref="FluentRadioContext"/> in the context's ancestors with the matching <paramref name="groupName"/>.
    /// </summary>
    /// <param name="groupName">The group name of the ancestor <see cref="FluentRadioContext"/>.</param>
    /// <returns>The <see cref="FluentRadioContext"/>, or <c>null</c> if none was found.</returns>
    public FluentRadioContext? FindContextInAncestors(string groupName)
        => string.Equals(GroupName, groupName) ? this : ParentContext?.FindContextInAncestors(groupName);
}
