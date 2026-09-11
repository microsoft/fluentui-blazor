// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components.Rendering;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.DataGrid;

public class TemplateColumnTests
{
    private sealed class TestTemplateColumn : TemplateColumn<string>
    {
        public void InvokeCellContent(RenderTreeBuilder builder, string item)
            => base.CellContent(builder, item);

        public string? InvokeRawCellContent(string item)
            => base.RawCellContent(item);

        public bool InvokeIsSortableByDefault()
            => base.IsSortableByDefault();
    }

    private sealed class TestGridSort : IGridSort<string>
    {
        public IReadOnlyCollection<SortedProperty> ToPropertyList(bool ascending)
            => new[]
            {
                new SortedProperty { PropertyName = nameof(string.Length), Direction = DataGridSortDirection.Ascending }
            };

        public IOrderedQueryable<string> Apply(IQueryable<string> queryable, bool ascending)
            => ascending ? queryable.OrderBy(item => item) : queryable.OrderByDescending(item => item);
    }

    [Fact]
    public void CellContent_WhenChildContentIsNotSet_UsesEmptyChildContent()
    {
        // Arrange
        var column = new TestTemplateColumn();
        var builder = new RenderTreeBuilder();

        // Act
        column.InvokeCellContent(builder, "value");

        // Assert
        Assert.NotNull(builder);
    }

    [Fact]
    public void RawCellContent_WhenTooltipTextIsNull_ReturnsNull()
    {
        // Arrange
        var column = new TestTemplateColumn();

        // Act
        var result = column.InvokeRawCellContent("value");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void IsSortableByDefault_WhenSortByIsPresent_ReturnsTrue()
    {
        // Arrange
#pragma warning disable BL0005 // Component parameter should not be set outside of its component.
        var column = new TestTemplateColumn
        {
            SortBy = new TestGridSort()
        };
#pragma warning restore BL0005 // Component parameter should not be set outside of its component.

        // Act
        var result = column.InvokeIsSortableByDefault();

        // Assert
        Assert.True(result);
    }
}
