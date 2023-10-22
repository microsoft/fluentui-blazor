using Bunit;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Toolbar;
public class FluentToolbarTests : TestBase
{
    [Fact]
    public void FluentToolbar_Default()
    {
        //Arrange
        string childContent = "<b>render me</b>";
        Microsoft.FluentUI.AspNetCore.Components.Orientation? orientation = default!;
        var cut = TestContext.RenderComponent<FluentToolbar>(parameters => parameters
            .Add(p => p.Orientation, orientation)
            .AddChildContent(childContent)
        );
        //Act

        //Assert
		cut.Verify();
    }
}






