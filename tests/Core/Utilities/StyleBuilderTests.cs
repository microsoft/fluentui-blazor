using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Utilities;

public class StyleBuilderTests : TestBase
{
    [Fact]
    public void StyleBuilder_Raw()
    {
        // Assert
        var styleBuilder = new StyleBuilder();

        // Act
        styleBuilder.AddStyle("color: red");
        styleBuilder.AddStyle("background-color: blue");

        // Assert - Values are sorted
        Assert.Equal("color: red; background-color: blue;", styleBuilder.Build());
    }

    [Fact]
    public void StyleBuilder_RawWithExtraSpaces()
    {
        // Assert
        var styleBuilder = new StyleBuilder();

        // Act
        styleBuilder.AddStyle("  color: red  ");

        // Assert
        Assert.Equal("color: red;", styleBuilder.Build());
    }

    [Fact]
    public void StyleBuilder_RawWithExtraSemicolon()
    {
        // Assert
        var styleBuilder = new StyleBuilder();

        // Act
        styleBuilder.AddStyle("color: red;");

        // Assert - Remove the extra semicolon
        Assert.Equal("color: red;", styleBuilder.Build());
    }

    [Fact]
    public void CssBuilder_WithSimpleUserStyle()
    {
        // Assert
        var styleBuilder = new StyleBuilder("font-size: 12px;");

        // Act
        styleBuilder.AddStyle("color: red;");

        // Assert - Values are sorted
        Assert.Equal("color: red; font-size: 12px;", styleBuilder.Build());
    }

    [Fact]
    public void CssBuilder_WithComplexUserStyle()
    {
        // Assert
        var styleBuilder = new StyleBuilder("  font-size: 12px;;  font-name: courier;;  ");

        // Act
        styleBuilder.AddStyle("color: red;");

        // Assert - Values are sorted
        Assert.Equal("color: red; font-size: 12px; font-name: courier;", styleBuilder.Build());
    }

    [Fact]
    public void CssBuilder_WithComplexUserClasses()
    {
        // Assert
        var cssBuilder = new CssBuilder("  .my-user-class1  .my-user-class2  ");

        // Act
        cssBuilder.AddClass("class1");
        cssBuilder.AddClass("class2");

        // Assert - Values are sorted
        Assert.Equal("class1 class2 .my-user-class1 .my-user-class2", cssBuilder.Build());
    }

    [Fact]
    public void StyleBuilder_PropValue()
    {
        // Assert
        var styleBuilder = new StyleBuilder();

        // Act
        styleBuilder.AddStyle("color", "red");
        styleBuilder.AddStyle("background-color", "blue");

        // Assert
        Assert.Equal("color: red; background-color: blue;", styleBuilder.Build());
    }

    [Fact]
    public void StyleBuilder_WhenBooleanTrue()
    {
        // Assert
        var styleBuilder = new StyleBuilder();

        // Act
        styleBuilder.AddStyle("color", "red", when: true);

        // Assert
        Assert.Equal("color: red;", styleBuilder.Build());
    }

    [Fact]
    public void StyleBuilder_WhenBooleanFalse()
    {
        // Assert
        var styleBuilder = new StyleBuilder();

        // Act
        styleBuilder.AddStyle("color", "red", when: false);

        // Assert
        Assert.Null(styleBuilder.Build());
    }

    [Fact]
    public void StyleBuilder_WhenFunctionTrue()
    {
        // Assert
        var styleBuilder = new StyleBuilder();

        // Act
        styleBuilder.AddStyle("color", "red", when: () => true);

        // Assert
        Assert.Equal("color: red;", styleBuilder.Build());
    }

    [Fact]
    public void StyleBuilder_WhenFunctionFalse()
    {
        // Assert
        var styleBuilder = new StyleBuilder();

        // Act
        styleBuilder.AddStyle("color", "red", when: () => false);

        // Assert
        Assert.Null(styleBuilder.Build());
    }

    [Fact]
    public void StyleBuilder_ToString()
    {
        // Assert
        var styleBuilder = new StyleBuilder();

        // Act
        styleBuilder.AddStyle("color", "red");

        // Assert
        Assert.Equal("color: red;", styleBuilder.ToString());
    }
}
