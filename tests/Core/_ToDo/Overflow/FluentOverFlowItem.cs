using Bunit;
using Xunit;

namespace Microsoft.Fast.Components.FluentUI.Tests.Overflow;
public class FluentOverflowItemTests : TestBase
{
    [Fact(Skip = "Need to figure out how to do this test")]
    public void FluentOverFlowItem_Default()
    {
        //Arrange
        string childContent = "<b>render me</b>";
        Microsoft.Fast.Components.FluentUI.FluentOverflow container = default!;
        var cut = TestContext.RenderComponent<FluentOverflowItem>(parameters => parameters
            .AddChildContent(childContent)
            .Add(p => p.Container, container)
        );
        //Act

        //Assert
		cut.Verify();
    }
}






