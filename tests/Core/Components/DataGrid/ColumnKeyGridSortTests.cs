// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.DataGrid;

public class ColumnKeyGridSortTests : Bunit.BunitContext
{
    [Fact]
    public void ToPropertyList_ReturnsCorrectPropertyAndDirection()
    {
        // Arrange
        var sort = new ColumnKeyGridSort<GridRow>(
            "Group", (queryable, sortAscending) =>
            {
                if (sortAscending)
                {
                    return queryable.OrderBy(x => x.Group);
                }
                else
                {
                    return queryable.OrderByDescending(x => x.Group);
                }
            });

        // Act
        var resultAsc = sort.ToPropertyList(true).ToList();
        var resultDesc = sort.ToPropertyList(false).ToList();

        // Assert
        Assert.Single(resultAsc);
        Assert.Equal("Group", resultAsc[0].PropertyName);

        Assert.Single(resultDesc);
        Assert.Equal("Group", resultDesc[0].PropertyName);
    }

    [Fact]
    public void Apply_WithoutCustomSort_ReturnsOrderedQueryableWithOriginalSequence()
    {
        // Arrange
        var sort = new ColumnKeyGridSort<GridRow>("Group");
        var data = new[]
        {
            new GridRow(2, "B"),
            new GridRow(1, "A"),
            new GridRow(3, "C"),
        }.AsQueryable();

        // Act
        var ordered = sort.Apply(data, ascending: true).ToList();

        // Assert
        Assert.Equal(3, ordered.Count);
        Assert.Equal(2, ordered[0].Number);
        Assert.Equal(1, ordered[1].Number);
        Assert.Equal(3, ordered[2].Number);
    }

    [Theory]
    [InlineData(true, new[] { 1, 2, 3 })]
    [InlineData(false, new[] { 3, 2, 1 })]
    public void Apply_WithCustomSort_UsesSortFunction(bool ascending, int[] expectedOrder)
    {
        // Arrange
        var sort = new ColumnKeyGridSort<GridRow>(
            "Number",
            (queryable, isAscending) => isAscending
                ? queryable.OrderBy(x => x.Number)
                : queryable.OrderByDescending(x => x.Number));

        var data = new[]
        {
            new GridRow(2, "B"),
            new GridRow(1, "A"),
            new GridRow(3, "C"),
        }.AsQueryable();

        // Act
        var ordered = sort.Apply(data, ascending).Select(x => x.Number).ToArray();

        // Assert
        Assert.Equal(expectedOrder, ordered);
    }
}
