// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.DataGrid.Infrastructure;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.DataGrid;

public class GridSortTests : Bunit.BunitContext
{
    private static readonly GridRow[] _gridData = [
        new(2, "B"),
        new(1, "A"),
        new(4, "B"),
        new(3, "A")
    ];

#pragma warning disable CA1861 // Avoid constant arrays as arguments

    [Theory]
    [InlineData(true, new int[] { 1, 2, 3, 4 })]
    [InlineData(false, new int[] { 4, 3, 2, 1 })]
    public void GridSortTests_SortBy_Number(bool ascending, IList<int> expected)
    {
        var sort = GridSort<GridRow>.ByAscending(x => x.Number);
        var ordered = sort.Apply(_gridData.AsQueryable(), ascending);

        Assert.True(ordered.Select(x => x.Number).SequenceEqual(expected));
    }

    [Theory]
    [InlineData(true, new int[] { 1, 2, 3, 4 })]
    [InlineData(false, new int[] { 4, 3, 2, 1 })]
    public void GridSortTests_SortBy_Number_Descending(bool ascending, IList<int> expected)
    {
        var sort = GridSort<GridRow>.ByDescending(x => x.Number);
        var ordered = sort.Apply(_gridData.AsQueryable(), ascending);

        Assert.False(ordered.Select(x => x.Number).SequenceEqual(expected));
    }

    [Theory]
    [InlineData(true, new int[] { 1, 2, 3, 4 })]
    [InlineData(false, new int[] { 4, 3, 2, 1 })]
    public void GridSortTests_SortBy_Number_Descending_WithComparer(bool ascending, IList<int> expected)
    {
        var groupComparer = Comparer<int>.Create((x, y) => y.CompareTo(x));
        var sort = GridSort<GridRow>.ByDescending(x => x.Number, groupComparer);
        var ordered = sort.Apply(_gridData.AsQueryable(), ascending);

        Assert.True(ordered.Select(x => x.Number).SequenceEqual(expected));
    }

    [Theory]
    [InlineData(true, new int[] { 1, 3, 2, 4 })]
    [InlineData(false, new int[] { 4, 2, 3, 1 })]
    public void GridSortTests_SortBy_GroupThenNumberAscending(bool ascending, IList<int> expected)
    {
        var sort = GridSort<GridRow>
            .ByAscending(x => x.Group)
            .ThenAscending(x => x.Number);

        var ordered = sort.Apply(_gridData.AsQueryable(), ascending);

        Assert.True(ordered.Select(x => x.Number).SequenceEqual(expected));
    }

    [Theory]
    [InlineData(true, new int[] { 1, 3, 2, 4 })]
    [InlineData(false, new int[] { 4, 2, 3, 1 })]
    public void GridSortTests_SortBy_GroupThenNumberAscending_WithComparer(bool ascending, IList<int> expected)
    {
        var groupComparer = StringComparer.OrdinalIgnoreCase;
        var sort = GridSort<GridRow>
            .ByAscending(x => x.Group, groupComparer)
            .ThenAscending(x => x.Number);

        var ordered = sort.Apply(_gridData.AsQueryable(), ascending);

        Assert.True(ordered.Select(x => x.Number).SequenceEqual(expected));
    }

    [Theory]
    [InlineData(true, new int[] { 1, 3, 2, 4 })]
    [InlineData(false, new int[] { 4, 2, 3, 1 })]
    public void GridSortTests_SortBy_GroupThenNumberAscending_WithComparer2(bool ascending, IList<int> expected)
    {
        var groupComparer = Comparer<int>.Create((x, y) => y.CompareTo(x));
        var sort = GridSort<GridRow>
            .ByAscending(x => x.Group)
            .ThenAscending(x => x.Number, groupComparer);

        var ordered = sort.Apply(_gridData.AsQueryable(), ascending);

        Assert.False(ordered.Select(x => x.Number).SequenceEqual(expected));
    }

    [Theory]
    [InlineData(true, new int[] { 3, 1, 4, 2 })]
    [InlineData(false, new int[] { 2, 4, 1, 3 })]
    public void GridSortTests_SortBy_GroupThenNumberDescending(bool ascending, IList<int> expected)
    {
        var sort = GridSort<GridRow>
            .ByAscending(x => x.Group)
            .ThenDescending(x => x.Number);

        var ordered = sort.Apply(_gridData.AsQueryable(), ascending);

        Assert.True(ordered.Select(x => x.Number).SequenceEqual(expected));
    }

    [Theory]
    [InlineData(true, new int[] { 3, 1, 4, 2 })]
    [InlineData(false, new int[] { 2, 4, 1, 3 })]
    public void GridSortTests_SortBy_GroupThenNumberDescending_WithComparer(bool ascending, IList<int> expected)
    {
        // Create an int comparer
        // that compares numbers in descending order
        var groupComparer = Comparer<int>.Create((x, y) => y.CompareTo(x));
        var sort = GridSort<GridRow>
            .ByAscending(x => x.Group)
            .ThenDescending(x => x.Number, groupComparer);

        var ordered = sort.Apply(_gridData.AsQueryable(), ascending);

        Assert.False(ordered.Select(x => x.Number).SequenceEqual(expected));
    }

    [Theory]
    [InlineData(true, new int[] { 1, 3, 2, 4 })]
    [InlineData(false, new int[] { 2, 4, 1, 3 })]
    public void GridSortTests_SortBy_GroupThenNumberAlwaysAscending(bool ascending, IList<int> expected)
    {
        var sort = GridSort<GridRow>
            .ByAscending(x => x.Group)
            .ThenAlwaysAscending(x => x.Number);

        var ordered = sort.Apply(_gridData.AsQueryable(), ascending);

        Assert.True(ordered.Select(x => x.Number).SequenceEqual(expected));
    }

    [Theory]
    [InlineData(true, new int[] { 1, 3, 2, 4 })]
    [InlineData(false, new int[] { 2, 4, 1, 3 })]
    public void GridSortTests_SortBy_GroupThenNumberAlwaysAscending_WithComparer(bool ascending, IList<int> expected)
    {
        var groupComparer = Comparer<int>.Create((x, y) => y.CompareTo(x));
        var sort = GridSort<GridRow>
            .ByAscending(x => x.Group)
            .ThenAlwaysAscending(x => x.Number, groupComparer);

        var ordered = sort.Apply(_gridData.AsQueryable(), ascending);

        Assert.False(ordered.Select(x => x.Number).SequenceEqual(expected));
    }

    [Theory]
    [InlineData(true, new int[] { 3, 1, 4, 2 })]
    [InlineData(false, new int[] { 4, 2, 3, 1 })]
    public void GridSortTests_SortBy_GroupThenNumberAlwaysDescending(bool ascending, IList<int> expected)
    {
        var sort = GridSort<GridRow>
            .ByAscending(x => x.Group)
            .ThenAlwaysDescending(x => x.Number);

        var ordered = sort.Apply(_gridData.AsQueryable(), ascending);

        Assert.True(ordered.Select(x => x.Number).SequenceEqual(expected));
    }

    [Theory]
    [InlineData(true, new int[] { 3, 1, 4, 2 })]
    [InlineData(false, new int[] { 4, 2, 3, 1 })]
    public void GridSortTests_SortBy_GroupThenNumberAlwaysDescending_WithComparer(bool ascending, IList<int> expected)
    {
        var groupComparer = Comparer<int>.Create((x, y) => y.CompareTo(x));
        var sort = GridSort<GridRow>
            .ByAscending(x => x.Group)
            .ThenAlwaysDescending(x => x.Number, groupComparer);

        var ordered = sort.Apply(_gridData.AsQueryable(), ascending);

        Assert.False(ordered.Select(x => x.Number).SequenceEqual(expected));
    }

    [Fact]
    public void ToPropertyList_ReturnsCorrectPropertyAndDirection()
    {
        // Arrange
        var sort = GridSort<GridRow>
            .ByAscending(x => x.Group)
            .ThenDescending(x => x.Number);

        // Act
        var resultAsc = sort.ToPropertyList(true).ToList();
        var resultDesc = sort.ToPropertyList(false).ToList();

        // Assert
        Assert.Equal(2, resultAsc.Count);
        Assert.Equal("Group", resultAsc[0].PropertyName);
        Assert.Equal("Number", resultAsc[1].PropertyName);

        Assert.Equal(2, resultDesc.Count);
        Assert.Equal("Group", resultDesc[0].PropertyName);
        Assert.Equal("Number", resultDesc[1].PropertyName);
    }

    [Theory]
    [InlineData(true, new int[] { 4, 6, 5, 1, 2, 3 })]
    [InlineData(false, new int[] { 1, 3, 2, 4, 5, 6 })]
    public void GridSortTests_HierarchicalData_KeepsParentChildOrder(bool ascending, IList<int> expected)
    {
        var sort = GridSort<HierarchicalGridRow>.ByAscending(x => x.Group);

        var ordered = sort.Apply(CreateHierarchicalGridRows().AsQueryable(), ascending).ToList();

        Assert.True(ordered.Select(x => x.Number).SequenceEqual(expected));
    }

    [Fact]
    public void GridSortTests_CanApplyThen_IsTrue()
    {
        var sort = GridSort<GridRow>.ByAscending(x => x.Group);

        Assert.True(sort.CanApplyThen);
    }

    [Fact]
    public void IGridSort_DefaultCanApplyThen_IsFalse()
    {
        IGridSort<GridRow> sort = new FirstOnlySort();

        Assert.False(sort.CanApplyThen);
    }

    [Fact]
    public void IGridSort_DefaultApplyThen_ExplainsThatSecondarySortingIsUnsupported()
    {
        IGridSort<GridRow> sort = new FirstOnlySort();

        var exception = Assert.Throws<NotSupportedException>(() =>
            sort.ApplyThen(_gridData.AsQueryable().OrderBy(x => x.Number), ascending: true));

        Assert.Contains(nameof(IGridSort<GridRow>.ApplyThen), exception.Message, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData(true, new int[] { 1, 3, 2, 4 })]
    [InlineData(false, new int[] { 4, 2, 3, 1 })]
    public void GridSortTests_ApplyThen_AppendsTheSortToAnExistingOrdering(bool ascending, IList<int> expected)
    {
        // Two separate sorts, as a grid sorted by two columns uses them.
        var first = GridSort<GridRow>.ByAscending(x => x.Group);
        var then = GridSort<GridRow>.ByAscending(x => x.Number);

        var ordered = then.ApplyThen(first.Apply(_gridData.AsQueryable(), ascending), ascending);

        Assert.True(ordered.Select(x => x.Number).SequenceEqual(expected));
    }

    [Fact]
    public void GridSortTests_ApplyThen_KeepsTheSortsOwnThenClauses()
    {
        var first = GridSort<TripleRow>.ByAscending(x => x.Group);
        var then = GridSort<TripleRow>.ByAscending(x => x.Name).ThenDescending(x => x.Number);

        var data = new TripleRow[]
        {
            new(1, "A", "Bravo"),
            new(2, "A", "Alpha"),
            new(3, "A", "Alpha"),
            new(4, "B", "Alpha"),
        }.AsQueryable();

        var ordered = then.ApplyThen(first.Apply(data, ascending: true), ascending: true);

        // Group A first, then by Name, and the sort's own ThenDescending breaks the Alpha tie.
        Assert.True(ordered.Select(x => x.Number).SequenceEqual([3, 2, 1, 4]));
    }

    [Fact]
    public void GridSortTests_ThenAscending_WithComparer_AppliesComparer()
    {
        var comparer = Comparer<int>.Create((left, right) => right.CompareTo(left));
        var sort = GridSort<GridRow>
            .ByAscending(x => x.Group)
            .ThenAscending(x => x.Number, comparer);

        var ordered = sort.Apply(_gridData.AsQueryable(), ascending: true);

        Assert.Equal([3, 1, 4, 2], ordered.Select(x => x.Number));
    }

    [Fact]
    public void GridSortTests_ThenDescending_WithComparer_AppliesComparer()
    {
        var comparer = Comparer<int>.Create((left, right) => right.CompareTo(left));
        var sort = GridSort<GridRow>
            .ByAscending(x => x.Group)
            .ThenDescending(x => x.Number, comparer);

        var ordered = sort.Apply(_gridData.AsQueryable(), ascending: true);

        Assert.Equal([1, 3, 2, 4], ordered.Select(x => x.Number));
    }

    [Fact]
    public void GridSortTests_ApplyThen_HierarchicalData_OrdersChildrenUnderTheirParent()
    {
        // How FluentDataGrid applies a multi-column sort to hierarchical data: every level is applied first, and the
        // parent/child order is restored once, at the end. Restoring it per level would produce a total order and
        // make each following level a no-op.
        var first = GridSort<HierarchicalGridRow>.ByAscending(x => x.Group);
        var then = GridSort<HierarchicalGridRow>.ByDescending(x => x.Number);
        var queryable = CreateHierarchicalGridRowsWithTies().AsQueryable();

        var sorted = then.ApplyThen(first.ApplyStandardSorting(queryable, ascending: true), ascending: true);
        var ordered = HierarchicalSortHelper.RestoreHierarchyOrder(queryable, sorted).ToList();

        // Root Alpha (4) first with its children by descending number, then root Bravo (1) with its own.
        Assert.True(ordered.Select(x => x.Number).SequenceEqual([4, 6, 5, 1, 3, 2]));
    }

    [Fact]
    public void HierarchicalSortHelper_EmptySortedQuery_ReturnsOriginalQuery()
    {
        var queryable = Array.Empty<HierarchicalGridRow>().AsQueryable();
        var sorted = queryable.OrderBy(item => item.Number);

        var result = HierarchicalSortHelper.RestoreHierarchyOrder(queryable, sorted);

        Assert.Empty(result);
        Assert.Same(sorted, result);
    }

    [Fact]
    public void HierarchicalSortHelper_WithoutRootItems_ReturnsOriginalSortedQuery()
    {
        var items = new[]
        {
            new HierarchicalGridRow(2, "B", 1),
            new HierarchicalGridRow(1, "A", 1),
        };
        var queryable = items.AsQueryable();
        var sorted = queryable.OrderBy(item => item.Group);

        var result = HierarchicalSortHelper.RestoreHierarchyOrder(queryable, sorted);

        Assert.Equal([1, 2], result.Select(item => item.Number));
        Assert.Same(sorted, result);
    }

    [Fact]
    public void HierarchicalSortHelper_OverlappingHierarchy_DoesNotDuplicateItems()
    {
        var firstRoot = new HierarchicalGridRow(1, "A", 0);
        var secondRoot = new HierarchicalGridRow(2, "B", 0);
        var sharedChild = new HierarchicalGridRow(3, "C", 1);
        firstRoot.ChildRows.Add(sharedChild);
        secondRoot.ChildRows.Add(sharedChild);

        var items = new[] { secondRoot, sharedChild, firstRoot }.AsQueryable();
        var sorted = items.OrderBy(item => item.Group);

        var result = HierarchicalSortHelper.RestoreHierarchyOrder(items, sorted).ToList();

        Assert.Equal([1, 3, 2], result.Select(item => item.Number));
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public void HierarchicalSortHelper_IsHierarchicalInMemoryQueryable_RecognizesOnlySupportedQueries()
    {
        var hierarchical = new[] { new HierarchicalGridRow(1, "A", 0) }.AsQueryable();
        var flat = _gridData.AsQueryable();

        Assert.True(HierarchicalSortHelper.IsHierarchicalInMemoryQueryable(hierarchical));
        Assert.False(HierarchicalSortHelper.IsHierarchicalInMemoryQueryable(flat));
    }

    private static List<HierarchicalGridRow> CreateHierarchicalGridRowsWithTies()
    {
        var rootBravo = new HierarchicalGridRow(1, "Bravo", 0);
        var bravoChildAlpha = new HierarchicalGridRow(2, "Alpha", 1);
        var bravoChildAlphaToo = new HierarchicalGridRow(3, "Alpha", 1);

        var rootAlpha = new HierarchicalGridRow(4, "Alpha", 0);
        var alphaChildBravo = new HierarchicalGridRow(5, "Bravo", 1);
        var alphaChildBravoToo = new HierarchicalGridRow(6, "Bravo", 1);

        rootBravo.ChildRows.Add(bravoChildAlpha);
        rootBravo.ChildRows.Add(bravoChildAlphaToo);
        rootAlpha.ChildRows.Add(alphaChildBravo);
        rootAlpha.ChildRows.Add(alphaChildBravoToo);

        return [rootBravo, bravoChildAlpha, bravoChildAlphaToo, rootAlpha, alphaChildBravo, alphaChildBravoToo];
    }

    private static List<HierarchicalGridRow> CreateHierarchicalGridRows()
    {
        var rootBravo = new HierarchicalGridRow(1, "Bravo", 0);
        var bravoChildAlpha = new HierarchicalGridRow(2, "Alpha", 1);
        var bravoChildCharlie = new HierarchicalGridRow(3, "Charlie", 1);

        var rootAlpha = new HierarchicalGridRow(4, "Alpha", 0);
        var alphaChildDelta = new HierarchicalGridRow(5, "Delta", 1);
        var alphaChildBravo = new HierarchicalGridRow(6, "Bravo", 1);

        rootBravo.ChildRows.Add(bravoChildAlpha);
        rootBravo.ChildRows.Add(bravoChildCharlie);
        rootAlpha.ChildRows.Add(alphaChildDelta);
        rootAlpha.ChildRows.Add(alphaChildBravo);

        return [rootBravo, bravoChildAlpha, bravoChildCharlie, rootAlpha, alphaChildDelta, alphaChildBravo];
    }

    public class NestedRow
    {
        public InnerRow Inner { get; set; } = new();
    }

    public class InnerRow
    {
        public int Value { get; set; }
    }

    private sealed class FirstOnlySort : IGridSort<GridRow>
    {
        public IOrderedQueryable<GridRow> Apply(IQueryable<GridRow> queryable, bool ascending)
            => queryable.OrderBy(item => item.Number);

        public IReadOnlyCollection<SortedProperty> ToPropertyList(bool ascending) => [];
    }
}

public class GridRow(int number, string group)
{
    public int Number { get; } = number;
    public string Group { get; } = group;
}

public class TripleRow(int number, string group, string name)
{
    public int Number { get; } = number;
    public string Group { get; } = group;
    public string Name { get; } = name;
}

public class HierarchicalGridRow(int number, string group, int depth) : IHierarchicalGridItem
{
    public int Number { get; } = number;
    public string Group { get; } = group;
    public int Depth { get; set; } = depth;
    public bool IsHidden { get; set; }
    public bool IsCollapsed { get; set; }
    public bool HasChildren => ChildRows.Count > 0;
    public bool? IsSelected { get; set; }
    public List<HierarchicalGridRow> ChildRows { get; } = [];

    public IEnumerable<IHierarchicalGridItem> Children => ChildRows;
}
