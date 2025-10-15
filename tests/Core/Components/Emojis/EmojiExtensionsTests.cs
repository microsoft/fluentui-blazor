// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.Emojis;

public class EmojiExtensionsTests : Bunit.TestContext
{
    [Fact]
    [RequiresUnreferencedCode("This test requires dynamic access to code.")]
    public void GetInstance_WithInvalidEmoji_ThrowsArgumentException()
    {
        // Arrange
        var emojiInfo = new EmojiInfo
        {
            Name = "NonExistentEmoji123456789",
            Size = EmojiSize.Size32,
            Group = EmojiGroup.Food_Drink,
            Skintone = EmojiSkintone.Default,
            Style = EmojiStyle.Color
        };

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => emojiInfo.GetInstance());
        Assert.Contains("not found", ex.Message, StringComparison.InvariantCultureIgnoreCase);
        Assert.Contains("NonExistentEmoji123456789", ex.Message);
    }

    [Fact]
    [RequiresUnreferencedCode("This test requires dynamic access to code.")]
    public void GetAllEmojis_DoesNotThrow_AndReturnsEnumerable()
    {
        // Arrange
        IEnumerable<EmojiInfo>? all = null;

        // Act
        var exception = Record.Exception(() =>
        {
            all = EmojiExtensions.GetAllEmojis();
        });

        // Assert
        Assert.Null(exception);
        Assert.NotNull(all);
    }

    [Fact]
    [RequiresUnreferencedCode("This test requires dynamic access to code.")]
    public void AllEmojis_Property_ReturnsEnumerable_AndMatchesGetAllEmojis()
    {
        // Arrange & Act
        var fromMethod = EmojiExtensions.GetAllEmojis();
        var fromProperty = EmojiExtensions.AllEmojis;

        // Assert
        Assert.NotNull(fromMethod);
        Assert.NotNull(fromProperty);
        Assert.Equal(fromMethod.Count(), fromProperty.Count());
    }
}
