// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.FluentUI.AspNetCore.Components.DataGrid.Infrastructure;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Parameters for data to be supplied by a <see cref="FluentDataGrid{TGridItem}"/>'s <see cref="FluentDataGrid{TGridItem}.ItemsProvider"/>.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
public readonly struct GridItemsProviderRequest<TGridItem>
{
    private readonly IReadOnlyList<DataGridSortColumn<TGridItem>>? _sortColumns;

    /// <summary>
    /// Gets or sets the zero-based index of the first item to be supplied.
    /// </summary>
    public int StartIndex { get; init; }

    /// <summary>
    /// If set, the maximum number of items to be supplied. If not set, the maximum number is unlimited.
    /// </summary>
    public int? Count { get; init; }

    /// <summary>
    /// Gets the columns the grid is sorted by, in priority order: the first entry is the primary sort, the ones after
    /// it break its ties. Contains at most one entry unless <see cref="FluentDataGrid{TGridItem}.SortMode"/> is
    /// <see cref="DataGridSortMode.Multiple"/>, and is empty when the grid is not sorted.
    ///
    /// Rather than inferring the sort rules manually, you should normally call either
    /// <see cref="ApplySorting(IQueryable{TGridItem})"/> or <see cref="GetSortByProperties"/>, since they apply every
    /// sort level for you.
    /// </summary>
    public IReadOnlyList<DataGridSortColumn<TGridItem>> SortColumns
    {
        get => _sortColumns ?? [];
        init => _sortColumns = value;
    }

    /// <summary>
    /// Gets or sets a token that indicates if the request should be cancelled.
    /// </summary>
    public CancellationToken CancellationToken { get; init; }

    internal GridItemsProviderRequest(
        int startIndex, int? count, IReadOnlyList<DataGridSortColumn<TGridItem>> sortColumns,
        CancellationToken cancellationToken)
    {
        StartIndex = startIndex;
        Count = count;
        _sortColumns = sortColumns;
        CancellationToken = cancellationToken;
    }

    /// <summary>
    /// Applies the request's sorting rules to the supplied <see cref="IQueryable{TGridItem}"/>.
    /// </summary>
    /// <param name="source">An <see cref="IQueryable{TGridItem}"/>.</param>
    /// <returns>A new <see cref="IQueryable{TGridItem}"/> representing the <paramref name="source"/> with sorting rules applied.</returns>
    public IQueryable<TGridItem> ApplySorting(IQueryable<TGridItem> source)
    {
        var levels = GetSortableLevels();

        if (levels.Count == 0)
        {
            return source;
        }

        if (levels.Count == 1)
        {
            return levels[0].Column.SortBy!.Apply(source, levels[0].Ascending);
        }

        // The first level is applied without restoring hierarchical order: that turns the result into a total
        // order, which would make every ThenBy below a no-op. It is restored once, after the last level.
        var primary = levels[0];
        var ordered = primary.Column.SortBy is GridSort<TGridItem> gridSort
            ? gridSort.ApplyStandardSorting(source, primary.Ascending)
            : primary.Column.SortBy!.Apply(source, primary.Ascending);

        for (var i = 1; i < levels.Count; i++)
        {
            ordered = levels[i].Column.SortBy!.ApplyThen(ordered, levels[i].Ascending);
        }

        return HierarchicalSortHelper.IsHierarchicalInMemoryQueryable(source)
            ? HierarchicalSortHelper.RestoreHierarchyOrder(source, ordered)
            : ordered;
    }

    /// <summary>
    /// Produces a collection of (property name, direction) pairs representing the sorting rules.
    /// When the grid is sorted by more than one column, the pairs of all sort levels are returned in priority order.
    /// </summary>
    /// <returns>A collection of (property name, direction) pairs representing the sorting rules</returns>
    [ExcludeFromCodeCoverage(Justification = "This is a not reachable in a unit test scenario.")]
    public IReadOnlyCollection<SortedProperty> GetSortByProperties()
    {
        var levels = GetSortableLevels();

        if (levels.Count == 0)
        {
            return [];
        }

        if (levels.Count == 1)
        {
            return levels[0].Column.SortBy!.ToPropertyList(levels[0].Ascending);
        }

        var properties = new List<SortedProperty>();
        foreach (var level in levels)
        {
            properties.AddRange(level.Column.SortBy!.ToPropertyList(level.Ascending));
        }

        return properties;
    }

    /// <summary>
    /// Determines whether the specified request is equivalent to the current request.
    /// </summary>
    /// <param name="req">The <see cref="GridItemsProviderRequest{TGridItem}"/> to compare with the current request.</param>
    /// <returns><see langword="true"/> if the specified request has the same start index, count, sort columns, and sort
    /// order as the current request; otherwise, <see langword="false"/>.</returns>
    [ExcludeFromCodeCoverage(Justification = "This is a not reachable in a unit test scenario.")]
    public bool IsSameRequest(GridItemsProviderRequest<TGridItem> req)
    {
        if (StartIndex != req.StartIndex)
        {
            return false;
        }

        if (Count != req.Count)
        {
            return false;
        }

        var sortColumns = SortColumns;
        var otherSortColumns = req.SortColumns;

        if (sortColumns.Count != otherSortColumns.Count)
        {
            return false;
        }

        for (var i = 0; i < sortColumns.Count; i++)
        {
            if (sortColumns[i].Column.Index != otherSortColumns[i].Column.Index
                || sortColumns[i].Ascending != otherSortColumns[i].Ascending)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Gets the sort levels that can actually sort the data, keeping their priority order. Levels whose column has no
    /// <see cref="ColumnBase{TGridItem}.SortBy"/>, and secondary levels whose sort cannot be appended to an existing
    /// ordering, are left out.
    /// </summary>
    private List<DataGridSortColumn<TGridItem>> GetSortableLevels()
    {
        var sortColumns = SortColumns;
        var levels = new List<DataGridSortColumn<TGridItem>>(sortColumns.Count);

        foreach (var level in sortColumns)
        {
            var sortBy = level.Column.SortBy;
            if (sortBy is null || (levels.Count > 0 && !sortBy.CanApplyThen))
            {
                continue;
            }

            levels.Add(level);
        }

        return levels;
    }
}
