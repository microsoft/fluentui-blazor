// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Xunit;

#pragma warning disable FLUENTUI0001

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Serialization;

public class EventArgsJsonTests
{
    public static TheoryData<EventArgs> CustomEventArgs => new()
    {
        new AccordionItemEventArgs { Id = "accordion-1", Expanded = true, HeaderText = "Accordion 1" },
        new DialogToggleEventArgs { Id = "dialog-1", OldState = "closed", NewState = "open", Type = "toggle" },
        new DropdownEventArgs { Id = "dropdown-1", Type = "change", SelectedOptions = "option-1;option-2" },
        new MenuItemEventArgs { Id = "menu-item-1", Checked = true, Text = "Menu item 1" },
        new OverflowChangedEventArgs
        {
            Id = "overflow-1",
            Items =
            [
                new OverflowChangedItem
                {
                    Id = "overflow-item-1",
                    Overflow = true,
                    Text = "Overflow item 1",
                    Behavior = OverflowBehavior.Fixed,
                    Index = 1,
                },
            ],
            OverflowCount = 1,
            FirstOverflowIndex = 1,
            OrderedItemIds = ["overflow-item-1"],
        },
        new RadioEventArgs { Id = "radio-1", Value = "value-1" },
        new TabChangeEventArgs { Id = "tabs-1", ActiveId = "tab-1" },
        new TreeItemChangedEventArgs { Id = "tree-item-1", Selected = true },
        new TreeItemToggleEventArgs { Id = "tree-item-1", OldState = "closed", NewState = "open", Type = "toggle" },
    };

    [Theory]
    [MemberData(nameof(CustomEventArgs))]
    public void EventArgs_RoundTripsUsingGeneratedMetadata(EventArgs value)
    {
        var typeInfo = GetTypeInfo(value.GetType());
        var json = JsonSerializer.Serialize(value, typeInfo);

        var result = JsonSerializer.Deserialize(json, typeInfo);

        Assert.NotNull(result);
        Assert.Equal(value.GetType(), result.GetType());
        Assert.Equal(json, JsonSerializer.Serialize(result, typeInfo));
    }

    [Fact]
    public void MenuItemEventArgs_DeserializesBrowserPayloadWithoutItem()
    {
        const string json = """
            {"id":"item-1","text":"Item 1","checked":true,"item":{}}
            """;

        var typeInfo = GetTypeInfo<MenuItemEventArgs>();
        var result = JsonSerializer.Deserialize(json, typeInfo);

        Assert.NotNull(result);
        Assert.Equal("item-1", result.Id);
        Assert.Equal("Item 1", result.Text);
        Assert.True(result.Checked);
        Assert.Null(result.Item);
        Assert.DoesNotContain("\"item\":", JsonSerializer.Serialize(result, typeInfo), StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void AccordionItemEventArgs_DeserializesBrowserPayloadWithoutItem()
    {
        const string json = """
            {"id":"item-1","expanded":true,"headerText":"Item 1","item":{}}
            """;

        var typeInfo = GetTypeInfo<AccordionItemEventArgs>();
        var result = JsonSerializer.Deserialize(json, typeInfo);

        Assert.NotNull(result);
        Assert.Equal("item-1", result.Id);
        Assert.True(result.Expanded);
        Assert.Equal("Item 1", result.HeaderText);
        Assert.Null(result.Item);
        Assert.DoesNotContain("\"item\":", JsonSerializer.Serialize(result, typeInfo), StringComparison.OrdinalIgnoreCase);
    }

    private static JsonTypeInfo GetTypeInfo(Type type) =>
        FluentUIJsonSerializerContext.Default.GetTypeInfo(type) ??
        throw new InvalidOperationException($"No JSON metadata was generated for {type}.");

    private static JsonTypeInfo<T> GetTypeInfo<T>() => (JsonTypeInfo<T>)GetTypeInfo(typeof(T));
}

#pragma warning restore FLUENTUI0001