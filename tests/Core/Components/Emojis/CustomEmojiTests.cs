// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.Emojis;

public class CustomEmojiTests : Bunit.TestContext
{
    [Fact]
    public void DefaultConstructor_SetsExpectedDefaults()
    {
        // Arrange & Act
        var emoji = new CustomEmoji();

        // Assert
        Assert.Equal(string.Empty, emoji.Name);
        Assert.Equal(EmojiSize.Size32, emoji.Size);
        Assert.Equal(EmojiGroup.Objects, emoji.Group);
        Assert.Equal(EmojiSkintone.Default, emoji.Skintone);
        Assert.Equal(EmojiStyle.Color, emoji.Style);
    }

    [Fact]
    public void Constructor_WithEmoji_CopiesValues()
    {
        // Arrange
        var source = new Samples.Emojis.Samples.Hamburger();

        // Act
        var emoji = new CustomEmoji(source);

        // Assert
        Assert.Equal("Hamburger", emoji.Name);
        Assert.Equal(EmojiSize.Size32, emoji.Size);
        Assert.Equal(EmojiGroup.Food_Drink, emoji.Group);
        Assert.Equal(EmojiSkintone.Default, emoji.Skintone);
        Assert.Equal(EmojiStyle.Color, emoji.Style);
        Assert.NotEmpty(emoji.Content);
    }
}
