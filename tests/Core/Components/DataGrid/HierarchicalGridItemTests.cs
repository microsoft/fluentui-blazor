// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.DataGrid;

public class HierarchicalGridItemTests
{
    [Fact]
    public void IsCollapsed_SetTrue_HidesChildrenRecursively()
    {
        // Arrange
        var root = new TestGridItem { Item = new TestItem("0") };
        var child = new TestGridItem { Item = new TestItem("1"), Parent = root };
        var grandChild = new TestGridItem { Item = new TestItem("2"), Parent = child };
        root.Children.Add(child);
        child.Children.Add(grandChild);

        // Act
        root.IsCollapsed = true;

        // Assert
        Assert.True(child.IsHidden);
        Assert.True(grandChild.IsHidden);
    }

    [Fact]
    public void IsCollapsed_SetFalse_UnhidesChildrenRecursively()
    {
        // Arrange
        var root = new TestGridItem { Item = new TestItem("0") };
        var child = new TestGridItem { Item = new TestItem("1"), Parent = root };
        var grandChild = new TestGridItem { Item = new TestItem("2"), Parent = child };
        root.Children.Add(child);
        child.Children.Add(grandChild);

        root.IsCollapsed = true; 

        // Act
        root.IsCollapsed = false;

        // Assert
        Assert.False(child.IsHidden);
        Assert.False(grandChild.IsHidden);
    }

    [Fact]
    public void IsCollapsed_HidingChildren_RespectsChildCollapsedState()
    {
        // Arrange
        var root = new TestGridItem { Item = new TestItem("0") };
        var child = new TestGridItem { Item = new TestItem("1"), Parent = root };
        var grandChild = new TestGridItem { Item = new TestItem("2"), Parent = child };
        root.Children.Add(child);
        child.Children.Add(grandChild);

        child.IsCollapsed = true; 
        Assert.True(grandChild.IsHidden);

        // Act
        root.IsCollapsed = true; 

        // Assert
        Assert.True(child.IsHidden);
        Assert.True(grandChild.IsHidden);

        // Act
        root.IsCollapsed = false; 

        // Assert
        Assert.False(child.IsHidden);
        Assert.True(grandChild.IsHidden);
    }

    [Fact]
    public void IsSelected_SetTrue_PropagatesToChildrenRecursively()
    {
        // Arrange
        var root = new TestGridItem { Item = new TestItem("0") };
        var child = new TestGridItem { Item = new TestItem("1"), Parent = root };
        var grandChild = new TestGridItem { Item = new TestItem("2"), Parent = child };
        root.Children.Add(child);
        child.Children.Add(grandChild);

        // Act
        root.IsSelected = true;

        // Assert
        Assert.True(root.IsSelected);
        Assert.True(child.IsSelected);
        Assert.True(grandChild.IsSelected);
    }

    [Fact]
    public void IsSelected_SetFalse_PropagatesToChildrenRecursively()
    {
        // Arrange
        var root = new TestGridItem { Item = new TestItem("0") };
        var child = new TestGridItem { Item = new TestItem("1"), Parent = root };
        var grandChild = new TestGridItem { Item = new TestItem("2"), Parent = child };
        root.Children.Add(child);
        child.Children.Add(grandChild);

        root.IsSelected = true;

        // Act
        root.IsSelected = false;

        // Assert
        Assert.False(root.IsSelected);
        Assert.False(child.IsSelected);
        Assert.False(grandChild.IsSelected);
    }

    [Fact]
    public void IsSelected_SetFromChild_UpdatesParentState()
    {
        // Arrange
        var root = new TestGridItem { Item = new TestItem("0") };
        var child1 = new TestGridItem { Item = new TestItem("1.1"), Parent = root };
        var child2 = new TestGridItem { Item = new TestItem("1.2"), Parent = root };
        root.Children.Add(child1);
        root.Children.Add(child2);

        // Act
        child1.IsSelected = true;

        // Assert
        Assert.Null(root.IsSelected); 

        // Act
        child2.IsSelected = true;

        // Assert
        Assert.True(root.IsSelected); 
    }

    [Fact]
    public void IsSelected_MixedChildren_ParentIndeterminate()
    {
        // Arrange
        var root = new TestGridItem { Item = new TestItem("0") };
        var child1 = new TestGridItem { Item = new TestItem("1"), Parent = root };
        var child2 = new TestGridItem { Item = new TestItem("2"), Parent = root };
        root.Children.Add(child1);
        root.Children.Add(child2);

        // Act
        child1.IsSelected = true;
        child2.IsSelected = false;

        // Assert
        Assert.Null(root.IsSelected);
    }

    [Fact]
    public void IsSelected_AllChildrenIndeterminate_ParentIndeterminate()
    {
        // Arrange
        var root = new TestGridItem { Item = new TestItem("0") };
        var child1 = new TestGridItem { Item = new TestItem("1"), Parent = root };
        var child2 = new TestGridItem { Item = new TestItem("2"), Parent = root };
        root.Children.Add(child1);
        root.Children.Add(child2);
        
        child1.IsSelected = true;
        child2.IsSelected = true;
        Assert.True(root.IsSelected);

        // Act
        child1.IsSelected = null;
        child2.IsSelected = null;

        // Assert
        Assert.Null(root.IsSelected);
    }

    [Fact]
    public void IsSelected_SettingNull_DoesNotPropagateToChildren()
    {
        // Arrange
        var root = new TestGridItem { Item = new TestItem("0") };
        var child = new TestGridItem { Item = new TestItem("1"), Parent = root };
        root.Children.Add(child);
        root.IsSelected = true;
        child.IsSelected = true;

        // Act
        root.IsSelected = null;

        // Assert
        Assert.Null(root.IsSelected);
        Assert.True(child.IsSelected); 
    }

    [Fact]
    public void IsSelected_SameValue_ReturnsEarly()
    {
        // Arrange
        var root = new TestGridItem { Item = new TestItem("0") };
        root.IsSelected = true;

        // Act
        root.IsSelected = true;

        // Assert
        Assert.True(root.IsSelected);
    }

    [Fact]
    public void Children_ExplicitInterface_ReturnsCorrectChildren()
    {
        // Arrange
        var root = new TestGridItem { Item = new TestItem("0") };
        var child = new TestGridItem { Item = new TestItem("1") };
        root.Children.Add(child);
        IHierarchicalGridItem interfaceItem = root;

        // Act
        var children = interfaceItem.Children;

        // Assert
        Assert.Single(children);
        Assert.Same(child, children.First());
    }

    [Fact]
    public void Properties_SetAndGet_ReturnCorrectValues()
    {
        // Arrange
        var item = new TestGridItem { Item = new TestItem("0") };

        // Act
        item.Depth = 5;
        item.IsHidden = true;

        // Assert
        Assert.Equal(5, item.Depth);
        Assert.True(item.IsHidden);
        Assert.False(item.HasChildren);

        item.Children.Add(new TestGridItem { Item = new TestItem("1") });
        Assert.True(item.HasChildren);
    }

    [Fact]
    public void UpdateSelectionFromChildren_NoChildren_ReturnsEarly()
    {
        // Arrange
        var root = new TestGridItem { Item = new TestItem("root") };
        var child = new TestGridItem { Item = new TestItem("child"), Parent = root };
        // NOTE: We deliberately DO NOT add child to root.Children here
        // this allows root.UpdateSelectionFromChildren() to be called via child, 
        // with root having 0 children.

        root.IsSelected = false;

        // Act
        child.IsSelected = true;

        // Assert
        Assert.False(root.IsSelected, "Parent selection should not be updated if it has no children in its list.");
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
