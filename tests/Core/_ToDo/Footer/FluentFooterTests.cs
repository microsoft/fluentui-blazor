using Bunit;
using Xunit;

namespace Microsoft.Fast.Components.FluentUI.Tests.Footer;
public class FluentFooterTests : TestBase
{
    [Fact]
    public void FluentFooter_Default()
    {
        //Arrange
        string childContent = "<b>render me</b>";
        var cut = TestContext.RenderComponent<FluentFooter>(parameters => parameters
            .AddChildContent(childContent)
        );
        //Act

        //Assert
		cut.Verify();
    }
}







