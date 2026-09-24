// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.DataGrid;

public class ColumnKeyGridSortTests : Bunit.BunitContext
{
    [Fact]
    public void ColumnKeyGridSort_ToPropertyList_ReturnsCorrectPropertyAndDirection()
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
    public void ColumnKeyGridSort_CanApplyThen_WithoutASortFunction_IsTrue()
    {
        // The data source sorts by the column key itself, so this sort works at any level.
        var sort = new ColumnKeyGridSort<GridRow>("Group");

        Assert.True(sort.CanApplyThen);
    }

    [Fact]
    public void ColumnKeyGridSort_CanApplyThen_WithASortFunctionOnly_IsFalse()
    {
        // OrderBy would replace the ordering of the levels before it, so this sort can only be the first level.
        var sort = new ColumnKeyGridSort<GridRow>("Group", (queryable, ascending) => queryable.OrderBy(x => x.Group));

        Assert.False(sort.CanApplyThen);
    }

    [Fact]
    public void ColumnKeyGridSort_ApplyThen_WithAThenSortFunction_AppendsTheSort()
    {
        var sort = new ColumnKeyGridSort<GridRow>(
            "Number",
            (queryable, ascending) => ascending ? queryable.OrderBy(x => x.Number) : queryable.OrderByDescending(x => x.Number),
            (queryable, ascending) => ascending ? queryable.ThenBy(x => x.Number) : queryable.ThenByDescending(x => x.Number));

        var data = new GridRow[] { new(2, "B"), new(1, "A"), new(4, "B"), new(3, "A") }.AsQueryable();
        var ordered = sort.ApplyThen(data.OrderBy(x => x.Group), ascending: false);

        Assert.True(sort.CanApplyThen);
        Assert.True(ordered.Select(x => x.Number).SequenceEqual([3, 1, 4, 2]));
    }

    [Fact]
    public void ColumnKeyGridSort_ApplyThen_WithoutAThenSortFunction_LeavesTheOrderingAlone()
    {
        var sort = new ColumnKeyGridSort<GridRow>("Group");

        var data = new GridRow[] { new(2, "B"), new(1, "A") }.AsQueryable();
        var ordered = sort.ApplyThen(data.OrderBy(x => x.Number), ascending: true);

        Assert.True(ordered.Select(x => x.Number).SequenceEqual([1, 2]));
    }

    [Fact]
    public void ColumnKeyGridSort_KeepsTheConstructorThatTookAColumnKeyAndASortFunction()
    {
        // The then-sort function was added as an extra constructor rather than as an optional argument on the
        // existing one: an optional argument changes the compiled signature, so callers built against an earlier
        // version would fail with a MissingMethodException instead of just recompiling.
        var parameterCounts = typeof(ColumnKeyGridSort<GridRow>)
            .GetConstructors()
            .Select(constructor => constructor.GetParameters().Length)
            .ToList();

        Assert.Contains(2, parameterCounts);
        Assert.Contains(3, parameterCounts);
    }
}
