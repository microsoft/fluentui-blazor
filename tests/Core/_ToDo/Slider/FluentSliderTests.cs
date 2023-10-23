using Bunit;
using Xunit;

namespace Microsoft.Fast.Components.FluentUI.Tests.Slider;


public class FluentSliderTests: TestBase
    {
    [Fact]
    public void FluentSlider_Default()
    {
        //Arrange
        string childContent = "<b>render me</b>";
        int min = default!;
        int max = default!;
        int step = default!;
        Orientation? orientation = default!;
        SliderMode? mode = default!;
        var cut = TestContext.RenderComponent<FluentSlider<int>>(parameters => parameters
            .Add(p => p.Min, min)
            .Add(p => p.Max, max)
            .Add(p => p.Step, step)
            .Add(p => p.Orientation, orientation)
            .Add(p => p.Mode, mode)
            .AddChildContent(childContent)
        );
        //Act

        //Assert
		cut.Verify();
    }
}






