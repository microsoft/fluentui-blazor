using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Utilities;

public class InlineStyleBuilderTests : TestBase
{
    [Fact]
    public void InlineStyleBuilder_Default()
    {
        // Assert
        var styleBuilder = new InlineStyleBuilder();

        // Act
        styleBuilder.AddStyle("div", "color", "red");
        styleBuilder.AddStyle("div", "background-color", "blue");
        styleBuilder.AddStyle("span", "color", "yellow");

        // Assert - Values are sorted
        Assert.Equal(@"<style> div { color: red; background-color: blue; } span { color: yellow; } </style>", styleBuilder.Build(newLineSeparator: false));
    }

    [Fact]
    public void InlineStyleBuilder_SingleStyle()
    {
        // Assert
        var styleBuilder = new InlineStyleBuilder();

        // Act
        styleBuilder.AddStyle("div", "color", "red");

        // Assert - Values are sorted
        Assert.Equal(@"<style> div { color: red; } </style>", styleBuilder.Build(newLineSeparator: false));
    }

    [Fact]
    public void InlineStyleBuilder_NoStyle()
    {
        // Assert
        var styleBuilder = new InlineStyleBuilder();

        // Act
        styleBuilder.AddStyle("div", "color", string.Empty);

        // Assert - Values are sorted
        Assert.Null(styleBuilder.Build(newLineSeparator: false));
        Assert.Equal(string.Empty, styleBuilder.BuildMarkupString().Value);
    }
}
