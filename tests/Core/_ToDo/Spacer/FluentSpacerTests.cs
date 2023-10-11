using Bunit;
using Xunit;

namespace Microsoft.Fast.Components.FluentUI.Tests.Spacer;
public class FluentSpacerTests : TestBase
{
    [Fact]
    public void FluentSpacer_Default()
    {
        //Arrange
        int? width = default!;
        var cut = TestContext.RenderComponent<FluentSpacer>(parameters => parameters
            .Add(p => p.Width, width)
        );
        //Act

        //Assert
		cut.Verify();
    }
}






