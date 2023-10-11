using Bunit;
using Xunit;

namespace Microsoft.Fast.Components.FluentUI.Tests.Toolbar;
public class FluentToolbarTests : TestBase
{
    [Fact]
    public void FluentToolbar_Default()
    {
        //Arrange
        string childContent = "<b>render me</b>";
        Microsoft.Fast.Components.FluentUI.Orientation? orientation = default!;
        var cut = TestContext.RenderComponent<FluentToolbar>(parameters => parameters
            .Add(p => p.Orientation, orientation)
            .AddChildContent(childContent)
        );
        //Act

        //Assert
		cut.Verify();
    }
}






