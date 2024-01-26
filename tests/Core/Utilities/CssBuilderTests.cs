using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Utilities;

public class CssBuilderTests : TestBase
{
    [Fact]
    public void CssBuilder_Raw()
    {
        // Assert
        var cssBuilder = new CssBuilder();

        // Act
        cssBuilder.AddClass("class1");
        cssBuilder.AddClass("class2");

        // Assert - Values are sorted
        Assert.Equal("class1 class2", cssBuilder.Build());
    }

    [Fact]
    public void CssBuilder_WithSimpleUserClass()
    {
        // Assert
        var cssBuilder = new CssBuilder(".my-user-class");

        // Act
        cssBuilder.AddClass("class1");
        cssBuilder.AddClass("class2");

        // Assert - Values are sorted
        Assert.Equal("class1 class2 .my-user-class", cssBuilder.Build());
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
    public void CssBuilder_RawWithExtraSpaces()
    {
        // Assert
        var cssBuilder = new CssBuilder();

        // Act
        cssBuilder.AddClass("  my-class-1  ");
        cssBuilder.AddClass("  my-class-2  ");

        // Assert
        Assert.Equal("my-class-1 my-class-2", cssBuilder.Build());
    }

    [Fact]
    public void CssBuilder_WhenBoolean()
    {
        // Assert
        var cssBuilder = new CssBuilder();

        // Act
        cssBuilder.AddClass("my-class-1", when: true);
        cssBuilder.AddClass("my-class-2", when: false);

        // Assert
        Assert.Equal("my-class-1", cssBuilder.Build());
    }

    [Fact]
    public void CssBuilder_WhenFunctionTrue()
    {
        // Assert
        var cssBuilder = new CssBuilder();

        // Act
        cssBuilder.AddClass("my-class-1", when: () => true);

        // Assert
        Assert.Equal("my-class-1", cssBuilder.Build());
    }

    [Fact]
    public void CssBuilder_WhenFunctionFalse()
    {
        // Assert
        var cssBuilder = new CssBuilder();

        // Act
        cssBuilder.AddClass("my-class-2", when: () => false);

        // Assert
        Assert.Null(cssBuilder.Build());
    }

    [Fact]
    public void StyleBuilder_ToString()
    {
        // Assert
        var cssBuilder = new CssBuilder();

        // Act
        cssBuilder.AddClass("my-class");

        // Assert
        Assert.Equal("my-class", cssBuilder.ToString());
    }

}
