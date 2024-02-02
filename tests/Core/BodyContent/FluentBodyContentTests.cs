using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.BodyContent;
public class FluentBodyContentTests : TestBase
{

    [Fact]
    public void FluentBodyContent_WithAdditionalCssClass()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentBodyContent>(parameters =>
        {
            parameters.Add(p => p.Class, "additional_class");
            parameters.AddChildContent("childcontent");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentBodyContent_WithAdditionalStyle()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentBodyContent>(parameters =>
        {
            parameters.Add(p => p.Style, "background-color: red");
            parameters.AddChildContent("childcontent");
        });

        // Assert
        cut.Verify();
    }

}
