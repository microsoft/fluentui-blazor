using Bunit;
using Xunit;

namespace Microsoft.Fast.Components.FluentUI.Tests.Divider;
public class FluentDividerTests : TestBase
{
    [Fact]
    public void FluentDivider_Default()
    {
        //Arrange
        string childContent = "<b>render me</b>";
        DividerRole? role = DividerRole.Separator!;
        Orientation? orientation = Orientation.Horizontal!;
        var cut = TestContext.RenderComponent<FluentDivider>(parameters => parameters
            .Add(p => p.Role, role)
            .Add(p => p.Orientation, orientation)
            .AddChildContent(childContent)
        );
        //Act

        //Assert
		cut.Verify();
    }
}






