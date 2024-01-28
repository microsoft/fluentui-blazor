using Bunit;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Card;

public class FluentCardTests : TestBase
{
    [Fact]
    public void FluentCard_Default()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentCard>(parameters =>
        {
            parameters.AddChildContent("childcontent");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentCard_NotAreaRestricted()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentCard>(parameters =>
        {
            parameters.Add(p => p.AreaRestricted, false);
            parameters.AddChildContent("childcontent");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentCard_NotAreaRestricted_AdditionalStyle()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentCard>(parameters =>
        {
            parameters.Add(p => p.AreaRestricted, false);
            parameters.Add(p => p.Style, "background-color: red");
            parameters.AddChildContent("childcontent");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentCard_AdditionalCssClass()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentCard>(parameters =>
        {
            parameters.Add(p => p.Class, "css-class");
            parameters.AddChildContent("childcontent");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentCard_AdditionalStyle()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentCard>(parameters =>
        {
            parameters.Add(p => p.Style, "background-color: red");
            parameters.AddChildContent("childcontent");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentCard_AdditionalParameter()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentCard>(parameters =>
        {
            parameters.AddUnmatched("additional-parameter-name", "additional-parameter-value");
            parameters.AddChildContent("childcontent");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentCard_AdditionalParameters()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentCard>(parameters =>
        {
            parameters.AddUnmatched("additional-parameter1-name", "additional-parameter1-value");
            parameters.AddUnmatched("additional-parameter2-name", "additional-parameter2-value");
            parameters.AddChildContent("childcontent");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentCard_Width()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentCard>(parameters =>
        {
            parameters.Add(p => p.Width, "400px");
            parameters.AddChildContent("childcontent");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentCard_Height()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentCard>(parameters =>
        {
            parameters.Add(p => p.Height, "400px");
            parameters.AddChildContent("childcontent");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentCard_WidthAndHeight()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentCard>(parameters =>
        {
            parameters.Add(p => p.Width, "400px");
            parameters.Add(p => p.Height, "400px");
            parameters.AddChildContent("childcontent");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentCard_Id()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentCard>(parameters =>
        {
            parameters.Add(p => p.Id, "customid");
            parameters.AddChildContent("childcontent");
        });

        // Assert
        cut.Verify();
    }
}
