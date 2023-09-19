using Microsoft.Fast.Components.FluentUI.Utilities;
using Xunit;

namespace Microsoft.Fast.Components.FluentUI.Tests.Utilities;

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
    public void CssBuilder_WhenFunction()
    {
        // Assert
        var cssBuilder = new CssBuilder();

        // Act
        cssBuilder.AddClass("my-class-1", when: () => true);
        cssBuilder.AddClass("my-class-2", when: () => false);

        // Assert
        Assert.Equal("my-class-1", cssBuilder.Build());
    }
}
