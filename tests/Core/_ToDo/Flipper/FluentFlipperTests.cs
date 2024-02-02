using Bunit;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Flipper;
public class FluentFlipperTests : TestBase
{
    [Fact]
    public void FluentFlipper_Previous()
    {
        //Arrange
        var childContent = "<b>render me</b>";
        bool? disabled = false;
        bool? ariaHidden = false;
        FlipperDirection? direction = FlipperDirection.Previous;
        var cut = TestContext.RenderComponent<FluentFlipper>(parameters => parameters
            .Add(p => p.Disabled, disabled)
            .Add(p => p.AriaHidden, ariaHidden)
            .Add(p => p.Direction, direction)
            .AddChildContent(childContent)
        );
        //Act

        //Assert
        cut.Verify();
    }

    [Fact]
    public void FluentFlipper_Next()
    {
        //Arrange
        var childContent = "<b>render me</b>";
        bool? disabled = true;
        bool? ariaHidden = true;
        FlipperDirection? direction = FlipperDirection.Next;
        var cut = TestContext.RenderComponent<FluentFlipper>(parameters => parameters
            .Add(p => p.Disabled, disabled)
            .Add(p => p.AriaHidden, ariaHidden)
            .Add(p => p.Direction, direction)
            .AddChildContent(childContent)
        );
        //Act

        //Assert
        cut.Verify();
    }
}

