using Bunit;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Overflow;
public class FluentOverflowItemTests : TestBase
{
    [Fact(Skip = "Need to figure out how to do this test")]
    public void FluentOverFlowItem_Default()
    {
        //Arrange
        var childContent = "<b>render me</b>";
        FluentOverflow container = default!;
        var cut = TestContext.RenderComponent<FluentOverflowItem>(parameters => parameters
            .AddChildContent(childContent)
            .Add(p => p.Container, container)
        );
        //Act

        //Assert
        cut.Verify();
    }
}

