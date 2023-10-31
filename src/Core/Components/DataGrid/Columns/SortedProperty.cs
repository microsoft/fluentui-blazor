namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Holds the name of a property and the direction to sort by.
/// </summary>
public readonly struct SortedProperty
{
    /// <summary>
    /// The property name for the sorting rule.
    /// </summary>
    public /*required*/ string PropertyName { get; init; }

    /// <summary>
    /// The direction to sort by.
    /// </summary>
    public SortDirection Direction { get; init; }
}
