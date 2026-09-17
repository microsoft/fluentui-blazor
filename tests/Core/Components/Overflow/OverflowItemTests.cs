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
        Assert.Null(item.Text);
        Assert.Equal(0, item.Index);
    }

    [Fact]
    public void OverflowItem_Sets_AllProperties()
    {
        // Act
        var item = new OverflowItem
        {
            Id = "item-1",
            Text = "Item 1",
            Index = 2,
        };

        // Assert
        Assert.Equal("item-1", item.Id);
        Assert.Equal("Item 1", item.Text);
        Assert.Equal(2, item.Index);
    }
}
