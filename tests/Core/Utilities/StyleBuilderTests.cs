using Microsoft.Fast.Components.FluentUI.Utilities;
using Xunit;

namespace Microsoft.Fast.Components.FluentUI.Tests.Utilities;

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
        Assert.Equal("background-color: blue; color: red;", styleBuilder.Build());
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

        // Assert - Keep the extra semicolon
        Assert.Equal("color: red;;", styleBuilder.Build());
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
        Assert.Equal("background-color: blue; color: red;", styleBuilder.Build());
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
}
