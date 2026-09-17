// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.DataGrid;

public class HierarchicalGridUtilitiesTests
{
    [Fact]
    public void OrderHierarchically_EmptyList_ReturnsEmpty()
    {
        // Arrange
        var items = new List<TestGridItem>();

        // Act
        var result = HierarchicalGridUtilities.OrderHierarchically<TestGridItem, TestItem>(
            items,
            it => it.Id,
            it => it.ParentId,
            it => false);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void OrderHierarchically_FlatList_ReturnsCorrectOrder()
    {
        // Arrange
        var items = new List<TestGridItem>
        {
            new() { Item = new TestItem("1") },
            new() { Item = new TestItem("2") }
        };

        // Act
        var result = HierarchicalGridUtilities.OrderHierarchically<TestGridItem, TestItem>(
            items,
            it => it.Id,
            it => it.ParentId,
            it => false);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("1", result[0].Item.Id);
        Assert.Equal("2", result[1].Item.Id);
        Assert.Equal(0, result[0].Depth);
        Assert.Equal(0, result[1].Depth);
        Assert.False(result[0].IsHidden);
        Assert.False(result[1].IsHidden);
    }

    [Fact]
    public void OrderHierarchically_SimpleHierarchy_ReturnsCorrectOrderAndStructure()
    {
        // Arrange
        var items = new List<TestGridItem>
        {
            new() { Item = new TestItem("2", "1") },
            new() { Item = new TestItem("1") }
        };

        // Act
        var result = HierarchicalGridUtilities.OrderHierarchically<TestGridItem, TestItem>(
            items,
            it => it.Id,
            it => it.ParentId,
            it => false);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("1", result[0].Item.Id);
        Assert.Equal("2", result[1].Item.Id);
        Assert.Equal(0, result[0].Depth);
        Assert.Equal(1, result[1].Depth);
        Assert.Single(result[0].Children);
        Assert.Same(result[1], result[0].Children[0]);
    }

    [Fact]
    public void OrderHierarchically_NestedHierarchy_ReturnsCorrectDepth()
    {
        // Arrange
        var items = new List<TestGridItem>
        {
            new() { Item = new TestItem("3", "2") },
            new() { Item = new TestItem("2", "1") },
            new() { Item = new TestItem("1") }
        };

        // Act
        var result = HierarchicalGridUtilities.OrderHierarchically<TestGridItem, TestItem>(
            items,
            it => it.Id,
            it => it.ParentId,
            it => false);

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal("1", result[0].Item.Id);
        Assert.Equal("2", result[1].Item.Id);
        Assert.Equal("3", result[2].Item.Id);
        Assert.Equal(0, result[0].Depth);
        Assert.Equal(1, result[1].Depth);
        Assert.Equal(2, result[2].Depth);
    }

    [Fact]
    public void OrderHierarchically_CaseInsensitiveIds_Works()
    {
        // Arrange
        var items = new List<TestGridItem>
        {
            new() { Item = new TestItem("child", "PARENT") },
            new() { Item = new TestItem("PARENT") }
        };

        // Act
        var result = HierarchicalGridUtilities.OrderHierarchically<TestGridItem, TestItem>(
            items,
            it => it.Id,
            it => it.ParentId,
            it => false);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("PARENT", result[0].Item.Id);
        Assert.Equal("child", result[1].Item.Id);
    }

    [Fact]
    public void OrderHierarchically_MultipleRoots_PreservesOrder()
    {
        // Arrange
        var items = new List<TestGridItem>
        {
            new() { Item = new TestItem("1") },
            new() { Item = new TestItem("1.1", "1") },
            new() { Item = new TestItem("2") },
            new() { Item = new TestItem("2.1", "2") }
        };

        // Act
        var result = HierarchicalGridUtilities.OrderHierarchically<TestGridItem, TestItem>(
            items,
            it => it.Id,
            it => it.ParentId,
            it => false);

        // Assert
        Assert.Equal(4, result.Count);
        Assert.Equal("1", result[0].Item.Id);
        Assert.Equal("1.1", result[1].Item.Id);
        Assert.Equal("2", result[2].Item.Id);
        Assert.Equal("2.1", result[3].Item.Id);
    }

    [Fact]
    public void OrderHierarchically_CollapsedRoot_HidesChildren()
    {
        // Arrange
        var items = new List<TestGridItem>
        {
            new() { Item = new TestItem("1") },
            new() { Item = new TestItem("1.1", "1") }
        };

        // Act
        var result = HierarchicalGridUtilities.OrderHierarchically<TestGridItem, TestItem>(
            items,
            it => it.Id,
            it => it.ParentId,
            it => it.Id == "1");

        // Assert
        Assert.Equal("1", result[0].Item.Id);
        Assert.True(result[0].IsCollapsed);
        Assert.False(result[0].IsHidden);

        Assert.Equal("1.1", result[1].Item.Id);
        Assert.True(result[1].IsHidden);
    }

    [Fact]
    public void OrderHierarchically_DeepHiding_HidesAllDescendants()
    {
        // Arrange
        var items = new List<TestGridItem>
        {
            new() { Item = new TestItem("1") },
            new() { Item = new TestItem("1.1", "1") },
            new() { Item = new TestItem("1.1.1", "1.1") }
        };

        // Act
        var result = HierarchicalGridUtilities.OrderHierarchically<TestGridItem, TestItem>(
            items,
            it => it.Id,
            it => it.ParentId,
            it => it.Id == "1");

        // Assert
        Assert.True(result[1].IsHidden, "1.1 should be hidden");
        Assert.True(result[2].IsHidden, "1.1.1 should be hidden");
    }

    [Fact]
    public void OrderHierarchically_OrphanItems_AreTreatedAsRoots()
    {
        // Arrange
        var items = new List<TestGridItem>
        {
            new() { Item = new TestItem("orphan", "non-existent-parent") }
        };

        // Act
        var result = HierarchicalGridUtilities.OrderHierarchically<TestGridItem, TestItem>(
            items,
            it => it.Id,
            it => it.ParentId,
            it => false);

        // Assert
        Assert.Single(result);
        Assert.Equal("orphan", result[0].Item.Id);
        Assert.Equal(0, result[0].Depth);
    }

    private class TestItem(string id, string? parentId = null)
    {
        public string Id { get; } = id;
        public string? ParentId { get; } = parentId;
    }

    private class TestGridItem : HierarchicalGridItem<TestItem, TestGridItem>
    {
    }
}
