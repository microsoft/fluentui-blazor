// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Microsoft.FluentUI.AspNetCore.Components.DataGrid.Infrastructure;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents a sort order specification used within <see cref="FluentDataGrid{TGridItem}"/>.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
public sealed class GridSort<TGridItem> : IGridSort<TGridItem>
{
    private const string ExpressionNotRepresentableMessage = "The supplied expression can't be represented as a property name for sorting. Only simple member expressions, such as @(x => x.SomeProperty), can be converted to property names.";

    private readonly Func<IQueryable<TGridItem>, bool, IOrderedQueryable<TGridItem>> _first;
    private readonly Func<IOrderedQueryable<TGridItem>, bool, IOrderedQueryable<TGridItem>> _firstAsThen;
    private List<Func<IOrderedQueryable<TGridItem>, bool, IOrderedQueryable<TGridItem>>>? _then;

    private (LambdaExpression, bool) _firstExpression;
    private List<(LambdaExpression, bool)>? _thenExpressions;

    private IReadOnlyCollection<SortedProperty>? _cachedPropertyListAscending;
    private IReadOnlyCollection<SortedProperty>? _cachedPropertyListDescending;

    internal GridSort(
        Func<IQueryable<TGridItem>, bool, IOrderedQueryable<TGridItem>> first,
        Func<IOrderedQueryable<TGridItem>, bool, IOrderedQueryable<TGridItem>> firstAsThen,
        (LambdaExpression, bool) firstExpression)
    {
        _first = first;
        _firstAsThen = firstAsThen;
        _firstExpression = firstExpression;
        _then = default;
        _thenExpressions = default;
    }

    /// <summary>
    /// Produces a <see cref="GridSort{T}"/> instance that sorts according to the specified <paramref name="expression"/>, ascending.
    /// </summary>
    /// <typeparam name="U">The type of the expression's value.</typeparam>
    /// <param name="expression">An expression defining how a set of <typeparamref name="TGridItem"/> instances are to be sorted.</param>
    /// <returns>A <see cref="GridSort{T}"/> instance representing the specified sorting rule.</returns>
#pragma warning disable MA0018 // Do not declare static members on generic types (deprecated; use CA1000 instead)
    public static GridSort<TGridItem> ByAscending<U>(Expression<Func<TGridItem, U>> expression)
        => new((queryable, asc) => asc ? queryable.OrderBy(expression) : queryable.OrderByDescending(expression),
            (queryable, asc) => asc ? queryable.ThenBy(expression) : queryable.ThenByDescending(expression),
            (expression, true));

    /// <summary>
    /// Produces a <see cref="GridSort{T}"/> instance that sorts according to the specified <paramref name="expression"/>
    /// using the specified <paramref name="comparer"/>, ascending.
    /// </summary>
    /// <typeparam name="U">The type of the expression's value.</typeparam>
    /// <param name="expression">An expression defining how a set of <typeparamref name="TGridItem"/> instances are to be sorted.</param>
    /// <param name="comparer">Defines how a items in a set of <typeparamref name="TGridItem"/> instances are to be compared.</param>
    /// <returns>A <see cref="GridSort{T}"/> instance representing the specified sorting rule.</returns>
    public static GridSort<TGridItem> ByAscending<U>(Expression<Func<TGridItem, U>> expression, IComparer<U> comparer)
        => new((queryable, asc) => asc ? queryable.OrderBy(expression, comparer) : queryable.OrderByDescending(expression, comparer),
            (queryable, asc) => asc ? queryable.ThenBy(expression, comparer) : queryable.ThenByDescending(expression, comparer),
            (expression, true));

    /// <summary>
    /// Produces a <see cref="GridSort{T}"/> instance that sorts according to the specified <paramref name="expression"/>, descending.
    /// </summary>
    /// <typeparam name="U">The type of the expression's value.</typeparam>
    /// <param name="expression">An expression defining how a set of <typeparamref name="TGridItem"/> instances are to be sorted.</param>
    /// <returns>A <see cref="GridSort{T}"/> instance representing the specified sorting rule.</returns>
    public static GridSort<TGridItem> ByDescending<U>(Expression<Func<TGridItem, U>> expression)
        => new((queryable, asc) => asc ? queryable.OrderByDescending(expression) : queryable.OrderBy(expression),
            (queryable, asc) => asc ? queryable.ThenByDescending(expression) : queryable.ThenBy(expression),
            (expression, false));

    /// <summary>
    /// Produces a <see cref="GridSort{T}"/> instance that sorts according to the specified <paramref name="expression"/>
    /// using the specified <paramref name="comparer"/>, descending.
    /// </summary>
    /// <typeparam name="U">The type of the expression's value.</typeparam>
    /// <param name="expression">An expression defining how a set of <typeparamref name="TGridItem"/> instances are to be sorted.</param>
    /// <param name="comparer">Defines how a items in a set of <typeparamref name="TGridItem"/> instances are to be compared.</param>
    /// <returns>A <see cref="GridSort{T}"/> instance representing the specified sorting rule.</returns>
    public static GridSort<TGridItem> ByDescending<U>(Expression<Func<TGridItem, U>> expression, IComparer<U> comparer)
#pragma warning restore MA0018 // Do not declare static members on generic types (deprecated; use CA1000 instead)
        => new((queryable, asc) => asc ? queryable.OrderByDescending(expression, comparer) : queryable.OrderBy(expression, comparer),
            (queryable, asc) => asc ? queryable.ThenByDescending(expression, comparer) : queryable.ThenBy(expression, comparer),
            (expression, false));

    /// <summary>
    /// Updates a <see cref="GridSort{T}"/> instance by appending a further sorting rule.
    /// </summary>
    /// <typeparam name="U">The type of the expression's value.</typeparam>
    /// <param name="expression">An expression defining how a set of <typeparamref name="TGridItem"/> instances are to be sorted.</param>
    /// <returns>A <see cref="GridSort{T}"/> instance representing the specified sorting rule.</returns>
    public GridSort<TGridItem> ThenAscending<U>(Expression<Func<TGridItem, U>> expression)
    {
        return AddThenExpression(
            (queryable, asc) => asc ? queryable.ThenBy(expression) : queryable.ThenByDescending(expression),
            (expression, true)
        );
    }

    /// <summary>
    /// Updates a <see cref="GridSort{T}"/> instance by appending a further sorting rule.
    /// </summary>
    /// <typeparam name="U">The type of the expression's value.</typeparam>
    /// <param name="expression">An expression defining how a set of <typeparamref name="TGridItem"/> instances are to be sorted.</param>
    /// <param name="comparer">Defines how a items in a set of <typeparamref name="TGridItem"/> instances are to be compared.</param>
    /// <returns>A <see cref="GridSort{T}"/> instance representing the specified sorting rule.</returns>
    public GridSort<TGridItem> ThenAscending<U>(Expression<Func<TGridItem, U>> expression, IComparer<U> comparer)
    {
        return AddThenExpression(
            (queryable, asc) => asc ? queryable.ThenBy(expression, comparer) : queryable.ThenByDescending(expression, comparer),
            (expression, true)
        );
    }

    /// <summary>
    /// Updates a <see cref="GridSort{T}"/> instance by appending a further sorting rule.
    /// </summary>
    /// <typeparam name="U">The type of the expression's value.</typeparam>
    /// <param name="expression">An expression defining how a set of <typeparamref name="TGridItem"/> instances are to be sorted.</param>
    /// <returns>A <see cref="GridSort{T}"/> instance representing the specified sorting rule.</returns>
    public GridSort<TGridItem> ThenDescending<U>(Expression<Func<TGridItem, U>> expression)
    {
        return AddThenExpression(
            (queryable, asc) => asc ? queryable.ThenByDescending(expression) : queryable.ThenBy(expression),
            (expression, false));
    }

    /// <summary>
    /// Updates a <see cref="GridSort{T}"/> instance by appending a further sorting rule.
    /// </summary>
    /// <typeparam name="U">The type of the expression's value.</typeparam>
    /// <param name="expression">An expression defining how a set of <typeparamref name="TGridItem"/> instances are to be sorted.</param>
    /// <param name="comparer">Defines how a items in a set of <typeparamref name="TGridItem"/> instances are to be compared.</param>
    /// <returns>A <see cref="GridSort{T}"/> instance representing the specified sorting rule.</returns>
    public GridSort<TGridItem> ThenDescending<U>(Expression<Func<TGridItem, U>> expression, IComparer<U> comparer)
    {
        return AddThenExpression(
            (queryable, asc) => asc ? queryable.ThenByDescending(expression, comparer) : queryable.ThenBy(expression, comparer),
            (expression, false)
        );
    }

    /// <summary>
    /// Updates a <see cref="GridSort{T}"/> instance by appending a further sorting rule.
    /// </summary>
    /// <typeparam name="U">The type of the expression's value.</typeparam>
    /// <param name="expression">An expression defining how a set of <typeparamref name="TGridItem"/> instances are to be sorted.</param>
    /// <returns>A <see cref="GridSort{T}"/> instance representing the specified sorting rule.</returns>
    public GridSort<TGridItem> ThenAlwaysAscending<U>(Expression<Func<TGridItem, U>> expression)
    {
        return AddThenExpression(
            (queryable, _) => queryable.ThenBy(expression),
            (expression, true));
    }

    /// <summary>
    /// Updates a <see cref="GridSort{T}"/> instance by appending a further sorting rule.
    /// </summary>
    /// <typeparam name="U">The type of the expression's value.</typeparam>
    /// <param name="expression">An expression defining how a set of <typeparamref name="TGridItem"/> instances are to be sorted.</param>
    /// <param name="comparer">Defines how a items in a set of <typeparamref name="TGridItem"/> instances are to be compared.</param>
    /// <returns>A <see cref="GridSort{T}"/> instance representing the specified sorting rule.</returns>
    public GridSort<TGridItem> ThenAlwaysAscending<U>(Expression<Func<TGridItem, U>> expression, IComparer<U> comparer)
    {
        return AddThenExpression(
            (queryable, _) => queryable.ThenBy(expression, comparer),
            (expression, true)
        );
    }

    /// <summary>
    /// Updates a <see cref="GridSort{T}"/> instance by appending a further sorting rule.
    /// </summary>
    /// <typeparam name="U">The type of the expression's value.</typeparam>
    /// <param name="expression">An expression defining how a set of <typeparamref name="TGridItem"/> instances are to be sorted.</param>
    /// <returns>A <see cref="GridSort{T}"/> instance representing the specified sorting rule.</returns>
    public GridSort<TGridItem> ThenAlwaysDescending<U>(Expression<Func<TGridItem, U>> expression)
    {
        return AddThenExpression(
            (queryable, _) => queryable.ThenByDescending(expression),
            (expression, false));
    }

    /// <summary>
    /// Updates a <see cref="GridSort{T}"/> instance by appending a further sorting rule.
    /// </summary>
    /// <typeparam name="U">The type of the expression's value.</typeparam>
    /// <param name="expression">An expression defining how a set of <typeparamref name="TGridItem"/> instances are to be sorted.</param>
    /// <param name="comparer">Defines how a items in a set of <typeparamref name="TGridItem"/> instances are to be compared.</param>
    /// <returns>A <see cref="GridSort{T}"/> instance representing the specified sorting rule.</returns>
    public GridSort<TGridItem> ThenAlwaysDescending<U>(Expression<Func<TGridItem, U>> expression, IComparer<U> comparer)
    {
        return AddThenExpression(
            (queryable, _) => queryable.ThenByDescending(expression, comparer),
            (expression, false)
        );
    }

    private GridSort<TGridItem> AddThenExpression(Func<IOrderedQueryable<TGridItem>, bool, IOrderedQueryable<TGridItem>> thenSortExpression, (LambdaExpression, bool) thenExpression)
    {
        _then ??= [];
        _thenExpressions ??= [];
        _then.Add(thenSortExpression);
        _thenExpressions.Add(thenExpression);
        _cachedPropertyListAscending = null;
        _cachedPropertyListDescending = null;

        return this;
    }

    /// <inheritdoc />
    public bool CanApplyThen => true;

    /// <summary>
    /// Apply the sort function to the collection
    /// </summary>
    /// <param name="queryable">The collection to sort</param>
    /// <param name="ascending">Sort ascending (true) or descending (false)</param>
    /// <returns>The ordered collection</returns>
    public IOrderedQueryable<TGridItem> Apply(IQueryable<TGridItem> queryable, bool ascending)
    {
        var sortedQueryable = ApplyStandardSorting(queryable, ascending);

        return HierarchicalSortHelper.IsHierarchicalInMemoryQueryable(queryable)
            ? HierarchicalSortHelper.RestoreHierarchyOrder(queryable, sortedQueryable)
            : sortedQueryable;
    }

    /// <summary>
    /// Appends this sort's rules to a collection that is already ordered.
    /// </summary>
    /// <param name="queryable">The already ordered collection.</param>
    /// <param name="ascending">Sort ascending (true) or descending (false)</param>
    /// <returns>The ordered collection</returns>
    /// <remarks>
    /// Hierarchical ordering is not restored here: with more than one sort level that has to happen once, after the
    /// last level has been applied. <see cref="GridItemsProviderRequest{TGridItem}.ApplySorting(IQueryable{TGridItem})"/>
    /// takes care of it.
    /// </remarks>
    public IOrderedQueryable<TGridItem> ApplyThen(IOrderedQueryable<TGridItem> queryable, bool ascending)
        => ApplyThenClauses(_firstAsThen(queryable, ascending), ascending);

    /// <summary>
    /// Applies the sort's rules without restoring hierarchical order, so that further sort levels can be appended.
    /// </summary>
    internal IOrderedQueryable<TGridItem> ApplyStandardSorting(IQueryable<TGridItem> queryable, bool ascending)
        => ApplyThenClauses(_first(queryable, ascending), ascending);

    private IOrderedQueryable<TGridItem> ApplyThenClauses(IOrderedQueryable<TGridItem> orderedQueryable, bool ascending)
    {
        if (_then is not null)
        {
            foreach (var clause in _then)
            {
                orderedQueryable = clause(orderedQueryable, ascending);
            }
        }

        return orderedQueryable;
    }

    /// <summary>
    /// Produces a readonly collection of (property name, direction) pairs representing the sorting rules.
    /// </summary>
    /// <param name="ascending"></param>
    /// <returns>The readonly collection of properties that can be sorted on</returns>
    public IReadOnlyCollection<SortedProperty> ToPropertyList(bool ascending)
    {
        if (ascending)
        {
            _cachedPropertyListAscending ??= BuildPropertyList(ascending: true);
            return _cachedPropertyListAscending;
        }

        _cachedPropertyListDescending ??= BuildPropertyList(ascending: false);
        return _cachedPropertyListDescending;
    }

    private List<SortedProperty> BuildPropertyList(bool ascending)
    {
        var result = new List<SortedProperty>
        {
            new() { PropertyName = ToPropertyName(_firstExpression.Item1), Direction = (_firstExpression.Item2 ^ ascending) ? DataGridSortDirection.Descending : DataGridSortDirection.Ascending },
        };

        if (_thenExpressions is not null)
        {
            foreach (var (thenLambda, thenAscending) in _thenExpressions)
            {
                result.Add(new SortedProperty { PropertyName = ToPropertyName(thenLambda), Direction = (thenAscending ^ ascending) ? DataGridSortDirection.Descending : DataGridSortDirection.Ascending });
            }
        }

        return result;
    }

    // Not sure we really want this level of complexity, but it converts expressions like @(c => c.Medals.Gold) to "Medals.Gold"
    // Makes it too complex to test, so we exclude from coverage
    [ExcludeFromCodeCoverage(Justification = "Find a way to test this at a later date.")]
#pragma warning disable MA0015 // Specify the parameter name in ArgumentException
    private static string ToPropertyName(LambdaExpression expression)
    {
        if (expression.Body is not MemberExpression body)
        {
            throw new ArgumentException(ExpressionNotRepresentableMessage);
        }

        // Handles cases like @(x => x.Name)
        if (body.Expression is ParameterExpression)
        {
            return body.Member.Name;
        }

        // First work out the length of the string we'll need, so that we can use string.Create
        var length = body.Member.Name.Length;
        var node = body;
        while (node.Expression is not null)
        {
            if (node.Expression is MemberExpression parentMember)
            {
                length += parentMember.Member.Name.Length + 1;
                node = parentMember;
            }
            else
            {
                if (node.Expression is ParameterExpression)
                {
                    break;
                }

                throw new ArgumentException(ExpressionNotRepresentableMessage);
            }
        }

        return string.Create(length, body, action);
    }

    [ExcludeFromCodeCoverage(Justification = "Find a way to test this at a later date.")]
    private static void action(Span<char> chars, MemberExpression body)
    {

        var nextPos = chars.Length;
        while (body is not null)
        {
            nextPos -= body.Member.Name.Length;
            body.Member.Name.CopyTo(chars[nextPos..]);
            if (nextPos > 0)
            {
                chars[--nextPos] = '.';
            }

            body = (body.Expression as MemberExpression)!;
        }
    }
}
#pragma warning restore MA0015 // Specify the parameter name in ArgumentException
