using Bunit;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Layout;
public class FluentLayoutTests : TestBase
{
    [Fact]
    public void FluentLayout_Default()
    {
        //Arrange
        var childContent = "<b>render me</b>";
        var cut = TestContext.RenderComponent<FluentLayout>(parameters => parameters
            .AddChildContent(childContent)
        );
        //Act

        //Assert
        cut.Verify();
    }
}

