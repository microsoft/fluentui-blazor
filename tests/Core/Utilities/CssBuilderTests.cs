using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Utilities;

public class CssBuilderTests : TestBase
{
    [Fact]
    public void CssBuilder_AddSingleClasses()
    {
        // Arrange
        var cssBuilder = new CssBuilder();

        // Act
        cssBuilder.AddClass("class1");
        cssBuilder.AddClass("class2");

        // Assert
        Assert.Equal("class1 class2", cssBuilder.Build());
    }

    [Fact]
    public void CssBuilder_AddValidUserClass()
    {
        // Arrange
        var cssBuilder = new CssBuilder("my-user-class");

        // Act
        cssBuilder.AddClass("class1");
        cssBuilder.AddClass("class2");

        // Assert
        Assert.Equal("class1 class2 my-user-class", cssBuilder.Build());
    }

    [Fact]
    public void CssBuilder_AddMultipleValidUserClasses()
    {
        // Arrange
        var cssBuilder = new CssBuilder("my-user-class1 my-user-class2");

        // Act
        cssBuilder.AddClass("class1");
        cssBuilder.AddClass("class2");

        // Assert
        Assert.Equal("class1 class2 my-user-class1 my-user-class2", cssBuilder.Build());
    }

    [Fact]
    public void CssBuilder_AddClassesWithExtraSpaces()
    {
        // Arrange
        var cssBuilder = new CssBuilder();

        // Act
        cssBuilder.AddClass("  my-class-1  ");
        cssBuilder.AddClass("  my-class-2  ");

        // Assert
        Assert.Equal("my-class-1 my-class-2", cssBuilder.Build());
    }

    [Fact]
    public void CssBuilder_AddMultipleClassesAtOnce()
    {
        // Arrange
        var cssBuilder = new CssBuilder();

        // Act
        cssBuilder.AddClass("class1 class2");

        // Assert
        Assert.Equal("class1 class2", cssBuilder.Build());
    }

    [Fact]
    public void CssBuilder_AddMultipleClassesAtOnceWithUserClass()
    {
        // Arrange
        var cssBuilder = new CssBuilder("user-class");

        // Act
        cssBuilder.AddClass("class1 class2");

        // Assert
        Assert.Equal("class1 class2 user-class", cssBuilder.Build());
    }

    [Fact]
    public void CssBuilder_AddMultipleClassesAtOnceWithExtraSpaces()
    {
        // Arrange
        var cssBuilder = new CssBuilder();

        // Act
        cssBuilder.AddClass("  class1  class2  ");

        // Assert
        Assert.Equal("class1 class2", cssBuilder.Build());
    }

    [Fact]
    public void CssBuilder_AddClassesBasedOnCondition()
    {
        // Arrange
        var cssBuilder = new CssBuilder();

        // Act
        cssBuilder.AddClass("my-class-1", when: true);
        cssBuilder.AddClass("my-class-2", when: false);

        // Assert
        Assert.Equal("my-class-1", cssBuilder.Build());
    }

    [Fact]
    public void CssBuilder_AddClassesBasedOnFunctionTrue()
    {
        // Arrange
        var cssBuilder = new CssBuilder();

        // Act
        cssBuilder.AddClass("my-class-1", when: () => true);

        // Assert
        Assert.Equal("my-class-1", cssBuilder.Build());
    }

    [Fact]
    public void CssBuilder_AddClassesBasedOnFunctionFalse()
    {
        // Arrange
        var cssBuilder = new CssBuilder();

        // Act
        cssBuilder.AddClass("my-class-2", when: () => false);

        // Assert
        Assert.Null(cssBuilder.Build());
    }

    [Fact]
    public void CssBuilder_ToStringReturnsBuiltClasses()
    {
        // Arrange
        var cssBuilder = new CssBuilder();

        // Act
        cssBuilder.AddClass("my-class");

        // Assert
        Assert.Equal("my-class", cssBuilder.ToString());
    }

    [Fact]
    public void CssBuilder_InvalidClassNamesAreIgnored()
    {
        // Arrange
        var cssBuilder = new CssBuilder();

        // Act
        cssBuilder.AddClass("valid-class");
        cssBuilder.AddClass("123-invalid-class");

        // Assert
        Assert.Equal("valid-class", cssBuilder.Build());
    }

    [Fact]
    public void CssBuilder_CombinesValidUserAndAddedClasses()
    {
        // Arrange
        var cssBuilder = new CssBuilder("user-class");

        // Act
        cssBuilder.AddClass("added-class");

        // Assert
        Assert.Equal("added-class user-class", cssBuilder.Build());
    }

    [Theory]
    [InlineData("min-h-[16px] user-class", "min-h-[16px] ")]
    [InlineData("bg-red-500/50 user-class", " bg-red-500/50")]
    [InlineData("bg-[#ff0000] user-class", " bg-[#ff0000] ")]
    [InlineData("a:hover user-class", "a:hover")]
    [InlineData("min-h-[16px] a:hover user-class", "min-h-[16px]", "a:hover")]
    public void CssBuilder_ValidateClassNames_AcceptInvalid(string expected, params string[] value)
    {
        // Arrange
        var cssBuilder = new CssBuilder(validateClassNames: false, userClasses: "user-class");

        // Act
        foreach (var item in value)
        {
            cssBuilder.AddClass(item);
        }

        // Assert
        Assert.Equal(expected, cssBuilder.Build());
    }
}
