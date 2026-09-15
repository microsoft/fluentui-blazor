// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json;
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
                Text = "Item 1",
                Index = 3
            }
        ];

        // Act
        var args = new OverflowChangedEventArgs
        {
            Id = "overflow-1",
            Items = items,
            OverflowCount = 5,
        };

        // Assert
        Assert.Equal("overflow-1", args.Id);
        Assert.Same(items, args.Items);
        Assert.Equal(5, args.OverflowCount);
    }

    [Fact]
    public void OverflowChangedEventArgs_CamelCasePayload_DeserializesHiddenItems()
    {
        const string json = """
            { "id": "overflow", "items": [
                { "id": "item-1", "text": "Item 1", "index": 2 },
                { "id": "item-2", "text": "Item 2", "index": 4 }
              ], "overflowCount": 6 }
            """;

        var args = JsonSerializer.Deserialize<OverflowChangedEventArgs>(json, new JsonSerializerOptions(JsonSerializerDefaults.Web));

        Assert.NotNull(args);
        Assert.Equal("overflow", args.Id);
        Assert.Equal(6, args.OverflowCount);
        Assert.NotNull(args.Items);
        Assert.Equal(["item-1", "item-2"], args.Items.Select(item => item.Id));
        Assert.Equal(["Item 1", "Item 2"], args.Items.Select(item => item.Text));
        Assert.Equal([2, 4], args.Items.Select(item => item.Index));
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
        Assert.Null(item.Text);
        Assert.Equal(0, item.Index);
    }

    [Fact]
    public void OverflowChangedItem_Sets_AllProperties()
    {
        // Act
        var item = new OverflowChangedItem
        {
            Id = "item-2",
            Text = "Item 2",
            Index = 4
        };

        // Assert
        Assert.Equal("item-2", item.Id);
        Assert.Equal("Item 2", item.Text);
        Assert.Equal(4, item.Index);
    }
}
