// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.Paginator;
public class PaginationStateTest : Bunit.TestContext
{
    [Fact]
    public async Task PaginationState_GetHashCode_ReturnsExpectedAsync()
    {
        // Arrange
        var paginationState = new PaginationState();
        paginationState.ItemsPerPage = 20;

        await paginationState.SetTotalItemCountAsync(100);
        await paginationState.SetItemsPerPageAsync(20);
        await paginationState.SetCurrentPageIndexAsync(2);

        // Act
        var hashCode = paginationState.GetHashCode();

        // Assert
        Assert.NotEqual(0, hashCode);
    }
    [Fact]
    public async Task SetTotalItemCountAsync_UpdatesTotalItemCountAndKeepsCurrentPageIndexValid()
    {
        // Arrange
        var paginationState = new PaginationState();
        paginationState.ItemsPerPage = 10;
        await paginationState.SetItemsPerPageAsync(10);
        await paginationState.SetCurrentPageIndexAsync(3);

        // Act
        await paginationState.SetTotalItemCountAsync(25, force: true);
        await paginationState.SetTotalItemCountAsync(25, force: false);

        // Assert
        Assert.Equal(25, paginationState.TotalItemCount);
        // With 10 items per page and 25 items, there are 3 pages (0,1,2)
        // So current page index should be adjusted to 2 if it was set to 3
        Assert.Equal(2, paginationState.CurrentPageIndex);
    }
}
