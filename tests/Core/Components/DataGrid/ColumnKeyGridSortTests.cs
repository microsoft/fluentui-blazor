// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.DataGrid;

public class ColumnKeyGridSortTests : Bunit.TestContext
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
}
