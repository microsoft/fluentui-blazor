// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.Overflow;

public class OverflowStateTests
{
    [Fact]
    public void OverflowState_DeserializesBrowserPayload()
    {
        // Arrange
        const string json = """
            {
              "overflowItems": [
                { "id": "item-1", "overflow": true, "text": "Item 1", "behavior": "fixed", "index": 2 },
                { "id": "item-2", "overflow": true, "text": "Item 2", "behavior": "ellipsis", "index": 3 },
                { "id": "item-3", "overflow": false, "text": "Item 3", "behavior": null, "index": 4 }
              ],
              "overflowCount": 2,
              "firstOverflowIndex": 2,
              "orderedItemIds": ["item-1", "item-2", "item-3"]
            }
            """;

        // Act
        var state = JsonSerializer.Deserialize<OverflowState>(json, new JsonSerializerOptions(JsonSerializerDefaults.Web));

        // Assert
        Assert.NotNull(state);
        Assert.NotNull(state.OverflowItems);
        Assert.Collection(
            state.OverflowItems,
            item => Assert.Equal(OverflowBehavior.Fixed, item.Behavior),
            item => Assert.Equal(OverflowBehavior.Ellipsis, item.Behavior),
            item => Assert.Null(item.Behavior));
        Assert.Equal(2, state.OverflowCount);
        Assert.Equal(2, state.FirstOverflowIndex);
        Assert.NotNull(state.OrderedItemIds);
        Assert.Equal(["item-1", "item-2", "item-3"], state.OrderedItemIds);
    }

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
