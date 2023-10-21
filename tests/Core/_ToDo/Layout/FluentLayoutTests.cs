using Bunit;
using Xunit;

namespace Microsoft.Fast.Components.FluentUI.Tests.Layout;
public class FluentLayoutTests : TestBase
{
    [Fact]
    public void FluentLayout_Default()
    {
        //Arrange
        string childContent = "<b>render me</b>";
        var cut = TestContext.RenderComponent<FluentLayout>(parameters => parameters
            .AddChildContent(childContent)
        );
        //Act

        //Assert
		cut.Verify();
    }
}






