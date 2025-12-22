// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Defines a contract for applying sorting rules to a collection of grid items.
/// </summary>
/// <typeparam name="TGridItem">The type of items in the grid to be sorted.</typeparam>
public interface IGridSort<TGridItem>
{
    /// <summary>
    /// Produces a readonly collection of (property name, direction) pairs representing the sorting rules.
    /// </summary>
    /// <param name="ascending"></param>
    /// <returns>The readonly collection of properties that can be sorted on</returns>
    IReadOnlyCollection<SortedProperty> ToPropertyList(bool ascending);

    /// <summary>
    /// Apply the sort function to the collection
    /// </summary>
    /// <param name="queryable">The collection to sort</param>
    /// <param name="ascending">Sort ascending (true) or descending (false)</param>
    /// <returns>The ordered collection</returns>
    IOrderedQueryable<TGridItem> Apply(IQueryable<TGridItem> queryable, bool ascending);
}
