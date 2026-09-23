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

    /// <summary>
    /// Gets a value indicating whether this sort can be applied as a secondary sort level through
    /// <see cref="ApplyThen(IOrderedQueryable{TGridItem}, bool)"/>.
    /// A column whose sort returns <see langword="false"/> can only be used as the first sort level when
    /// <see cref="FluentDataGrid{TGridItem}.SortMode"/> is <see cref="DataGridSortMode.Multiple"/>.
    /// </summary>
    bool CanApplyThen => false;

    /// <summary>
    /// Appends this sort's rules to a collection that is already ordered, as
    /// <see cref="Queryable.ThenBy{TSource, TKey}(IOrderedQueryable{TSource}, System.Linq.Expressions.Expression{Func{TSource, TKey}})"/> does.
    /// Only called when <see cref="CanApplyThen"/> is <see langword="true"/>.
    /// </summary>
    /// <param name="queryable">The already ordered collection.</param>
    /// <param name="ascending">Sort ascending (true) or descending (false), with the same meaning as in <see cref="Apply(IQueryable{TGridItem}, bool)"/>.</param>
    /// <returns>The ordered collection.</returns>
    IOrderedQueryable<TGridItem> ApplyThen(IOrderedQueryable<TGridItem> queryable, bool ascending)
        => throw new NotSupportedException($"{GetType().Name} cannot be used as a secondary sort level. Implement {nameof(ApplyThen)} and return true from {nameof(CanApplyThen)} to support it.");
}
