// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.Overflow;

public class OverflowStateTests
{
    [Fact]
    public void OverflowState_Defaults_AreExpected()
    {
        // Act
        var state = new OverflowState();

        // Assert
        Assert.Null(state.OverflowItems);
        Assert.Equal(0, state.OverflowCount);
        Assert.Equal(0, state.FirstOverflowIndex);
        Assert.Null(state.OrderedItemIds);
    }

    [Fact]
    public void OverflowState_Sets_AllProperties()
    {
        // Arrange
        OverflowItem[] overflowItems =
        [
            new()
            {
                Id = "item-2",
                Overflow = true,
                Text = "Item 2",
                Behavior = OverflowBehavior.Ellipsis,
                Index = 3,
            },
        ];
        string[] orderedItemIds = ["item-0", "item-1", "item-2"];

        // Act
        var state = new OverflowState
        {
            OverflowItems = overflowItems,
            OverflowCount = 4,
            FirstOverflowIndex = 3,
            OrderedItemIds = orderedItemIds,
        };

        // Assert
        Assert.Same(overflowItems, state.OverflowItems);
        Assert.Equal(4, state.OverflowCount);
        Assert.Equal(3, state.FirstOverflowIndex);
        Assert.Same(orderedItemIds, state.OrderedItemIds);
    }
}
