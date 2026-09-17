// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

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
