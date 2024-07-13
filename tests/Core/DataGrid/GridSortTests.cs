using FluentAssertions;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.DataGrid;

public class GridSortTests : TestBase
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

        ordered.Select(x => x.Number)
            .SequenceEqual(expected)
            .Should().BeTrue();
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

        ordered.Select(x => x.Number)
            .SequenceEqual(expected)
            .Should().BeTrue();
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

        ordered.Select(x => x.Number)
            .SequenceEqual(expected)
            .Should().BeTrue();
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

        ordered.Select(x => x.Number)
            .SequenceEqual(expected)
            .Should().BeTrue();
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

        ordered.Select(x => x.Number)
            .SequenceEqual(expected)
            .Should().BeTrue();
    }

#pragma warning restore CA1861 // Avoid constant arrays as arguments

    public class GridRow(int number, string group)
    {
        public int Number { get; } = number;
        public string Group { get; } = group;
    }
}
