using Bunit;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Footer;
public class FluentFooterTests : TestBase
{
    [Fact]
    public void FluentFooter_Default()
    {
        //Arrange
        var childContent = "<b>render me</b>";
        var cut = TestContext.RenderComponent<FluentFooter>(parameters => parameters
            .AddChildContent(childContent)
        );
        //Act

        //Assert
        cut.Verify();
    }
}

