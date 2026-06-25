// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.Overflow;

public class OverflowChangedEventArgsTests
{
    [Fact]
    public void OverflowChangedEventArgs_Defaults_AreExpected()
    {
        // Act
        var args = new OverflowChangedEventArgs();

        // Assert
        Assert.Null(args.Id);
        Assert.Null(args.Items);
        Assert.Equal(0, args.OverflowCount);
        Assert.Equal(-1, args.FirstOverflowIndex);
    }

    [Fact]
    public void OverflowChangedEventArgs_Sets_AllProperties()
    {
        // Arrange
        IReadOnlyList<OverflowChangedItem> items =
        [
            new()
            {
                Id = "item-1",
                Overflow = true,
                Text = "Item 1",
                Fixed = "fixed",
                Index = 3
            }
        ];

        // Act
        var args = new OverflowChangedEventArgs
        {
            Id = "overflow-1",
            Items = items,
            OverflowCount = 5,
            FirstOverflowIndex = 3
        };

        // Assert
        Assert.Equal("overflow-1", args.Id);
        Assert.Same(items, args.Items);
        Assert.Equal(5, args.OverflowCount);
        Assert.Equal(3, args.FirstOverflowIndex);
    }
}

public class OverflowChangedItemTests
{
    [Fact]
    public void OverflowChangedItem_Defaults_AreExpected()
    {
        // Act
        var item = new OverflowChangedItem();

        // Assert
        Assert.Null(item.Id);
        Assert.False(item.Overflow);
        Assert.Null(item.Text);
        Assert.Null(item.Fixed);
        Assert.Equal(0, item.Index);
    }

    [Fact]
    public void OverflowChangedItem_Sets_AllProperties()
    {
        // Act
        var item = new OverflowChangedItem
        {
            Id = "item-2",
            Overflow = true,
            Text = "Item 2",
            Fixed = "ellipsis",
            Index = 4
        };

        // Assert
        Assert.Equal("item-2", item.Id);
        Assert.True(item.Overflow);
        Assert.Equal("Item 2", item.Text);
        Assert.Equal("ellipsis", item.Fixed);
        Assert.Equal(4, item.Index);
    }
}
