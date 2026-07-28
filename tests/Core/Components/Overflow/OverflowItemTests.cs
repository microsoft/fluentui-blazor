// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.Overflow;

public class OverflowItemTests
{
    [Fact]
    public void OverflowItem_Defaults_AreExpected()
    {
        // Act
        var item = new OverflowItem();

        // Assert
        Assert.Null(item.Id);
        Assert.False(item.Overflow);
        Assert.Null(item.Text);
        Assert.Null(item.Behavior);
        Assert.Equal(0, item.Index);
    }

    [Fact]
    public void OverflowItem_Sets_AllProperties()
    {
        // Act
        var item = new OverflowItem
        {
            Id = "item-1",
            Overflow = true,
            Text = "Item 1",
            Behavior = OverflowBehavior.Fixed,
            Index = 2,
        };

        // Assert
        Assert.Equal("item-1", item.Id);
        Assert.True(item.Overflow);
        Assert.Equal("Item 1", item.Text);
        Assert.Equal(OverflowBehavior.Fixed, item.Behavior);
        Assert.Equal(2, item.Index);
    }
}
