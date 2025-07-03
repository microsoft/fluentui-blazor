// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Holds the name of a property and the direction to sort by.
/// </summary>
public readonly struct SortedProperty
{
    /// <summary>
    /// Gets or sets the property name for the sorting rule.
    /// </summary>
    public string PropertyName { get; init; }

    /// <summary>
    /// Gets or sets the direction to sort by.
    /// </summary>
    public DataGridSortDirection Direction { get; init; }
}
